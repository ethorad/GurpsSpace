using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal class RandomSpecies : ISpeciesCreator
    {
        public string? GetName(Species s)
        {
            return "Randomish-" + DiceBag.Rand(1, 100);
        }

        public eSpeciesDiet? GetDiet(Species s)
        {
            int n = DiceBag.Rand(0, 2);
            switch (n)
            {
                case 0:
                    return eSpeciesDiet.Herbivore;
                case 1:
                    return eSpeciesDiet.Carnivore;
                case 2:
                    return eSpeciesDiet.Omnivore;
            }
            // shouldn't ever get here
            return null;
        }
    }
}
