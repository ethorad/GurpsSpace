
using System;
using System.Windows;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Xaml;

namespace GurpsSpace.PlanetCreation
{
    internal class RandomPlanet : IPlanetCreator
    {
        public string GetName(Planet p)
        {
            string name = "Randomia-" + DiceBag.Rand(1, 100);
            return name;
        }

        public (eSize, eSubtype) GetSizeAndSubtype(Planet p)
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
                default:
                    (size, subtype) = (eSize.None, eSubtype.None);
                    break;
            }
            return (size, subtype);
        }

        public eResourceValueCategory GetResourceValueCategory(Planet p)
        {
            int roll = DiceBag.Roll(3);
            if (p.IsPlanet)
                return RuleBook.ResourceValueCategoryPlanet[roll];
            else
                return RuleBook.ResourceValueCategoryAsteroidBelt[roll];
        }

        public double GetAtmosphericMass(Planet p)
        {

            if (!RuleBook.PlanetParams.ContainsKey((p.Size, p.Subtype)) || !RuleBook.PlanetParams[(p.Size, p.Subtype)].HasAtmosphere)
                return 0;

            else
            {
                // base level likely 0.5 to 1.5
                double mass = (double)DiceBag.Roll(3) / 10;

                // adjust by +/-5%
                double adj = (double)DiceBag.Rand(-5, 5) / 100;

                return mass + adj;
            }

        }

        public (fAtmosphericConditions, string) GetAtmosphericConditions(Planet p)
        {

            if (!RuleBook.PlanetParams.ContainsKey((p.Size, p.Subtype)) || !RuleBook.PlanetParams[(p.Size, p.Subtype)].HasAtmosphere)
            {
                return (fAtmosphericConditions.None, "None. ");
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

                return (baseCon | margCon, baseDesc + margDesc);
            }

        }
        private (fAtmosphericConditions, string) GetBaseAtmosphericConditions(Planet p)
        {
            if (!RuleBook.PlanetParams.ContainsKey((p.Size, p.Subtype)) || !RuleBook.PlanetParams[(p.Size, p.Subtype)].HasAtmosphere)
                return (fAtmosphericConditions.None, "None");

            int roll = DiceBag.Roll(3);
            if (roll <= RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereANumber)
                return RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereA;
            else
                return RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereB;

        }
        private (fAtmosphericConditions, string) GetMarginalAtmosphericConditions(Planet p)
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

        public double GetHydrographicCoverage(Planet p)
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

            return cover;
        }

        public int GetAverageSurfaceTempK(Planet p)
        {
            int tempMin = p.MinSurfaceTemperatureK;
            int tempStep = p.StepSurfaceTemperatureK;
            int tempK = tempMin + (DiceBag.Roll(3) - 3) * tempStep;
            return tempK;
        }

        public double GetDensity(Planet p)
        {
            switch(p.CoreType)
            {
                case eCoreType.Icy:
                    return RuleBook.DensityIcyCore[DiceBag.Roll(3)];
                case eCoreType.SmallIron:
                    return RuleBook.DensitySmallIronCore[DiceBag.Roll(3)];
                case eCoreType.LargeIron:
                    return RuleBook.DensityLargeIronCore[DiceBag.Roll(3)];
            }
            return p.Density;
        }

        public double GetGravity(Planet p)
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

            return grav;
        }

        public (eSettlementType, int, bool) GetSettlementType(Planet p)
        {
            eSettlementType settType = eSettlementType.None;
            int colonyAge = 0;
            bool interstellar = true;

            int roll = DiceBag.Roll(1);
            switch(roll)
            {
                case 1:
                    settType = eSettlementType.None;
                    break;
                case 2:
                case 3:
                    settType = eSettlementType.Outpost;
                    break;
                case 4:
                case 5:
                    settType = eSettlementType.Colony;
                    break;
                case 6:
                    settType = eSettlementType.Homeworld;
                    break;
            }
            if (settType == eSettlementType.Colony)
                colonyAge = DiceBag.Rand(1, 200);
            if (settType == eSettlementType.Homeworld)
                interstellar = (DiceBag.Roll(1) <= 5);

            return (settType, colonyAge, interstellar);
        }

        public Species GetLocalSpecies(Planet p)
        {
            int numSpecies = p.Setting.Species.Count;
            int randNum = DiceBag.Rand(0, numSpecies - 1);
            return p.Setting.Species[randNum];
        }

        public (int, eTechLevelRelativity) GetLocalTechLevel(Planet p)
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

            int tl = p.Setting.TechLevel;
            eTechLevelRelativity adj = eTechLevelRelativity.Normal;

            switch (res)
            {
                case "Primitive":
                    tl = Math.Max(0, DiceBag.Roll(3) - 12);
                    break;
                case "Standard -3":
                    tl = p.Setting.TechLevel - 3;
                    break;
                case "Standard -2":
                    tl = p.Setting.TechLevel-2;
                    break;
                case "Standard -1":
                    tl = p.Setting.TechLevel-1;
                    break;
                case "Standard (Delayed)":
                    tl = p.Setting.TechLevel;
                    adj = eTechLevelRelativity.Delayed;
                    break;
                case "Standard":
                    tl = p.Setting.TechLevel;
                    break;
                case "Standard (Advanced)":
                    tl = p.Setting.TechLevel;
                    adj = eTechLevelRelativity.Advanced;
                    break;
            }

            return (tl, adj);
        }

        public double GetPopulation(Planet p)
        {
            switch (p.SettlementType)
            {
                case eSettlementType.Homeworld:
                    return GetPopulationHomeworld(p);
                case eSettlementType.Colony:
                    return GetPopulationColony(p);
                case eSettlementType.Outpost:
                    return GetPopulationOutpost(p);
                default:
                    return 0;
            }
        }
        private double GetPopulationHomeworld(Planet p)
        {
            double proportion;
            if (p.LocalTechLevel <= 4)
                proportion = ((double)DiceBag.Roll(2) + 3) / 10;
            else
                proportion = 10 / (double)DiceBag.Roll(2);
            double pop = proportion * p.CarryingCapacity;
            pop = RuleBook.RoundToSignificantFigures(pop, 2);
            return pop;
        }
        private double GetPopulationColony(Planet p)
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
            double population = s.StartingColonyPopulation * Math.Pow(1 + s.AnnualGrowthRate, effectiveDecadesOfGrowth * 10);
            population = RuleBook.RoundToSignificantFigures(population, 2);
            if (population > s.CarryingCapacity(p))
                population = s.CarryingCapacity(p);

            return population;
        }
        private double GetPopulationOutpost(Planet p)
        {
            int roll = DiceBag.Roll(3);
            double population = RuleBook.OutpostPopulation[roll];
            double adj = 1 + (double)(DiceBag.Roll(2) - 7) * 0.1;
            population = population * adj;
            population = RuleBook.RoundToSignificantFigures(population, 2);
            return population;
        }

        public (eWorldUnityLevel, fGovernmentSpecialConditions) GetWorldGovernance(Planet p)
        {
            int roll;
            eWorldUnityLevel unity;
            bool hasSpecial;
            fGovernmentSpecialConditions specCond = fGovernmentSpecialConditions.None;
            fGovernmentSpecialConditions secondCond = fGovernmentSpecialConditions.None;

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
            (unity, hasSpecial) = RuleBook.WorldUnityLevel[roll];

            if (hasSpecial)
            {
                bool hasSecond;
                do
                {
                    roll = DiceBag.Roll(3);
                    (specCond, hasSecond) = RuleBook.GovernmentSpecialConditions[roll];
                } while (p.LocalTechLevel <= 7 && specCond == fGovernmentSpecialConditions.Cyberocracy);

                if (hasSecond && DiceBag.Roll(1) <= 3)
                {
                    do
                    {
                        roll = DiceBag.Roll(3);
                        (secondCond, hasSecond) = RuleBook.GovernmentSpecialConditions[roll];
                    } while (p.LocalTechLevel <= 7 && secondCond == fGovernmentSpecialConditions.Cyberocracy);
                }
            }

            return (unity, specCond | secondCond);
        }

        public eSocietyType GetSocietyType(Planet p)
        {
            int roll = DiceBag.Roll(3);

            roll += Math.Min(10, p.LocalTechLevel);

            if (!p.Interstellar)
                return RuleBook.SocietyTypeAnarchy[roll];
            
            switch (p.Setting.SocietyType)
            {
                case eSettingSocietyType.Anarchy:
                    return RuleBook.SocietyTypeAnarchy[roll];
                case eSettingSocietyType.Alliance:
                    return RuleBook.SocietyTypeAlliance[roll];
                case eSettingSocietyType.Federation:
                    return RuleBook.SocietyTypeFederation[roll];
                case eSettingSocietyType.CorporateState:
                    return RuleBook.SocietyTypeCorporateState[roll];
                case eSettingSocietyType.Empire:
                    return RuleBook.SocietyTypeEmpire[roll];
                default:
                    return RuleBook.SocietyTypeAnarchy[roll];
            }

        }

        public int GetControlRating(Planet p)
        {
            int minCR = RuleBook.SocietyTypeParams[p.SocietyType].MinControlRating;
            int maxCR = RuleBook.SocietyTypeParams[p.SocietyType].MaxControlRating;

            // adjust for any special conditions
            // see Campaigns p510 for details

            if (p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Cyberocracy) ||
                p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Meritocracy) ||
                p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Oligarchy) ||
                p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Socialist))
            {
                if (minCR < 3) minCR = 3;
                if (maxCR < minCR) maxCR = minCR;
            }
            if (p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Bureaucracy) ||
                p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.MilitaryGovernment) ||
                p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Subjugated))
            {
                if (minCR < 4) minCR = 4;
                if (maxCR < minCR) maxCR = minCR;
            }
            if (p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Sanctuary))
            {
                if (maxCR > 4) maxCR = 4;
                if (minCR > maxCR) minCR = maxCR;
            }
            if (p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Colony))
            {
                // strictly reads like it should be 1 CR below parent
                // here I'm just reducing min and max by 1
                minCR--;
                maxCR--;
            }
            if (p.HasGovernmentSpecialCondition(fGovernmentSpecialConditions.Utopia))
            {
                // rulebook says the CR seems low
                // here I'm just reducing min and max by 2
                minCR -= 2;
                maxCR -= 2;
            }

            if (minCR<0) minCR= 0;
            if (maxCR>6) maxCR= 6;

            return DiceBag.Rand(minCR, maxCR);
        }

        public double GetTradeVolume(Planet p)
        { 
            // For trade volume between worlds use:
            // T = K * V1 * V2 / D
            // where:
            // T = Trade volume in $tn
            // K = constant in setting, determine by trial and error
            // V1, V2 = economic volumes of the two worlds
            // D = distance between worlds (say in parsecs)

            // However, without a trade route map, I'm just going to generate some random percentages of GDP
            // Assume:
            // If uncontacted, then trade volume = 0
            // If homeworld, then low trade volume say 10-40%
            // If colony, then higher trade volume, say 30%-70%
            // If outpost then virtually all trade volume, say 80%-100%

            double tradeProp = 0;

            if (!p.Interstellar)
                tradeProp = 0;
            else if (p.SettlementType == eSettlementType.Homeworld)
                tradeProp = ((double)DiceBag.Rand(1, 4)) * 0.1;
            else if (p.SettlementType == eSettlementType.Colony)
                tradeProp = ((double)DiceBag.Rand(3, 7)) * 0.1;
            else if (p.SettlementType == eSettlementType.Outpost)
                tradeProp = ((double)DiceBag.Rand(8, 10)) * 0.1;

            double trade = tradeProp * p.EconomicVolume;
            trade = RuleBook.RoundToSignificantFigures(trade, 2);
            return trade;
        }

        public int GetSpaceportClass(Planet p)
        {
            // check for class V
            if (p.TradeVolume > RuleBook.TradeForSpaceportV ||
                (p.PopulationRating >= 6 && DiceBag.Roll(3) <= p.PopulationRating + 2))
                return 5;

            // check for class IV
            else if (p.TradeVolume > RuleBook.TradeForSpaceportIV ||
                (p.PopulationRating >= 6 && DiceBag.Roll(3) <= p.PopulationRating + 5))
                return 4;

            // check for class III
            else if (p.TradeVolume > RuleBook.TradeForSpaceportIII ||
                DiceBag.Roll(3) <= p.PopulationRating + 8)
                return 3;

            // check for class II
            else if (DiceBag.Roll(3) <= p.Population + 7)
                return 2;

            // check for class I
            else if (DiceBag.Roll(3) <= 14)
                return 1;

            // if all the above fail, then no spaceport
            else
                return 0;

        }

        public List<Installation> GetInstallations(Planet p)
        {
            List<Installation> lst = new List<Installation>();

            foreach (InstallationParameters instParam in RuleBook.InstallationParams)
            {
                CheckInstallation(p, lst, instParam);
            }

            // prison only if the only other installations are naval and patrol bases
            // first check if there is a prison, and if there are installations other than naval/patrol bases
            // then if so, delete the prison
            int prisonIndex = -1;
            bool hasNonMilitaryInstallation = false;
            for (int i = 0; i < lst.Count(); i++)
            {
                if (lst[i].Type == "Prison")
                    prisonIndex = i;
                else if (lst[i].Type != "Naval base" &&
                    lst[i].Type != "Patrol base")
                    hasNonMilitaryInstallation = true;
            }
            if (hasNonMilitaryInstallation && prisonIndex >= 0)
                lst.RemoveAt(prisonIndex);

            // sort to alphabetical and return
            lst = lst.OrderBy(x => x.Name).ToList();

            return lst;

        }
        private bool CheckInstallation(Planet p, List<Installation> lst, InstallationParameters instParam)
        {
            if (!instParam.IsValidInstallation(p))
                return false;

            int startCount = lst.Count; // used to check if any were added

            int count = 0;
            bool added;
            do
            {
                added = false;
                int roll = DiceBag.Roll(3);
                int target = instParam.TargetNumber(p, count);
                if (roll <= target)
                {
                    instParam.SetWeight(DiceBag.Rand(0, instParam.MaxWeight)); // in case there's multiple options
                    int pop =
                        DiceBag.Roll(instParam.PopulationDice) + instParam.PopulationAdj
                        + DiceBag.Rand(instParam.PopulationRangeMin, instParam.PopulationRangeMax);
                    if (pop < 1 && instParam.PopulationDice > 0)
                        pop = 1; // min PR 1 if you're rolling dice
                    lst.Add(new Installation(instParam.Type, instParam.SubType, pop));
                    count++;
                    added = true;
                }
            } while (added && count < instParam.MaxCount);

            if (count > 0 && instParam.HasSecond && instParam.SecondInstallation != null)
                CheckInstallation(p, lst, instParam.SecondInstallation);

            return (lst.Count > startCount);

        }

        public List<Installation> GetInstallation(Planet p, string installationType)
        {
            List<Installation> lst = new List<Installation>();
            InstallationParameters instParam = RuleBook.InstallationParams[0];
            foreach (InstallationParameters ip in RuleBook.InstallationParams)
                if (ip.Type== installationType)
                    instParam = ip;
            CheckInstallation(p, lst, instParam);
            return lst;
        }
    }
}
