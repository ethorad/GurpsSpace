
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

        double SetGravity(ViewModelPlanet p);

        eSettlementType SetSettlementType(ViewModelPlanet p);

        Species SetLocalSpecies(ViewModelPlanet p);

        int SetLocalTechLevel(ViewModelPlanet p);

        double SetPopulation(ViewModelPlanet p);

        (eWorldUnityLevel, fGovernmentSpecialConditions) SetWorldGovernance(ViewModelPlanet p);

        eSocietyType SetSocietyType(ViewModelPlanet p);

        int SetControlRating(ViewModelPlanet p);

        double SetTradeVolume(ViewModelPlanet p);

        int GetSpaceportClass(Planet p);

        List<Installation> GetInstallations(Planet p);
        List<Installation> GetInstallation(Planet p, string installationType);
    }
}
