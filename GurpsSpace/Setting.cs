using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GurpsSpace
{
    public class Setting : INotifyPropertyChanged
    {
        private string name;public string Name { get { return name; } }
        private int techLevel;
        public int TechLevel { get { return techLevel; } }
        private eSettingSocietyType societyType; public eSettingSocietyType SocietyType { get { return societyType; } }
        public int AverageIncome { get { return RuleBook.TechLevelParams[TechLevel].BaseIncome; } }
        private List<Planet> planets; public List<Planet> Planets { get { return planets; } }
        private List<Species> species; public List<Species> Species { get { return species; } }
        public Species MainSpecies { get { return species[0]; } }

        public string TechLevelDescription { get { return RuleBook.TechLevelParams[TechLevel].Description; } }

        public Setting()
        {
            name = "GURPS Space game";

            // choose TL randomly from 8 to 11, with 10 having a double chance
            techLevel = DiceBag.Rand(8, 12);
            if (techLevel == 12)
                techLevel = 10;

            // choose the society type randomly
            Array a = Enum.GetValues(typeof(eSettingSocietyType));
            int i = DiceBag.Rand(0, a.Length - 1);
            societyType = (eSettingSocietyType)a.GetValue(i);

            planets = new List<Planet>();
            species = new List<Species>();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
        }

        public void AddPlanet(Planet p)
        {
            planets.Add(p);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
        }
        public void ClearPlanets()
        {
            planets.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
        }

        public void AddSpecies(Species s)
        {
            species.Add(s);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
        }
        public void ClearSpecies()
        {
            species.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
