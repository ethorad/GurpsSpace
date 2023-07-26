using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal class RandomSpecies : ISpeciesCreator
    {
        public string? GetName(SpeciesFactory sf)
        {
            return "Randomish-" + DiceBag.Rand(1, 100);
        }

        public eSpeciesDiet? GetDiet(SpeciesFactory sf)
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

        public (int?, bool?) GetConsumption(SpeciesFactory sf)
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

        public double? GetStartingColonyPopulation(SpeciesFactory sf)
        {
            // want to choose a random number from say 1,000 to 100,000
            // human average of 10,000 in the middle
            // do it exponentially to reflect the way population growth works
            // so say:
            // roll 0 = 1,000
            // roll 10 = 10,000
            // roll 20 = 100,000

            // to calculate the effect of +1 on the roll, calculate
            // (multiple for +X on the roll) ^ (1 / X) - 1
            // i.e. for here, we have *100 for a +20
            // so increase for +1 is 100^(1/20)-1 = 25.9%

            // so generate a random number 0-20
            // then start population is 1,000 * (1 + 25.9%) ^ roll

            double minStartPop = 1000;
            double maxStartPop = 100000;
            int maxRoll = 20;

            double increase = Math.Pow((maxStartPop / minStartPop), (1 / (double)maxRoll)) - 1;

            int roll = DiceBag.Rand(0, maxRoll);
            double pop = minStartPop * Math.Pow(1 + increase, roll);
            pop = RuleBook.RoundToSignificantFigures(pop, 2);

            return pop;

        }

        public double? GetAnnualGrowthRate(SpeciesFactory sf)
        {
            // human is 2.3%
            // choose a random number between 0.5% and 5.0%
            return Math.Round((double)DiceBag.Rand(5, 50) / 10 / 100, 3);
        }

        public double? GetAffinityMultiplier(SpeciesFactory sf)
        {
            // assume a value between 1 + 3d/10 gives a value from 1.3 to 2.8 centered around 2
            // and 2 is the Human default

            return 1 + (double)DiceBag.Roll(3) / 10;
        }

        public (eLifeChemistry?, List<Trait>?) GetLifeChemistry(SpeciesFactory sf)
        {
            int roll = DiceBag.Roll(3);
            eLifeChemistry chem = RuleBook.LifeChemistry[roll];
            chem = eLifeChemistry.Hydrogen;

            List<Trait> traitLst = new List<Trait>();
            Trait t;

            switch (chem)
            {
                case eLifeChemistry.Hydrogen:
                    traitLst.Add(new Trait(eTrait.DecreasedTimeRate));
                    t = new Trait(eTrait.ReducedBasicSpeed);
                    t.Level = 4;
                    traitLst.Add(t);
                    break;
            }

            return (chem, traitLst);
        }
    }
}
