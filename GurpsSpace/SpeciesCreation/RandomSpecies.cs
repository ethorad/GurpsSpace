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

        public (int?, bool?) GetConsumption(Species s)
        {
            // have come up with some random rules for consumption stats
            // 10% = doesn't eat or drink
            // 20% = reduced consumption
            // 20% = increased consumption
            // 50% = normal

            // for reduced or increased consumption, roll 1d6:
            // 1-3 = level 1
            // 4-5 = level 2
            // 6 = level 3

            int roll;
            bool? noEatOrDrink;
            int? consumption;
            int level = 0;
            roll = DiceBag.Roll(1);
            switch(roll)
            {
                case 1:
                case 2:
                case 3:
                    level = 1;
                    break;
                case 4:
                case 5:
                    level = 2;
                    break;
                case 6:
                    level = 3;
                    break;
            }

            roll = DiceBag.Rand(1, 10);
            switch (roll)
            {
                case 1:
                    noEatOrDrink = true;
                    consumption = 0;
                    break;
                case 2:
                case 3:
                    noEatOrDrink = false;
                    consumption = -level;
                    break;
                    case 4:
                case 5:
                    noEatOrDrink = false;
                    consumption = level;
                    break;
                default:
                    noEatOrDrink = false;
                    consumption = 0;
                    break;
            }
            return (consumption, noEatOrDrink);

        }
    }
}
