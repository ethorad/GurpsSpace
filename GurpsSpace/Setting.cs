using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GurpsSpace
{
    public class Setting
    {
        private string name;
        public string Name { get { return name; } set { name = value; } }
        private int techLevel;
        public int TechLevel { get { return techLevel; } set { techLevel = value; } }
        private eSettingSocietyType societyType;
        public eSettingSocietyType SocietyType { get { return societyType; } set { societyType = value; } }
        public int AverageIncome { get { return RuleBook.TechLevelParams[TechLevel].BaseIncome; } }
        private List<Planet> planets;
        public List<Planet> Planets { get { return planets; } }
        private List<Species> species;
        public List<Species> Species { get { return species; } }
        private int mainSpeciesID;
        public Species MainSpecies { get { return species[mainSpeciesID]; } }


        public Setting()
        {
            name = "GURPS Space game";
            planets = new List<Planet>();
            species = new List<Species>();
            mainSpeciesID = 0;

            Randomise();
        }
        private void Randomise()
        {

            // choose TL randomly from 8 to 11, with 10 having a double chance
            techLevel = DiceBag.Rand(8, 12);
            if (techLevel == 12)
                techLevel = 10;

            // choose the society type randomly
            Array a = Enum.GetValues(typeof(eSettingSocietyType));
            int i = DiceBag.Rand(0, a.Length - 1);
            societyType = (eSettingSocietyType)(a.GetValue(i) ?? 0);
        }

        public void AddPlanet(Planet p)
        {
            planets.Add(p);
        }
        public void ClearPlanets()
        {
            planets.Clear();
        }

        public void AddSpecies(Species s)
        {
            species.Add(s);
        }
        public void ClearSpecies()
        {
            species.Clear();
        }

    }
}
