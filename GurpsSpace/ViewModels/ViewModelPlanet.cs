
using System.Collections.Generic;

namespace GurpsSpace.PlanetCreation
{
    public class ViewModelPlanet : ViewModel
    {
        private Planet planet;
        public Planet Planet { get { return planet; } }

        public ViewModelPlanet(Planet p)
        {
            planet = p;
            installationsList = new ViewModelInstallationList(planet.Installations);
        }

        // Basic information
        public Setting Setting
        {
            get { return planet.Setting; }
            set { planet.Setting = value; MemberUpdated(); }
        }
        public string Name
        {
            get { return planet.Name; }
            set { planet.Name = value; MemberUpdated(); }
        }
        public eSize Size
        {
            get { return planet.Size; }
            set { planet.Size = value; MemberUpdated(); }
        }
        public eSubtype Subtype
        {
            get { return planet.Subtype; }
            set { planet.Subtype = value; MemberUpdated(); }
        }
        public string TypeString
        {
            get
            {
                if ((planet.Size == eSize.None) || (planet.Subtype == eSubtype.None))
                    return "n/a";
                if (planet.Size == eSize.AsteroidBelt)
                    return "Asteroid Belt (" + planet.OverallType + ")";
                else
                    return planet.Size + " " + planet.Subtype + " (" + planet.OverallType + ")";
            }
        }
        public eResourceValueCategory ResourceValueCategory
        {
            get { return planet.ResourceValueCategory; }
            set { planet.ResourceValueCategory = value; MemberUpdated(); }
        }
        public string ResourceValueString
        { 
            get 
            { 
                return planet.ResourceValueModifier.ToString() + " (" + planet.ResourceValueCategory.ToString() + ")"; 
            } 
        }
        public string Description
        {
            get { return planet.Description; }
            set { planet.Description = value; MemberUpdated(); }
        }
        public bool IsPlanet
        {
            get { return planet.IsPlanet; }
        }

        // Atmosphere
        public bool HasAtmosphere
        {
            get { return planet.HasAtmosphere; }
        }
        public double AtmosphericMass
        {
            get { return planet.AtmosphericMass; }
            set { planet.AtmosphericMass = value; MemberUpdated(); }
        }
        public string AtmosphericConditionsString
        {
            get
            {
                if (!planet.HasAtmosphere) // no atmosphere
                    return "n/a";
                if (planet.HasAtmosphere && planet.AtmosphericDescription == "") // has atmosphere, but not yet set
                    return "tbc";
                else
                    return planet.AtmosphericConditions.ToString();
            }
        }
        public bool HasAtmosphericOptions
        {
            get { return planet.HasAtmosphericOptions; }
        }
        public fAtmosphericConditions AtmosphericConditions
        {
            get { return planet.AtmosphericConditions; }
            set { planet.AtmosphericConditions = value; MemberUpdated(); }
        }
        public string AtmosphericDescription
        {
            get { return planet.AtmosphericDescription; }
            set { planet.AtmosphericDescription = value; MemberUpdated(); }
        }
        public string AtmosphericPressureString
        {
            get
            {
                return planet.AtmosphericPressure.ToString("N2") + " (" + planet.AtmosphericPressureCategory.ToString() + ")";
            }
        }

        // Aquasphere
        public bool HasLiquid
        {
            get { return planet.HasLiquid; }
        }
        public double MinimumHydrographicCoverage
        {
            get { return planet.MinimumHydrographicCoverage; }
        }
        public double MaximumHydrographicCoverage
        {
            get { return planet.MaximumHydrographicCoverage; }
        }
        public double HydrographicCoverage
        {
            get { return planet.HydrographicCoverage; }
            set { planet.HydrographicCoverage = value; MemberUpdated(); }
        }
        public string LiquidType
        {
            get { return planet.LiquidType.ToString(); }
        }

        // Climate
        public int TempMin
        {
            get { return planet.MinSurfaceTemperatureK; }
        }
        public int TempMax
        {
            get { return planet.MaxSurfaceTemperatureK; }
        }
        public int TempStep
        {
            get { return planet.StepSurfaceTemperatureK; }
        }
        public int AverageSurfaceTempK
        {
            get { return planet.AverageSurfaceTempK; }
            set { planet.AverageSurfaceTempK = value; MemberUpdated(); }
        }
        public string ClimateTypeString
        {
            get { return planet.ClimateType.ToString(); }
        }
        public int BlackbodyTempK
        {
            get { return planet.BlackbodyTempK; }
        }

        // Lithosphere
        public eCoreType CoreType
        {
            get { return planet.CoreType; }
        }
        public string CoreTypeString
        {
            get { return planet.CoreType.ToString(); }
        }
        public double MinDensity
        {
            get { return planet.MinDensity; }
        }
        public double MaxDensity
        {
            get { return planet.MaxDensity; }
        }
        public double Density
        {
            get { return planet.Density; }
            set { planet.Density = value; MemberUpdated(); }
        }
        public double MinGravity
        {
            get { return planet.MinGravity; }
        }
        public double MaxGravity
        {
            get { return planet.MaxGravity; }
        }
        public double Gravity
        {
            get { return planet.Gravity; }
            set { planet.Gravity = value; MemberUpdated(); }
        }
        public string DiameterString
        {
            get { return planet.DiameterEarths.ToString("N2") + " Earths (" + planet.DiameterMiles.ToString("N0") + " Miles)"; }
        }
        public string MassString
        {
            get { return planet.Mass.ToString("N2") + " Earths"; }
        }

        // Social
        public eSettlementType SettlementType
        {
            get { return planet.SettlementType; }
            set { planet.SettlementType = value; MemberUpdated(); }
        }
        public bool HasSettlement { get { return planet.HasSettlement; } }
        public string SettlementTypeString
        {
            get
            {
                string res = planet.SettlementType.ToString();
                if (planet.SettlementType==eSettlementType.Colony)
                {
                    res += " (" + planet.ColonyAge.ToString("N0") + " years old)";
                }
                if (planet.SettlementType == eSettlementType.Homeworld)
                {
                    if (planet.Interstellar)
                        res += " (Interstellar)";
                    else
                        res += " (Uncontacted)";
                }
                return res;
            }
        }
        public int ColonyAge
        {
            get { return planet.ColonyAge; }
            set { planet.ColonyAge = value; MemberUpdated(); }
        }
        public bool Interstellar
        {
            get { return planet.Interstellar; }
            set { planet.Interstellar = value; MemberUpdated(); }
        }
        public string LocalSpeciesName
        {
            get { return planet.LocalSpecies.Name; }
        }
        public int AffinityScore { get { return planet.AffinityScore; } }
        public string AffinityString
        {
            get
            {
                string res = planet.AffinityScore.ToString("N0");
                res += " (Resources: " + planet.ResourceValueModifier.ToString("N0") + ", ";
                res += "Habitability: " + planet.Habitability.ToString("N0") + ")";
                return res;

            }
        }
        public int Habitability { get { return planet.Habitability; } }
        public int LocalTechLevel
        {
            get { return planet.LocalTechLevel; }
            set { planet.LocalTechLevel = value; MemberUpdated(); }
        }
        public eTechLevelRelativity LocalTechLevelRelativity
        {
            get { return planet.LocalTechLevelRelativity; }
            set { planet.LocalTechLevelRelativity = value; MemberUpdated(); }
        }
        public string LocalTechLevelString
        {
            get
            {
                string TLval = "TL" + planet.LocalTechLevel.ToString("N0");
                string AgeDesc = "(" + planet.LocalTechLevelAge;
                if (planet.LocalTechLevelRelativity == eTechLevelRelativity.Delayed)
                {
                    TLval += "-1";
                    AgeDesc += " - Delayed";
                }
                if (planet.LocalTechLevelRelativity == eTechLevelRelativity.Advanced)
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
            get { return planet.LocalSpecies; }
            set { planet.LocalSpecies = value; MemberUpdated(); }
        }

        // habitants
        public double CarryingCapacity { get { return planet.CarryingCapacity; } }
        public double Population
        {
            get { return planet.Population; }
            set
            {
                planet.Population = value;
                MemberUpdated();
            }
        }
        public int PopulationRating { get { return planet.PopulationRating; } }
        public string PopulationString
        {
            get
            {
                return planet.Population.ToString("N0") + " (PR " + planet.PopulationRating.ToString("N0") + ")";
            }
        }

        // governance
        public eWorldUnityLevel WorldUnityLevel 
        { 
            get { return planet.WorldUnityLevel; }
            set 
            {
                planet.WorldUnityLevel = value;
                MemberUpdated(); 
            }
        }
        public fGovernmentSpecialConditions GovernmentSpecialConditions
        {
            get { return planet.GovernmentSpecialConditions; }
            set
            {
                planet.GovernmentSpecialConditions = value;
                MemberUpdated();
            }
        }
        public eSocietyType SocietyType
        {
            get { return planet.SocietyType; }
            set
            {
                planet.SocietyType = value;
                MemberUpdated();
            }
        }
        public string SocietyTypeString
        {
            get
            {
                string res = planet.SocietyType.ToString();
                if (planet.GovernmentSpecialConditions != fGovernmentSpecialConditions.None)
                    res += " (" + planet.GovernmentSpecialConditions.ToString() + ")";
                return res;
            }
        }
        public int ControlRating
        {
            get { return planet.ControlRating; }
            set { planet.ControlRating = value; MemberUpdated(); }
        }
        public string ControlRatingString
        {
            get
            {
                return "CR " + ControlRating.ToString() + " (" + RuleBook.ControlRatings[ControlRating] + ")";
            }
        }

        // economics
        public double IncomePerCapita { get { return planet.IncomePerCapita; } }
        public eWealthLevel WealthLevel { get { return planet.WealthLevel; } }
        public string IncomePerCapitaString 
        { 
            get { return "$" + IncomePerCapita.ToString("N0") + " (" + WealthLevel + ")"; }
        }
        public double EconomicVolume { get { return planet.EconomicVolume; } }
        public string EconomicVolumeString
        {
            get
            {
                return "$" + planet.EconomicVolume.ToString("N0");
            }
        }
        public double TradeVolume
        {
            get { return planet.TradeVolume; }
            set { planet.TradeVolume = value; MemberUpdated(); }
        }
        public string TradeVolumeString
        {
            get { return "$" + TradeVolume.ToString("N0"); }
        }
        public int SpaceportClass
        {
            get { return planet.SpaceportClass; }
            set { planet.SpaceportClass = value; MemberUpdated(); }
        }
        public string SpaceportClassString
        {
            get
            {
                string classNum = "";
                switch (planet.SpaceportClass)
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

                return "Class " + classNum + " (" + RuleBook.SpaceportName[planet.SpaceportClass] + ")";

            }
        }
        private ViewModelInstallationList installationsList;
        public ViewModelInstallationList InstallationsList
        {
            get { return installationsList; }
            set
            {
                installationsList = value;
                planet.Installations.Clear();
                foreach (ViewModelInstallation vmInst in installationsList.Installations)
                    planet.Installations.Add(vmInst.Installation);
                MemberUpdated();
            }
        }
        public void AddInstallations(List<Installation> newInst)
        {
            foreach (Installation inst in newInst)
                AddInstallations(inst);
        }
        public void AddInstallations(Installation inst)
        {
            installationsList.Add(inst);
            planet.Installations.Add(inst);
            MemberUpdated();
        }
        public void ClearInstallations(string instType)
        {
            for (int i = planet.Installations.Count - 1; i >= 0; i--)
                if (planet.Installations[i].Type == instType)
                {
                    planet.Installations.RemoveAt(i);
                    installationsList.Installations.RemoveAt(i);
                }
            MemberUpdated();
        }
        public ViewModelInstallationList GetInstallations(string instType)
        {
            return new ViewModelInstallationList(planet.GetInstallations(instType));
        }
    }
}
