using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    public class ViewModelSpecies : ViewModel
    {
        public override string SummaryType { get { return Name ?? "tbc"; } }

        private Species species;
        public Species Species
        { 
            get { return species; }
            set { species = value; MemberUpdated(); }
        }
        public string? Name
        {
            get { return species.Name ?? "tbc"; }
            set
            {
                species.Name = value;
                MemberUpdated();
            }
        }
        public string? Description
        {
            get { return species.Description ?? "tbc"; }
            set
            {
                species.Description = value;
                MemberUpdated();
            }
        }
        public string DietString
        {
            get
            {
                if (species.Diet == null)
                    return "tbc";
                else
                    return species.Diet.ToString()!;
            }
        }
        public eSpeciesDiet? Diet
        {
            get { return species.Diet; }
            set
            {
                species.Diet = value;
                MemberUpdated();
            }
        }
        public int Consumption
        {
            get { return species.GetTraitLevel(eTrait.IncreasedConsumption)-species.GetTraitLevel(eTrait.ReducedConsumption); }
            set 
            {
                if (value > 0)
                {
                    species.AddTrait(eTrait.IncreasedConsumption, value);
                }
                else if (value<0)
                {
                    species.AddTrait(eTrait.ReducedConsumption, -value);
                }
                else // value == 0
                {
                    species.RemoveTrait(eTrait.IncreasedConsumption);
                    species.RemoveTrait(eTrait.ReducedConsumption);
                    species.RemoveTrait(eTrait.DoesntEatOrDrink);
                }
                MemberUpdated();
            }
        }
        public bool DoesNotEatOrDrink
        {
            get { return species.HasTrait(eTrait.DoesntEatOrDrink); }
        }
        public string ConsumptionString
        {
            get
            {
                if (species.HasTrait(eTrait.DoesntEatOrDrink))
                    return "Doesn't eat or drink";
                else if (species.HasTrait(eTrait.ReducedConsumption))
                    return "Reduced (" + species.GetTraitLevel(eTrait.ReducedConsumption).ToString() + ")";
                else if (species.HasTrait(eTrait.IncreasedConsumption))
                    return "Increased (" + species.GetTraitLevel(eTrait.IncreasedConsumption).ToString() + ")";
                else // so both zero
                    return "Normal";
            }
        }
        public double? StartingColonyPopulation
        {
            get { return species.StartingColonyPopulation; }
            set { species.StartingColonyPopulation = value; MemberUpdated() ; }
        }
        public string StartingColonyPopulationString
        {
            get
            {
                if (species.StartingColonyPopulation == null)
                    return "tbc";
                else
                    return (species.StartingColonyPopulation ?? 0).ToString("N0");
            }
        }
        public double? AnnualGrowthRate
        {
            get { return species.AnnualGrowthRate; }
            set { species.AnnualGrowthRate = value; MemberUpdated(); }
        }
        public string AnnualGrowthRateString
        {
            get
            {
                if (species.AnnualGrowthRate == null)
                    return "tbc";
                else
                    return ((species.StartingColonyPopulation ?? 0) * 100).ToString("N1") + "%";
            }
        }
        public double? AffinityMultiplier
        {
            get { return species.AffinityMultiplier; }
            set { Species.AffinityMultiplier = value; MemberUpdated(); }
        }
        public string AffinityMultiplierString
        {
            get
            {
                if (species.AffinityMultiplier == null)
                    return "tbc";
                else
                    return (species.StartingColonyPopulation ?? 0).ToString("N2");
            }
        }

        public ViewModelSpecies(Species species)
        {
            this.species = species;
        }
    }
}
