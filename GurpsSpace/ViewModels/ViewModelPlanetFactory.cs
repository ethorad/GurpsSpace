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

        public bool HasLiquid
        {
            get
            {
                if (planetFactory.HasLiquid == null)
                    return false;
                else
                    return planetFactory.HasLiquid ?? false;
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

        private ViewModelList<ViewModelInstallation> installationsList;
        public ViewModelList<ViewModelInstallation> InstallationsList
        {
            get { return installationsList; }
            //set
            //{
            //    installationsList = value;
            //    planetFactory.Installations.Clear();
            //    foreach (ViewModelInstallation vmInst in installationsList.Items)
            //        planetFactory.Installations.Add(vmInst.Installation);
            //    MemberUpdated();
            //}
        }

        public ViewModelPlanetFactory(PlanetFactory pf)
        {
            planetFactory = pf;
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
            MemberUpdated();
        }
        public void RandomParameter(string param)
        {
            planetFactory.RandomParameter(param);
            MemberUpdated();
        }
    }
}
