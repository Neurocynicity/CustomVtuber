using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CustomRagdollCreator
{
    [ExecuteInEditMode]
    public class CustomRagdoll : MonoBehaviour
    {
        public string DisplayName;
        
        public List<RagdollBone> Bones { get; private set; }
        
        public float symmetryPosAllowance = 0.1f;
        public float symmetryRotAllowance = 20f;

        public List<CloseSymmetry> CloseSymmetries;

        public void SetUpCustomRagdoll(IEnumerable<RagdollBone> bones)
        {
            Bones = bones.ToList();
            
            foreach (var bone in Bones)
                bone.SetUpJoint();
        }

        public static CustomRagdoll GenerateCustomRagdoll(GameObject gameObject)
        {
            CustomRagdoll newRagdoll = gameObject.AddComponent<CustomRagdoll>();
            newRagdoll.DisplayName = gameObject.name;
            newRagdoll.GenerateBonesRecursive(gameObject.transform, null);
            newRagdoll.SetUpCustomRagdoll(gameObject.GetComponentsInChildren<RagdollBone>());
            return newRagdoll;
        }
        
        private void GenerateBonesRecursive(Transform transform, RagdollBone baseConnection)
        {
            // if we've reached the end then this is a marker for the end of a bone
            // instead of generating a new bone we tell the current baseConnection to
            // generate itself down the chain
            if (transform.childCount == 0)
            {
                RagdollBoneEnd.GenerateRagdollBoneEnd(transform, baseConnection);
                return;
            }

            RagdollBone createdBone;

            if (!baseConnection)
                createdBone = transform.AddComponent<RagdollBoneBase>();
            
            else
                createdBone = RagdollBone.GenerateRagdollBone(transform, baseConnection);
            
            for (int i = 0; i < transform.childCount; i++)
                GenerateBonesRecursive(transform.GetChild(i), createdBone);
            
        }
    }
}