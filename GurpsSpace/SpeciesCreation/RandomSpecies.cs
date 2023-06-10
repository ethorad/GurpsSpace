using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal class RandomSpecies : ISpeciesCreator
    {
        public string GetName(Species s)
        {
            return "Randomish-" + DiceBag.Rand(1, 100);
        }

        public eSpeciesDiet GetDiet(Species s)
        {
            return eSpeciesDiet.Herbivore;
        }
    }
}
