using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal class UserSpecies : ISpeciesCreator
    {
        public string? GetName(Species s)
        {
            throw new NotImplementedException();
        }

        public eSpeciesDiet? GetDiet(Species s)
        {
            List<(int, string, string)> options = new List<(int, string, string)>();
            options.Add(((int)eSpeciesDiet.Herbivore, "Herbivore", "Eats plants."));
            options.Add(((int)eSpeciesDiet.Carnivore, "Carnivore", "Eats animals"));
            options.Add(((int)eSpeciesDiet.Omnivore, "Omnivore", "Eats plants and animals"));

            string question = "Select the species' diet type.";

            int? initial = null;
            switch (s.Diet)
            {
                case eSpeciesDiet.Herbivore: initial = 0; break;
                case eSpeciesDiet.Carnivore: initial = 1; break;
                case eSpeciesDiet.Omnivore: initial = 2; break;
            }

            InputRadio inDiag = new InputRadio(question, options, initial);
            if (inDiag.ShowDialog() == true)
            {
                return (eSpeciesDiet)inDiag.Answer.Item1;
            }
            return null;

        }

        public (int?, bool?) GetConsumption(Species s)
        {
            SelectConsumption selectConsumption = new SelectConsumption(s);
            if (selectConsumption.ShowDialog()==true)
            {
                int? consumption = selectConsumption.Consumption;
                bool? doesNotEatOrDrink = selectConsumption.DoesNotEatOrDrink;
                return (consumption, doesNotEatOrDrink);
            }
            return (null, null);
        }

        public double? GetStartingColonyPopulation(Species s)
        {
            string question = "Enter the population size used to start a new colony.  Human default is 10,000.";
            InputString input = new InputString(question,s.StartingColonyPopulation.ToString()??"",true);
            if (input.ShowDialog() == true)
                return double.Parse(input.Answer);
            else
                return null;
        }

        public double? GetAnnualGrowthRate(Species s)
        {
            string question = "Enter the annual percentage increase in population size.  Human default is 2.3%.";
            InputString input = new InputString(question, s.AnnualGrowthRate.ToString() ?? "", true);
            if (input.ShowDialog() == true)
                return double.Parse(input.Answer) / 100;
            else
                return null;
        }
    }
}
