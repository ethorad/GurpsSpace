using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media.Animation;

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

        protected double? startingColonyPopulation;
        public double? StartingColonyPopulation
        { 
            get { return startingColonyPopulation; }
            set { startingColonyPopulation = value; }
        }
        protected double? annualGrowthRate;
        public double? AnnualGrowthRate 
        { 
            get { return annualGrowthRate; }
            set { annualGrowthRate = value;}
        }
        protected double? affinityMultiplier;
        public double? AffinityMultiplier
        { 
            get { return affinityMultiplier; }
            set {  affinityMultiplier = value;}
        }
        protected eLifeChemistry? lifeChemistry;
        public eLifeChemistry? LifeChemistry 
        { 
            get { return lifeChemistry; }
            set { lifeChemistry = value; }
        }

        public List<Trait> Traits;

        public Species(Setting setting)
        {
            this.setting = setting;
            Traits = new List<Trait>();
        }
        public Species(Setting setting, string name, string description,
            eSpeciesDiet diet,
            long startingColonyPopulation, double annualGrowthRate, double affinityMultiplier)
        {
            this.setting = setting;
            this.name = name;
            this.description = description;
            this.diet = diet;
            this.startingColonyPopulation = startingColonyPopulation;
            this.annualGrowthRate = annualGrowthRate;
            this.affinityMultiplier = affinityMultiplier;
            Traits = new List<Trait>();
        }
        public Species(Setting s, string name) : this(s, name, "A generic species",
            eSpeciesDiet.Omnivore,
            10000, 0.023, 2)
        { }
        public Species(Setting s, string name, string description) : this(s, name, description,
            eSpeciesDiet.Omnivore,
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
            StartingColonyPopulation = s.StartingColonyPopulation;
            AnnualGrowthRate = s.AnnualGrowthRate;
            AffinityMultiplier = s.AffinityMultiplier;
            LifeChemistry = s.LifeChemistry;

            Traits = new List<Trait>();
            foreach (Trait trait in s.Traits)
                Traits.Add(new Trait(trait));
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

        public virtual double? CarryingCapacity(Planet p)
        {
            if (p.Habitability == null || p.LocalTechLevel == null)
                return null;

            if (p.Habitability <= 3 && p.LocalTechLevel <= 7)
                return 0;
            else
                return DefaultCarryingCapacity(p);
        }
        protected long DefaultCarryingCapacity(Planet p)
        {
            double defaultCarryCap = CarryingCapacityBase(p);
            defaultCarryCap *= CarryingCapacityMultiplier(p);
            defaultCarryCap *= CarryingCapacityPlanetSizeModifier(p);
            defaultCarryCap *= CarryingCapacityDietModifier(p);
            defaultCarryCap *= CarryingCapacityIncreasedConsumptionModifier();
            defaultCarryCap *= CarryingCapacityReducedConsumptionModifier();
            defaultCarryCap *= CarryingCapacityDoesNotEatOrDrinkModifier();

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
            double power = GetTraitLevel(eTrait.IncreasedConsumption);
            return Math.Pow(2, -power);
        }
        protected double CarryingCapacityReducedConsumptionModifier()
        {
            int redCon = GetTraitLevel(eTrait.ReducedConsumption);

            if (HasTrait(eTrait.DoesntEatOrDrink))
                return 1; // the modifier for doesn't eat or drink supersedes this
            else if (redCon == 0)
                return 1;
            else if (redCon == 1)
                return 1.5;
            else if (redCon == 2)
                return 3;
            else if (redCon >= 3)
                return 10;
            else
                return 1;
        }
        protected double CarryingCapacityDoesNotEatOrDrinkModifier()
        {
            if (HasTrait(eTrait.DoesntEatOrDrink))
                return 10;
            else
                return 1;
        }

        public bool HasTrait(eTrait trait)
        {
            foreach (Trait t in Traits)
                if (t.TraitType == trait)
                    return true;

            return false;
        }
        public int GetTraitLevel(eTrait trait)
        {
            foreach (Trait t in Traits)
                if (t.TraitType==trait)
                    return t.Level;
            return 0;
        }
        public void RemoveTrait(eTrait traitToRemove)
        {
            for (int i=Traits.Count-1; i>=0; i--)
                if (Traits[i].TraitType== traitToRemove)
                    Traits.RemoveAt(i);
        }
        public Trait AddTrait(eTrait traitToAdd)
        {
            // remove the trait if it already exists
            RemoveTrait(traitToAdd);

            // then add it
            Trait t = new Trait(traitToAdd, "");
            Traits.Add(t);

            // remove any banned traits
            foreach (eTrait bannedTrait in RuleBook.TraitParams[traitToAdd].BannedTraits)
                RemoveTrait(bannedTrait);

            // return the added trait so it can be further amended
            return t;
        }
        public Trait AddTrait(eTrait traitToAdd, int traitLevel)
        {
            Trait t = AddTrait(traitToAdd);
            t.Level = traitLevel;
            return t;
        }

    }
}
