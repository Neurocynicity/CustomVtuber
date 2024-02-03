using Unity.VisualScripting;
using UnityEngine;

namespace CustomRagdollCreator
{
    public class RagdollBoneEnd : RagdollBone
    {
        public static void GenerateRagdollBoneEnd(Transform transform, RagdollBone connectionBase)
        {
            RagdollBoneEnd thisBoneEnd = transform.AddComponent<RagdollBoneEnd>();
            thisBoneEnd.connectionBase = connectionBase;
            connectionBase.RecursiveUnfoldGenerateBone(thisBoneEnd);
        }

        // Since we shouldn't have a rigidbody or collider of any kind then all we
        // do is send the recursive unfolding back down
        public override void RecursiveUnfoldGenerateBone(RagdollBone newConnectionEnd)
        {
            if (connectionBase)
                connectionBase.RecursiveUnfoldGenerateBone(this);
        }

        // bone ends don't need joints, so we hide the joint generating code here
        public override void SetUpJoint() { }

        public override void GenerateDefaultCapsule() { }
    }
}