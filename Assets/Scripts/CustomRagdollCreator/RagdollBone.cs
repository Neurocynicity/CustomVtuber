using UnityEditor;
using UnityEngine;

namespace CustomRagdollCreator
{
    public class RagdollBone : MonoBehaviour
    {
        [SerializeField]
        public RagdollBone connectionBase, connectionEnd;

        [SerializeField]
        public new Rigidbody rigidbody;
        [SerializeField]
        public CapsuleCollider capsuleCollider;
        [SerializeField]
        public CharacterJoint characterJoint;

        public static RagdollBone GenerateRagdollBone(Transform transform, RagdollBone connectionBase)
        {
            RagdollBone thisBone = transform.gameObject.AddComponent<RagdollBone>();
            thisBone.connectionBase = connectionBase;
            return thisBone;
        }
        
        public virtual void RecursiveUnfoldGenerateBone(RagdollBone newConnectionEnd)
        {
            // ensure we're only generated once
            if (connectionEnd)
                return;
            
            connectionEnd = newConnectionEnd;
            SetUpRigidbodyAndCapsuleCollider();

            if (connectionBase)
                connectionBase.RecursiveUnfoldGenerateBone(this);
        }

        public virtual void SetUpJoint()
        {
            if (!connectionBase)
                return;
            
            gameObject.EnsureAttachedComponent(ref characterJoint);
            
            characterJoint.connectedBody = connectionBase.rigidbody;
            characterJoint.enableProjection = true;
            characterJoint.enablePreprocessing = false;
        }

        private void SetUpRigidbodyAndCapsuleCollider()
        {
            gameObject.EnsureAttachedComponent(ref rigidbody);
            gameObject.EnsureAttachedComponent(ref capsuleCollider);

            GenerateDefaultCapsule();
        }

        public virtual void GenerateDefaultCapsule()
        {
            // calls to transform are external to c++, cached it here
            Transform myTransform = transform;

            
            Vector3 localCapsuleDir = capsuleCollider.GetLocalDirectionOfCapsuleCollider();
            Vector3 capsuleTopPos = connectionEnd.transform.position;
            Vector3 localTopPos = myTransform.InverseTransformPoint(capsuleTopPos);
            localTopPos = localTopPos.Multiply(localCapsuleDir);
            
            capsuleCollider.height = localTopPos.magnitude;
            capsuleCollider.center = 0.5f * localTopPos;
            capsuleCollider.radius = capsuleCollider.height / 5f;
        }

#if UNITY_EDITOR

        public void DestroyBone()
        {
            string undoActionName = $"Removed bone {gameObject.name}";
            
            if (connectionBase)
                Undo.RegisterFullObjectHierarchyUndo(connectionBase.gameObject, undoActionName);
            else
                Undo.RegisterFullObjectHierarchyUndo(gameObject, undoActionName);
            
            DestroyImmediate(capsuleCollider);
            DestroyImmediate(characterJoint);
            DestroyImmediate(rigidbody);

            connectionBase.connectionEnd = connectionEnd;
            connectionEnd.connectionBase = connectionBase;

            connectionBase.GenerateDefaultCapsule();
            connectionBase.SetUpJoint();
            connectionEnd.GenerateDefaultCapsule();
            connectionEnd.SetUpJoint();

            if (TryGetComponent(out RagdollBoneSymmetry symmetry))
            {
                RagdollBone otherBone;

                if (symmetry.symmetricalBone1 == this)
                    otherBone = symmetry.symmetricalBone2;
                else
                    otherBone = symmetry.symmetricalBone1;

                if (TryGetComponent(out RagdollBoneSymmetry otherSymmetry))
                    DestroyImmediate(otherSymmetry);
    
                otherBone.DestroyBone();
            }
            
            DestroyImmediate(this);
        }
#endif
    }
}