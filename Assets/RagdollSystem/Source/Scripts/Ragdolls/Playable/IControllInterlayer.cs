namespace RagdollSystem
{
    public interface IControllInterlayer
    {
        bool UnderControll { get; }

        void StartControll();
        void StopControll();
    }
}
