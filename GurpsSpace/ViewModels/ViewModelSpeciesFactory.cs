using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    public class ViewModelSpeciesFactory : ViewModel
    {
        private SpeciesFactory speciesFactory;

        public override string SummaryType { get { return speciesFactory.Name ?? "tbc"; } }
        public ViewModelSpeciesFactory(SpeciesFactory speciesFactory)
        {
            this.speciesFactory = speciesFactory;
        }

        public string Name
        { 
            get { return speciesFactory.Name ?? "tbc"; }
            set 
            { 
                speciesFactory.Name = value;
                MemberUpdated();
            }
        }
        public string DietString
        {
            get
            {
                if (speciesFactory.Diet == null)
                    return "tbc";
                else
                    return speciesFactory.Diet.ToString()!;
            }
        }
        public string ConsumptionString
        {
            get
            {
                if (speciesFactory.Consumption == null || speciesFactory.DoesNotEatOrDrink == null)
                    return "tbc";
                else if (speciesFactory.DoesNotEatOrDrink ?? false)
                    return "Doesn't eat or drink";
                else if (speciesFactory.ReducedConsumption > 0)
                    return "Reduced (" + speciesFactory.ReducedConsumption.ToString() + ")";
                else if (speciesFactory.IncreasedConsumption > 0)
                    return "Increased (" + speciesFactory.IncreasedConsumption.ToString() + ")";
                else // so both zero
                    return "Normal";
            }
        }
        public string StartingColonyPopulationString
        {
            get
            {
                if (speciesFactory.StartingColonyPopulation == null)
                    return "tbc";
                else
                    return (speciesFactory.StartingColonyPopulation ?? 0).ToString("N0");
            }
        }
        public string AnnualGrowthRateString
        {
            get
            {
                if (speciesFactory.AnnualGrowthRate == null)
                    return "tbc";
                else
                    return ((speciesFactory.AnnualGrowthRate ?? 0) * 100).ToString("N1") + "%";
            }
        }
        public string AffinityMultiplierString
        {
            get
            {
                if (speciesFactory.AffinityMultiplier == null)
                    return "tbc";
                else
                    return (speciesFactory.AffinityMultiplier ?? 0).ToString("N2");
            }
        }
        public string LifeChemistryString
        {
            get
            {
                if (speciesFactory.LifeChemistry == null)
                    return "tbc";
                else
                    return speciesFactory.LifeChemistry.ToString()!;
            }
        }

        public void RandomParameter(string param)
        {
            speciesFactory.RandomParameter(param);
            MemberUpdated();
        }
        public void SelectParameter(string param)
        {
            speciesFactory.SelectParameter(param);
            MemberUpdated();
        }
        public void FullRandom()
        {
            speciesFactory.FullRandom();
            MemberUpdated();
        }
    }
}
