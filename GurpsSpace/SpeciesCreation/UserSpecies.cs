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
            List<(string, string)> options = new List<(string, string)>();
            options.Add(("Herbivore", "Eats plants."));
            options.Add(("Carnivore", "Eats animals"));
            options.Add(("Omnivore", "Eats plants and animals"));

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
                switch (inDiag.Selected)
                {
                    case 0: return eSpeciesDiet.Herbivore;
                    case 1: return eSpeciesDiet.Carnivore;
                    case 2: return eSpeciesDiet.Omnivore;
                }
            }
            return null;

        }
    }
}
