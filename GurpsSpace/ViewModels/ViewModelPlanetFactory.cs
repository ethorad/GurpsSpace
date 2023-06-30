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
