using UnityEngine;

namespace Destructible2D
{
    public interface IDamagable
    {
        DestructableData DestructableData { get; }

        void ApplyDamage(int value, bool force = false, DamageType type = DamageType.None, Vector2 contact = default);

        void CriticalDamage(bool crit = false);
    }
}
