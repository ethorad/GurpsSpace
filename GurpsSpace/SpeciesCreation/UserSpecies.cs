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
    }
}
