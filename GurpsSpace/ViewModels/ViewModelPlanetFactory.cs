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
