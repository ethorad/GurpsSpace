
namespace GurpsSpace
{
    public class TechLevelParameters
    {
        private int tL; public int TL { get { return tL; } }
        private string age; public string Age { get { return age; } }

        private bool isDelayed;
        public bool IsDelayed
        {
            get { return isDelayed; }
            set
            {
                isDelayed = value;
                if (isDelayed)
                    isAdvanced = false;
            }
        }
        private bool isAdvanced;
        public bool IsAdvanced
        {
            get { return isAdvanced; }
            set
            {
                isAdvanced = value;
                if (isAdvanced)
                    isDelayed = false;
            }
        }

        private int baseIncome; public int BaseIncome { get { return baseIncome; } }
        private int baseCarryingCapacity; public int BaseCarryingCapacity { get { return baseCarryingCapacity; } }

        public TechLevelParameters(int tL, string age, int baseIncome, int baseCarringCapacity)
        {
            this.tL = tL;
            this.age = age;
            this.baseIncome = baseIncome;
            this.baseCarryingCapacity = baseCarringCapacity;
        }

        public TechLevelParameters Copy()
        {
            TechLevelParameters tlp = new TechLevelParameters(TL, Age, BaseIncome, BaseCarryingCapacity);
            tlp.IsDelayed = isDelayed;
            tlp.IsAdvanced = isAdvanced;
            return tlp;
        }
    }
}
