using GurpsSpace.PlanetCreation;
using System;
using System.Collections.Generic;

namespace GurpsSpace
{
    public class Planet
    {
        private Setting setting;
        public Setting Setting { get { return setting; } set { setting = value; } }

        private PlanetParameters? parameters;
        private string? name;
        public string? Name { get { return name; } set { name = value; } }
        public eOverallType? OverallType { get { return (parameters == null) ? null : parameters.OverallType; } }
        private string? description;
        public string? Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }
        private eSize? size; 
        public eSize SizeVal { get { return size ?? eSize.None; } }
        public eSize? Size
        {
            get { return size; }
            set
            {
                bool change = (size != value);
                size = value;
                if (change)
                    PlanetTypeChanged();
            }
        }
        private eSubtype? subtype;
        public eSubtype SubtypeVal { get { return subtype?? eSubtype.None; } }
        public eSubtype? Subtype
        {
            get { return subtype; }
            set
            {
                bool change = (subtype != value);
                subtype = value;
                if (change)
                    PlanetTypeChanged();
            }
        }

        public bool? IsPlanet
        {
            get
            {
                if (Size == null)
                    return null;
                else if (Size == eSize.None || Size == eSize.AsteroidBelt)
                    return false;
                else
                    return true;
            }
        }

        public bool? HasAtmosphere { get { return (parameters == null) ? null : parameters.HasAtmosphere; } }
        private double? atmosphericMass;
        public double? AtmosphericMass
        {
            get { return atmosphericMass; }
            set
            {
                if (value < 0)
                    value = 0;
                atmosphericMass = value;
                CheckRanges();
            }
        }
        public bool? HasAtmosphericOptions { get { return (parameters == null) ? null : (parameters.AtmosphereANumber < 18); } }
        private fAtmosphericConditions? atmosphericConditions;
        public fAtmosphericConditions? AtmosphericConditions
        {
            get { return atmosphericConditions; }
            set
            {
                atmosphericConditions = value;
            }
        }
        private string? atmosphericDescription;
        public string? AtmosphericDescription { get { return atmosphericDescription; } set { atmosphericDescription = value; } }
        public bool IsSuffocating { get { return ((AtmosphericConditions & fAtmosphericConditions.Suffocating) == fAtmosphericConditions.Suffocating); } }
        public bool IsMildlyToxic { get { return ((AtmosphericConditions & fAtmosphericConditions.MildlyToxic) == fAtmosphericConditions.MildlyToxic); } }
        public bool IsHighlyToxic { get { return ((AtmosphericConditions & fAtmosphericConditions.HighlyToxic) == fAtmosphericConditions.HighlyToxic); } }
        public bool IsLethallyToxic { get { return ((AtmosphericConditions & fAtmosphericConditions.LethallyToxic) == fAtmosphericConditions.LethallyToxic); } }
        public bool IsToxic { get { return (IsMildlyToxic || IsHighlyToxic || IsLethallyToxic); } }
        public bool IsCorrosive { get { return ((AtmosphericConditions & fAtmosphericConditions.Corrosive) == fAtmosphericConditions.Corrosive); } }
        public bool IsMarginal { get { return ((AtmosphericConditions & fAtmosphericConditions.Marginal) == fAtmosphericConditions.Marginal); } }
        public bool IsBreathable { get { return ((AtmosphericMass > 0) && (!IsSuffocating) && (!IsToxic) && (!IsCorrosive)); } }
        
        public bool? HasLiquid { get { return (parameters == null) ? null : !(parameters.Liquid == eLiquid.None); } }
        public double? MinimumHydrographicCoverage { get { return (parameters == null) ? null : parameters.HydroMin; } }
        public double? MaximumHydrographicCoverage { get { return (parameters == null) ? null : parameters.HydroMax; } }
        private double? hydrographicCoverage;
        public double? HydrographicCoverage
        {
            get { return hydrographicCoverage; }
            set
            {
                if (value < 0) value = 0;
                if (value > 1) value = 1;
                hydrographicCoverage = value;
                CheckRanges();
            }
        }
        public eLiquid? LiquidType { get { return (parameters == null) ? null : parameters.Liquid; } }

        public int? MinSurfaceTemperatureK { get { return (parameters == null) ? null : parameters.MinSurfaceTemperatureK; } }
        public int? MaxSurfaceTemperatureK { get { return (parameters == null) ? null : parameters.MaxSurfaceTemperatureK; } }
        public int? StepSurfaceTemperatureK { get { return (parameters == null) ? null : parameters.StepSurfaceTemperatureK; } }
        private int? averageSurfaceTempK;
        public int? AverageSurfaceTempK
        {
            get { return averageSurfaceTempK; }
            set
            {
                averageSurfaceTempK = value;
                CheckRanges();
            }
        }
        public eClimateType? ClimateType
        {
            get
            {
                if (AverageSurfaceTempK == null)
                    return null;
                else
                    return RuleBook.ClimateType(AverageSurfaceTempK ?? 0); // OK to use ?? as we know it isn't null here
            }
        }
        public int? BlackbodyTempK
        {
            get
            {
                // check for values we need
                if (parameters == null || AtmosphericMass == null || AverageSurfaceTempK == null)
                    return null;
                // so can now use ?? on all nullable values

                double absorption = (parameters == null) ? 0 : parameters.BlackbodyAbsorption;
                double greenhouse = (parameters == null) ? 0 : parameters.BlackbodyGreenhouse;

                if (Subtype == eSubtype.Ocean || Subtype == eSubtype.Garden)
                {
                    if (HydrographicCoverage < 0.21)
                        absorption = 0.95;
                    else if (HydrographicCoverage < 0.51)
                        absorption = 0.92;
                    else if (HydrographicCoverage < 0.91)
                        absorption = 0.88;
                    else
                        absorption = 0.84;
                }

                double bbCorrection = absorption * (1 + (AtmosphericMass ?? 0) * greenhouse);
                double bbTemp = (AverageSurfaceTempK ?? 0) / bbCorrection;

                return ((int)Math.Round(bbTemp, 0));
            }
        }

        public eCoreType? CoreType { get { return (parameters == null) ? null : parameters.CoreType; } }
        public double? MinDensity
        {
            get
            {
                switch (CoreType ?? eCoreType.None) // so falls to default if null
                {
                    case eCoreType.Icy:
                        return RuleBook.DensityIcyCore[0];
                    case eCoreType.SmallIron:
                        return RuleBook.DensitySmallIronCore[0];
                    case eCoreType.LargeIron:
                        return RuleBook.DensityLargeIronCore[0];
                    default:
                        return null;
                }
            }
        }
        public double? MaxDensity
        {
            get
            {
                switch (CoreType ?? eCoreType.None) // so falls to default if null
                {
                    case eCoreType.Icy:
                        return RuleBook.DensityIcyCore[20];
                    case eCoreType.SmallIron:
                        return RuleBook.DensitySmallIronCore[20];
                    case eCoreType.LargeIron:
                        return RuleBook.DensityLargeIronCore[20];
                    default:
                        return 0;
                }
            }
        }
        private double? density;
        public double? Density
        {
            get { return density; }
            set
            {
                density = value;
                CheckRanges();
            }
        }
        public double? MinGravity
        {
            get
            {
                // check for values we need
                if (parameters == null || BlackbodyTempK == null || Density == null)
                    return null;
                // so can now use ?? on all nullable values

                double minSizeFactor = (parameters == null) ? 0 : parameters.MinSizeFactor;
                double minG = Math.Sqrt((double)(BlackbodyTempK ?? 0) * (Density ?? 0)) * minSizeFactor;
                return Math.Round(minG, 2);
            }
        }
        public double? MaxGravity
        {
            get
            {
                // check for values we need
                if (parameters == null || BlackbodyTempK == null || Density == null)
                    return null;
                // so can now use ?? on all nullable values

                double maxSizeFactor = (parameters == null) ? 0 : parameters.MaxSizeFactor;
                double maxG = Math.Sqrt((double)(BlackbodyTempK ?? 0) * (Density ?? 0)) * maxSizeFactor;
                return Math.Round(maxG, 2);
            }
        }
        private double? gravity;
        public double? Gravity
        {
            get { return gravity; }
            set
            {
                gravity = value;
                CheckRanges();
            }
        }
        public double? DiameterEarths
        {
            get
            {
                // check for values we need
                if (Density == null || Gravity == null)
                    return null;
                // so can now use ?? on all nullable values

                else if (Density == 0)
                    return 0;
                else
                    return Math.Round((Gravity ?? 0) / (Density ?? 0), 2);
            }
        }
        public double? DiameterMiles 
        {
            get
            {
                if (DiameterEarths == null)
                    return null;
                else
                    return (DiameterEarths ?? 0) * RuleBook.EarthDiameterInMiles;
            } 
        }
        public double? Mass 
        { 
            get 
            {
                if (Density == null || DiameterEarths == null)
                    return null;
                else
                    return (Density ?? 0) * (DiameterEarths ?? 0) * (DiameterEarths ?? 0) * (DiameterEarths ?? 0); 
            }
        }

        public double? AtmosphericPressure
        {
            get
            {
                if (parameters == null || AtmosphericMass == null || Gravity == null)
                    return null;
                else
                {
                    double pressureFac = (parameters == null) ? 0 : parameters.PressureFactor;
                    return (AtmosphericMass ?? 0) * pressureFac * (Gravity ?? 0);
                }
            }
        }
        public ePressureCategory? AtmosphericPressureCategory 
        {
            get
            {
                if (AtmosphericPressure == null)
                    return null;
                else
                    return RuleBook.PressureCategory((AtmosphericPressure ?? 0));
            }
        }

        private Species? localSpecies;
        public Species? LocalSpecies { get { return localSpecies; } set { localSpecies = value; } }

        private int? localTechLevel;
        public int? LocalTechLevel
        {
            get { return localTechLevel; }
            set
            {
                localTechLevel = value;
                localTechLevelRelativity = eTechLevelRelativity.Normal;
            }
        }
        public string? LocalTechLevelAge 
        { 
            get 
            {
                if (LocalTechLevel == null)
                    return null;
                else
                    return RuleBook.TechLevelParams[(LocalTechLevel ?? 0)].Age; 
            } 
        }
        private eTechLevelRelativity? localTechLevelRelativity;
        public eTechLevelRelativity? LocalTechLevelRelativity
        {
            get { return localTechLevelRelativity; }
            set { localTechLevelRelativity = value;}
        }
        private eResourceValueCategory? resourceValueCategory;
        public eResourceValueCategory? ResourceValueCategory { get { return resourceValueCategory; } set { resourceValueCategory = value; } }
        public int? ResourceValueModifier 
        {
            get
            {
                if (ResourceValueCategory == null)
                    return null;
                else
                    return (int)ResourceValueCategory;
            } 
        }
        public int? Habitability
        {
            get
            {
                if (LocalSpecies == null)
                    return null;
                else
                    return LocalSpecies.Habitability(this);
            } 
        }
        public int? AffinityScore
        {
            get
            {
                if (ResourceValueModifier == null || Habitability == null)
                    return null;
                else
                    return ResourceValueModifier + Habitability;
            }
        }

        private eSettlementType? settlementType;
        public eSettlementType? SettlementType
        {
            get { return settlementType; }
            set
            {
                bool change = (settlementType != value);
                settlementType = value;
                if (change)
                    SettlementTypeChanged();
            }
        }
        public bool? HasSettlement 
        {
            get
            {
                if (SettlementType == null)
                    return null;
                else
                    return (SettlementType != eSettlementType.None);
            }
        }
        private bool? interstellar;
        public bool? Interstellar
        {
            get { return interstellar; }
            set { interstellar = value; }
        }
        private int? colonyAge;
        public int? ColonyAge
        {
            get { return colonyAge; }
            set
            {
                colonyAge = value;
            }
        }

        public double? CarryingCapacity
        {
            get
            {
                if (LocalSpecies == null)
                    return null;
                else
                    return LocalSpecies.CarryingCapacity(this);
            }
        }
        private double? population;
        public double? Population
        {
            get { return population; }
            set { population = value; }
        }
        public int? PopulationRating
        { 
            get 
            {
                if (Population == null)
                    return null;
                else if (Population == 0)
                    return 0;
                else
                    return (int)Math.Log10(Population ?? 0); 
            } 
        }

        private eWorldUnityLevel? worldUnityLevel;
        public eWorldUnityLevel? WorldUnityLevel 
        { 
            get { return worldUnityLevel; }
            set {  worldUnityLevel = value; }
        }
        private fGovernmentSpecialConditions? governmentSpecialConditions;
        public fGovernmentSpecialConditions? GovernmentSpecialConditions
        {
            get { return governmentSpecialConditions; }
            set { governmentSpecialConditions = value; CheckRanges(); }
        }
        public bool HasGovernmentSpecialCondition(fGovernmentSpecialConditions flag)
        {
            if (GovernmentSpecialConditions == null)
                return false;
            else
                return (GovernmentSpecialConditions & flag) == flag;
        }
        private eSocietyType? societyType;
        public eSocietyType? SocietyType
        {
            get { return societyType; }
            set { societyType = value; CheckRanges(); }
        }
        public int? MinControlRating 
        {
            get
            {
                if (SocietyType == null)
                    return null;
                else
                    return RuleBook.SocietyTypeParams[(SocietyType ?? eSocietyType.Anarchy)].MinControlRating;
            }
        }
        public int? MaxControlRating
        {
            get
            {
                if (SocietyType == null)
                    return null;
                else
                    return RuleBook.SocietyTypeParams[(SocietyType ?? eSocietyType.Anarchy)].MaxControlRating;
            }
        }
        private int? controlRating;
        public int? ControlRating 
        {
            get { return controlRating; }
            set { controlRating = value; }
        }

        public int? IncomePerCapita 
        {
            get
            {
                if (LocalTechLevel == null || AffinityScore == null || CarryingCapacity == null || Population == null)
                    return null;
                else
                {
                    double baseIncome = RuleBook.TechLevelParams[(LocalTechLevel ?? 0)].BaseIncome;
                    double multiplier = 1;
                    switch ((AffinityScore ?? 0))
                    {
                        case <= 0:
                            multiplier += -0.3;
                            break;
                        case > 1 and <= 3:
                            multiplier += -0.2;
                            break;
                        case > 3 and <= 6:
                            multiplier += -0.1;
                            break;
                        case > 6 and <= 8:
                            multiplier += 0;
                            break;
                        case 9:
                            multiplier += 0.2;
                            break;
                        case > 9:
                            multiplier += 0.4;
                            break;
                    }

                    if (CarryingCapacity < Population)
                        multiplier *= ((double)CarryingCapacity / (double)Population);

                    return (int)(baseIncome * multiplier);
                }
            }
        }
        public eWealthLevel? WealthLevel
        {
            get
            {
                if (IncomePerCapita == null)
                    return null;

                double relativeWealth = (IncomePerCapita ?? 0) / Setting.AverageIncome;
                switch (relativeWealth)
                {
                    case <= 0.09:
                        return eWealthLevel.DeadBroke;
                    case <= 0.31:
                        return eWealthLevel.Poor;
                    case <= 0.72:
                        return eWealthLevel.Struggling;
                    case <= 1.39:
                        return eWealthLevel.Average;
                    default:
                        return eWealthLevel.Comfortable;
                }
            }
        }
        public double? EconomicVolume
        {
            get
            {
                if (IncomePerCapita == null || Population == null)
                    return null;
                else
                    return RuleBook.RoundToSignificantFigures((double)(IncomePerCapita ?? 0) * (Population ?? 0), 2);
            }
        }
        private double? tradeVolume;
        public double? TradeVolume
        {
            get { return tradeVolume; }
            set { tradeVolume = value; }
        }
        private int? spaceportClass;
        public int? SpaceportClass
        {
            get { return spaceportClass; }
            set { spaceportClass = value; }
        }
        internal List<Installation> Installations;

        public Planet(Setting setting)
        {
            this.setting = setting;
            Installations = new List<Installation>();
            CheckRanges();
        }

        // disabling this check as the non-nullable fields all get set in the various set properties
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Planet(Planet p)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            // creates a copy of p
            Setting = p.Setting; // don't need to copy this since not editing the setting
            Name = p.Name;
            Description=p.Description;
            Size = p.Size;
            Subtype = p.Subtype;
            AtmosphericMass = p.AtmosphericMass;
            AtmosphericConditions = p.AtmosphericConditions;
            AtmosphericDescription = p.AtmosphericDescription;
            HydrographicCoverage = p.HydrographicCoverage;
            AverageSurfaceTempK = p.AverageSurfaceTempK;
            Density = p.Density;
            Gravity = p.Gravity;
            LocalSpecies =p.LocalSpecies; // don't need to copy this since aren't going to edit the species here
            LocalTechLevel = p.LocalTechLevel;
            LocalTechLevelRelativity = p.LocalTechLevelRelativity;
            ResourceValueCategory = p.ResourceValueCategory;
            SettlementType = p.SettlementType;
            Interstellar = p.Interstellar;
            ColonyAge = p.ColonyAge;
            Population = p.Population;
            WorldUnityLevel = p.WorldUnityLevel;
            GovernmentSpecialConditions = p.GovernmentSpecialConditions;
            SocietyType = p.SocietyType;
            ControlRating = p.ControlRating;
            TradeVolume = p.TradeVolume;
            SpaceportClass = p.SpaceportClass;
            Installations = new List<Installation>(); // copy this so we don't edit the origial's installation list
            foreach (Installation inst in p.Installations)
            {
                Installations.Add(new Installation(inst.Type, inst.Subtype, inst.PR));
            }
        }

        private void CheckRanges()
        {

            // check that any values are still in the (min, max) range
            if (HydrographicCoverage < MinimumHydrographicCoverage)
                HydrographicCoverage = MinimumHydrographicCoverage;
            if (HydrographicCoverage > MaximumHydrographicCoverage)
                HydrographicCoverage = MaximumHydrographicCoverage;

            if (AverageSurfaceTempK < MinSurfaceTemperatureK)
                AverageSurfaceTempK = MinSurfaceTemperatureK;
            if (AverageSurfaceTempK > MaxSurfaceTemperatureK)
                AverageSurfaceTempK = MaxSurfaceTemperatureK;

            if (Density < MinDensity)
                Density = MinDensity;
            if (Density > MaxDensity)
                Density = MaxDensity;

            if (Gravity < MinGravity)
                Gravity = MinGravity;
            if (Gravity > MaxGravity)
                Gravity = MaxGravity;

            if (ControlRating < MinControlRating ||
                ControlRating > MaxControlRating)
                ControlRating = (MinControlRating + MaxControlRating) / 2;

        }

        public List<Installation> GetInstallations(string instType)
        {
            List<Installation> lst = new List<Installation>();
            foreach (Installation inst in Installations)
            {
                if (inst.Type == instType)
                    lst.Add(inst);
            }
            return lst;
        }


        private void PlanetTypeChanged()
        {
            // update the parameters
            if (RuleBook.PlanetParams.ContainsKey((SizeVal, SubtypeVal)))
                parameters = RuleBook.PlanetParams[(SizeVal, SubtypeVal)];
            else
                parameters = null;
            CheckRanges();

            // refresh various parameters if the planet type has updated

            // set atmosphere to 1, or 0 if there is no atmosphere
            if (HasAtmosphere == null)
                AtmosphericMass = null;
            else if (HasAtmosphere == true)
                AtmosphericMass = 1;
            else
                AtmosphericMass = 0;

            // if there's no choice over atmosphere, set it to the single option
            // otherwise set as blank
            if (HasAtmosphericOptions == null)
            {
                AtmosphericConditions = null;
                AtmosphericDescription = null;
            }
            if (HasAtmosphericOptions == false)
                (AtmosphericConditions, AtmosphericDescription) = RuleBook.PlanetParams[(SizeVal, SubtypeVal)].AtmosphereA;
            else
                (AtmosphericConditions, AtmosphericDescription) = (fAtmosphericConditions.None, "");

            // set ranged inputs to be the midpoint
            HydrographicCoverage = (MinimumHydrographicCoverage + MaximumHydrographicCoverage) / 2;
            AverageSurfaceTempK = (MinSurfaceTemperatureK + MaxSurfaceTemperatureK) / 2;
            Density = Math.Round(((MinDensity + MaxDensity) ?? 0) / 2, 1);
            Gravity = Math.Round(((MinGravity + MaxGravity) ?? 0) / 2, 2);
        }

        private void SettlementTypeChanged()
        {
            ColonyAge = 0;
            Interstellar = true;
            Population = 0;
        }

    }
}
