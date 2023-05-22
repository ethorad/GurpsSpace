using System;
using System.Text;

namespace GurpsSpace
{
    public class Planet
    {
        private Setting setting;
        public Setting Setting { get { return setting; } set { setting = value; } }

        private PlanetParameters? parameters;
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value; 
                CheckRanges();
            }
        }
        public eOverallType OverallType { get { return (parameters == null) ? eOverallType.None : parameters.OverallType; } }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                CheckRanges();
            }
        }
        private eSize size; 
        public eSize Size
        {
            get { return size; }
            set
            {
                bool change = (size != value);
                size = value;
                if (RuleBook.PlanetParams.ContainsKey((Size, Subtype)))
                    parameters = RuleBook.PlanetParams[(Size, Subtype)];
                else
                    parameters = null;
                CheckRanges();
                if (change)
                    PlanetTypeChanged();
            }
        }
        private eSubtype subtype;
        public eSubtype Subtype
        {
            get { return subtype; }
            set
            {
                bool change = (subtype != value);
                subtype = value;
                if (RuleBook.PlanetParams.ContainsKey((Size, Subtype)))
                    parameters = RuleBook.PlanetParams[(Size, Subtype)];
                else
                    parameters = null;
                CheckRanges();
                if (change)
                    PlanetTypeChanged();
            }
        }

        public bool IsPlanet
        {
            get
            {
                if (Size == eSize.None || Size == eSize.AsteroidBelt)
                    return false;
                else
                    return true;
            }
        }

        public bool HasAtmosphere { get { return (parameters == null) ? false : parameters.HasAtmosphere; } }
        private double atmosphericMass;
        public double AtmosphericMass
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
        public bool HasAtmosphericOptions { get { return (parameters == null) ? true : (parameters.AtmosphereANumber < 18); } }
        private fAtmosphericConditions atmosphericConditions;
        public fAtmosphericConditions AtmosphericConditions
        {
            get { return atmosphericConditions; }
            set
            {
                atmosphericConditions = value;
                CheckRanges();
            }
        }
        private string atmosphericDescription;
        public string AtmosphericDescription
        {
            get { return atmosphericDescription; }
            set
            {
                atmosphericDescription = value;
                CheckRanges();
            }
        }
        public bool IsSuffocating { get { return ((AtmosphericConditions & fAtmosphericConditions.Suffocating) == fAtmosphericConditions.Suffocating); } }
        public bool IsMildlyToxic { get { return ((AtmosphericConditions & fAtmosphericConditions.MildlyToxic) == fAtmosphericConditions.MildlyToxic); } }
        public bool IsHighlyToxic { get { return ((AtmosphericConditions & fAtmosphericConditions.HighlyToxic) == fAtmosphericConditions.HighlyToxic); } }
        public bool IsLethallyToxic { get { return ((AtmosphericConditions & fAtmosphericConditions.LethallyToxic) == fAtmosphericConditions.LethallyToxic); } }
        public bool IsToxic { get { return (IsMildlyToxic || IsHighlyToxic || IsLethallyToxic); } }
        public bool IsCorrosive { get { return ((AtmosphericConditions & fAtmosphericConditions.Corrosive) == fAtmosphericConditions.Corrosive); } }
        public bool IsMarginal { get { return ((AtmosphericConditions & fAtmosphericConditions.Marginal) == fAtmosphericConditions.Marginal); } }
        public bool IsBreathable { get { return ((AtmosphericMass > 0) && (!IsSuffocating) && (!IsToxic) && (!IsCorrosive)); } }
        
        public bool HasLiquid { get { return (parameters == null) ? false : !(parameters.Liquid == eLiquid.None); } }
        public double MinimumHydrographicCoverage { get { return (parameters == null) ? 0 : parameters.HydroMin; } }
        public double MaximumHydrographicCoverage { get { return (parameters == null) ? 0 : parameters.HydroMax; } }
        private double hydrographicCoverage;
        public double HydrographicCoverage
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
        public eLiquid LiquidType { get { return (parameters == null) ? eLiquid.None : parameters.Liquid; } }

        public int TempMin { get { return (parameters == null) ? 0 : parameters.TempMin; } }
        public int TempMax { get { return (parameters == null) ? 0 : parameters.TempMax; } }
        public int TempStep { get { return (parameters == null) ? 0 : parameters.TempStep; } }
        private int averageSurfaceTempK;
        public int AverageSurfaceTempK
        {
            get { return averageSurfaceTempK; }
            set
            {
                averageSurfaceTempK = value;
                CheckRanges();
            }
        }
        public eClimateType ClimateType { get { return RuleBook.ClimateType(AverageSurfaceTempK); } }
        public int BlackbodyTempK
        {
            get
            {
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

                double bbCorrection = absorption * (1 + AtmosphericMass * greenhouse);
                double bbTemp = AverageSurfaceTempK / bbCorrection;

                return ((int)Math.Round(bbTemp, 0));
            }
        }

        public eCoreType CoreType { get { return (parameters == null) ? eCoreType.None : parameters.CoreType; } }
        public double MinDensity
        {
            get
            {
                switch (CoreType)
                {
                    case eCoreType.Icy:
                        return RuleBook.DensityIcyCore[0];
                    case eCoreType.SmallIron:
                        return RuleBook.DensitySmallIronCore[0];
                    case eCoreType.LargeIron:
                        return RuleBook.DensityLargeIronCore[0];
                    default:
                        return 0;
                }
            }
        }
        public double MaxDensity
        {
            get
            {
                switch (CoreType)
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
        private double density;
        public double Density
        {
            get { return density; }
            set
            {
                density = value;
                CheckRanges();
            }
        }
        public double MinGravity
        {
            get
            {
                double minSizeFactor = (parameters == null) ? 0 : parameters.MinSizeFactor;
                double minG = Math.Sqrt((double)BlackbodyTempK * Density) * minSizeFactor;
                return Math.Round(minG, 2);
            }
        }
        public double MaxGravity
        {
            get
            {
                double maxSizeFactor = (parameters == null) ? 0 : parameters.MaxSizeFactor;
                double maxG = Math.Sqrt((double)BlackbodyTempK * Density) * maxSizeFactor;
                return Math.Round(maxG, 2);
            }
        }
        private double gravity;
        public double Gravity
        {
            get { return gravity; }
            set
            {
                gravity = value;
                CheckRanges();
            }
        }
        public double DiameterEarths 
        {
            get
            {
                if (Density == 0)
                    return 0;
                else
                    return Math.Round(Gravity / Density, 2);
            }
        }
        public double DiameterMiles { get { return DiameterEarths * RuleBook.EarthDiameterInMiles; } }
        public double Mass { get { return Density * DiameterEarths * DiameterEarths * DiameterEarths; } }

        public double AtmosphericPressure
        {
            get
            {
                double pressureFac = (parameters == null) ? 0 : parameters.PressureFactor;
                return AtmosphericMass * pressureFac * Gravity;
            }
        }
        public ePressureCategory AtmosphericPressureCategory { get { return RuleBook.PressureCategory(AtmosphericPressure); } }

        private Species localSpecies;
        public Species LocalSpecies
        {
            get { return localSpecies; }
            set
            {
                localSpecies = value;
                CheckRanges();
            }
        }

        private int localTechLevel;
        public int LocalTechLevel
        {
            get { return localTechLevel; }
            set
            {
                localTechLevel = value;
                localTechLevelRelativity = eTechLevelRelativity.Normal;
            }
        }
        public string LocalTechLevelAge { get { return RuleBook.TechLevelParams[LocalTechLevel].Age; } }
        private eTechLevelRelativity localTechLevelRelativity;
        public eTechLevelRelativity LocalTechLevelRelativity
        {
            get { return localTechLevelRelativity; }
            set { localTechLevelRelativity = value;}
        }
        private eResourceValueCategory resourceValueCategory;
        public eResourceValueCategory ResourceValueCategory
        {
            get { return resourceValueCategory; }
            set
            {
                resourceValueCategory = value;
                CheckRanges();
            }
        }
        public int ResourceValueModifier { get { return (int)ResourceValueCategory; } }
        public int Habitability { get { return LocalSpecies.Habitability(this); } }
        public int AffinityScore { get { return ResourceValueModifier + Habitability; } }

        private eSettlementType settlementType;
        public eSettlementType SettlementType
        {
            get { return settlementType; }
            set
            {
                bool change = (settlementType != value);
                settlementType = value;
                CheckRanges();
                if (change)
                    SettlementTypeChanged();
            }
        }
        public bool HasSettlement { get { return (SettlementType != eSettlementType.None); } }
        private bool interstellar;
        public bool Interstellar
        {
            get { return interstellar; }
            set { interstellar = value; }
        }
        private int colonyAge;
        public int ColonyAge
        {
            get { return colonyAge; }
            set
            {
                colonyAge = value;
                CheckRanges();
            }
        }

        public ulong CarryingCapacity
        {
            get { return LocalSpecies.CarryingCapacity(this); }
        }
        private ulong population;
        public ulong Population
        {
            get { return population; }
            set { population = value; }
        }
        public int PopulationRating { get { return (int)Math.Log10(Population); } }

        private eWorldUnityLevel worldUnityLevel;
        public eWorldUnityLevel WorldUnityLevel 
        { 
            get { return worldUnityLevel; }
            set {  worldUnityLevel = value; }
        }
        private fGovernmentSpecialConditions governmentSpecialConditions;
        public fGovernmentSpecialConditions GovernmentSpecialConditions
        {
            get { return governmentSpecialConditions; }
            set { governmentSpecialConditions = value; }
        }
        private eSocietyType societyType;
        public eSocietyType SocietyType
        {
            get { return societyType; }
            set { societyType = value; CheckRanges(); }
        }
        public int MinControlRating { get { return RuleBook.SocietyTypeParams[SocietyType].MinControlRating; } }
        public int MaxControlRating { get { return RuleBook.SocietyTypeParams[SocietyType].MaxControlRating; } }
        private int controlRating;
        public int ControlRating 
        {
            get { return controlRating; }
            set { controlRating = value; }
        }

        public Planet(Setting setting)
        {
            this.setting = setting;
            localSpecies = setting.MainSpecies;
            name = "";
            description = "";
            atmosphericDescription = "";
            localTechLevel = setting.TechLevel;
            CheckRanges();
        }

        private void CheckRanges()
        {

            // check that any values are still in the (min, max) range
            if (HydrographicCoverage < MinimumHydrographicCoverage)
                HydrographicCoverage = MinimumHydrographicCoverage;
            if (HydrographicCoverage > MaximumHydrographicCoverage)
                HydrographicCoverage = MaximumHydrographicCoverage;

            if (AverageSurfaceTempK < TempMin)
                AverageSurfaceTempK = TempMin;
            if (AverageSurfaceTempK > TempMax)
                AverageSurfaceTempK = TempMax;

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

        private void PlanetTypeChanged()
        {
            // refresh various parameters if the planet type has updated

            // set atmosphere to 1, or 0 if there is no atmosphere
            if (HasAtmosphere)
                AtmosphericMass = 1;
            else
                AtmosphericMass = 0;

            // if there's no choice over atmosphere, set it to the single option
            // otherwise set as blank
            if (!HasAtmosphericOptions)
                (AtmosphericConditions, AtmosphericDescription) = RuleBook.PlanetParams[(Size, Subtype)].AtmosphereA;
            else
                (AtmosphericConditions, AtmosphericDescription) = (fAtmosphericConditions.None, "");

            // set ranged inputs to be the midpoint
            HydrographicCoverage = (MinimumHydrographicCoverage + MaximumHydrographicCoverage) / 2;
            AverageSurfaceTempK = (TempMin + TempMax) / 2;
            Density = Math.Round((MinDensity + MaxDensity) / 2, 1);
            Gravity = Math.Round((MinGravity + MaxGravity) / 2, 2);
        }

        private void SettlementTypeChanged()
        {
            ColonyAge = 0;
            Interstellar = true;
            Population = 0;
        }

    }
}
