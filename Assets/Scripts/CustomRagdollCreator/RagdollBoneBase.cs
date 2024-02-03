namespace CustomRagdollCreator
{
    public class RagdollBoneBase : RagdollBone
    {
        // we want the base to have a rigidbody and no capsule collider,
        // so instead of setting them both up here we just add the rigidbody
        public override void RecursiveUnfoldGenerateBone(RagdollBone newConnectionEnd)
        {
            connectionEnd = newConnectionEnd;
            gameObject.EnsureAttachedComponent(ref rigidbody);
        }

        // we don't want the root bone to have a joint, so we don't do anything here
        public override void SetUpJoint() { }

        public override void GenerateDefaultCapsule() { }
    }
}