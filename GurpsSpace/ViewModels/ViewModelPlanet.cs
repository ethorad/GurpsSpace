
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GurpsSpace.PlanetCreation
{
    public class ViewModelPlanet : ViewModel
    {
        private Planet planet;
        public Planet Planet
        { 
            get { return planet; } 
            set { planet = value; MemberUpdated(); }
        }

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
            get { return planet.Name ?? "tbc"; }
            set { planet.Name = value; MemberUpdated(); }
        }
        public eSize Size
        {
            set { planet.Size = value; MemberUpdated(); }
        }
        public eSubtype Subtype
        {
            set { planet.Subtype = value; MemberUpdated(); }
        }
        public string TypeString
        {
            get
            {
                if ((planet.Size == null) || (planet.Subtype == null))
                    return "tbc";
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
            set { planet.ResourceValueCategory = value; MemberUpdated(); }
        }
        public string ResourceValueString
        {
            get
            {
                if (planet.ResourceValueCategory == null)
                    return "tbc";
                else
                    return planet.ResourceValueModifier.ToString() + " (" + planet.ResourceValueCategory.ToString() + ")";
            }
        }
        public string Description
        {
            get { return planet.Description ?? "tbc"; }
            set { planet.Description = value; MemberUpdated(); }
        }

        // Atmosphere
        public bool HasAtmosphere
        {
            get { return (planet.AtmosphericMass ?? 0) > 0; }
        }
        public double AtmosphericMass
        {
            get { return (planet.AtmosphericMass ?? 0); }
            set { planet.AtmosphericMass = value; MemberUpdated(); }
        }
        public string AtmosphericConditionsString
        {
            get
            {
                return (planet.AtmosphericConditions.ToString()) ?? "tbc";
            }
        }
        public fAtmosphericConditions AtmosphericConditions
        {
            set { planet.AtmosphericConditions = value; MemberUpdated(); }
        }
        public string AtmosphericDescription
        {
            get { return planet.AtmosphericDescription ?? "tbc"; }
            set { planet.AtmosphericDescription = value; MemberUpdated(); }
        }
        public string AtmosphericPressureString
        {
            get
            {
                if (planet.AtmosphericPressure == null)
                    return "tbc";
                else
                    return (planet.AtmosphericPressure ?? 0).ToString("N2") + " (" + planet.AtmosphericPressureCategory.ToString() + ")";
            }
        }

        // Aquasphere
        public bool HasLiquid
        {
            get { return planet.HasLiquid ?? false; }
        }
        public double HydrographicCoverage
        {
            get { return (planet.HydrographicCoverage ?? 0); }
            set { planet.HydrographicCoverage = value; MemberUpdated(); }
        }
        public string LiquidType
        {
            get { return planet.LiquidType.ToString() ?? "tbc"; }
        }

        // Climate
        public int AverageSurfaceTempK
        {
            get { return (planet.AverageSurfaceTemperatureK ?? 0); }
            set { planet.AverageSurfaceTemperatureK = value; MemberUpdated(); }
        }
        public string ClimateTypeString
        {
            get { return planet.ClimateType.ToString() ?? "tbc"; }
        }
        public int BlackbodyTempK
        {
            get { return (planet.BlackbodyTemperatureK ?? 0); }
        }

        // Lithosphere
        public string CoreTypeString
        {
            get 
            {
                if (planet.CoreType == null)
                    return "tbc";
                else
                    return planet.CoreType.ToString()!; 
            }
        }

        public double Density
        {
            get { return (planet.Density ?? 0); }
            set { planet.Density = value; MemberUpdated(); }
        }
        public double Gravity
        {
            get { return (planet.Gravity ?? 0); }
            set { planet.Gravity = value; MemberUpdated(); }
        }
        public string DiameterString
        {
            get
            {
                if (planet.DiameterEarths == null)
                    return "tbc";
                else
                    return (planet.DiameterEarths ?? 0).ToString("N2") + " Earths (" + (planet.DiameterMiles ?? 0).ToString("N0") + " Miles)";
            }
        }
        public string MassString
        {
            get 
            {
                if (planet.Mass == null)
                    return "tbc";
                else
                    return (planet.Mass ?? 0).ToString("N2") + " Earths"; 
            }
        }

        // Social
        public eSettlementType SettlementType
        {
            set
            { 
                planet.SettlementType = value;
                if (planet.SettlementType == eSettlementType.None)
                {
                    planet.LocalSpecies = null;
                    planet.LocalTechLevel = null;
                    planet.LocalTechLevelRelativity = null;
                }
                MemberUpdated(); 
            }
        }
        public bool HasSettlement { get { return planet.HasSettlement ?? false; } }
        public string SettlementTypeString
        {
            get
            {
                if (planet.SettlementType == null)
                    return "tbc";

                string res = planet.SettlementType.ToString() ?? "tbc";
                if (planet.SettlementType == eSettlementType.Colony)
                {
                    if (planet.ColonyAge == null)
                        res += " (age tbc)";
                    else
                        res += " (" + (planet.ColonyAge ?? 0).ToString("N0") + " years old)";
                }
                if (planet.SettlementType == eSettlementType.Homeworld)
                {
                    if (planet.Interstellar != null)
                    {
                        if (planet.Interstellar ?? false)
                            res += " (Interstellar)";
                        else
                            res += " (Uncontacted)";
                    }
                }
                return res;
            }
        }
        public int ColonyAge
        {
            get { return (planet.ColonyAge ?? 0); }
            set { planet.ColonyAge = value; MemberUpdated(); }
        }
        public bool Interstellar
        {
            get { return planet.Interstellar ?? true; }
            set { planet.Interstellar = value; MemberUpdated(); }
        }
        public string LocalSpeciesName
        {
            get
            {
                if (planet.LocalSpecies == null)
                    return "tbc";
                else
                    return planet.LocalSpecies.Name!;
            }
        }
        public int AffinityScore { get { return (planet.AffinityScore ?? 0); } }
        public string AffinityString
        {
            get
            {
                if (planet.AffinityScore == null)
                    return "tbc";
                else
                {
                    // if affinity score is not null, then resources and habitability are both not null
                    string res = (planet.AffinityScore ?? 0).ToString("N0");
                    res += " (Resources: " + (planet.ResourceValueModifier ?? 0).ToString("N0") + ", ";
                    res += "Habitability: " + (planet.Habitability ?? 0).ToString("N0") + ")";
                    return res;
                }
            }
        }
        public int Habitability { get { return (planet.Habitability ?? 0); } }
        public int LocalTechLevel
        {
            set { planet.LocalTechLevel = value; MemberUpdated(); }
        }
        public eTechLevelRelativity LocalTechLevelRelativity
        {
            set { planet.LocalTechLevelRelativity = value; MemberUpdated(); }
        }
        public string LocalTechLevelString
        {
            get
            {
                if (planet.LocalTechLevel == null || planet.LocalTechLevelRelativity == null)
                    return "tbc";
                else
                {
                    string TLval = "TL" + (planet.LocalTechLevel ?? 0).ToString("N0");
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
        }
        public Species LocalSpecies
        {
            get { return planet.LocalSpecies!; }
            set { planet.LocalSpecies = value; MemberUpdated(); }
        }

        // habitants
        public double CarryingCapacity { get { return (planet.CarryingCapacity ?? 0); } }
        public double Population
        {
            get { return planet.Population ?? 0; }
            set
            {
                planet.Population = value;
                MemberUpdated();
            }
        }
        public string PopulationString
        {
            get
            {
                if (planet.Population == null)
                    return "tbc";
                else
                    return (planet.Population ?? 0).ToString("N0") + " (PR " + (planet.PopulationRating ?? 0).ToString("N0") + ")";
            }
        }

        // governance
        public eWorldUnityLevel WorldUnityLevel 
        { 
            get
            {
                return planet.WorldUnityLevel ?? eWorldUnityLevel.Diffuse; 
            }
            set 
            {
                planet.WorldUnityLevel = value;
                MemberUpdated(); 
            }
        }
        public string WorldUnityString
        {
            get
            {
                if (planet.WorldUnityLevel == null)
                    return "tbc";
                else
                    return planet.WorldUnityLevel.ToString()!;
            }
        }
        public fGovernmentSpecialConditions GovernmentSpecialConditions
        {
            get 
            { 
                return planet.GovernmentSpecialConditions ?? fGovernmentSpecialConditions.None; 
            }
            set
            {
                planet.GovernmentSpecialConditions = value;
                MemberUpdated();
            }
        }
        public eSocietyType SocietyType
        {
            get { return planet.SocietyType ?? eSocietyType.Anarchy; }
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
                if (planet.SocietyType == null)
                    return "tbc";
                string res = planet.SocietyType.ToString()!;
                if (planet.GovernmentSpecialConditions != null
                    && planet.GovernmentSpecialConditions != fGovernmentSpecialConditions.None)
                    res += " (" + planet.GovernmentSpecialConditions.ToString() + ")";
                return res;
            }
        }
        public int ControlRating
        {
            get { return (planet.ControlRating ?? 0); }
            set { planet.ControlRating = value; MemberUpdated(); }
        }
        public string ControlRatingString
        {
            get
            {
                if (planet.ControlRating == null)
                    return "tbc";
                else
                    return "CR " + ControlRating.ToString() + " (" + RuleBook.ControlRatings[ControlRating] + ")";
            }
        }

        // economics
        public eWealthLevel WealthLevel { get { return (planet.WealthLevel ?? 0); } }
        public string IncomePerCapitaString 
        {
            get
            {
                if (planet.IncomePerCapita == null)
                    return "tbc";
                else
                    return "$" + (planet.IncomePerCapita ?? 0).ToString("N0") + " (" + planet.WealthLevel + ")";
            }
        }
        public double EconomicVolume { get { return (planet.EconomicVolume ?? 0); } }
        public string EconomicVolumeString
        {
            get
            {
                if (planet.EconomicVolume == null)
                    return "tbc";
                else
                    return "$" + (planet.EconomicVolume ?? 0).ToString("N0");
            }
        }
        public double TradeVolume
        {
            get { return planet.TradeVolume ?? 0; }
            set { planet.TradeVolume = value; MemberUpdated(); }
        }
        public string TradeVolumeString
        {
            get 
            {
                if (Interstellar == true)
                    return "$" + TradeVolume.ToString("N0");
                else if (Interstellar == false)
                    return "Uncontacted";
                else
                    return "tbc";
            }
        }
        public int SpaceportClass
        {
            get { return planet.SpaceportClass ?? 0; }
            set { planet.SpaceportClass = value; MemberUpdated(); }
        }
        public string SpaceportClassString
        {
            get
            {
                if (planet.SpaceportClass == null)
                    return "tbc";

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

                return "Class " + classNum + " (" + RuleBook.SpaceportName[(planet.SpaceportClass ?? 0)] + ")";

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


        public ViewModelInstallationList GetInstallations(string instType)
        {
            return new ViewModelInstallationList(planet.GetInstallations(instType));
        }
    }
}
