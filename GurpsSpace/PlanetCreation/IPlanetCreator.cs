
using System.Collections.Generic;

namespace GurpsSpace.PlanetCreation
{
    internal interface IPlanetCreator
    {
        string GetName(Planet p);
        (eSize, eSubtype) GetSizeAndSubtype(Planet p);
        eResourceValueCategory GetResourceValueCategory(Planet p);

        double GetAtmosphericMass(Planet p);
        (fAtmosphericConditions, string) GetAtmosphericConditions(Planet p);

        double GetHydrographicCoverage(Planet p);

        int GetAverageSurfaceTempK(Planet p);

        double GetDensity(Planet p);

        double GetGravity(Planet p);

        (eSettlementType, int, bool) GetSettlementType(Planet p);

        Species GetLocalSpecies(Planet p);

        (int, eTechLevelRelativity) GetLocalTechLevel(Planet p);

        double GetPopulation(Planet p);

        (eWorldUnityLevel, fGovernmentSpecialConditions) GetWorldGovernance(Planet p);

        eSocietyType GetSocietyType(Planet p);

        int GetControlRating(Planet p);

        double GetTradeVolume(Planet p);

        int GetSpaceportClass(Planet p);

        List<Installation> GetInstallations(Planet p);
        List<Installation> GetInstallation(Planet p, string installationType);
    }
}
