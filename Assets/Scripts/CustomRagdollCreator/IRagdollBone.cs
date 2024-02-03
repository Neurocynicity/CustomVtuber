namespace CustomRagdollCreator
{
    public interface IRagdollBone
    {
        public IRagdollBone ConnectionStart { get; set; }
        public IRagdollBone ConnectionEnd { get; set; }

        /// <summary>
        /// Called after all bones have been generated while "unfolding."
        /// After using recursion to add RagdollBones all the way to the end bone,
        /// this function is called to add all necessary components to them
        /// </summary>
        /// <param name="newConnectionEnd"></param>
        public void RecursiveUnfoldGenerateBone(RagdollBone newConnectionEnd);
        
        /// <summary>
        /// This will set up the CharacterJoint between bones, which is only used
        /// </summary>
        public void SetUpJoint();

        public void GenerateDefaultCapsule();
    }
}