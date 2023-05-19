using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GurpsSpace
{
    static internal class RuleBook
    {
        static public readonly int EarthDiameterInMiles = 7930;

        static public Dictionary<(eSize, eSubtype), PlanetParameters> PlanetParams;
        static public List<TechLevelParameters> TechLevelParams;

        static public IndexedTable1D<eOverallType> OverallType = new IndexedTable1D<eOverallType>(new eOverallType[]
        {
            eOverallType.Hostile, // 7 (as 7 or less is hostile)
            eOverallType.Barren, // 8
            eOverallType.Barren, // 9
            eOverallType.Barren, // 10
            eOverallType.Barren, // 11
            eOverallType.Barren, // 12
            eOverallType.Barren, // 13
            eOverallType.Garden // 14+
        }, 7);
        static public IndexedTable1D<(eSize, eSubtype)> HostileWorlds = new IndexedTable1D<(eSize, eSubtype)>(new (eSize, eSubtype)[]
        {
            (eSize.Standard, eSubtype.Chthonian), // 3
            (eSize.Standard, eSubtype.Chthonian), // 4
            (eSize.Standard, eSubtype.Greenhouse), // 5
            (eSize.Standard, eSubtype.Greenhouse), // 6
            (eSize.Tiny, eSubtype.Sulphur), // 7
            (eSize.Tiny, eSubtype.Sulphur), // 8
            (eSize.Tiny, eSubtype.Sulphur), // 9
            (eSize.Standard, eSubtype.Ammonia), // 10
            (eSize.Standard, eSubtype.Ammonia), // 11
            (eSize.Standard, eSubtype.Ammonia), // 12
            (eSize.Large, eSubtype.Ammonia), // 13
            (eSize.Large, eSubtype.Ammonia), // 14
            (eSize.Large, eSubtype.Greenhouse), // 15
            (eSize.Large, eSubtype.Greenhouse), // 16
            (eSize.Large, eSubtype.Chthonian), // 17
            (eSize.Large, eSubtype.Chthonian) // 18
        }, 3);
        static public IndexedTable1D<(eSize, eSubtype)> BarrenWorlds = new IndexedTable1D<(eSize, eSubtype)>(new (eSize, eSubtype)[]
        {
            (eSize.Small, eSubtype.Hadean), // 3
            (eSize.Small, eSubtype.Ice), // 4
            (eSize.Small, eSubtype.Rock), // 5
            (eSize.Small, eSubtype.Rock), // 6
            (eSize.Tiny, eSubtype.Rock), // 7
            (eSize.Tiny, eSubtype.Rock), // 8
            (eSize.Tiny, eSubtype.Ice), // 9
            (eSize.Tiny, eSubtype.Ice), // 10
            (eSize.AsteroidBelt, eSubtype.AsteroidBelt), // 11
            (eSize.AsteroidBelt, eSubtype.AsteroidBelt), // 12
            (eSize.Standard, eSubtype.Ocean), // 13
            (eSize.Standard, eSubtype.Ocean), // 14
            (eSize.Standard, eSubtype.Ice), // 15
            (eSize.Standard, eSubtype.Hadean), // 16
            (eSize.Large, eSubtype.Ocean), // 17
            (eSize.Large, eSubtype.Ice) // 18
        }, 3);
        static public IndexedTable1D<(eSize, eSubtype)> GardenWorlds = new IndexedTable1D<(eSize, eSubtype)>(new (eSize, eSubtype)[]
        {
            (eSize.Standard, eSubtype.Garden), // 16
            (eSize.Large, eSubtype.Garden) // 17
        }, 16);

        static public eClimateType ClimateType(int tempK)
        {
            int f = KtoF(tempK);
            if (f < -20)
                return eClimateType.Frozen;
            else if (f < 0)
                return eClimateType.VeryCold;
            else if (f < 20)
                return eClimateType.Cold;
            else if (f < 40)
                return eClimateType.Chilly;
            else if (f < 60)
                return eClimateType.Cool;
            else if (f < 80)
                return eClimateType.Normal;
            else if (f < 100)
                return eClimateType.Warm;
            else if (f < 120)
                return eClimateType.Tropical;
            else if (f < 140)
                return eClimateType.Hot;
            else if (f < 160)
                return eClimateType.VeryHot;
            else
                return eClimateType.Infernal;
        }

        static public IndexedTable1D<double> DensityIcyCore = new IndexedTable1D<double>(new double[]
        {
            0.3, // 6
            0.4, // 7
            0.4, // 8
            0.4, // 9
            0.4, // 10
            0.5, // 11
            0.5, // 12
            0.5, // 13
            0.5, // 14
            0.6, // 15
            0.6, // 16
            0.6, // 17
            0.7 // 18
        }, 6);
        static public IndexedTable1D<double> DensitySmallIronCore = new IndexedTable1D<double>(new double[]
        {
            0.6, // 6
            0.7, // 7
            0.7, // 8
            0.7, // 9
            0.7, // 10
            0.8, // 11
            0.8, // 12
            0.8, // 13
            0.8, // 14
            0.9, // 15
            0.9, // 16
            0.9, // 17
            1.0 // 18
        }, 6);
        static public IndexedTable1D<double> DensityLargeIronCore = new IndexedTable1D<double>(new double[]
        {
            0.8, // 6
            0.9, // 7
            0.9, // 8
            0.9, // 9
            0.9, // 10
            1.0, // 11
            1.0, // 12
            1.0, // 13
            1.0, // 14
            1.1, // 15
            1.1, // 16
            1.1, // 17
            1.2 // 18
        }, 6);

        static public IndexedTable1D<eResourceValueCategory> ResourceValueCategoryPlanet = new IndexedTable1D<eResourceValueCategory>(new eResourceValueCategory[]
            {
                eResourceValueCategory.VeryPoor,     // 3
                eResourceValueCategory.VeryPoor,     // 4
                eResourceValueCategory.Poor,         // 5
                eResourceValueCategory.Poor,         // 6
                eResourceValueCategory.Poor,         // 7
                eResourceValueCategory.Average,      // 8
                eResourceValueCategory.Average,      // 9
                eResourceValueCategory.Average,      // 10
                eResourceValueCategory.Average,      // 11
                eResourceValueCategory.Average,      // 12
                eResourceValueCategory.Average,      // 13
                eResourceValueCategory.Abundant,     // 14
                eResourceValueCategory.Abundant,     // 15
                eResourceValueCategory.Abundant,     // 16
                eResourceValueCategory.VeryAbundant, // 17
                eResourceValueCategory.VeryAbundant  // 18
            }, 3);
        static public IndexedTable1D<eResourceValueCategory> ResourceValueCategoryAsteroidBelt = new IndexedTable1D<eResourceValueCategory>(new eResourceValueCategory[]
            {
                eResourceValueCategory.Worthless,    // 3
                eResourceValueCategory.VeryScant,    // 4
                eResourceValueCategory.Scant,        // 5
                eResourceValueCategory.VeryPoor,     // 6
                eResourceValueCategory.VeryPoor,     // 7
                eResourceValueCategory.Poor,         // 8
                eResourceValueCategory.Poor,         // 9
                eResourceValueCategory.Average,      // 10
                eResourceValueCategory.Average,      // 11
                eResourceValueCategory.Abundant,     // 12
                eResourceValueCategory.Abundant,     // 13
                eResourceValueCategory.VeryAbundant, // 14
                eResourceValueCategory.VeryAbundant, // 15
                eResourceValueCategory.Rich,         // 16
                eResourceValueCategory.VeryRich,     // 17
                eResourceValueCategory.Motherlode    // 18
            }, 3);

        static public IndexedTable1D<string> TechLevelTable = new IndexedTable1D<string>(new string[]
        {
            "Primitive", // 3
            "Standard -3", // 4
            "Standard -2", // 5
            "Standard -1", // 6
            "Standard -1", // 7
            "Standard (Delayed)", // 8
            "Standard (Delayed)", // 9
            "Standard (Delayed)", // 10
            "Standard (Delayed)", // 11
            "Standard", // 12
            "Standard", // 13
            "Standard", // 14
            "Standard", // 15
            "Standard (Advanced)" // 16
        }, 3);

        static public IndexedTable1D<double> CarryingCapacityMultiplierByAffinity = new IndexedTable1D<double>(
            new double[] { 0.03, 0.06, 0.13, 0.25, 0.5, 1, 2, 4, 8, 15, 30, 60, 130, 250, 500, 1000 }, -5);

        static public IndexedTable1D<ulong> OutpostPopulation = new IndexedTable1D<ulong>(
            new ulong[] { 100, 150, 250, 400, 600, 1000, 1500, 2500, 4000, 6000, 10000, 15000, 25000, 40000, 60000, 100000 }, 3);



        static public ePressureCategory PressureCategory(double pressure)
        {
            switch (pressure)
            {
                case 0:
                    return ePressureCategory.None;
                case < 0.02:
                    return ePressureCategory.Trace;
                case <= 0.5:
                    return ePressureCategory.VeryThin;
                case <= 0.8:
                    return ePressureCategory.Thin;
                case <= 1.2:
                    return ePressureCategory.Standard;
                case <= 1.5:
                    return ePressureCategory.Dense;
                case <= 10:
                    return ePressureCategory.VeryDense;
                default:
                    return ePressureCategory.Superdense;
            }
        }

        static public int KtoC(int k)
        {
            // convert Kelvin to Celsius
            return k - 273;
        }
        static public int KtoF(int k)
        {
            // covert Kelvin to Farenheit
            int c = KtoC(k);
            float f = (float)c * 1.8F + 32F;
            return (int)(Math.Round(f, 0));
        }
        public static double RoundToSignificantFigures(double value, int sigFig)
        {
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            value /= magnitude;
            value = Math.Round(value, sigFig);
            value *= magnitude;
            return value;
        }
        public static long RoundToSignificantFigures(long value, int sigFig)
        {
            int magnitude = (int)Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1 - sigFig);
            value /= magnitude;
            value = (int)(Math.Round((double)value, sigFig));
            value *= magnitude;
            return value;
        }
        public static ulong RoundToSignificantFigures(ulong value, int sigFig)
        {
            ulong magnitude = (ulong)Math.Pow(10, Math.Floor(Math.Log10(value)) + 1 - sigFig);
            value /= magnitude;
            value = (ulong)(Math.Round((double)value, sigFig));
            value *= magnitude;
            return value;
        }

        static RuleBook()
        {
            PlanetParams = new Dictionary<(eSize, eSubtype), PlanetParameters>();
            SetUpPlanetParameters();
            TechLevelParams = new List<TechLevelParameters>();
            SetUpTechLevelParameters();

        }

        static private void SetUpPlanetParameters()
        {

            PlanetParameters pp;

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.AsteroidBelt, eSubtype.AsteroidBelt,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0, // hydro
                140, 500, 24, 0.97, 0, // temp
                eCoreType.None, 0, 0, // density
                0); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Hostile, eSize.Standard, eSubtype.Ammonia,
                true, (fAtmosphericConditions.Suffocating
                    | fAtmosphericConditions.LethallyToxic
                    | fAtmosphericConditions.Corrosive, "Nitrogen, large quantities of ammonia and methane. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.Ammonia, 0.2, 1, "Oceans of liquid ammonia mixed with some water, with freezing point much lower than pure ammonia or water.  ", 2, 0,
                140, 215, 5, 0.84, 0.2, // temp
                eCoreType.Icy, 0.03, 0.065, // density
                1); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Hostile, eSize.Large, eSubtype.Ammonia,
                true, (fAtmosphericConditions.Suffocating
                    | fAtmosphericConditions.LethallyToxic
                    | fAtmosphericConditions.Corrosive, "Helium with large quantities of ammonia and methane. "), 20, (fAtmosphericConditions.None, "None ."),// atmos
                eLiquid.Ammonia, 0.2, 1, "Oceans of liquid ammonia mixed with some water, with freezing point much lower than pure ammonia or water.  ", 2, 0,
                140, 215, 5, 0.84, 0.2, // temp
                eCoreType.Icy, 0.065, 0.091, // density
                5); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Hostile, eSize.Standard, eSubtype.Chthonian,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                500, 950, 30, 0.97, 0, // temp
                eCoreType.LargeIron, 0.03, 0.065, // density
                0.0005); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Hostile, eSize.Large, eSubtype.Chthonian,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                500, 950, 30, 0.97, 0, // temp
                eCoreType.LargeIron, 0.065, 0.091, // density
                0.0005); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Garden, eSize.Standard, eSubtype.Garden,
                true, (fAtmosphericConditions.None, "Dominated by nitrogen with significant free oxygen. "), 11, (fAtmosphericConditions.Marginal, "Dominated by nitrogen with significant free oxygen. "), // atmos
                eLiquid.Water, 0.5, 1, "Likely to have a large amount of water coverage.  ", 1, 4,
                250, 340, 6, 0, 0.16, // temp
                eCoreType.LargeIron, 0.03, 0.065, // density
                1); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Garden, eSize.Large, eSubtype.Garden,
                true, (fAtmosphericConditions.None, "Dominated by nitrogen with significant free oxygen. "), 11, (fAtmosphericConditions.Marginal, "Dominated by nitrogen with significant free oxygen. "), // atmos
                eLiquid.Water, 0.5, 1, "Likely to have a large amount of water coverage.  ", 1, 4,
                250, 340, 6, 0, 0.16, // temp
                eCoreType.LargeIron, 0.065, 0.091, // density
                5); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Hostile, eSize.Standard, eSubtype.Greenhouse,
                true, (fAtmosphericConditions.Suffocating
                    | fAtmosphericConditions.LethallyToxic
                    | fAtmosphericConditions.Corrosive,"If dry: Dominated by carbon dioxide. If wet: nitrogen and water vapour. "),20,(fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.Acidic, 0, 0.5, "Likely to have no coverage, but may have up to 50% acidic oceans.  ", 2, -7,
                500, 950, 30, 0.77, 2, // temp
                eCoreType.LargeIron, 0.03, 0.065, // density
                100); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Hostile, eSize.Large, eSubtype.Greenhouse,
                true, (fAtmosphericConditions.Suffocating
                    | fAtmosphericConditions.LethallyToxic
                    | fAtmosphericConditions.Corrosive, "If dry: Dominated by carbon dioxide. If wet: nitrogen and water vapour. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.Acidic, 0, 0.5, "Likely to have no coverage, but may have up to 50% acidic oceans.  ", 2, -7,
                500, 950, 30, 0.77, 2, // temp
                eCoreType.LargeIron, 0.065, 0.091, // density
                500); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Small, eSubtype.Hadean,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                50, 80, 2, 0.67, 0, // temp
                eCoreType.Icy, 0.024, 0.03, // density
                0); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Standard, eSubtype.Hadean,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                50, 80, 2, 0.67, 0, // temp
                eCoreType.Icy, 0.03, 0.065, // density
                0); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Tiny, eSubtype.Ice,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                80, 140, 4, 0.86, 0, // temp
                eCoreType.Icy, 0.004, 0.024, // density
                0); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Small, eSubtype.Ice,
                true, (fAtmosphericConditions.Suffocating | fAtmosphericConditions.MildlyToxic, "Nitrogen and methane. "), 15, (fAtmosphericConditions.Suffocating | fAtmosphericConditions.HighlyToxic, "Nitrogen and methane. "),  // atmos
                eLiquid.Hydrocarbons, 0.3, 0.8, "Has liquid hydrocarbons rather than water.  ", 1, 2,
                80, 140, 4, 0.93, 0.1, // temp
                eCoreType.Icy, 0.024, 0.03, // density
                10);
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Standard, eSubtype.Ice,
                true, (fAtmosphericConditions.Suffocating,"Carbon dioxide and nitrogen. "),12, (fAtmosphericConditions.Suffocating | fAtmosphericConditions.MildlyToxic,
                        "Carbon dioxide and nitrogen with toxins from volcanic activity or other processes. "), // atmos
                eLiquid.Water, 0, 0.2, "Generally no liquid, but may have a small amout of water.  ", 2, -10,
                80, 230, 10, 0.86, 0.2, // temp
                eCoreType.LargeIron, 0.03, 0.065, // density
                1); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Large, eSubtype.Ice,
                true, (fAtmosphericConditions.Suffocating
                    | fAtmosphericConditions.HighlyToxic,
                    "Helium and nitrogen, with toxins from volcanic activity or other processes. "), 20, (fAtmosphericConditions.None, "None. "),// atmos
                eLiquid.Water, 0, 0.2, "Generally no liquid, but may have a small amout of water.  ", 2, -10,
                80, 230, 10, 0.86, 0.2, // temp
                eCoreType.LargeIron, 0.065, 0.091, // density
                5); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Standard, eSubtype.Ocean,
                true, (fAtmosphericConditions.Suffocating,"Carbon dioxide and nitrogen. "), 12, (fAtmosphericConditions.Suffocating | fAtmosphericConditions.MildlyToxic,
                        "Carbon dioxide and nitrogen with toxins from volcanic activity or other processes. "), // atmos
                eLiquid.Water, 0.5, 1, "Likely to have a large amount of water coverage.  ", 1, 4,
                250, 340, 6, 0, 0.16, // temp
                eCoreType.LargeIron, 0.03, 0.065, // density
                1); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Large, eSubtype.Ocean,
                true, (fAtmosphericConditions.Suffocating
                    | fAtmosphericConditions.HighlyToxic,
                    "Helium and nitrogen, with toxins from volcanic activity or other processes. "), 20, (fAtmosphericConditions.None, "None. "),// atmos
                eLiquid.Water, 0.7, 1, "Likely to have a huge amounts of water coverage, with oceans up to hundreds of miles deep.  ", 1, 6,
                250, 340, 6, 0, 0.16, // temp
                eCoreType.LargeIron, 0.065, 0.091, // density
                5);
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Tiny, eSubtype.Rock,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                140, 500, 24, 0.97, 0, // temp
                eCoreType.SmallIron, 0.004, 0.024, // density
                0); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Barren, eSize.Small, eSubtype.Rock,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                140, 500, 24, 0.96, 0, // temp
                eCoreType.SmallIron, 0.024, 0.03, // density
                0.0005); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);

            pp = new PlanetParameters(
                eOverallType.Hostile, eSize.Tiny, eSubtype.Sulphur,
                false, (fAtmosphericConditions.None, "None. "), 20, (fAtmosphericConditions.None, "None. "), // atmos
                eLiquid.None, 0, 0, "No surface liquids", 0, 0,
                80, 140, 4, 0.77, 0, // temp
                eCoreType.Icy, 0.004, 0.024, // density
                0); // pressure
            PlanetParams.Add((pp.Size, pp.SubType), pp);
        }

        static private void SetUpTechLevelParameters()
        {
            // Note - TechLevelParams is a list, so need to add them in the right order
            // otherwise indexing to find the right parameters won't work

            TechLevelParameters tlp;

            tlp = new TechLevelParameters(0, "Stone Age",
                7500, // cash
                10000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(1, "Bronze Age", 
                7800, // cash
                100000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(2, "Iron Age", 
                8100, // cash
                500000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(3, "Medieval", 
                8400, // cash
                600000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(4, "Age of Sail", 
                9600, // cash
                700000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(5, "Industrial Revolution", 
                13000, // cash
                2500000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(6, "Mechanised Age", 
                19000, // cash
                5000000);  // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(7, "Nuclear Age", 
                25000, // cash
                7500000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(8, "Digital Age", 
                31000, // cash
                10000000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(9, "Microtech Age", 
                43000, // cash
                15000000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(10, "Robotic Age", 
                67000, // cash
                20000000); // carry cap
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(11, "Age of Exotic Matter", 
                97000, // cash
                30000000); // carry cap - GM option, set to 30m
            TechLevelParams.Add(tlp);

            tlp = new TechLevelParameters(12, "Far Future", 
                130000, // cash
                45000000); // carry cap - GM option, set to 45m
            TechLevelParams.Add(tlp);
        }

    }
}
