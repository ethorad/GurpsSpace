using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    public class ViewModelTrait : ViewModel
    {
        private Trait trait;
        public Trait Trait { get { return trait; } }

        public ViewModelTrait(Trait trait)
        {
            this.trait = trait;
            MemberUpdated();
        }

        public override string SummaryType { get { return TraitType; } }

        public string TraitType { get { return trait.TraitType.ToString(); } }
        public string Name { get { return trait.Name; } }
        public string SpecialtyString
        {
            get
            {
                if (trait.Specialty == "")
                    return "";
                else
                    return "(" + trait.Specialty + ")";
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
