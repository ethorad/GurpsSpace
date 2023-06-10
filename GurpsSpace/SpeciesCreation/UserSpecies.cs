using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal class UserSpecies : ISpeciesCreator
    {
        public string GetName(Species s)
        {
            throw new NotImplementedException();
        }


        public eSpeciesDiet GetDiet(Species s)
        {
            return eSpeciesDiet.Carnivore;
        }
    }
}
