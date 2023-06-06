
using System.Collections.Generic;

namespace GurpsSpace.PlanetCreation
{
    internal interface IPlanetCreator
    {
        string SetName(ViewModelPlanet p);
        (eSize, eSubtype) SetSizeAndSubtype(ViewModelPlanet p);
        eResourceValueCategory SetResourceValueCategory(ViewModelPlanet p);

        double SetAtmosphericMass(ViewModelPlanet p);
        (fAtmosphericConditions, string) SetAtmosphericConditions(ViewModelPlanet p);

        double SetHydrographicCoverage(ViewModelPlanet p);

        int SetAverageSurfaceTempK(ViewModelPlanet p);

        double SetDensity(ViewModelPlanet p);

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
