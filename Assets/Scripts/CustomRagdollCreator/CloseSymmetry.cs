using System;

namespace CustomRagdollCreator
{
    [Serializable]
    public struct CloseSymmetry : IEquatable<CloseSymmetry>
    {
        public RagdollBone Bone1, Bone2;
        public float RequiredPositionalAllowance, RequiredRotationalAllowance;

        public CloseSymmetry(RagdollBone bone1, RagdollBone bone2,
            float requiredPositionalAllowance, float requiredRotationalAllowance)
        {
            Bone1 = bone1;
            Bone2 = bone2;
            RequiredPositionalAllowance = requiredPositionalAllowance;
            RequiredRotationalAllowance = requiredRotationalAllowance;
        }

        public bool Equals(CloseSymmetry other)
        {
            return (Equals(Bone1, other.Bone1) || Equals(Bone1, other.Bone2)) &&
                   (Equals(Bone2, other.Bone1) || Equals(Bone2, other.Bone2));
        }

        public override bool Equals(object obj)
        {
            return obj is CloseSymmetry other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Bone1, Bone2, RequiredPositionalAllowance, RequiredRotationalAllowance);
        }
    }
}