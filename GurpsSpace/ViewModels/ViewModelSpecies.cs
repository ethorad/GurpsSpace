using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    public class ViewModelSpecies : ViewModel
    {
        private Species species;
        public Species Species { get { return species; } }
        public string? Name
        {
            get { return species.Name ?? "tbc"; }
            set
            {
                species.Name = value;
                MemberUpdated();
            }
        }
        public string? Description
        {
            get { return species.Description ?? "tbc"; }
            set
            {
                species.Description = value;
                MemberUpdated();
            }
        }
        public string DietString
        {
            get
            {
                if (species.Diet == null)
                    return "tbc";
                else
                    return species.Diet.ToString()!;
            }
        }
        public eSpeciesDiet? Diet
        {
            get { return species.Diet; }
            set
            {
                species.Diet = value;
                MemberUpdated();
            }
        }

        public ViewModelSpecies(Species species)
        {
            this.species = species;
        }
    }
}
