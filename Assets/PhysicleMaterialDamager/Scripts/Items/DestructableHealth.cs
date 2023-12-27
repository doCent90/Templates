namespace PhysicleMaterialsDamager
{
    internal class DestructableHealth
    {
        private readonly DestructableSplitter _destructableSplitter;
        private readonly float _maxHealth;
        private float _health;

        public DestructableHealth(float maxHealth, DestructableSplitter destructableSplitter)
        {
            _destructableSplitter = destructableSplitter;
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        public bool IsAlive => _health > 0;

        public void TakeDamage(float value)
        {
            if (IsAlive == false) return;

            _health -= value;

            if (_health <= 0)
                _destructableSplitter.Broke();
        }

        public void OnBroken() => _health = 0;
    }
}
