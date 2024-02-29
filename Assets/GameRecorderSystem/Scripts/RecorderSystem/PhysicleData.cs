using System;
using UnityEngine;

namespace GameRecorder
{
    [Serializable]
    public struct PhysicleData
    {
        public PhysicleData(bool exist = false) : this() => Exist = exist;

        public bool Exist;

        public ulong Ticks;

        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public Vector2 VelocityRB;
        public float RotationRB;
        public float AngularDragRB;
        public float AngularVelocityRB;
    }
}
