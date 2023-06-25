using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal class ViewModelSpeciesList : ViewModel
    {
        private ObservableCollection<ViewModelSpecies> species;
        public ObservableCollection<ViewModelSpecies> Species
        {
            get { return species; }
        }
        public int Count { get { return species.Count; } }
        public void Add(Species s)
        {
            species.Add(new ViewModelSpecies(s));
            MemberUpdated();
        }
        public void Add(ViewModelSpecies vmSpecies)
        {
            species.Add(vmSpecies);
            MemberUpdated();
        }

        public ViewModelSpeciesList(List<ViewModelSpecies> speciesLst)
        {
            species = new ObservableCollection<ViewModelSpecies>(speciesLst);
            MemberUpdated();
        }
        public ViewModelSpeciesList(List<Species> speciesLst)
        {
            species = new ObservableCollection<ViewModelSpecies>();
            foreach (Species s in speciesLst)
                species.Add(new ViewModelSpecies(s));
            MemberUpdated();
        }
        public ViewModelSpeciesList()
        {
            species = new ObservableCollection<ViewModelSpecies>();
            MemberUpdated();
        }
    }
}
