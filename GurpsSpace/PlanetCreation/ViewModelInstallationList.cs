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
        private ObservableCollection<Installation> installations;
        public ObservableCollection<Installation> Installations
        {
            get { return installations; }
            set 
            { 
                installations = value;
                MemberUpdated();
            }
        }

        public ViewModelInstallationList(List<Installation> inst)
        {
            installations = new ObservableCollection<Installation>(inst);
            MemberUpdated() ;
        }

    }
}
