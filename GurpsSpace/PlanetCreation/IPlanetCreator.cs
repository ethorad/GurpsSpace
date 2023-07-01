
using System.Collections.Generic;

namespace GurpsSpace.PlanetCreation
{
    internal interface IPlanetCreator
    {
        string? GetName(PlanetFactory pf);
        (eSize?, eSubtype?) GetSizeAndSubtype(PlanetFactory pf);
        eResourceValueCategory? GetResourceValueCategory(PlanetFactory pf);

        double? GetAtmosphericMass(PlanetFactory pf);
        (fAtmosphericConditions?, string?) GetAtmosphericConditions(PlanetFactory pf);

        double? GetHydrographicCoverage(PlanetFactory pf);

        int? GetAverageSurfaceTemperatureK(PlanetFactory pf);

        double? GetDensity(PlanetFactory pf);

        double? GetGravity(PlanetFactory pf);

        (eSettlementType?, int?, bool?) GetSettlementType(PlanetFactory pf);

        Species? GetLocalSpecies(PlanetFactory pf);

        (int?, eTechLevelRelativity?) GetLocalTechLevel(PlanetFactory pf);

        double? GetPopulation(PlanetFactory pf);

        (eWorldUnityLevel?, fGovernmentSpecialConditions?) GetWorldGovernance(PlanetFactory pf);

        eSocietyType? GetSocietyType(PlanetFactory pf);

        int? GetControlRating(PlanetFactory pf);

        double? GetTradeVolume(PlanetFactory pf);

        int? GetSpaceportClass(PlanetFactory pf);

        List<Installation>? GetInstallations(PlanetFactory pf);
        List<Installation>? GetInstallation(PlanetFactory pf, string installationType);
    }
}
