using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    public class ViewModelTrait : ViewModel
    {
        private Trait trait;
        public Trait Trait { get { return trait; } }

        private ObservableCollection<ViewModelTrait> items;
        public ObservableCollection<ViewModelTrait> Items { get { return items; } }

        public ViewModelTrait(Trait trait)
        {
            this.trait = trait;
            items = new ObservableCollection<ViewModelTrait>();
            foreach(Trait t in trait.SubTraits)
            {
                items.Add(new ViewModelTrait(t));
            }
            MemberUpdated();
        }

        public override string SummaryType { get { return TraitType; } }

        public string TraitType { get { return trait.TraitType.ToString(); } }
        public string NameString
        { 
            get 
            {
                if (trait.Specialty == "")
                    return trait.Name;
                else
                    return trait.Name + ": " + trait.Specialty;
            } 
        }
        public string LevelString
        {
            get
            {
                if (trait.Level == 0)
                    return "";
                else
                    return "Lvl: "+trait.Level.ToString();
            }
        }
        public string CostString
        {
            get
            {
                return "[" + trait.Cost.ToString("N0") + "]";
            }
        }

    }
}
