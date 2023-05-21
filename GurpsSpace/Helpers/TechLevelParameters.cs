
namespace GurpsSpace
{
    public class TechLevelParameters
    {
        private int tL; public int TL { get { return tL; } }
        private string age; public string Age { get { return age; } }
        private string earthYears; public string EarthYears { get { return earthYears; } }

        private int baseIncome; public int BaseIncome { get { return baseIncome; } }
        private int baseCarryingCapacity; public int BaseCarryingCapacity { get { return baseCarryingCapacity; } }

        public string Description
        {
            get { return "TL "+TL.ToString()+" ("+Age+")"; }
        }

        public TechLevelParameters(int tL, string age, string earthYears, int baseIncome, int baseCarringCapacity)
        {
            this.tL = tL;
            this.age = age;
            this.earthYears = earthYears;
            this.baseIncome = baseIncome;
            this.baseCarryingCapacity = baseCarringCapacity;
        }
    }
}
