
namespace GurpsSpace.PlanetCreation
{
    internal interface IPlanetCreator
    {
        string SetName(Planet p);
        (eSize, eSubtype) SetSizeAndSubtype(Planet p);
        eResourceValueCategory SetResourceValueCategory(Planet p);

        double SetAtmosphericMass(Planet p);
        (fAtmosphericConditions, string) SetAtmosphericConditions(Planet p);

        double SetHydrographicCoverage(Planet p);

        int SetAverageSurfaceTempK(Planet p);

        double SetDensity(Planet p);

        double SetGravity(Planet p);

        eSettlementType SetSettlementType(Planet p);

        Species SetLocalSpecies(Planet p);

        int SetLocalTechLevel(Planet p);
    }
}
