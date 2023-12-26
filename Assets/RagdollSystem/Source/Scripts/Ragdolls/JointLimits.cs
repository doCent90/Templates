using System;
using System.Collections.Generic;
using UnityEngine;

namespace RagdollSystem
{
    [CreateAssetMenu(fileName = "JointLimits", menuName = "SO/JointLimits")]
    public class JointLimits : ScriptableObject
    {
        public List<JointLimit> Limits;
    }

    [Serializable]
    public class JointLimit
    {
        public CharacterBoneType Type;
        public bool FixedJoint;
        public bool UseLimit;
        public Vector2 Value;
    }
}