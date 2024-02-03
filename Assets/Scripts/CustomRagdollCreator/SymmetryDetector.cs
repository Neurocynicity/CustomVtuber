using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomRagdollCreator
{
    public static class SymmetryDetector
    {
        private static CustomRagdoll _ragdoll;
        private static List<CloseSymmetry> _closeSymmetries;
        
        public static void EvaluateAllSymmetries(CustomRagdoll ragdoll, bool destroyExistingSymmetries = false)
        {
            _ragdoll = ragdoll;
            _closeSymmetries = new();
            
            if (destroyExistingSymmetries)
            {
                Undo.RegisterFullObjectHierarchyUndo(ragdoll.gameObject, "Regenerated ragdoll symmetries");

                var existingSymmetries = ragdoll.GetComponentsInChildren<RagdollBoneSymmetry>();
                for (int i = 0; i < existingSymmetries.Length; i++)
                    Object.DestroyImmediate(existingSymmetries[i]);
            }

            RecursivelyCheckSymmetry(ragdoll.transform);

            CleanCloseSymmetries();

            _ragdoll.CloseSymmetries = _closeSymmetries;
        }

        private static void RecursivelyCheckSymmetry(Transform parent)
        {
            foreach (Transform child1 in parent)
            {
                foreach (Transform child2 in parent)
                {
                    if (child1 == child2) continue;

                    if (!child1.TryGetComponent(out RagdollBone bone1) ||
                        !child2.TryGetComponent(out RagdollBone bone2))
                        continue;

                    EvaluateSymmetry(bone1, bone2, parent);
                }

                RecursivelyCheckSymmetry(child1);
            }
        }

        private static void CleanCloseSymmetries()
        {
            List<CloseSymmetry> cleanedCloseSymmetries = new();

            for (int i = 0; i < _closeSymmetries.Count; i++)
            {
                if (!cleanedCloseSymmetries.Contains(_closeSymmetries[i]))
                    cleanedCloseSymmetries.Add(_closeSymmetries[i]);
            }

            _closeSymmetries = cleanedCloseSymmetries;
        }

        private static void EvaluateSymmetry(RagdollBone bone1, RagdollBone bone2, Transform symmetricalParent)
        {
            Transform bone1Transform = bone1.transform;
            Transform bone2Transform = bone2.transform;

            float positionalSymmetry = EvaluatePositionalSymmetry(bone1Transform, bone2Transform, symmetricalParent);
            float rotationalSymmetry = EvaluateRotationalSymmetry(bone1Transform, bone2Transform);

            if (positionalSymmetry > Mathf.Max(0.1f, 2 * _ragdoll.symmetryPosAllowance) ||
                rotationalSymmetry > Mathf.Max(45f, 2 * _ragdoll.symmetryRotAllowance))
                return;

            if (positionalSymmetry > _ragdoll.symmetryPosAllowance ||
                rotationalSymmetry > _ragdoll.symmetryRotAllowance)
            {
                _closeSymmetries.Add(new CloseSymmetry(bone1, bone2, positionalSymmetry, rotationalSymmetry));
                return;
            }

            RagdollBoneSymmetry symmetry = bone1.gameObject.AddComponent<RagdollBoneSymmetry>();
            symmetry.SetUpSymmetry(bone1, bone2);

            foreach (Transform bone1Child in bone1.transform)
            {
                foreach (Transform bone2Child in bone2.transform)
                {
                    if (bone1Child == bone2Child) continue;

                    if (!bone1Child.TryGetComponent(out RagdollBone checkBone1) ||
                        !bone2Child.TryGetComponent(out RagdollBone checkBone2))
                        continue;

                    EvaluateSymmetry(checkBone1, checkBone2, symmetricalParent);
                }
            }
        }

        private static float EvaluatePositionalSymmetry(Transform bone1, Transform bone2, Transform symmetricalParent)
        {
            Vector3 baseLossyScale = symmetricalParent.transform.lossyScale;

            Vector3 bone1RootLocalPos = symmetricalParent.transform.InverseTransformPoint(bone1.position);
            Vector3 bone2RootLocalPos = symmetricalParent.transform.InverseTransformPoint(bone2.position);

            Vector3 bone1AbsolutelocalPosition = GetAbsoluteVectorValue(bone1RootLocalPos.Multiply(baseLossyScale));
            Vector3 bone2AbsolutelocalPosition = GetAbsoluteVectorValue(bone2RootLocalPos.Multiply(baseLossyScale));

            return Vector3.Distance(bone1AbsolutelocalPosition, bone2AbsolutelocalPosition);
        }

        private static float EvaluateRotationalSymmetry(Transform bone1, Transform bone2)
        {
            Vector3 bone1AbsolutelocalRotation =
                GetAbsoluteVectorValue(NormaliseEulers(bone1.localRotation.eulerAngles));
            Vector3 bone2AbsolutelocalRotation =
                GetAbsoluteVectorValue(NormaliseEulers(bone2.localRotation.eulerAngles));

            float distance = Vector3.Distance(bone1AbsolutelocalRotation, bone2AbsolutelocalRotation);

            return distance;
        }

        private static Vector3 GetAbsoluteVectorValue(Vector3 input)
            => new Vector3(
                Mathf.Abs(input.x),
                Mathf.Abs(input.y),
                Mathf.Abs(input.z)
            );

        private static Vector3 NormaliseEulers(Vector3 input)
            => new Vector3(
                NormaliseAngle(input.x),
                NormaliseAngle(input.y),
                NormaliseAngle(input.z)
            );

        private static float NormaliseAngle(float angle)
        {
            // Adjust the angle by adding or subtracting 180 until it's between -180 and 180
            while (angle > 180f)
                angle -= 360f;

            while (angle < -180f)
                angle += 360f;

            return angle;
        }
    }
}