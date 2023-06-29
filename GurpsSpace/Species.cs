using System;
using System.ComponentModel;

namespace GurpsSpace
{
    public class Species
    {
        protected Setting setting;
        public Setting Setting { get { return setting; } set { setting = value; } }

        protected string? name;
        public string? Name { get { return name; } set { name = value; } }
        protected string? description;
        public string? Description { get { return description; } set { description = value; } }
        protected eSpeciesDiet? diet;
        public eSpeciesDiet? Diet { get { return diet; } set { diet = value; } }
        protected int? consumption;
        public int? Consumption
        { 
            get { return consumption; }
            set 
            {
                consumption = value;
                if (value == null) // if this is null, also null out doesn't eat or drink
                    doesNotEatOrDrink = null;
                else if (value != 0) // if this is non-zero, then does not eat or drink has to be false
                    doesNotEatOrDrink = false;
            }

        }
        public int? IncreasedConsumption
        { 
            get 
            { 
                if (consumption == null) 
                    return null;
                if (consumption > 0)
                    return consumption;
                return 0;
            }
            set { consumption = value; }
        }
        public int? ReducedConsumption
        {
            get
            {
                if (consumption == null)
                    return null;
                if (consumption < 0)
                    return -consumption;
                return 0;
            }
            set { consumption = -value; }
        }
        protected bool? doesNotEatOrDrink;
        public bool? DoesNotEatOrDrink 
        { 
            get { return doesNotEatOrDrink; }
            set
            {
                doesNotEatOrDrink = value;
                if (value == null)
                    consumption = null; // if this is null, also null out consumption
                if (value == true)
                    consumption = 0; // if they don't eat or drink, remove any levels of consumption
            }
        }
        protected double? startingColonyPopulation;
        public double? StartingColonyPopulation
        { 
            get { return startingColonyPopulation; }
            set { startingColonyPopulation = value; }
        }
        public double StartingColonyPopulationValue { get { return startingColonyPopulation ?? 0; } }
        protected double? annualGrowthRate;
        public double? AnnualGrowthRate 
        { 
            get { return annualGrowthRate; }
            set { annualGrowthRate = value;}
        }
        public double AnnualGrowthRateValue { get { return annualGrowthRate ?? 0; } }
        protected double? affinityMultiplier;
        public double? AffinityMultiplier
        { 
            get { return affinityMultiplier; }
            set {  affinityMultiplier = value;}
        }
        public double AffinityMultiplierValue { get { return affinityMultiplier ?? 1; } }


        public Species(Setting setting)
        {
            this.setting = setting;
        }
        public Species(Setting setting, string name, string description,
            eSpeciesDiet diet, int consumption, bool doesNotEatOrDrink,
            long startingColonyPopulation, double annualGrowthRate, double affinityMultiplier)
        {
            this.setting = setting;
            this.name = name;
            this.description = description;
            this.diet = diet;
            this.consumption = consumption;
            this.doesNotEatOrDrink = doesNotEatOrDrink;
            this.startingColonyPopulation = startingColonyPopulation;
            this.annualGrowthRate = annualGrowthRate;
            this.affinityMultiplier = affinityMultiplier;
        }
        public Species(Setting s, string name) : this(s, name, "A generic species",
            eSpeciesDiet.Omnivore, 0, false,
            10000, 0.023, 2)
        { }
        public Species(Setting s, string name, string description) : this(s, name, description,
            eSpeciesDiet.Omnivore, 0, false,
            10000, 0.023, 2)
        { }

        // disabling this check as the non-nullable fields all get set in the various set properties
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Species(Species s)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            // creates a copy of s
            Setting = s.Setting; // don't need to copy this since not editing the setting
            Name = s.Name;
            Description = s.Description;
            Diet = s.Diet;
            Consumption = s.Consumption;
            DoesNotEatOrDrink = s.DoesNotEatOrDrink;
            StartingColonyPopulation = s.StartingColonyPopulation;
            AnnualGrowthRate = s.AnnualGrowthRate;
            AffinityMultiplier = s.AffinityMultiplier;
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


        public virtual double CarryingCapacity(Planet p)
        {
            if (p.Habitability <= 3 && p.LocalTechLevel <= 7)
                return 0;
            else
                return DefaultCarryingCapacity(p);
        }
        protected long DefaultCarryingCapacity(Planet p)
        {
            double defaultCarryCap = CarryingCapacityBase(p)
                * CarryingCapacityMultiplier(p)
                * CarryingCapacityPlanetSizeModifier(p)
                * CarryingCapacityDietModifier(p)
                * CarryingCapacityIncreasedConsumptionModifier()
                * CarryingCapacityReducedConsumptionModifier()
                * CarryingCapacityDoesNotEatOrDrinkModifier();

            long carryCap = (long)defaultCarryCap;

            carryCap = RuleBook.RoundToSignificantFigures(carryCap, 2);

            return carryCap;
        }
        protected double CarryingCapacityBase(Planet p)
        {
            return RuleBook.TechLevelParams[(p.LocalTechLevel ?? 0)].BaseCarryingCapacity;
        }
        protected double CarryingCapacityMultiplier(Planet p)
        {
            return RuleBook.CarryingCapacityMultiplierByAffinity[(p.AffinityScore ?? 0)];
        }
        protected double CarryingCapacityPlanetSizeModifier(Planet p)
        {
            return (p.Size == eSize.AsteroidBelt) ? 50 : ((p.DiameterEarths * p.DiameterEarths) ?? 1);
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
            double power = IncreasedConsumption ?? 0;
            return Math.Pow(2, -power);
        }
        protected double CarryingCapacityReducedConsumptionModifier()
        {
            if (DoesNotEatOrDrink ?? false)
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
            if (DoesNotEatOrDrink ?? false)
                return 10;
            else
                return 1;
        }

    }
}
