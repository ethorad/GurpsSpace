
namespace GurpsSpace.PlanetCreation
{
    public class ViewModelPlanet : ViewModel
    {
        private Planet planet;
        public Planet Planet
        {
            get { return planet; }
            set
            {
                planet = value;
                MemberUpdated();
            }
        }

        public ViewModelPlanet(Planet p)
        {
            planet = p;
        }

        // Basic information
        public Setting Setting
        {
            get { return Planet.Setting; }
            set { Planet.Setting = value; MemberUpdated(); }
        }
        public string Name
        {
            get { return Planet.Name; }
            set { Planet.Name = value; MemberUpdated(); }
        }
        public eSize Size
        {
            get { return Planet.Size; }
            set { Planet.Size = value; MemberUpdated(); }
        }
        public eSubtype Subtype
        {
            get { return Planet.Subtype; }
            set { Planet.Subtype = value; MemberUpdated(); }
        }
        public string TypeString
        {
            get
            {
                if ((Planet.Size == eSize.None) || (Planet.Subtype == eSubtype.None))
                    return "n/a";
                if (Planet.Size == eSize.AsteroidBelt)
                    return "Asteroid Belt (" + Planet.OverallType + ")";
                else
                    return Planet.Size + " " + Planet.Subtype + " (" + Planet.OverallType + ")";
            }
        }
        public eResourceValueCategory ResourceValueCategory
        {
            get { return Planet.ResourceValueCategory; }
            set { Planet.ResourceValueCategory = value; MemberUpdated(); }
        }
        public string ResourceValueString
        { 
            get 
            { 
                return Planet.ResourceValueModifier.ToString() + " (" + Planet.ResourceValueCategory.ToString() + ")"; 
            } 
        }
        public string Description
        {
            get { return Planet.Description; }
            set { Planet.Description = value; MemberUpdated(); }
        }
        public bool IsPlanet
        {
            get { return Planet.IsPlanet; }
        }

        // Atmosphere
        public bool HasAtmosphere
        {
            get { return Planet.HasAtmosphere; }
        }
        public double AtmosphericMass
        {
            get { return Planet.AtmosphericMass; }
            set { Planet.AtmosphericMass = value; MemberUpdated(); }
        }
        public string AtmosphericConditionsString
        {
            get
            {
                if (!Planet.HasAtmosphere) // no atmosphere
                    return "n/a";
                if (Planet.HasAtmosphere && Planet.AtmosphericDescription == "") // has atmosphere, but not yet set
                    return "tbc";
                else
                    return Planet.AtmosphericConditions.ToString();
            }
        }
        public bool HasAtmosphericOptions
        {
            get { return Planet.HasAtmosphericOptions; }
        }
        public fAtmosphericConditions AtmosphericConditions
        {
            get { return Planet.AtmosphericConditions; }
            set { Planet.AtmosphericConditions = value; MemberUpdated(); }
        }
        public string AtmosphericDescription
        {
            get { return Planet.AtmosphericDescription; }
            set { Planet.AtmosphericDescription = value; MemberUpdated(); }
        }
        public string AtmosphericPressureString
        {
            get
            {
                return Planet.AtmosphericPressure.ToString("N2") + " (" + Planet.AtmosphericPressureCategory.ToString() + ")";
            }
        }

        // Aquasphere
        public bool HasLiquid
        {
            get { return Planet.HasLiquid; }
        }
        public double MinimumHydrographicCoverage
        {
            get { return Planet.MinimumHydrographicCoverage; }
        }
        public double MaximumHydrographicCoverage
        {
            get { return Planet.MaximumHydrographicCoverage; }
        }
        public double HydrographicCoverage
        {
            get { return Planet.HydrographicCoverage; }
            set { Planet.HydrographicCoverage = value; MemberUpdated(); }
        }
        public string LiquidType
        {
            get { return Planet.LiquidType.ToString(); }
        }

        // Climate
        public int TempMin
        {
            get { return Planet.TempMin; }
        }
        public int TempMax
        {
            get { return Planet.TempMax; }
        }
        public int TempStep
        {
            get { return Planet.TempStep; }
        }
        public int AverageSurfaceTempK
        {
            get { return Planet.AverageSurfaceTempK; }
            set { Planet.AverageSurfaceTempK = value; MemberUpdated(); }
        }
        public string ClimateTypeString
        {
            get { return Planet.ClimateType.ToString(); }
        }
        public int BlackbodyTempK
        {
            get { return Planet.BlackbodyTempK; }
        }

        // Lithosphere
        public eCoreType CoreType
        {
            get { return Planet.CoreType; }
        }
        public string CoreTypeString
        {
            get { return Planet.CoreType.ToString(); }
        }
        public double MinDensity
        {
            get { return Planet.MinDensity; }
        }
        public double MaxDensity
        {
            get { return Planet.MaxDensity; }
        }
        public double Density
        {
            get { return Planet.Density; }
            set { Planet.Density = value; MemberUpdated(); }
        }
        public double MinGravity
        {
            get { return Planet.MinGravity; }
        }
        public double MaxGravity
        {
            get { return Planet.MaxGravity; }
        }
        public double Gravity
        {
            get { return Planet.Gravity; }
            set { Planet.Gravity = value; MemberUpdated(); }
        }
        public string DiameterString
        {
            get { return Planet.DiameterEarths.ToString("N2") + " Earths (" + Planet.DiameterMiles.ToString("N0") + " Miles)"; }
        }
        public string MassString
        {
            get { return Planet.Mass.ToString("N2") + " Earths"; }
        }

        // Social
        public eSettlementType SettlementType
        {
            get { return Planet.SettlementType; }
            set { Planet.SettlementType = value; MemberUpdated(); }
        }
        public string SettlementTypeString
        {
            get
            {
                string res = Planet.SettlementType.ToString();
                if (Planet.SettlementType==eSettlementType.Colony)
                {
                    res += " (" + Planet.ColonyAge.ToString("N0") + " years old)";
                }
                if (Planet.SettlementType == eSettlementType.Homeworld)
                {
                    if (Planet.Interstellar)
                        res += " (Interstellar)";
                    else
                        res += " (Uncontacted)";
                }
                return res;
            }
        }
        public int ColonyAge
        {
            get { return Planet.ColonyAge; }
            set { Planet.ColonyAge = value; MemberUpdated(); }
        }
        public bool Interstellar
        {
            get { return Planet.Interstellar; }
            set { Planet.Interstellar = value; MemberUpdated(); }
        }
        public string LocalSpeciesName
        {
            get { return Planet.LocalSpecies.Name; }
        }
        public string AffinityString
        {
            get
            {
                string res = Planet.AffinityScore.ToString("N0");
                res += " (Resources: " + Planet.ResourceValueModifier.ToString("N0") + ", ";
                res += "Habitability: " + Planet.Habitability.ToString("N0") + ")";
                return res;

            }
        }
        public int LocalTechLevel
        {
            get { return Planet.LocalTechLevel; }
            set { Planet.LocalTechLevel = value; MemberUpdated(); }
        }
        public bool LocalTechLevelIsDelayed
        {
            get { return Planet.LocalTechLevelIsDelayed; }
            set { Planet.LocalTechLevelIsDelayed = value; MemberUpdated(); }
        }
        public bool LocalTechLevelIsAdvanced
        {
            get { return Planet.LocalTechLevelIsDelayed; }
            set { Planet.LocalTechLevelIsAdvanced = value; MemberUpdated(); }
        }
        public string LocalTechLevelString
        {
            get
            {
                string TLval = "TL" + Planet.LocalTechLevel.ToString("N0");
                string AgeDesc = "(" + Planet.LocalTechLevelAge;
                if (Planet.LocalTechLevelIsDelayed)
                {
                    TLval += "-1";
                    AgeDesc += " - Delayed";
                }
                if (Planet.LocalTechLevelIsAdvanced)
                {
                    TLval += "+1";
                    AgeDesc += " - Advanced";
                }
                AgeDesc += ")";
                return TLval + " " + AgeDesc;
            }
        }
        public Species LocalSpecies
        {
            get { return Planet.LocalSpecies; }
            set { Planet.LocalSpecies = value; MemberUpdated(); }
        }

    }
}
