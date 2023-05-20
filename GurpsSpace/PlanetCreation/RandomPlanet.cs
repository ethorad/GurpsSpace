
using System;

namespace GurpsSpace.PlanetCreation
{
    internal class RandomPlanet : IPlanetCreator
    {
        public string SetName(ViewModelPlanet p)
        {
            string name = "Randomia-" + DiceBag.Rand(1, 100);
            p.Name = name;
            return p.Name;
        }

        public (eSize, eSubtype) SetSizeAndSubtype(ViewModelPlanet p)
        {
            eSize size;
            eSubtype subtype;

            eOverallType overallType = RuleBook.OverallType[DiceBag.Roll(3)];
            switch (overallType)
            {
                case eOverallType.Hostile:
                    (size, subtype) = RuleBook.HostileWorlds[DiceBag.Roll(3)];
                    break;
                case eOverallType.Barren:
                    (size, subtype) = RuleBook.BarrenWorlds[DiceBag.Roll(3)];
                    break;
                case eOverallType.Garden:
                    (size, subtype) = RuleBook.GardenWorlds[DiceBag.Roll(3)];
                    break;
                case eOverallType.None:
                    (size, subtype) = (eSize.None, eSubtype.None);
                    break;
                default:
                    (size, subtype) = (eSize.None, eSubtype.None);
                    break;
            }
            (p.Size, p.Subtype) = (size, subtype);
            return (p.Size, p.Subtype);
        }

        public eResourceValueCategory SetResourceValueCategory(ViewModelPlanet p)
        {
            int roll = DiceBag.Roll(3);
            if (p.IsPlanet)
                p.ResourceValueCategory = RuleBook.ResourceValueCategoryPlanet[roll];
            else
                p.ResourceValueCategory = RuleBook.ResourceValueCategoryAsteroidBelt[roll];
            return p.ResourceValueCategory;
        }

        public double SetAtmosphericMass(ViewModelPlanet p)
        {

            if (!RuleBook.PlanetParams.ContainsKey((p.Size, p.Subtype)) || !RuleBook.PlanetParams[(p.Size, p.Subtype)].HasAtmosphere)
                p.AtmosphericMass = 0;

            else
            {
                // base level likely 0.5 to 1.5
                double mass = (double)DiceBag.Roll(3) / 10;

                // adjust by +/-5%
                double adj = (double)DiceBag.Rand(-5, 5) / 100;

                p.AtmosphericMass = mass + adj;
            }

            return p.AtmosphericMass;
        }

        public (fAtmosphericConditions, string) SetAtmosphericConditions(ViewModelPlanet p)
        {

            if (!RuleBook.PlanetParams.ContainsKey((p.Size, p.Subtype)) || !RuleBook.PlanetParams[(p.Size, p.Subtype)].HasAtmosphere)
            {
                p.AtmosphericConditions = fAtmosphericConditions.None;
                p.AtmosphericDescription = "None. ";
            }
            else
            {
                fAtmosphericConditions baseCon;
                string baseDesc;
                fAtmosphericConditions margCon = fAtmosphericConditions.None;
                string margDesc = "";

                (baseCon, baseDesc) = GetBaseAtmosphericConditions(p);
                if ((baseCon & fAtmosphericConditions.Marginal) == fAtmosphericConditions.Marginal)
                    (margCon, margDesc) = GetMarginalAtmosphericConditions(p);

                p.AtmosphericConditions = baseCon | margCon;
                p.AtmosphericDescription = baseDesc + margDesc;
            }

            return (p.AtmosphericConditions, p.AtmosphericDescription);
        }
        private (fAtmosphericConditions, string) GetBaseAtmosphericConditions(ViewModelPlanet p)
        {
            if (!RuleBook.PlanetParams.ContainsKey((p.Size, p.Subtype)) || !RuleBook.PlanetParams[(p.Size, p.Subtype)].HasAtmosphere)
                return (fAtmosphericConditions.None, "None");

            int roll = DiceBag.Roll(3);
            if (roll <= RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereANumber)
                return RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereA;
            else
                return RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereB;

        }
        private (fAtmosphericConditions, string) GetMarginalAtmosphericConditions(ViewModelPlanet p)
        {
            int roll = DiceBag.Roll(3);

            switch (roll)
            {
                case 3:
                case 4:
                    // Chlorine or Fluorine
                    // assumed 1-5 Cl, 6 F
                    // since free fluorine is apparently extremely unusual (S80)
                    int CLorF = DiceBag.Roll(1);
                    if (CLorF <= 5)
                        return (fAtmosphericConditions.HighlyToxic, "Significant chlorine. Pools of high concentration lethally toxic and corrosive. ");
                    else
                        return (fAtmosphericConditions.HighlyToxic, "Significant fluorine. ");
                case 5:
                case 6:
                    return (fAtmosphericConditions.MildlyToxic, "Sulphur compounds. Highly toxic near source. ");
                case 7:
                    return (fAtmosphericConditions.MildlyToxic, "Nitrogen compounds. Highly toxic near source. ");
                case 8:
                case 9:
                    return (fAtmosphericConditions.MildlyToxic, "Organic toxins. May count as respiratory poison or disease.");
                case 10:
                case 11:
                    return (fAtmosphericConditions.None, "Low oxygen, count as one pressure level lower for breathability. ");
                case 12:
                case 13:
                    return (fAtmosphericConditions.MildlyToxic, "Non-organic toxins, toxicity may be localised to sources such as volcanoes");
                case 14:
                    // assuming 1-4 is moderate, 5-6 is high
                    int NonOrgTox = DiceBag.Roll(1);
                    if (NonOrgTox <= 4)
                        return (fAtmosphericConditions.None, "Moderate carbon dioxide, breathable but treat as very dense atmosphere. ");
                    else
                        return (fAtmosphericConditions.MildlyToxic, "Very high carbon dioxide, not breathable. ");
                case 15:
                case 16:
                    // assuming 1-4 is moderate, 5-6 is high
                    int OxLvl = DiceBag.Roll(1);
                    if (OxLvl <= 4)
                        return (fAtmosphericConditions.None, "High oxygen, count as one pressure level higher for breathability. ");
                    else
                        return (fAtmosphericConditions.MildlyToxic, "Very high oxygen, count as one pressure level higher for breathability and all materials one flammability class higher. ");
                case 17:
                case 18:
                    return (fAtmosphericConditions.None, "Inert gases, high levels can cause inert gas narcosis - behave as if tipsy. ");
                default:
                    return (fAtmosphericConditions.None, "");
            }

        }

        public double SetHydrographicCoverage(ViewModelPlanet p)
        {
            PlanetParameters pp = RuleBook.PlanetParams[(p.Size, p.Subtype)];
            double cover = 0;

            cover = ((double)(DiceBag.Roll(pp.HydroDice) + pp.HydroAdj)) * 0.1;

            // adjust by +/-5%
            if (cover > 0.01)
                cover += ((double)DiceBag.Rand(-5, 5)) / 100;

            // apply min/max of 0/1 (although this is done again in the set property)
            if (cover > 1) cover = 1;
            if (cover < 0) cover = 0;

            p.HydrographicCoverage = cover;
            return p.HydrographicCoverage;
        }

        public int SetAverageSurfaceTempK(ViewModelPlanet p)
        {
            int tempMin = p.TempMin;
            int tempStep = p.TempStep;
            p.AverageSurfaceTempK = tempMin + (DiceBag.Roll(3) - 3) * tempStep;
            return p.AverageSurfaceTempK;
        }

        public double SetDensity(ViewModelPlanet p)
        {
            switch(p.CoreType)
            {
                case eCoreType.Icy:
                    p.Density = RuleBook.DensityIcyCore[DiceBag.Roll(3)];
                    break;
                case eCoreType.SmallIron:
                    p.Density = RuleBook.DensitySmallIronCore[DiceBag.Roll(3)];
                    break;
                case eCoreType.LargeIron:
                    p.Density = RuleBook.DensityLargeIronCore[DiceBag.Roll(3)];
                    break;
            }
            return p.Density;
        }

        public double SetGravity(ViewModelPlanet p)
        {
            double minG = p.MinGravity;
            double maxG = p.MaxGravity;
            int roll = DiceBag.Roll(2) - 2;
            int adj = DiceBag.Rand(-5, 5);
            double perc = (double)roll / 10 + (double)adj / 100;
            double grav = minG * perc + maxG * (1 - perc);
            if (grav < minG)
                grav = minG;
            if (grav > maxG)
                grav = maxG;

            p.Gravity = grav;
            return p.Gravity;
        }

        public eSettlementType SetSettlementType(ViewModelPlanet p)
        {
            int roll = DiceBag.Roll(1);
            switch(roll)
            {
                case 1:
                    p.SettlementType = eSettlementType.None;
                    break;
                case 2:
                case 3:
                    p.SettlementType = eSettlementType.Outpost;
                    break;
                case 4:
                case 5:
                    p.SettlementType = eSettlementType.Colony;
                    break;
                case 6:
                    p.SettlementType = eSettlementType.Homeworld;
                    break;
            }
            if (p.SettlementType == eSettlementType.Colony)
                p.ColonyAge = DiceBag.Rand(1, 200);
            if (p.SettlementType == eSettlementType.Homeworld)
                p.Interstellar = (DiceBag.Roll(1) <= 5);

            return p.SettlementType;
        }

        public Species SetLocalSpecies(ViewModelPlanet p)
        {
            int numSpecies = p.Setting.Species.Count;
            int randNum = DiceBag.Rand(0, numSpecies - 1);
            p.LocalSpecies = p.Setting.Species[randNum];
            return p.LocalSpecies;
        }

        public int SetLocalTechLevel(ViewModelPlanet p)
        {
            int roll = DiceBag.Roll(3);

            // -10 if uncontacted homeworld
            if (p.SettlementType == eSettlementType.Homeworld && !p.Interstellar)
                roll -= 10;

            if ((p.Habitability >= 4 && p.Habitability <= 6) &&
                (p.SettlementType == eSettlementType.Colony || p.SettlementType == eSettlementType.Homeworld))
                roll += 1;

            if ((p.Habitability <=3) &&
                (p.SettlementType == eSettlementType.Colony || p.SettlementType == eSettlementType.Homeworld))
                roll += 2;

            if (p.SettlementType == eSettlementType.Outpost)
                roll += 3;

            string res = RuleBook.TechLevelTable[roll];

            // could try and read the return strings from that table, however would be complex as a few different formats
            // since there's not many return values, easier just to do a switch on them

            switch (res)
            {
                case "Primitive":
                    p.LocalTechLevel = Math.Max(0, DiceBag.Roll(3) - 12);
                    break;
                case "Standard -3":
                    p.LocalTechLevel = p.Setting.TechLevel - 3;
                    break;
                case "Standard -2":
                    p.LocalTechLevel = p.Setting.TechLevel-2;
                    break;
                case "Standard -1":
                    p.LocalTechLevel= p.Setting.TechLevel-1;
                    break;
                case "Standard (Delayed)":
                    p.LocalTechLevel = p.Setting.TechLevel;
                    p.LocalTechLevelIsDelayed = true;
                    break;
                case "Standard":
                    p.LocalTechLevel = p.Setting.TechLevel;
                    break;
                case "Standard (Advanced)":
                    p.LocalTechLevel = p.Setting.TechLevel;
                    p.LocalTechLevelIsAdvanced = true;
                    break;
            }

            return p.LocalTechLevel;
        }

        public ulong SetPopulation(ViewModelPlanet p)
        {
            switch (p.SettlementType)
            {
                case eSettlementType.Homeworld:
                    return SetPopulationHomeworld(p);
                case eSettlementType.Colony:
                    return SetPopulationColony(p);
                case eSettlementType.Outpost:
                    return SetPopulationOutpost(p);
                default:
                    p.Population = 0;
                    return p.Population;
            }
        }
        private ulong SetPopulationHomeworld(ViewModelPlanet p)
        {
            double proportion;
            if (p.LocalTechLevel <= 4)
                proportion = ((double)DiceBag.Roll(2) + 3) / 10;
            else
                proportion = 10 / (double)DiceBag.Roll(2);
            ulong pop = (ulong)(proportion * (double)p.CarryingCapacity);
            p.Population = RuleBook.RoundToSignificantFigures(pop, 2);
            return p.Population;
        }
        private ulong SetPopulationColony(ViewModelPlanet p)
        { 
            // rather than using the table directly, instead calculating based on the box on page 93
            // each race has a starting colony size, a growth rate, and an affinity multiplier

            // min size is on a roll of 10 + average affinity of 5 * affinity multiplier
            // each +1 on the roll represents 10y growth, so is *(1+growth)^10
            // the affinity multiplier shows how much extra for each +1 affinity
            // LN(affinitymult) / LN(1+growth) gives how many years of growth is equal to the affinity multiplier
            // and this figure / 10 is how many rows on the table from each +1 to affinity

            Species s = p.LocalSpecies;

            int ageInDecades = p.ColonyAge / 10;
            int affinityMod = (int)Math.Round(Math.Log(s.AffinityMultiplier) / Math.Log(1 + s.AnnualGrowthRate) / 10, 0);
            int minRoll = 10 + 5 * affinityMod;

            int roll = DiceBag.Roll(3) + p.AffinityScore * affinityMod + ageInDecades;

            // effective decades of growth is the difference between the roll and the min
            int effectiveDecadesOfGrowth = Math.Max(0, roll - minRoll);

            // then calculate the population
            ulong population = (ulong)(s.StartingColonyPopulation * Math.Pow(1 + s.AnnualGrowthRate, effectiveDecadesOfGrowth * 10));
            population = RuleBook.RoundToSignificantFigures(population, 2);
            if (population > s.CarryingCapacity(p.Planet))
                population = s.CarryingCapacity(p.Planet);

            p.Population = population;
            return p.Population;
        }
        private ulong SetPopulationOutpost(ViewModelPlanet p)
        {
            int roll = DiceBag.Roll(3);
            ulong population = RuleBook.OutpostPopulation[roll];
            double adj = 1 + (double)(DiceBag.Roll(2) - 7) * 0.1;
            population = (ulong)((double)population * adj);
            population = RuleBook.RoundToSignificantFigures(population, 2);
            p.Population = population;
            return p.Population;
        }

        public (eWorldUnityLevel, fGovernmentSpecialConditions) SetWorldGovernance(ViewModelPlanet p)
        {
            int roll;
            bool hasSpecial;
            if (p.LocalTechLevel<=7)
                roll = DiceBag.Roll(1);
            else
                roll = DiceBag.Roll(2);

            switch(p.PopulationRating)
            {
                case <= 4:
                    roll += 4;
                    break;
                case 5:
                    roll += 3;
                    break;
                case 6:
                    roll += 2;
                    break;
                case 7:
                    roll += 1;
                    break; ;
            }
            (p.WorldUnityLevel, hasSpecial) = RuleBook.WorldUnityLevel[roll];

            if (hasSpecial)
            {
                bool hasSecond;
                do
                {
                    roll = DiceBag.Roll(3);
                    (p.GovernmentSpecialConditions, hasSecond) = RuleBook.GovernmentSpecialConditions[roll];
                } while (p.LocalTechLevel <= 7 && p.GovernmentSpecialConditions == fGovernmentSpecialConditions.Cyberocracy);
                
                if (hasSecond)
                {
                    fGovernmentSpecialConditions secondCond;
                    do
                    {
                        roll = DiceBag.Roll(3);
                        (secondCond, hasSecond) = RuleBook.GovernmentSpecialConditions[roll];
                    } while (p.LocalTechLevel <= 7 && secondCond == fGovernmentSpecialConditions.Cyberocracy);
                    p.GovernmentSpecialConditions |= secondCond;
                }
            }
            else
                p.GovernmentSpecialConditions = fGovernmentSpecialConditions.None;

            return (p.WorldUnityLevel, p.GovernmentSpecialConditions);
        }

        public eSocietyType SetSocietyType(ViewModelPlanet p)
        {
            return p.SocietyType;
        }
    }
}
