using System;

namespace GurpsSpace
{
    static internal class DiceBag
    {
        static private Random rng;

        static DiceBag()
        {
            rng = new Random();
        }

        static public int Rand(int minVal, int maxVal)
        {
            // return a random number between minVal and maxVal inclusive
            return rng.Next(minVal, maxVal + 1);
        }
        static public int Roll(int numDice, int sides)
        {
            // roll the specified number of dice with the given sides
            int result = 0;
            for (int i = 0; i < numDice; i++)
                result += Rand(1, sides);
            return result;
        }
        static public int Roll(int numDice)
        {
            // roll a specified number of 6-sided dice
            return Roll(numDice, 6);
        }
    }
}
