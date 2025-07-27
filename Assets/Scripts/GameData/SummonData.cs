namespace GameData
{
    [System.Serializable]
    public class SummonData
    {
        public string name;
        public int id;
        public bool selected;
        public bool locked;

        public SummonData(string name = "", int id = -1, bool selected = false, bool locked = false)
        {
            this.name = name;
            this.id = id;
            this.selected = selected;
            this.locked = locked;
        }
    }
}