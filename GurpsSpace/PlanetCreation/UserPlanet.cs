using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace GurpsSpace.PlanetCreation
{
    internal class UserPlanet : IPlanetCreator
    {


        public string GetName(Planet p)
        {
            InputString inStr = new("Enter planet's name:", p.Name);
            if (inStr.ShowDialog() == true)
                return inStr.Answer;
            else // clicked cancel
                return p.Name;
        }

        public (eSize, eSubtype) GetSizeAndSubtype(Planet p)
        {
            PlanetTypeSelection typeDiag = new();
            if (typeDiag.ShowDialog() == true)
                return (typeDiag.Size, typeDiag.Subtype);
            else
                return (p.Size, p.Subtype);
        }

        public eResourceValueCategory GetResourceValueCategory(Planet p)
        {
            List<(int, string, string)> options = new List<(int, string, string)>();
            List<int> vals = ((int[])Enum.GetValues(typeof(eResourceValueCategory))).ToList<int>();
            int? startVal = null;
            vals.Sort();
            foreach (int i in vals)
                options.Add((i, i.ToString(), ((eResourceValueCategory)i).ToString()));

            for (int i = 0; i < options.Count; i++)
                if (options[i].Item2 == p.ResourceValueCategory.ToString())
                    startVal = i;

            string question = "Select the resource value for this " + ((p.IsPlanet) ? "planet. " : "asteroid belt. ");
            if (p.IsPlanet)
                question += "\r\nFor a planet, this is normally between -2 (Very Poor) and +2 (Very Abundant).";

            InputRadio radioDiag = new InputRadio(question, options, startVal);
            if (radioDiag.ShowDialog() == true)
                return (eResourceValueCategory)radioDiag.Answer.Item1;
            else // clicked cancel
                return p.ResourceValueCategory;
        }

        public double GetAtmosphericMass(Planet p)
        {
            throw new NotImplementedException();
        }

        public (fAtmosphericConditions, string) GetAtmosphericConditions(Planet p)
        {
            if (!p.HasAtmosphericOptions)
                return RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereA;

            PlanetParameters pp = RuleBook.PlanetParams[(p.Size, p.Subtype)];
            int? initial = null;
            if (p.AtmosphericDescription == pp.AtmosphereA.Item2)
                initial = 0;
            if (p.AtmosphericDescription == pp.AtmosphereB.Item2)
                initial = 1;
            List<(int, string, string)> options = new List<(int, string, string)>()
                {
                    (0, pp.AtmosphereA.Item1.ToString(), pp.AtmosphereA.Item2),
                    (1, pp.AtmosphereB.Item1.ToString(), pp.AtmosphereB.Item2)
                };

            InputRadio radioDiag = new("Select atmospheric options:", options, initial);


            if (radioDiag.ShowDialog() == true)
            {
                if (radioDiag.Selected == 0)
                    return pp.AtmosphereA;
                else if (radioDiag.Selected == 1)
                    return pp.AtmosphereB;
            }
            return (p.AtmosphericConditions, p.AtmosphericDescription);
        }

        public double GetHydrographicCoverage(Planet p)
        {
            throw new NotImplementedException();
        }

        public int GetAverageSurfaceTempK(Planet p)
        { 
            throw new NotImplementedException(); 
        }

        public double GetDensity(Planet p)
        {
            throw new NotImplementedException();
        }

        public double GetGravity(Planet p)
        {
            throw new NotImplementedException();
        }

        public (eSettlementType, int, bool) GetSettlementType(Planet p)
        {

            string question = "Select the settlement type to be present:";
            List<(int, string, string)> options = new List<(int, string, string)>();
            options.Add(((int)eSettlementType.None, "None", "No settlement is present."));
            options.Add(((int)eSettlementType.Outpost,"Outpost", "A minor outpost is present.  For example, a military base or research station."));
            options.Add(((int)eSettlementType.Colony, "Colony", "A full fledged colony is present.  This will be part of a larger interstellar civilisation.  It will usually have at least positive affinity, i.e. either attractive resource level or a good habitability."));
            options.Add(((int)eSettlementType.Homeworld, "Homeworld", "This is the homeworld for a species.  This may be part of an interstellar civilisation.  This wll generally have high habitability for the selected species, as it will have evolved to live here."));

            int? initial = null;
            switch (p.SettlementType)
            {
                case eSettlementType.None: initial = 0;break;
                case eSettlementType.Outpost: initial = 1; break;
                case eSettlementType.Colony: initial = 2; break;
                case eSettlementType.Homeworld: initial = 3; break;
            }

            InputRadio radioDiag = new InputRadio(question, options, initial);

            if (radioDiag.ShowDialog() == true)
            {
                switch ((eSettlementType)radioDiag.Answer.Item1)
                {
                    case eSettlementType.Outpost:
                        return (eSettlementType.Outpost, 0, true);

                    case eSettlementType.Colony:
                        // get age
                        InputString inStr = new InputString("Enter colony age.  This is used to aid with the population count.", p.ColonyAge.ToString(), true);
                        if (inStr.ShowDialog() == true)
                        {
                            int colonyAge = int.Parse(inStr.Answer);
                            return (eSettlementType.Colony, colonyAge, true);
                        }
                        else
                            return (p.SettlementType, p.ColonyAge, p.Interstellar);

                    case eSettlementType.Homeworld:
                        // get interstellar or not
                        options = new List<(int, string, string)>()
                        {
                    (0, "Interstellar","The homeworld is part of an interstellar civilisation."),
                    (1, "Uncontacted","The homeworld has not spread outside of its system, and has not been contacted.")
                        };
                        initial = null;
                        if (p.Interstellar)
                            initial = 0;
                        else
                            initial = 1;
                        InputRadio inRadio = new InputRadio("Is the homeworld part of an interstellar civilisation?", options, initial);
                        bool interstellar = true;
                        if (inRadio.ShowDialog() == true)
                        {
                            if (inRadio.Selected == 0)
                                interstellar = true;
                            if (inRadio.Selected == 1)
                                interstellar = false;
                            return (eSettlementType.Homeworld, 0, interstellar);
                        }
                        else
                            return (p.SettlementType, p.ColonyAge, p.Interstellar);

                    default:
                        // shouldn't ever get here, but needed to ensure all paths return a value
                        return (p.SettlementType, p.ColonyAge, p.Interstellar);
                }
            }
            else // clicked cancel
                return (p.SettlementType, p.ColonyAge, p.Interstellar);

        }

        public Species GetLocalSpecies(Planet p)
        {
            List<(int, string, string)> options = new List<(int, string, string)>();
            int? initial = null;
            for (int i = 0; i < p.Setting.Species.Count; i++)
            {
                options.Add((i, p.Setting.Species[i].Name + "\r\nHabitability: " + p.Setting.Species[i].Habitability(p), p.Setting.Species[i].Description));
                if (p.LocalSpecies.Name == p.Setting.Species[i].Name)
                    initial = i;
            }


            InputRadio radioDiag = new InputRadio("Select the main race inhabiting this " + ((p.IsPlanet) ? "planet:" : "asteroid belt:"), options, initial);
            if (radioDiag.ShowDialog() == true)
                return p.Setting.Species[radioDiag.Selected];
            else
                return p.LocalSpecies;
        }

        public (int, eTechLevelRelativity) GetLocalTechLevel(Planet p)
        {
            int tl = p.Setting.TechLevel;
            eTechLevelRelativity adj = eTechLevelRelativity.Normal;

            List<(int, string, string)> options = new List<(int, string, string)>();
            foreach (TechLevelParameters tlp in RuleBook.TechLevelParams.Values)
                options.Add((tlp.TL,tlp.TL.ToString(), tlp.Age));

            string question = "Select the main Tech Level for this " + ((p.IsPlanet) ? "planet. " : "asteroid belt. ");
            question += "\r\nThe setting's TL is " + p.Setting.TechLevel.ToString() + " so anything at " + (p.Setting.TechLevel - 4).ToString() + " or below would be considered primitive.";
            InputRadio radioDiag = new InputRadio(question, options, p.LocalTechLevel);
            if (radioDiag.ShowDialog() == true)
            {
                tl = radioDiag.Answer.Item1;

                question = "Select whether the settlement is delayed or advanced relative to TL " + tl.ToString() + ".";
                options.Clear();
                options.Add((0, "Delayed", "Settlement is behind normal for TL " + tl.ToString() + "."));
                options.Add((1, "Normal", "Settlement has a normal level of development for TL " + tl.ToString() + "."));
                options.Add((2, "Advanced", "Settlement is ahead of normal for TL " + tl.ToString() + "."));
                int? initial = 0;
                switch (p.LocalTechLevelRelativity)
                {
                    case eTechLevelRelativity.Delayed: initial = 0; break;
                    case eTechLevelRelativity.Normal: initial = 1; break;
                    case eTechLevelRelativity.Advanced: initial = 2; break;
                }
                radioDiag = new InputRadio(question, options, initial);
                if (radioDiag.ShowDialog() == true)
                {
                    switch (radioDiag.Answer.Item2)
                    {
                        case "Delayed":
                            adj = eTechLevelRelativity.Delayed;
                            break;
                        case "Normal":
                            adj = eTechLevelRelativity.Normal;
                            break;
                        case "Advanced":
                            adj = eTechLevelRelativity.Advanced;
                            break;
                    }
                    return (tl, adj);
                }
            }
            return (p.LocalTechLevel, p.LocalTechLevelRelativity);
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
            string question;
            if (p.LocalTechLevel <= 4)
            {
                question = "The carrying capacity for this " + ((p.IsPlanet) ? "planet" : "asteroid belt") + " is " + p.CarryingCapacity.ToString("N0") + ". " +
                    "At up to TL 4, homeworld populations will generally be around 50-150% of carrying capacity due to limited " +
                    "control over birth and death rates.  Enter the percentage below:";
            }
            else // TL 5+
            {
                question = "The carrying capacity for this " + ((p.IsPlanet) ? "planet" : "asteroid belt") + " is " + p.CarryingCapacity.ToString("N0") + ". " +
                    "At TL 5 and higher, advances in medical care and resource extraction mean the population can vary widely, from " +
                    "80-500%.  Enter the percentage below:";
            }
            int currPerc = (int)(p.Population / p.CarryingCapacity * 100);
            InputString inDiag = new InputString(question, currPerc.ToString("N0"), true);

            if (inDiag.ShowDialog() == true)
            {
                double perc = double.Parse(inDiag.Answer, NumberStyles.AllowThousands) / 100;
                double pop = p.CarryingCapacity * perc;
                pop = RuleBook.RoundToSignificantFigures(pop, 2);
                return pop;
            }

            return p.Population;
        }
        private double GetPopulationColony(Planet p)
        {
            // calculate the suggested colony size using the same approach as the random one
            // but assuming a roll of 10
            // this is to include in the user prompt

            Species s = p.LocalSpecies;

            int ageInDecades = p.ColonyAge / 10;
            int affinityMod = (int)Math.Round(Math.Log(s.AffinityMultiplier) / Math.Log(1 + s.AnnualGrowthRate) / 10, 0);
            int minRoll = 10 + 5 * affinityMod;

            int roll = 10 + p.AffinityScore * affinityMod + ageInDecades; // assuming a roll of 10

            // effective decades of growth is the difference between the roll and the min
            int effectiveDecadesOfGrowth = Math.Max(0, roll - minRoll);

            // then calculate the population
            double population = s.StartingColonyPopulation * Math.Pow(1 + s.AnnualGrowthRate, effectiveDecadesOfGrowth * 10);
            population = RuleBook.RoundToSignificantFigures(population, 2);
            if (population > s.CarryingCapacity(p))
                population = s.CarryingCapacity(p);

            string question = "Enter the colony population below. ";
            question += "For a " + s.Name + " colony, the minimum size is " + s.StartingColonyPopulation.ToString("N0") + ". ";
            question += "After " + p.ColonyAge + " years of growth, this is expected to have reached " + population.ToString("N0") + ". ";

            InputString inDiag = new InputString(question, p.Population.ToString("N0"), true);
            if (inDiag.ShowDialog() == true)
            {
                if (inDiag.Answer != "")
                    return double.Parse(inDiag.Answer, NumberStyles.AllowThousands);
            }

            return p.Population;
        }
        private double GetPopulationOutpost(Planet p)
        {
            string question = "Enter the outpost population below. This will generally be in the range 100 to 100,000.";
            InputString inDiag = new InputString(question, p.Population.ToString("N0"), true);
            if(inDiag.ShowDialog()==true)
            {
                return double.Parse(inDiag.Answer, NumberStyles.AllowThousands);
            }
            return p.Population;
        }

        public (eWorldUnityLevel, fGovernmentSpecialConditions) GetWorldGovernance(Planet p)
        {

            string question = "Select the degree to which the settlement is socially unified. " +
                "Higher technology levels, and smaller populations, tend to be more unified.";

            List<(int, string, string)> answers = new List<(int, string, string)>();
            answers.Add(((int)eWorldUnityLevel.Diffuse, "Diffuse", "A variety of social structure, none dominating."));
            answers.Add(((int)eWorldUnityLevel.Factionalised, "Factionalised", "A few competing social structures."));
            answers.Add(((int)eWorldUnityLevel.Coalition, "Coalition", "A few cooperating social structures."));
            answers.Add(((int)eWorldUnityLevel.WorldGovernment, "World Government", "A single world government, which may have special conditions."));
            int? initial = null;
            switch (p.WorldUnityLevel)
            {
                case eWorldUnityLevel.Diffuse: initial = 0; break;
                case eWorldUnityLevel.Factionalised: initial = 1; break;
                case eWorldUnityLevel.Coalition: initial = 2; break;
                case eWorldUnityLevel.WorldGovernment: initial = 3; break;
            }

            InputRadio inDiag = new InputRadio(question, answers, initial);

            eWorldUnityLevel unity = eWorldUnityLevel.Diffuse;
            fGovernmentSpecialConditions? specCond = fGovernmentSpecialConditions.None;
            bool hasSpecialCond = false;
            if (inDiag.ShowDialog()==true)
            {
                unity = (eWorldUnityLevel)inDiag.Answer.Item1;
                if (unity==eWorldUnityLevel.WorldGovernment)
                    hasSpecialCond = true;

                if (hasSpecialCond)
                    specCond = GetGovernmentSpecialConditions(p);

                if (specCond != null)
                {
                    // i.e. we must have clicked OK on the unity and any special conditions windows
                    return (unity, specCond ?? fGovernmentSpecialConditions.None);
                    // need the ?? as we don't want to return the nullable version
                    // since we've checked it is not null, shouldn't ever be an issue
                }

            }

            // if here then must have clicked cancel on one of the windows
            return (p.WorldUnityLevel, p.GovernmentSpecialConditions);
        }
        private fGovernmentSpecialConditions? GetGovernmentSpecialConditions(Planet p)
        {
            fGovernmentSpecialConditions specCond = fGovernmentSpecialConditions.None;

            string question = "Select a special condition, or none. ";
            List<(int, string, string)> options = new List<(int, string, string)>();
            options.Add(((int)fGovernmentSpecialConditions.None, "None", "No special conditions prevail."));
            options.Add(((int)fGovernmentSpecialConditions.Subjugated, "Subjugated", "Under the control of another civilisation, either militarily, economically or otherwise (e.g. mind control). Second condition is possible."));
            options.Add(((int)fGovernmentSpecialConditions.Sanctuary, "Sanctuary", "Particularly welcoming of refugees, criminals or terrorists wanted by other states.  Likely to be an independent settlement."));
            options.Add(((int)fGovernmentSpecialConditions.MilitaryGovernment, "Military Government", "Run by the military, citizenship likely tied to service.  Can lead to dictatorship or feudalism."));
            options.Add(((int)fGovernmentSpecialConditions.Socialist, "Socialist", "The government directly manages the economy, with citizens receiving education, medical care, housing and a job.  Second condition is possible."));
            options.Add(((int)fGovernmentSpecialConditions.Bureaucracy, "Bureaucracy", "Self-perpetuating civil service runs the society day to day.  Many laws and lots of red tape likely make the government unresponsive to citizens."));
            options.Add(((int)fGovernmentSpecialConditions.Colony, "Colony", "An offshoot of a larger galactic civilisation, ruled by an externally appointed governor.  There may be a locally elected council, however this will have limited power."));
            options.Add(((int)fGovernmentSpecialConditions.Oligarchy, "Oligarchy", "Ruled by a small group of wealthy powerful individuals, regardless of the nominal form of government. Second condition is possible."));
            options.Add(((int)fGovernmentSpecialConditions.Meritocracy, "Meritocracy", "Ruled according to merit, defined in some manner such as a series of tests. Second condition is possible."));
            options.Add(((int)fGovernmentSpecialConditions.Genderarchy, "Genderarchy", "Positions of authority are only open to a single gender (in Earth terms, a Patriarchy or Matriarchy)."));
            options.Add(((int)fGovernmentSpecialConditions.Utopia, "Utopia", "Egalitarian society which aims to be perfect.  All citizens are happy and satisfied (or at least they appear to be ...)."));
            if (p.LocalTechLevel > 7)
                options.Add(((int)fGovernmentSpecialConditions.Cyberocracy, "Cyberocracy", "A statewide computer system controls society administration, and potentially also legislation.  May be efficient or inhuman."));

            // not showing a default value here, since can be two things selected and can only show one
            InputRadio inDiag = new InputRadio(question, options);
            if (inDiag.ShowDialog() == true)
            {
                specCond = (fGovernmentSpecialConditions)inDiag.Answer.Item1;

                if (specCond == fGovernmentSpecialConditions.Subjugated ||
                    specCond == fGovernmentSpecialConditions.Socialist ||
                    specCond == fGovernmentSpecialConditions.Oligarchy ||
                    specCond == fGovernmentSpecialConditions.Meritocracy)
                {
                    // give option for a second condition
                    // can't seem to reopen the same dialog box, so refreshing it with new
                    inDiag = new InputRadio(question, options);
                    if (inDiag.ShowDialog() == true)
                        specCond |= (fGovernmentSpecialConditions)inDiag.Answer.Item1;
                }
                return specCond;
            }

            // only get here if we clicked cancel on one of the windows
            return null;

        }

        public eSocietyType GetSocietyType(Planet p)
        {
            string question = "Select the type of society prevalent in the settlement:";
            List<(int, string, string)> options = new List<(int, string, string)>();
            options.Add(((int)eSocietyType.Anarchy, "Anarchy", "No formal laws.  There may be communal expectations, based on what is deemed acceptable practice by other members of the community."));
            options.Add(((int)eSocietyType.ClanTribal, "Clan/Tribal", "One large interlocking family of allied clans or tribes.  Customs and traditions will be important to bind the various groups together."));
            options.Add(((int)eSocietyType.Caste, "Caste", "Similar to clan/tribal, however each clan has a set role.  Likely to be a hierarchy between the clans.  Those who reject their set role are often outcasts with a social stigma."));
            options.Add(((int)eSocietyType.Feudal, "Feudal", "A dictatorship or monarchy where subsidiary rulers retain local power.  The overall ruler therefore needs to maintain sufficient support from their subsidiaries.  Laws can vary by locality."));
            options.Add(((int)eSocietyType.Theocracy, "Theocracy", "Rule by a religious group. Restrictions on other religions are likely."));
            options.Add(((int)eSocietyType.Dictatorship, "Dictatorship", "Rule by a single individual.  If this is hereditary, it is essentially a monarchy.  To maintain control, there may be customary limitations on the ruler's power."));
            options.Add(((int)eSocietyType.RepresentativeDemocracy, "Representative Democracy", "A group of citizens are elected to run the society.  Will depend on the level of education and involvement of the electorate, and whether this is capturable by special interests."));
            options.Add(((int)eSocietyType.AthenianDemocracy, "Athenian Democracy", "Every citizen (however that is defined) votes on every action the society takes.  Below TL 9 this is only practical for groups of up to 10,000."));
            options.Add(((int)eSocietyType.Corporate, "Corporate State", "Society run by a single large corporation.  Most citizens will be employees.  Society tends to be run smoothly (not necessarily well) for the purpose of high profits."));
            options.Add(((int)eSocietyType.Technocracy, "Technocracy", "Engineers and scientists run society in the name of efficiency.  This can encompass relatively free and open societies to opressive dictatorships."));

            int? initial = null;
            switch (p.SocietyType)
            {
                case eSocietyType.Anarchy: initial = 0; break;
                case eSocietyType.ClanTribal: initial = 1; break;
                case eSocietyType.Caste: initial = 2; break;
                case eSocietyType.Feudal: initial = 3; break;
                case eSocietyType.Theocracy: initial = 4; break;
                case eSocietyType.Dictatorship: initial = 5; break;
                case eSocietyType.RepresentativeDemocracy: initial = 6; break;
                case eSocietyType.AthenianDemocracy: initial = 7; break;
                case eSocietyType.Corporate: initial = 8; break;
                case eSocietyType.Technocracy: initial = 9; break;
            }

            InputRadio inDiag = new InputRadio(question, options, initial);
            if (inDiag.ShowDialog() == true)
                return (eSocietyType)inDiag.Answer.Item1;

            // cancel was clicked
            return p.SocietyType;
        }

        public int GetControlRating(Planet p)
        {
            int minCR = RuleBook.SocietyTypeParams[p.SocietyType].MinControlRating;
            int maxCR = RuleBook.SocietyTypeParams[p.SocietyType].MaxControlRating;

            string question = "Select the control rating from the range below.";
            question += "\r\nFor a " + p.SocietyType.ToString() + " society this is generally CR " + minCR.ToString() + ((minCR == maxCR) ? "." : (" to CR " + maxCR.ToString() + "."));

            // adjust for any special conditions
            // see Campaigns p510 for details
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Bureaucracy, minCR, maxCR, question, CRfloor: 4);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Colony, minCR, maxCR, question, CRadj: -1);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Cyberocracy, minCR, maxCR, question, CRfloor: 3);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Meritocracy, minCR, maxCR, question, CRfloor: 3);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.MilitaryGovernment, minCR, maxCR, question, CRfloor: 4);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Oligarchy, minCR, maxCR, question, CRfloor: 3);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Sanctuary, minCR, maxCR, question, CRceiling:4);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Socialist, minCR, maxCR, question, CRfloor: 3);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Subjugated, minCR, maxCR, question, CRfloor: 4);
            (minCR, maxCR, question) = AdjustForSpecialConditions(p, fGovernmentSpecialConditions.Utopia, minCR, maxCR, question, CRadj: -2);

            if (minCR == maxCR)
                question += "\r\nOverall, this suggests CR of " + minCR.ToString() + ".";
            else
                question += "\r\nOverall, this suggests a CR of " + minCR.ToString() + " to " + maxCR.ToString() + ".";
            // removed this as showing all options even if not applicable
            //if (minCR==maxCR)
            //{
            //    // i.e. no options
            //    return minCR;
            //}

            List<(int, string, string)> options = new List<(int, string, string)>();
            for (int i = 0; i <= 6; i++) // showing all CR, not just those implied by society types
            {
                options.Add((i, "CR " + i.ToString(), RuleBook.ControlRatings[i]));
            }
            int? initial = null;
            initial = p.ControlRating;

            InputRadio inDiag = new InputRadio(question, options, initial);
            if (inDiag.ShowDialog() == true)
                return inDiag.Answer.Item1;
            else
                return p.ControlRating;

        }
        private (int, int, string) AdjustForSpecialConditions(Planet p, fGovernmentSpecialConditions specCond, int minCR, int maxCR, string question, int CRceiling = -1, int CRfloor = -1, int CRadj = 0)
        {
            // if the special condition is present, then make the relevant adjustment to min/max CR and add to the question
            if (p.HasGovernmentSpecialCondition(specCond))
            {
                if (CRceiling >= 0) // if we want to apply a maximum to the CR
                {
                    question += "\r\nA society of type " + specCond.ToString() + " will often have at most CR " + CRceiling.ToString() + ".";
                    maxCR = CRceiling;
                    if (minCR >= maxCR) minCR = maxCR;
                }
                if (CRfloor >= 0) // if we want to apply a minimum to the CR
                {
                    question += "\r\nA society of type " + specCond.ToString() + " will often have at least CR " + CRfloor.ToString() + ".";
                    minCR = CRfloor;
                    if (maxCR <= minCR) maxCR = minCR;
                }
                if (CRadj != 0) // if we want to adjust existing CRs
                {
                    question += "\r\nA society of type " + specCond.ToString() + " will often have the CR " + Math.Abs(CRadj).ToString()+" "+((CRadj>0)?"higher":"lower") + ".";
                    minCR += CRadj;
                    maxCR += CRadj;
                    if (minCR < 0) minCR = 0;
                    if (minCR > 6) minCR = 6;
                    if (maxCR < 0) maxCR = 0;
                    if (maxCR > 6) maxCR = 6;
                }
            }

            return (minCR, maxCR, question);
        }

        public double GetTradeVolume(Planet p)
        {
            if (!p.Interstellar)
                // no interstellar trade if uncontacted
                return 0;

            string question = "Enter the trade volume as a percentage of the total economic volume ($" + p.EconomicVolume.ToString("N0") + "). ";
            switch (p.SettlementType)
            {
                case eSettlementType.Homeworld:
                    question += "As a homeworld it is likely to be fairly self-sufficient so trade will likely form a small part of the overall economic volume.  Say up to 40%.";
                    break;
                case eSettlementType.Colony:
                    question += "As a colony, it is likely to be semi-reliant on its connections to the parent civilisation, so trade will be a large portion of economic volume.  Say 30-70%.";
                    break;
                case eSettlementType.Outpost:
                    question += "As an outpost, it is likely to be very reliant on supplies from the parent civilisation and shipping production back, so trade will be almost all of the economic volume.  Say 80-100%.";
                    break;
            }

            InputString inDiag = new InputString(question, (p.TradeVolume/p.EconomicVolume*100).ToString("N0"), true);
            if (inDiag.ShowDialog() == true)
            {
                double prop = double.Parse(inDiag.Answer, NumberStyles.AllowThousands) / 100;
                double trade = prop * p.EconomicVolume;
                trade = RuleBook.RoundToSignificantFigures(trade, 2);
                return trade;
            }
            else
                return p.TradeVolume;
        }

        public int GetSpaceportClass(Planet p)
        {
            int recommendedSpaceportClass = 0;
            if (p.TradeVolume > RuleBook.TradeForSpaceportV && p.PopulationRating >= 6)
                recommendedSpaceportClass = 5;
            else if (p.TradeVolume > RuleBook.TradeForSpaceportIV && p.PopulationRating >= 6)
                recommendedSpaceportClass = 4;
            else if (p.TradeVolume > RuleBook.TradeForSpaceportIII)
                recommendedSpaceportClass = 3;

            string question = "Select spaceport class.  ";
            if (recommendedSpaceportClass > 0)
                question += "The suggested class given the population and trade volume is Class " + RuleBook.ToRoman(recommendedSpaceportClass) + ". ";

            List<(int, string, string)> options = new List<(int, string, string)>();
            for (int i=5;i>=0;i--)
                options.Add((i, "Class " + ((i==0)?"0":RuleBook.ToRoman(i)), RuleBook.SpaceportName[i]));

            int? initial = 5 - p.SpaceportClass;
            InputRadio inDiag = new InputRadio(question, options, initial);
            if (inDiag.ShowDialog() == true)
                return inDiag.Answer.Item1;
            else
                return p.SpaceportClass;
        }

        public List<Installation> GetInstallations(Planet p)
        {
            throw new NotImplementedException();
        }

        public List<Installation> GetInstallation(Planet p, string installationType)
        {

            // and get the relevant parameters
            InstallationParameters parameters = RuleBook.InstallationParams[0];
            for (int i = 0; i < RuleBook.InstallationParams.Count; i++)
                if (RuleBook.InstallationParams[i].Type == installationType)
                    parameters = RuleBook.InstallationParams[i];

            int count = 0;
            bool added = false;
            bool success = false;
            List<Installation> instLst = new List<Installation>();
            Installation? inst;

            do
            {
                (success, inst) = CheckInstallation(p, parameters);

                if (success)
                {
                    if (inst == null)
                    {
                        added = false;
                    }
                    else
                    {
                        instLst.Add(inst);
                        added = true;
                        count++;
                    }
                }
            } while (added && count < parameters.MaxCount);

            if (count > 0 && parameters.HasSecond && parameters.SecondInstallation != null)
            {
                bool success2 = false;
                (success2, inst) = CheckInstallation(p, parameters.SecondInstallation);
                if (success2 && inst != null)
                {
                    instLst.Add(inst);
                }
            }

            if (!success) // i.e. user clicked cancel on first one
                return p.GetInstallations(installationType);
            else
                return instLst;
        }
        private (bool, Installation?) CheckInstallation(Planet p, InstallationParameters parameters)
        {

            // show the dialog to get user input
            SelectInstallation instDialog = new SelectInstallation(parameters);
            if (instDialog.ShowDialog() == true)
            {
                if (instDialog.Selected ==0)
                {
                    // i.e. None was selected so return an empty list
                    return (true, null);
                }
                else
                {
                    // i.e. something was selected, get subtype at index -1 since index 0 was "None"
                    string installationSubType = parameters.SubTypeAtIndex(instDialog.Selected - 1);
                    int PR = instDialog.PopulationRating;
                    Installation retInst = new Installation(parameters.Type, installationSubType, PR);
                    return (true, retInst);
                }

            }
            else // i.e. cancel button was clicked, so return false
                return (false, null);
        }
    }
}
