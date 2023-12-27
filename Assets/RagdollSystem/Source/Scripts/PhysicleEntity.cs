using Core;

namespace RagdollSystem
{
    public abstract class PhysicleEntity : Entity, IEntity
    {
        public GameComponets GameComponets { get; protected set; }

        public virtual void Construct(GameComponets gameComponets) => GameComponets = gameComponets;
    }
}
