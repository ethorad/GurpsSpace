using GurpsSpace.PlanetCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    public class SpeciesFactory
    {
        private Species species;
        public Species Species { get { return species; } }

        public Setting Setting { get { return species.Setting; } }

        private ISpeciesCreator randomiser;
        private ISpeciesCreator userInput;

        public string? Name { get { return species.Name; } set { species.Name = value; } }

        public eSpeciesDiet? Diet { get { return species.Diet; } }
        public double? StartingColonyPopulation { get { return species.StartingColonyPopulation; }  }
        public double? AnnualGrowthRate { get { return species.AnnualGrowthRate; }  }
        public double? AffinityMultiplier { get { return species.AffinityMultiplier; } } 

        public int? Consumption { get { return species.Consumption; } }
        public bool? DoesNotEatOrDrink { get { return species.DoesNotEatOrDrink; } }
        public int? ReducedConsumption { get { return species.ReducedConsumption; } }
        public int? IncreasedConsumption { get { return species.IncreasedConsumption; } }

        public eLifeChemistry? LifeChemistry { get { return species.LifeChemistry; } }

        internal SpeciesFactory(Setting s, ISpeciesCreator rnd, ISpeciesCreator usr)
        {
            species = new Species(s);
            randomiser = rnd;
            userInput = usr;
        }
        internal SpeciesFactory(Species s, ISpeciesCreator rnd, ISpeciesCreator usr)
        {
            species = s;
            randomiser = rnd;
            userInput = usr;
        }

        public void SelectParameter(string param)
        {
            SetParameter(param, userInput);
        }
        public void RandomParameter(string param)
        {
            SetParameter(param, randomiser);
        }

        public void FullRandom()
        {
            SetName(randomiser);
            SetDiet(randomiser);
            SetConsumption(randomiser);
            SetStartingColonyPopulation(randomiser);
            SetAnnualGrowthRate(randomiser);
            SetAffinityMultiplier(randomiser);
            SetLifeChemistry(randomiser);
        }

        private void SetParameter(string param, ISpeciesCreator sc)
        {
            switch (param)
            {
                case "Name":
                    SetName(sc);
                    break;
                case "Diet":
                    SetDiet(sc);
                    break;
                case "Consumption":
                    SetConsumption(sc);
                    break;
                case "StartingColonyPopulation":
                    SetStartingColonyPopulation(sc);
                    break;
                case "AnnualGrowthRate":
                    SetAnnualGrowthRate(sc);
                    break;
                case "AffinityMultiplier":
                    SetAffinityMultiplier(sc);
                    break;
                case "LifeChemistry":
                    SetLifeChemistry(sc);
                    break;
            }
        }

        private void SetName(ISpeciesCreator sc)
        {
            string? name = sc.GetName(this);
            if (name != null)
                species.Name = name;
        }

        private void SetDiet(ISpeciesCreator sc)
        {
            eSpeciesDiet? diet = sc.GetDiet(this);
            if (diet != null)
                species.Diet = diet;
        }

        private void SetConsumption(ISpeciesCreator sc)
        {
            int? consumption;
            bool? noEatOrDrink;
            (consumption, noEatOrDrink) = sc.GetConsumption(this);
            if (consumption != null && noEatOrDrink != null)
            {
                species.Consumption = consumption;
                species.DoesNotEatOrDrink = noEatOrDrink;
            }
        }

        private void SetStartingColonyPopulation(ISpeciesCreator sc)
        {
            double? colonyPop = sc.GetStartingColonyPopulation(this);
            if (colonyPop != null)
                species.StartingColonyPopulation = colonyPop;
        }

        private void SetAnnualGrowthRate(ISpeciesCreator sc)
        {
            double? growth = sc.GetAnnualGrowthRate(this);
            if (growth != null)
                species.AnnualGrowthRate = growth;
        }

        private void SetAffinityMultiplier(ISpeciesCreator sc)
        {
            double? mult = sc.GetAffinityMultiplier(this);
            if (mult != null)
                species.AffinityMultiplier = mult;
        }

        private void SetLifeChemistry(ISpeciesCreator sc)
        {
            eLifeChemistry? lifeChem = sc.GetLifeChemistry(this);
            if (lifeChem != null)
                species.LifeChemistry = lifeChem;
        }
    }
}