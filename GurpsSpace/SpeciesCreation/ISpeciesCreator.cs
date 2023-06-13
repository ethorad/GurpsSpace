using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal interface ISpeciesCreator
    {
        string? GetName(Species s);

        eSpeciesDiet? GetDiet(Species s);

        (int?, bool?) GetConsumption(Species s);
    }
}
