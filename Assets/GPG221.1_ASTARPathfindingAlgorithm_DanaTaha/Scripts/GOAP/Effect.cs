namespace GOAP.Effect
{
    /// <summary>
    /// The result of a completed Action.
    /// </summary>
    [System.Serializable]
    public class Effect
    {
        public string key;
        public bool value;

        public Effect(string key, bool value = true)
        {
            this.key = key;
            this.value = value;
        }
    }
}