
using System.Collections.Generic;

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
            installations = new ViewModelInstallationList(planet.Installations);
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
            get { return Planet.MinSurfaceTemperatureK; }
        }
        public int TempMax
        {
            get { return Planet.MaxSurfaceTemperatureK; }
        }
        public int TempStep
        {
            get { return Planet.StepSurfaceTemperatureK; }
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
        public bool HasSettlement { get { return Planet.HasSettlement; } }
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
        public int AffinityScore { get { return Planet.AffinityScore; } }
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
        public int Habitability { get { return Planet.Habitability; } }
        public int LocalTechLevel
        {
            get { return Planet.LocalTechLevel; }
            set { Planet.LocalTechLevel = value; MemberUpdated(); }
        }
        public eTechLevelRelativity LocalTechLevelRelativity
        {
            get { return Planet.LocalTechLevelRelativity; }
            set { Planet.LocalTechLevelRelativity = value; MemberUpdated(); }
        }
        public string LocalTechLevelString
        {
            get
            {
                string TLval = "TL" + Planet.LocalTechLevel.ToString("N0");
                string AgeDesc = "(" + Planet.LocalTechLevelAge;
                if (Planet.LocalTechLevelRelativity == eTechLevelRelativity.Delayed)
                {
                    TLval += "-1";
                    AgeDesc += " - Delayed";
                }
                if (Planet.LocalTechLevelRelativity == eTechLevelRelativity.Advanced)
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

        // habitants
        public double CarryingCapacity { get { return Planet.CarryingCapacity; } }
        public double Population
        {
            get { return Planet.Population; }
            set
            {
                Planet.Population = value;
                MemberUpdated();
            }
        }
        public int PopulationRating { get { return Planet.PopulationRating; } }
        public string PopulationString
        {
            get
            {
                return Planet.Population.ToString("N0") + " (PR " + Planet.PopulationRating.ToString("N0") + ")";
            }
        }

        // governance
        public eWorldUnityLevel WorldUnityLevel 
        { 
            get { return Planet.WorldUnityLevel; }
            set 
            { 
                Planet.WorldUnityLevel = value;
                MemberUpdated(); 
            }
        }
        public fGovernmentSpecialConditions GovernmentSpecialConditions
        {
            get { return Planet.GovernmentSpecialConditions; }
            set
            {
                Planet.GovernmentSpecialConditions = value;
                MemberUpdated();
            }
        }
        public eSocietyType SocietyType
        {
            get { return Planet.SocietyType; }
            set
            {
                Planet.SocietyType = value;
                MemberUpdated();
            }
        }
        public string SocietyTypeString
        {
            get
            {
                string res = Planet.SocietyType.ToString();
                if (Planet.GovernmentSpecialConditions != fGovernmentSpecialConditions.None)
                    res += " (" + Planet.GovernmentSpecialConditions.ToString() + ")";
                return res;
            }
        }
        public int ControlRating
        {
            get { return Planet.ControlRating; }
            set { Planet.ControlRating = value; MemberUpdated(); }
        }
        public string ControlRatingString
        {
            get
            {
                return "CR " + ControlRating.ToString() + " (" + RuleBook.ControlRatings[ControlRating] + ")";
            }
        }

        // economics
        public double IncomePerCapita { get { return Planet.IncomePerCapita; } }
        public eWealthLevel WealthLevel { get { return Planet.WealthLevel; } }
        public string IncomePerCapitaString 
        { 
            get { return "$" + IncomePerCapita.ToString("N0") + " (" + WealthLevel + ")"; }
        }
        public double EconomicVolume { get { return Planet.EconomicVolume; } }
        public string EconomicVolumeString
        {
            get
            {
                return "$" + Planet.EconomicVolume.ToString("N0");
            }
        }
        public double TradeVolume
        {
            get { return Planet.TradeVolume; }
            set { Planet.TradeVolume = value; MemberUpdated(); }
        }
        public string TradeVolumeString
        {
            get { return "$" + TradeVolume.ToString("N0"); }
        }
        public int SpaceportClass
        {
            get { return Planet.SpaceportClass; }
            set { Planet.SpaceportClass = value; MemberUpdated(); }
        }
        public string SpaceportClassString
        {
            get
            {
                string classNum = "";
                switch (Planet.SpaceportClass)
                {
                    case 0:
                        classNum = "0";
                        break;
                    case 1:
                        classNum = "I";
                        break;
                    case 2:
                        classNum = "II";
                        break;
                    case 3:
                        classNum = "III";
                        break;
                    case 4:
                        classNum = "IV";
                        break;
                    case 5:
                        classNum = "V";
                        break;
                }

                return "Class " + classNum + " (" + RuleBook.SpaceportName[Planet.SpaceportClass] + ")";

            }
        }
        private ViewModelInstallationList installations;
        public ViewModelInstallationList Installations
        {
            get { return installations; }
            set
            {
                installations = value;
                Planet.Installations.Clear();
                foreach (ViewModelInstallation vmInst in installations.Installations)
                    Planet.Installations.Add(vmInst.Installation);
                MemberUpdated();
            }
        }
        //private List<ViewModelInstallation> installations;
        //public List<ViewModelInstallation> Installations
        //{
        //    get { return installations; }
        //    set
        //    {
        //        installations = value;
        //        Planet.Installations.Clear();
        //        foreach (ViewModelInstallation vmInst in installations)
        //            Planet.Installations.Add(vmInst.Installation);
        //        MemberUpdated();
        //    }
        //}
        public string InstallationsSummaryString
        {
            get
            {
                return Installations.Installations.Count.ToString("N0") + " installations present";
            }
        }
    }
}
