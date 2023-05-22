
namespace GurpsSpace
{
    public class PlanetParameters
    {
        private readonly eOverallType overallType; public eOverallType OverallType { get { return overallType; } }
        private readonly eSize size; public eSize Size { get { return size; } }
        private readonly eSubtype subType; public eSubtype SubType { get { return subType; } }
        private readonly bool hasAtmosphere; public bool HasAtmosphere { get { return hasAtmosphere; } }
        private readonly (fAtmosphericConditions, string) atmosphereA; public (fAtmosphericConditions, string) AtmosphereA { get { return  atmosphereA; } }
        private readonly int atmosphereANumber; public int AtmosphereANumber { get { return atmosphereANumber; } }
        private readonly (fAtmosphericConditions, string) atmosphereB; public (fAtmosphericConditions, string) AtmosphereB { get { return atmosphereB; } }
        private readonly eLiquid liquid; public eLiquid Liquid { get { return liquid; } }
        private readonly double hydroMin; public double HydroMin { get { return hydroMin; } }
        private readonly double hydroMax; public double HydroMax { get { return hydroMax; } }
        private readonly string hydroDesc; public string HydroDesc { get { return hydroDesc; } }
        private readonly int hydroDice; public int HydroDice { get { return hydroDice; } }
        private readonly int hydroAdj; public int HydroAdj { get { return hydroAdj; } }
        private readonly int minSurfaceTemperatureK; public int MinSurfaceTemperatureK { get { return minSurfaceTemperatureK; } }
        private readonly int maxSurfaceTemperatureK; public int MaxSurfaceTemperatureK { get { return maxSurfaceTemperatureK; } }
        private readonly int stepSurfaceTemperatureK; public int StepSurfaceTemperatureK { get { return stepSurfaceTemperatureK; } }
        private readonly double blackbodyAbsorption; public double BlackbodyAbsorption { get { return blackbodyAbsorption; } }
        private readonly double blackbodyGreenhouse; public double BlackbodyGreenhouse { get { return blackbodyGreenhouse; } }
        private readonly eCoreType coreType; public eCoreType CoreType { get { return coreType; } }
        private readonly double minSizeFactor; public double MinSizeFactor { get { return minSizeFactor; } }
        private readonly double maxSizeFactor; public double MaxSizeFactor { get { return maxSizeFactor; } }
        private readonly double pressureFactor; public double PressureFactor { get { return pressureFactor; } }

        public PlanetParameters(eOverallType overallType, eSize size, eSubtype subType,
            bool hasAtmosphere, (fAtmosphericConditions, string) atmosphereA, int atmosphereANumber, (fAtmosphericConditions, string) atmosphereB,
            eLiquid liquid, double hydroMin, double hydroMax, string hydroDesc, int hydroDice, int hydroAdj,
            int tempMin, int tempMax, int tempStep, double blackbodyAbsorption, double blackbodyGreenhouse,
            eCoreType coreType, double minSizeFactor, double maxSizeFactor,
            double pressureFactor)
        {
            this.overallType = overallType;
            this.size = size;
            this.subType = subType;
            this.hasAtmosphere = hasAtmosphere;
            this.atmosphereA = atmosphereA;
            this.atmosphereANumber = atmosphereANumber;
            this.atmosphereB = atmosphereB;
            this.liquid = liquid;
            this.hydroMin = hydroMin;
            this.hydroMax = hydroMax;
            this.hydroDesc = hydroDesc;
            this.hydroDice = hydroDice;
            this.hydroAdj = hydroAdj;
            this.minSurfaceTemperatureK = tempMin;
            this.maxSurfaceTemperatureK = tempMax;
            this.stepSurfaceTemperatureK = tempStep;
            this.blackbodyAbsorption = blackbodyAbsorption;
            this.blackbodyGreenhouse = blackbodyGreenhouse;
            this.coreType = coreType;
            this.minSizeFactor = minSizeFactor;
            this.maxSizeFactor = maxSizeFactor;
            this.pressureFactor = pressureFactor;
        }
    }
}
