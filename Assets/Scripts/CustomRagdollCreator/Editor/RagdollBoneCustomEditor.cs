using UnityEditor;
using UnityEngine;

namespace CustomRagdollCreator
{
    [CustomEditor(typeof(RagdollBone))]
    public class RagdollBoneCustomEditor : Editor
    {
        private float _sliderValue;

        private RagdollBone _ragdollBone;
        private RagdollBoneSymmetry _symmetry;
        
        public override void OnInspectorGUI()
        {
            _ragdollBone = (RagdollBone)target;
            _ragdollBone.TryGetComponent(out _symmetry);
            
            base.OnInspectorGUI();

            if (GUILayout.Button("Destroy Bone"))
                _ragdollBone.DestroyBone();
        }

        private void OnSceneGUI()
        {
            if (!_ragdollBone)
                return;
            
            Pose currentTopPose = GetCurrentCylinderEndPosition();

            EditorGUI.BeginChangeCheck();
            Vector3 currentTopPosition = Handles.PositionHandle(currentTopPose.position, currentTopPose.rotation);

            if (!EditorGUI.EndChangeCheck())
                return;
            
            Undo.RecordObject(_ragdollBone.capsuleCollider, "Edited Ragdoll Bone Capsule Length");
            RecalculateCapsuleFromEndPosition(currentTopPosition);

            if (!_symmetry)
                return;
            
            Undo.RecordObject(_symmetry.symmetricalBone1.capsuleCollider, "Edited Ragdoll Bone Capsule Length");
            Undo.RecordObject(_symmetry.symmetricalBone2.capsuleCollider, "Edited Ragdoll Bone Capsule Length");
            _symmetry.ApplySymmetry(_ragdollBone == _symmetry.symmetricalBone2);
        }

        private Pose GetCurrentCylinderEndPosition()
        {
            CapsuleCollider capsule = _ragdollBone.capsuleCollider;
            Vector3 localCapsuleDir = capsule.GetLocalDirectionOfCapsuleCollider();
            Vector3 worldCapsuleDir = _ragdollBone.transform.TransformDirection(localCapsuleDir);
            Vector3 toEndOfCapsule = capsule.center + 0.5f * capsule.height * localCapsuleDir;

            Camera sceneCam = Camera.current;
            Vector3 cameraForward = sceneCam ? sceneCam.transform.forward : Vector3.forward;
            Vector3 localCameraForward = _ragdollBone.transform.InverseTransformVector(cameraForward);
            Vector3 radiusOffset = Vector3.Cross(localCameraForward, localCapsuleDir).normalized * capsule.radius;
            
            Pose cylinderTopPose = new Pose();
            cylinderTopPose.position = _ragdollBone.transform.TransformPoint(toEndOfCapsule + radiusOffset);
            cylinderTopPose.rotation = Quaternion.LookRotation(worldCapsuleDir, cameraForward);
            
            return cylinderTopPose;
        }

        private void RecalculateCapsuleFromEndPosition(Vector3 capsuleTopAndRadiusPos)
        {
            // calls to transform are external to c++, cached it here
            Transform boneTransform = _ragdollBone.transform;
            CapsuleCollider capsule = _ragdollBone.capsuleCollider;
            Vector3 localCapsuleDir = capsule.GetLocalDirectionOfCapsuleCollider();

            Vector3 localTopAndRadiusPos = boneTransform.InverseTransformPoint(capsuleTopAndRadiusPos);
            Vector3 localTopPos = localTopAndRadiusPos.Multiply(localCapsuleDir);
            float radius = localTopAndRadiusPos.Multiply(Vector3.one - localCapsuleDir).magnitude;
            
            capsule.height = localTopPos.magnitude;
            capsule.center = 0.5f * localTopPos;
            capsule.radius = radius;
        }
    }
}