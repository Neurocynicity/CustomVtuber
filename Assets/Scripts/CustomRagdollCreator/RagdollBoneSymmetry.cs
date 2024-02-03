using UnityEngine;

namespace CustomRagdollCreator
{
    public class RagdollBoneSymmetry : MonoBehaviour
    {
        public RagdollBone symmetricalBone1, symmetricalBone2;

        public void SetUpSymmetry(RagdollBone bone1, RagdollBone bone2)
        {
            symmetricalBone1 = bone1;
            symmetricalBone2 = bone2;
        }

        public void ApplySymmetry(bool bone2Onto1)
        {
            CapsuleCollider capsuleToChange, correctCapsule;

            if (bone2Onto1)
            {
                correctCapsule = symmetricalBone2.capsuleCollider;
                capsuleToChange = symmetricalBone1.capsuleCollider;
            }
            else
            {
                correctCapsule = symmetricalBone1.capsuleCollider;
                capsuleToChange = symmetricalBone2.capsuleCollider;
            }

            capsuleToChange.height = correctCapsule.height;
            capsuleToChange.center = correctCapsule.center;
            capsuleToChange.radius = correctCapsule.radius;
        }

        public static void CreateSymmetry(RagdollBone bone1, RagdollBone bone2)
        {
            RagdollBoneSymmetry symmetry1 = bone1.gameObject.AddComponent<RagdollBoneSymmetry>();
            RagdollBoneSymmetry symmetry2 = bone2.gameObject.AddComponent<RagdollBoneSymmetry>();
            
            symmetry1.SetUpSymmetry(bone1, bone2);
            symmetry2.SetUpSymmetry(bone2, bone1);
        }
    }
}