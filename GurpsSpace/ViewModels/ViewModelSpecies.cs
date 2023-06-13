﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    public class ViewModelSpecies : ViewModel
    {
        private Species species;
        public Species Species { get { return species; } }
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
        public int? Consumption
        {
            get { return species.Consumption; }
            set { species.Consumption = value; MemberUpdated(); }
        }
        public bool? DoesNotEatOrDrink
        {
            get { return species.DoesNotEatOrDrink; }
            set
            {
                species.DoesNotEatOrDrink = value; MemberUpdated() ;
            }
        }
        public string ConsumptionString
        {
            get
            {
                if (species.Consumption == null || species.DoesNotEatOrDrink == null)
                    return "tbc";
                else if (species.DoesNotEatOrDrink ?? false)
                    return "Doesn't eat or drink";
                else if (species.ReducedConsumption > 0)
                    return "Reduced (" + species.ReducedConsumption.ToString() + ")";
                else if (species.IncreasedConsumption > 0)
                    return "Increased (" + species.IncreasedConsumption.ToString() + ")";
                else // so both zero
                    return "Normal";
            }
        }
        public string StartingColonyPopulationString
        {
            get
            {
                if (species.StartingColonyPopulation == null)
                    return "tbc";
                else
                    return species.StartingColonyPopulationValue.ToString("N0");
            }
        }
        public string AnnualGrowthRateString
        {
            get
            {
                if (species.AnnualGrowthRate == null)
                    return "tbc";
                else
                    return (species.AnnualGrowthRateValue * 100).ToString() + "%";
            }
        }
        public string AffinityMultiplierString
        {
            get
            {
                if (species.AffinityMultiplier == null)
                    return "tbc";
                else
                    return species.AffinityMultiplierValue.ToString("N2");
            }
        }

        public ViewModelSpecies(Species species)
        {
            this.species = species;
        }
    }
}
