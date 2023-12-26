namespace RagdollSystem
{
    public class InputPreset
    {
        public BaseInputConfig InputConfig { get; private set; }

        public InputPreset(BaseInputConfig inputConfig) => InputConfig = inputConfig;
    }
}