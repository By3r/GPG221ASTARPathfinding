namespace GOAP.Requisites
{
    /// <summary>
    /// Requirements to execute an action.
    /// </summary>
    [System.Serializable]
    public class Requisite
    {
        public string key;
        public bool value;

        public Requisite(string key, bool value)
        {
            this.key = key;
            this.value = value;
        }
    }
}