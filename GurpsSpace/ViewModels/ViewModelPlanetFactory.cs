using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GurpsSpace.ViewModels;

namespace GurpsSpace.PlanetCreation
{
    public class ViewModelPlanetFactory : ViewModel
    {
        private PlanetFactory planetFactory;

        public override string SummaryType { get { return TypeString; } }

        public string Name 
        { 
            get 
            { 
                return (planetFactory.Name ?? "tbc"); 
            } 
            set 
            { 
                planetFactory.Name = value; 
                MemberUpdated(); 
            } 
        }
        public string TypeString
        {
            get
            {
                if ((planetFactory.Size == null) || (planetFactory.Subtype == null))
                    return "tbc";
                if ((planetFactory.Size == eSize.None) || (planetFactory.Subtype == eSubtype.None))
                    return "n/a";
                if (planetFactory.Size == eSize.AsteroidBelt)
                    return "Asteroid Belt (" + planetFactory.OverallType + ")";
                else
                    return planetFactory.Size + " " + planetFactory.Subtype + " (" + planetFactory.OverallType + ")";
            }
        }
        public string ResourceValueString
        {
            get
            {
                if (planetFactory.ResourceValueCategory == null)
                    return "tbc";
                else
                    return planetFactory.ResourceValueModifier.ToString() + " (" + planetFactory.ResourceValueCategory.ToString() + ")";
            }
        }
        public string Description
        {
            get
            {
                return (planetFactory.Description ?? "tbc");
            }
            set
            {
                planetFactory.Description = value;
                MemberUpdated();
            }
        }

        public bool HasAtmosphere { get { return planetFactory.HasAtmosphere ?? false; } }
        public double AtmosphericMass
        {
            get
            {
                return planetFactory.AtmosphericMass ?? 0;
            }
            set
            {
                planetFactory.AtmosphericMass = value;
                MemberUpdated();
            }
        }
        public string AtmosphericConditionsString
        {
            get
            {
                if (planetFactory.AtmosphericConditions == null)
                    return "tbc";
                else
                    return planetFactory.AtmosphericConditions.ToString()!;
            }
        }
        public bool HasAtmosphericOptions { get { return (planetFactory.HasAtmosphericOptions ?? false); } }
        public string AtmosphericDescription
        {
            get
            {
                if (planetFactory.AtmosphericDescription == null)
                    return "tbc";
                else
                    return planetFactory.AtmosphericDescription;
            }
        }
        public string AtmosphericPressureString
        {
            get
            {
                if (planetFactory.AtmosphericPressure == null)
                    return "tbc";
                else
                    return (planetFactory.AtmosphericPressure ?? 0).ToString("N2") + " (" + planetFactory.AtmosphericPressureCategory.ToString() + ")";
            }
        }

        public bool HasHydrosphere
        {
            get
            {
                if (planetFactory.HasHydrosphere == null)
                    return false;
                else
                    return planetFactory.HasHydrosphere ?? false;
            }
        }
        public double MinimumHydrographicCoverage { get { return planetFactory.MinimumHydrographicCoverage ?? 0; } }
        public double MaximumHydrographicCoverage { get { return planetFactory.MaximumHydrographicCoverage ?? 0; } }
        public double HydrographicCoverage
        {
            get { return planetFactory.HydrographicCoverage ?? 0; }
            set
            {
                planetFactory.HydrographicCoverage = value;
                MemberUpdated();
            }
        }
        public string LiquidType
        {
            get
            {
                if (planetFactory.LiquidType == null)
                    return "tbc";
                else
                    return planetFactory.LiquidType.ToString()!;
            }
        }

        public int MinSurfaceTemperatureK { get { return planetFactory.MinSurfaceTemperatureK ?? 0; } }
        public int MaxSurfaceTemperatureK { get { return planetFactory.MaxSurfaceTemperatureK ?? 0; } }
        public int StepSurfaceTemperatureK { get { return planetFactory.StepSurfaceTemperatureK ?? 0; } }
        public int AverageSurfaceTemperatureK
        { 
            get { return planetFactory.AverageSurfaceTemperatureK ?? 0; }
            set 
            { 
                planetFactory.AverageSurfaceTemperatureK = value;
                MemberUpdated();
            }
        }
        public string AverageSurfaceTemperatureKString
        {
            get
            {
                if (planetFactory.AverageSurfaceTemperatureK == null)
                    return "tbc";
                else
                    return planetFactory.AverageSurfaceTemperatureK.ToString()! + " K";
            }
        }
        public string ClimateTypeString
        { 
            get 
            {
                if (planetFactory.ClimateType == null)
                    return "tbc";
                else
                    return planetFactory.ClimateType.ToString()!;
            }
        }
        public string BlackbodyTemperatureKString 
        { 
            get 
            {
                if (planetFactory.BlackbodyTemperatureK == null)
                    return "tbc";
                else
                    return planetFactory.BlackbodyTemperatureK.ToString()! + " K"; 
            } 
        }

        public bool HasLithosphere { get { return planetFactory.HasLithosphere ?? false; } }
        public string CoreTypeString
        {
            get
            {
                if (planetFactory.CoreType == null)
                    return "tbc";
                else
                    return planetFactory.CoreType.ToString()!;
            }
        }
        public double MinDensity { get { return planetFactory.MinDensity ?? 0; } }
        public double MaxDensity { get { return planetFactory.MaxDensity ?? 0; } }
        public double Density
        {
            get { return planetFactory.Density ?? 0; }
            set { planetFactory.Density = value; MemberUpdated(); }
        }
        public string DensityString
        {
            get
            {
                if (planetFactory.Size == eSize.AsteroidBelt)
                    return "n/a";
                else if (planetFactory.Density == null)
                    return "tbc";
                else
                    return Math.Round(planetFactory.Density ?? 0, 2).ToString();
            }
        }
        public double MinGravity { get { return planetFactory.MinGravity ?? 0; } }
        public double MaxGravity { get { return planetFactory.MaxGravity ?? 0; } }
        public double Gravity
        {
            get { return planetFactory.Gravity ?? 0; }
            set { planetFactory.Gravity = value; MemberUpdated(); }
        }
        public string GravityString
        {
            get
            {
                if (planetFactory.Size == eSize.AsteroidBelt)
                    return "n/a";
                else if (planetFactory.Gravity == null)
                    return "tbc";
                else
                    return Math.Round(planetFactory.Gravity ?? 0, 2).ToString();
            }
        }
        public string DiameterString
        {
            get
            {
                if (planetFactory.DiameterEarths == null)
                    return "tbc";
                else
                    return (planetFactory.DiameterEarths ?? 0).ToString("N2") + " Earths (" + (planetFactory.DiameterMiles ?? 0).ToString("N0") + " Miles)";
            }
        }
        public string MassString
        {
            get
            {
                if (planetFactory.Mass == null)
                    return "tbc";
                else
                    return (planetFactory.Mass ?? 0).ToString("N2") + " Earths";
            }
        }

        public eSettlementType SettlementType
        {
            get { return planetFactory.SettlementType ?? eSettlementType.None; }
            set
            {
                planetFactory.SettlementType = value;
                UpdateInstallationList();
                MemberUpdated();
            }
        }
        public string SettlementTypeString
        {
            get
            {
                if (planetFactory.SettlementType == null)
                    return "tbc";

                string res = planetFactory.SettlementType.ToString() ?? "tbc";
                if (planetFactory.SettlementType == eSettlementType.Colony)
                {
                    if (planetFactory.ColonyAge == null)
                        res += " (age tbc)";
                    else
                        res += " (" + (planetFactory.ColonyAge ?? 0).ToString("N0") + " years old)";
                }
                if (planetFactory.SettlementType == eSettlementType.Homeworld)
                {
                    if (planetFactory.Interstellar != null)
                    {
                        if (planetFactory.Interstellar ?? false)
                            res += " (Interstellar)";
                        else
                            res += " (Uncontacted)";
                    }
                }
                return res;
            }
        }
        public bool HasSettlement { get { return planetFactory.HasSettlement ?? false; } }
        public string LocalSpeciesName
        {
            get
            {
                if (planetFactory.LocalSpecies == null)
                    return "tbc";
                else
                    return planetFactory.LocalSpecies.Name!;
            }
        }
        public string AffinityScoreString
        {
            get
            {
                if (planetFactory.AffinityScore == null)
                    return "tbc";
                else
                {
                    // if affinity score is not null, then resources and habitability are both not null
                    string res = (planetFactory.AffinityScore ?? 0).ToString("N0");
                    res += " (Resources: " + (planetFactory.ResourceValueModifier ?? 0).ToString("N0") + ", ";
                    res += "Habitability: " + (planetFactory.Habitability ?? 0).ToString("N0") + ")";
                    return res;
                }
            }
        }
        public string LocalTechLevelString
        {
            get
            {
                if (planetFactory.LocalTechLevel == null || planetFactory.LocalTechLevelRelativity == null)
                    return "tbc";
                else
                {
                    string TLval = "TL" + (planetFactory.LocalTechLevel ?? 0).ToString("N0");
                    string AgeDesc = "(" + planetFactory.LocalTechLevelAge;
                    if (planetFactory.LocalTechLevelRelativity == eTechLevelRelativity.Delayed)
                    {
                        TLval += "-1";
                        AgeDesc += " - Delayed";
                    }
                    if (planetFactory.LocalTechLevelRelativity == eTechLevelRelativity.Advanced)
                    {
                        TLval += "+1";
                        AgeDesc += " - Advanced";
                    }
                    AgeDesc += ")";
                    return TLval + " " + AgeDesc;
                }
            }
        }

        public string CarryingCapacityString
        {
            get 
            {
                if (planetFactory.CarryingCapacity == null)
                    return "tbc";
                else
                    return (planetFactory.CarryingCapacity??0).ToString("N0"); 
            } 
        }
        public string PopulationString
        {
            get
            {
                if (planetFactory.Population == null)
                    return "tbc";
                else
                    return (planetFactory.Population ?? 0).ToString("N0") + " (PR " + (planetFactory.PopulationRating ?? 0).ToString("N0") + ")";
            }
        }

        public string WorldUnityString
        {
            get
            {
                if (planetFactory.WorldUnityLevel == null)
                    return "tbc";
                else
                    return planetFactory.WorldUnityLevel.ToString()!;
            }
        }
        public string SocietyTypeString
        {
            get
            {
                if (planetFactory.SocietyType == null)
                    return "tbc";
                string res = planetFactory.SocietyType.ToString()!;
                if (planetFactory.GovernmentSpecialConditions != null
                    && planetFactory.GovernmentSpecialConditions != fGovernmentSpecialConditions.None)
                    res += " (" + planetFactory.GovernmentSpecialConditions.ToString() + ")";
                return res;
            }
        }
        public string ControlRatingString
        {
            get
            {
                if (planetFactory.ControlRating == null)
                    return "tbc";
                else
                    return "CR " + planetFactory.ControlRating.ToString() + " (" + RuleBook.ControlRatings[planetFactory.ControlRating ?? 0] + ")";
            }
        }
        public string IncomePerCapitaString
        {
            get
            {
                if (planetFactory.IncomePerCapita == null)
                    return "tbc";
                else
                    return "$" + (planetFactory.IncomePerCapita ?? 0).ToString("N0") + " (" + planetFactory.WealthLevel + ")";
            }
        }
        public string EconomicVolumeString
        {
            get
            {
                if (planetFactory.EconomicVolume == null)
                    return "tbc";
                else
                    return "$" + (planetFactory.EconomicVolume ?? 0).ToString("N0");
            }
        }
        public string TradeVolumeString
        {
            get
            {
                if (planetFactory.TradeVolume == null)
                    return "tbc";
                else if (planetFactory.Interstellar == true)
                    return "$" + (planetFactory.TradeVolume ?? 0).ToString("N0");
                else if (planetFactory.Interstellar == false)
                    return "Uncontacted";
                else
                    return "tbc";
            }
        }

        public string SpaceportClassString
        {
            get
            {
                if (planetFactory.SpaceportClass == null)
                    return "tbc";

                string classNum = "";
                switch (planetFactory.SpaceportClass)
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

                return "Class " + classNum + " (" + RuleBook.SpaceportName[(planetFactory.SpaceportClass ?? 0)] + ")";
            }
        }

        private ViewModelList<ViewModelInstallation> installationsList;
        public ViewModelList<ViewModelInstallation> InstallationsList { get { return installationsList; } }

        public ViewModelPlanetFactory(PlanetFactory planetFactory)
        {
            this.planetFactory = planetFactory;
            installationsList = new ViewModelList<ViewModelInstallation>();
            UpdateInstallationList();
        }
        private void UpdateInstallationList()
        {
            installationsList.Clear();
            foreach (Installation inst in planetFactory.Installations)
                installationsList.Add(new ViewModelInstallation(inst));
            MemberUpdated();
        }

        public void SelectInstallation(string instType)
        {
            planetFactory.SelectInstallation(instType);
            UpdateInstallationList();
        }
        public void RandomInstallation(string instType)
        {
            planetFactory.RandomInstallation(instType);
            UpdateInstallationList();
        }
        public void SelectParameter(string param)
        {
            planetFactory.SelectParameter(param);
            if (param == "Installations")
                UpdateInstallationList();
            MemberUpdated();
        }
        public void RandomParameter(string param)
        {
            planetFactory.RandomParameter(param);
            if (param == "Installations")
                UpdateInstallationList();
            MemberUpdated();
        }
        public void FullRandom()
        {
            planetFactory.FullRandom();
            UpdateInstallationList();
            MemberUpdated();
        }
    }
}
