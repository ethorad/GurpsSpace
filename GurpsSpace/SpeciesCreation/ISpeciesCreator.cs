using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.SpeciesCreation
{
    internal interface ISpeciesCreator
    {
        string? GetName(SpeciesFactory sf);

        eSpeciesDiet? GetDiet(SpeciesFactory sf);

        (int?, bool?) GetConsumption(SpeciesFactory sf);

        double? GetStartingColonyPopulation(SpeciesFactory sf);

        double? GetAnnualGrowthRate(SpeciesFactory sf);

        double? GetAffinityMultiplier(SpeciesFactory sf);

        (eLifeChemistry?, Trait?) GetLifeChemistry(SpeciesFactory sf);
    }
}
