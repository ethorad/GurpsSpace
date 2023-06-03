using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public int Count { get { return Installations.Count; } }
        public void Add(Installation inst)
        {
            installations.Add(new ViewModelInstallation(inst));
        }
        public void Add(ViewModelInstallation inst)
        {
            installations.Add(inst);
        }

        public string this[string typeToSummarise]
        {
            get
            {
                if (typeToSummarise=="all")
                    return Installations.Count.ToString();

                int count = 0;
                foreach(ViewModelInstallation inst in Installations)
                    if (inst.Type==typeToSummarise)
                        count++;
                return count.ToString();
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
        public ViewModelInstallationList()
        {
            installations = new ObservableCollection<ViewModelInstallation>();
            MemberUpdated() ;
        }



    }
}
