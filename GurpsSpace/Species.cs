using System;
using System.ComponentModel;

namespace GurpsSpace
{
    public class Species : INotifyPropertyChanged
    {
        protected string name;
        public string Name { get { return name; } }
        private string description; public string Description { get { return description; } }
        protected Setting setting; public Setting Setting { get { return setting; } }
        protected eSpeciesDiet diet; public eSpeciesDiet Diet { get { return diet; } }
        protected int increasedConsumption; public int IncreasedConsumption { get { return increasedConsumption; } }
        protected int reducedConsumption; public int ReducedConsumption { get { return reducedConsumption; } }
        protected bool doesNotEatOrDrink; public bool DoesNotEatOrDrink { get { return doesNotEatOrDrink; } }
        protected ulong startingColonyPopulation; public ulong StartingColonyPopulation { get { return startingColonyPopulation; } }
        protected double annualGrowthRate; public double AnnualGrowthRate { get { return annualGrowthRate; } }
        protected double affinityMultiplier; public double AffinityMultiplier { get { return affinityMultiplier; } }


        public Species(Setting setting, string name, string description,
            eSpeciesDiet diet, int increasedConsumption, int reducedConsumption, bool doesNotEatOrDrink,
            ulong startingColonyPopulation, double annualGrowthRate, double affinityMultiplier)
        {
            this.setting = setting;
            this.name = name;
            this.description = description;
            this.diet = diet;
            this.increasedConsumption = increasedConsumption;
            this.reducedConsumption = reducedConsumption;
            this.doesNotEatOrDrink = doesNotEatOrDrink;
            this.startingColonyPopulation = startingColonyPopulation;
            this.annualGrowthRate = annualGrowthRate;
            this.affinityMultiplier = affinityMultiplier;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
        }
        public Species(Setting s, string name) : this(s, name, "A generic species",
            eSpeciesDiet.Omnivore, 0, 0, false,
            10000, 0.023, 2)
        {

        }
        public Species(Setting s, string name, string description) : this(s, name, description,
            eSpeciesDiet.Omnivore, 0, 0, false,
            10000, 0.023, 2)
        {

        }

        public virtual int Habitability(Planet p)
        {
            return BaseHabitability(p);
        }
        protected int BaseHabitability(Planet p)
        {
            int hab = 0;

            // atmospheric conditions
            if (p.AtmosphericPressureCategory == ePressureCategory.None || p.AtmosphericPressureCategory == ePressureCategory.Trace)
                hab += 0;
            if (p.AtmosphericPressureCategory != ePressureCategory.None && p.AtmosphericPressureCategory != ePressureCategory.Trace)
            {
                if (p.IsSuffocating && p.IsToxic && p.IsCorrosive)
                    hab += -2;
                if (p.IsSuffocating && p.IsToxic && !p.IsCorrosive)
                    hab += -1;
                if (p.IsSuffocating && !p.IsToxic && !p.IsCorrosive)
                    hab += 0;
            }
            if (p.IsBreathable)
            {
                if (p.AtmosphericPressureCategory == ePressureCategory.VeryThin)
                    hab += 1;
                if (p.AtmosphericPressureCategory == ePressureCategory.Thin)
                    hab += 2;
                if (p.AtmosphericPressureCategory == ePressureCategory.Standard || p.AtmosphericPressureCategory == ePressureCategory.Dense)
                    hab += 3;
                if (p.AtmosphericPressureCategory == ePressureCategory.VeryDense || p.AtmosphericPressureCategory == ePressureCategory.Superdense)
                    hab += 1;
                if (!p.IsMarginal)
                    hab += 1;
            }

            // Hydrographics
            if (p.LiquidType != eLiquid.Water || p.HydrographicCoverage == 0)
                hab += 0;
            if (p.LiquidType == eLiquid.Water)
            {
                if (p.HydrographicCoverage <= 0.59)
                    hab += 1;
                else if (p.HydrographicCoverage <= 0.9)
                    hab += 2;
                else if (p.HydrographicCoverage <= 0.99)
                    hab += 1;
                else // 100%
                    hab += 0;
            }

            // Climate
            if (p.IsBreathable)
            {
                if (p.ClimateType == eClimateType.Frozen || p.ClimateType == eClimateType.VeryCold)
                    hab += 0;
                else if (p.ClimateType == eClimateType.Cold)
                    hab += 1;
                else if (p.ClimateType == eClimateType.Hot)
                    hab += 1;
                else if (p.ClimateType == eClimateType.VeryHot || p.ClimateType == eClimateType.Infernal)
                    hab += 0;
                else // chilly, cool, normal, warm, tropical
                    hab += 2;
            }

            return hab;
        }


        public virtual ulong CarryingCapacity(Planet p)
        {
            if (p.Habitability <= 3 && p.LocalTechLevel <= 7)
                return 0;
            else
                return DefaultCarryingCapacity(p);
        }
        protected ulong DefaultCarryingCapacity(Planet p)
        {
            double defaultCarryCap = CarryingCapacityBase(p)
                * CarryingCapacityMultiplier(p)
                * CarryingCapacityPlanetSizeModifier(p)
                * CarryingCapacityDietModifier(p)
                * CarryingCapacityIncreasedConsumptionModifier()
                * CarryingCapacityReducedConsumptionModifier()
                * CarryingCapacityDoesNotEatOrDrinkModifier();

            defaultCarryCap = RuleBook.RoundToSignificantFigures(defaultCarryCap,2);

            return (ulong)defaultCarryCap;
        }
        protected double CarryingCapacityBase(Planet p)
        {
            return RuleBook.TechLevelParams[p.LocalTechLevel].BaseCarryingCapacity;
        }
        protected double CarryingCapacityMultiplier(Planet p)
        {
            return RuleBook.CarryingCapacityMultiplierByAffinity[p.AffinityScore];
        }
        protected double CarryingCapacityPlanetSizeModifier(Planet p)
        {
            return (p.Size == eSize.AsteroidBelt) ? 50 : (p.DiameterEarths * p.DiameterEarths);
        }
        protected double CarryingCapacityDietModifier(Planet p)
        {
            if (Diet == eSpeciesDiet.Carnivore && p.LocalTechLevel <= 8)
                return 0.1;
            else
                return 1;
        }
        protected double CarryingCapacityIncreasedConsumptionModifier()
        {
            return Math.Pow(2, -IncreasedConsumption);
        }
        protected double CarryingCapacityReducedConsumptionModifier()
        {
            if (DoesNotEatOrDrink)
                return 1; // the modifier for doesn't eat or drink supersedes this
            else if (ReducedConsumption == 0)
                return 1;
            else if (ReducedConsumption == 1)
                return 1.5;
            else if (ReducedConsumption == 2)
                return 3;
            else if (ReducedConsumption >= 3)
                return 10;
            else
                return 1;
        }
        protected double CarryingCapacityDoesNotEatOrDrinkModifier()
        {
            if (DoesNotEatOrDrink)
                return 10;
            else
                return 1;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
