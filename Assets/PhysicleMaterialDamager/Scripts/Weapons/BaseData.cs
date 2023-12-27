using System;
using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public abstract class BaseData : ScriptableObject
    {
        [SerializeField] internal string Id;
        [SerializeField] internal GameObject Prefab;
        [SerializeField] internal Sprite Image;
        [SerializeField] internal string Name;
        [SerializeField] internal AudioClip CrashAudio;

        public string GUID => Id;

        internal BaseData() => Id = Guid.NewGuid().ToString();

        [ContextMenu(nameof(RegenerateId))]
        public void RegenerateId() => Id = Guid.NewGuid().ToString();
    }
}
