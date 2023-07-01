using GurpsSpace.PlanetCreation;
using System;
using System.Collections.Generic;

namespace GurpsSpace
{
    public class Planet
    {
        private Setting setting;
        public Setting Setting { get { return setting; } set { setting = value; } }

        private string? name;
        public string? Name { get { return name; } set { name = value; } }
        public eOverallType? OverallType { get { return RuleBook.OverallTypeBySubtype(Subtype); } }
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
        public eSize? Size { get { return size; } set { size = value; } }
        private eSubtype? subtype;
        public eSubtype SubtypeVal { get { return subtype ?? eSubtype.None; } }
        public eSubtype? Subtype { get { return subtype; } set { subtype = value; } }

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
        
        public bool? HasLiquid { get { return (hydrographicCoverage == null) ? null : (hydrographicCoverage > 0); } }
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
        private eLiquid? liquidType;
        public eLiquid? LiquidType { get { return liquidType; } set { liquidType = value; } }

        private int? averageSurfaceTemperatureK;
        public int? AverageSurfaceTemperatureK
        {
            get { return averageSurfaceTemperatureK; }
            set
            {
                averageSurfaceTemperatureK = value;
                CheckRanges();
            }
        }
        public eClimateType? ClimateType
        {
            get
            {
                if (AverageSurfaceTemperatureK == null)
                    return null;
                else
                    return RuleBook.ClimateType(AverageSurfaceTemperatureK ?? 0); // OK to use ?? as we know it isn't null here
            }
        }
        public int? BlackbodyTemperatureK
        {
            get
            {
                double? absorption = RuleBook.BlackbodyAbsorption(this);
                double? greenhouse = RuleBook.BlackbodyGreenhouse(this);

                // check for values we need
                if (absorption == null ||
                    greenhouse == null ||
                    AtmosphericMass == null || 
                    AverageSurfaceTemperatureK == null)
                    return null;
                // so can now use ?? on all nullable values

                double bbCorrection = (absorption ?? 1) * (1 + (AtmosphericMass ?? 0) * (greenhouse ?? 1));
                double bbTemp = (AverageSurfaceTemperatureK ?? 0) / bbCorrection;

                return ((int)Math.Round(bbTemp, 0));
            }
        }

        private eCoreType? coreType;
        public eCoreType? CoreType { get { return coreType; } set { coreType = value; } }

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

        private double? pressureFactor;
        public double? PressureFactor { get { return pressureFactor; } set { pressureFactor = value; } }
        public double? AtmosphericPressure
        {
            get
            {
                if (pressureFactor == null || AtmosphericMass == null || Gravity == null)
                    return null;
                else
                    return (AtmosphericMass ?? 0) * pressureFactor * (Gravity ?? 0);
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
        public eSettlementType? SettlementType { get { return settlementType; } set { settlementType = value; } }
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
            CoreType = p.CoreType;
            AtmosphericMass = p.AtmosphericMass;
            AtmosphericConditions = p.AtmosphericConditions;
            AtmosphericDescription = p.AtmosphericDescription;
            HydrographicCoverage = p.HydrographicCoverage;
            LiquidType = p.LiquidType;
            AverageSurfaceTemperatureK = p.AverageSurfaceTemperatureK;
            Density = p.Density;
            Gravity = p.Gravity;
            LocalSpecies = p.LocalSpecies; // don't need to copy this since aren't going to edit the species here
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

    }
}
