using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.PlanetCreation
{
    public class ViewModelInstallationList : ViewModel
    {
        private ObservableCollection<ViewModelInstallation> installations;
        public ObservableCollection<ViewModelInstallation> Installations
        {
            get { return installations; }
            set 
            { 
                installations = value;
                MemberUpdated();
            }
        }

        public ViewModelInstallationList(List<ViewModelInstallation> instLst)
        {
            installations = new ObservableCollection<ViewModelInstallation>(instLst);
            MemberUpdated() ;
        }
        public ViewModelInstallationList(List<Installation> instLst)
        {
            installations = new ObservableCollection<ViewModelInstallation>();
            foreach (Installation inst in instLst)
                installations.Add(new ViewModelInstallation(inst));
            MemberUpdated();
        }

    }
}
