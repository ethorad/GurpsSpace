using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Documents;

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
            List<(string, string)> options = new List<(string, string)>();
            List<int> vals = ((int[])Enum.GetValues(typeof(eResourceValueCategory))).ToList<int>();
            vals.Sort();
            foreach (int i in vals)
            {
                options.Add((i.ToString(), ((eResourceValueCategory)i).ToString()));
            }

            string question = "Select the resource value for this " + ((p.IsPlanet) ? "planet. " : "asteroid belt. ");
            if (p.IsPlanet)
                question += "\r\nFor a planet, this is normally between -2 (Very Poor) and +2 (Very Abundant).";

            InputRadio radioDiag = new InputRadio(question, options);
            if (radioDiag.ShowDialog() == true)
            {
                return radioDiag.Answer.Item2.ToEnum<eResourceValueCategory>();
            }
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
            {
                return RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereA;
            }
            else
            {
                PlanetParameters pp = RuleBook.PlanetParams[(p.Size, p.Subtype)];
                InputRadio radioDiag = new("Select atmospheric options:", new List<(string, string)>()
                {
                    (pp.AtmosphereA.Item1.ToString(), pp.AtmosphereA.Item2),
                    (pp.AtmosphereB.Item1.ToString(), pp.AtmosphereB.Item2),
                });
                if (radioDiag.ShowDialog() == true)
                {
                    return (radioDiag.Answer.Item1.ToEnum<fAtmosphericConditions>(), radioDiag.Answer.Item2);
                }
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
            List<(string, string)> options = new List<(string, string)>();
            options.Add(("None", "No settlement is present."));
            options.Add(("Outpost", "A minor outpost is present.  For example, a military base or research station."));
            options.Add(("Colony", "A full fledged colony is present.  This will be part of a larger interstellar civilisation.  It will usually have at least positive affinity, i.e. either attractive resource level or a good habitability."));
            options.Add(("Homeworld", "This is the homeworld for a species.  This may be part of an interstellar civilisation.  This wll generally have high habitability for the selected species, as it will have evolved to live here."));
            InputRadio radioDiag = new InputRadio(question, options);

            if (radioDiag.ShowDialog() == true)
            {
                switch (radioDiag.Answer.Item1)
                {
                    case "Outpost":
                        return (eSettlementType.Outpost, 0, true);

                    case "Colony":
                        // get age
                        InputString inStr = new InputString("Enter colony age.  This is used to aid with the population count.", "", true);
                        if (inStr.ShowDialog() == true)
                        {
                            int colonyAge = int.Parse(inStr.Answer);
                            return (eSettlementType.Colony, colonyAge, true);
                        }
                        else
                            return (p.SettlementType, p.ColonyAge, p.Interstellar);

                    case "Homeworld":
                        // get interstellar or not
                        InputRadio inRadio = new InputRadio("Is the homeworld part of an interstellar civilisation?", new List<(string, string)>
                {
                    ("Interstellar","The homeworld is part of an interstellar civilisation."),
                    ("Uncontacted","The homeworld has not spread outside of its system, and has not been contacted.")
                });
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
            List<(string, string)> options = new List<(string, string)>();
            foreach (Species s in p.Setting.Species)
            {
                options.Add((s.Name, s.Description));
            }

            InputRadio radioDiag = new InputRadio("Select the main race inhabiting this " + ((p.IsPlanet) ? "planet:" : "asteroid belt:"), options);
            if (radioDiag.ShowDialog() == true)
            {
                return p.Setting.Species[radioDiag.Selected];
            }
            else
                return p.LocalSpecies;
        }

        public (int, eTechLevelRelativity) GetLocalTechLevel(Planet p)
        {
            int tl = p.Setting.TechLevel;
            eTechLevelRelativity adj = eTechLevelRelativity.Normal;

            List<(string, string)> options = new List<(string, string)>();
            foreach (TechLevelParameters tlp in RuleBook.TechLevelParams.Values)
            {
                options.Add((tlp.TL.ToString(), tlp.Age));
            }
            string question = "Select the main Tech Level for this " + ((p.IsPlanet) ? "planet. " : "asteroid belt. ");
            question += "The setting's TL is " + p.Setting.TechLevel.ToString() + " so anything at " + (p.Setting.TechLevel - 4).ToString() + " or below would be considered primitive.";
            InputRadio radioDiag = new InputRadio(question, options);
            if (radioDiag.ShowDialog() == true)
            {
                tl = radioDiag.Selected; // since in order of TL starting from zero can just use the selected index

                question = "Select whether the settlement is delayed or advanced relative to TL " + tl.ToString() + ".";
                options.Clear();
                options.Add(("Delayed", "Settlement is behind normal for TL " + tl.ToString() + "."));
                options.Add(("Normal", "Settlement has a normal level of development for TL " + tl.ToString() + "."));
                options.Add(("Advanced", "Settlement is ahead of normal for TL " + tl.ToString() + "."));
                radioDiag = new InputRadio(question, options);
                if (radioDiag.ShowDialog() == true)
                {
                    switch (radioDiag.Answer.Item1)
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
                }
                return (tl, adj);
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
            InputString inDiag = new InputString(question, "", true);

            if (inDiag.ShowDialog() == true)
            {
                double perc = double.Parse(inDiag.Answer) / 100;
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

            InputString inDiag = new InputString(question, "", true);
            if (inDiag.ShowDialog() == true)
            {
                if (inDiag.Answer != "")
                    return double.Parse(inDiag.Answer);
            }

            return p.Population;
        }
        private double GetPopulationOutpost(Planet p)
        {
            string question = "Enter the outpost population below. This will generally be in the range 100 to 100,000.";
            InputString inDiag = new InputString(question, "", true);
            if(inDiag.ShowDialog()==true)
            {
                return double.Parse(inDiag.Answer);
            }
            return p.Population;
        }

        public (eWorldUnityLevel, fGovernmentSpecialConditions) GetWorldGovernance(Planet p)
        {

            string question = "Select the degree to which the settlement is socially unified. " +
                "Higher technology levels, and smaller populations, tend to be more unified.";

            List<(string, string)> answers = new List<(string, string)>();
            answers.Add(("Diffuse", "A variety of social structure, none dominating."));
            answers.Add(("Factionalised", "A few competing social structures."));
            answers.Add(("Coalition", "A few cooperating social structures."));
            answers.Add(("World Government", "A single world government, which may have special conditions."));

            InputRadio inDiag = new InputRadio(question, answers);

            eWorldUnityLevel unity = eWorldUnityLevel.Diffuse;
            fGovernmentSpecialConditions? specCond = fGovernmentSpecialConditions.None;
            bool hasSpecialCond = false;
            if (inDiag.ShowDialog()==true)
            {
                switch(inDiag.Answer.Item1)
                {
                    case "Diffuse":
                        unity = eWorldUnityLevel.Diffuse;
                        break;
                    case "Factionalised":
                        unity = eWorldUnityLevel.Factionalised;
                        break;
                    case "Coalition":
                        unity = eWorldUnityLevel.Coalition;
                        break;
                    case "World Government":
                        unity = eWorldUnityLevel.WorldGovernment;
                        hasSpecialCond = true;
                        break;
                }

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
            List<(string, string)> options = new List<(string, string)>();
            List<(fGovernmentSpecialConditions, bool)> answers = new List<(fGovernmentSpecialConditions, bool)>();
            options.Add(("None", "No special conditions prevail."));
            answers.Add((fGovernmentSpecialConditions.None, false));
            options.Add(("Subjugated", "Under the control of another civilisation. Second condition is possible."));
            answers.Add((fGovernmentSpecialConditions.Subjugated, true));
            options.Add(("Sanctuary", "Particularly welcoming of refugees, likely to be an independent settlement."));
            answers.Add((fGovernmentSpecialConditions.Sanctuary, false));
            options.Add(("Military Government", "Run by the military, citizenship likely tied to service."));
            answers.Add((fGovernmentSpecialConditions.MilitaryGovernment, false));
            options.Add(("Socialist", "Significant government support to individuals.  Second condition is possible."));
            answers.Add((fGovernmentSpecialConditions.Socialist, true));
            options.Add(("Bureaucracy", "Significant rules and regulations governing life."));
            answers.Add((fGovernmentSpecialConditions.Bureaucracy, false));
            options.Add(("Colony", "An offshoot of a larger galactic civilisation."));
            answers.Add((fGovernmentSpecialConditions.Colony, false));
            options.Add(("Oligarchy", "Ruled by a small group of individuals, likely very wealthy. Second condition is possible."));
            answers.Add((fGovernmentSpecialConditions.Oligarchy, true));
            options.Add(("Meritocracy", "Ruled according to merit, defined in some manner. Second condition is possible."));
            answers.Add((fGovernmentSpecialConditions.Meritocracy, true));
            options.Add(("Genderarchy", "Ruled by a single gender (in Earth terms, a Patriarchy or Matriarchy)."));
            answers.Add((fGovernmentSpecialConditions.Genderarchy, false));
            options.Add(("Utopia", "Egalitarian society which aims to be perfect."));
            answers.Add((fGovernmentSpecialConditions.Utopia, false));
            if (p.LocalTechLevel > 7)
            {
                options.Add(("Cyberocracy", "Ruled by interconnected computer systems, with lifeforms only used for unusual situations."));
                answers.Add((fGovernmentSpecialConditions.Cyberocracy, false));
            }

            InputRadio inDiag = new InputRadio(question, options);
            if (inDiag.ShowDialog() == true)
            {
                specCond = answers[inDiag.Selected].Item1;

                if (answers[inDiag.Selected].Item2)
                {
                    // give option for a second condition
                    // can't seem to reopen the same dialog box, so refreshing it with new
                    inDiag = new InputRadio(question, options);
                    if (inDiag.ShowDialog() == true)
                    {
                        specCond |= answers[inDiag.Selected].Item1;
                        return specCond;
                    }
                }
            }

            // only get here if we clicked cancel on one of the windows
            return null;

        }

        public eSocietyType GetSocietyType(Planet p)
        {
            string question = "Select the type of society prevalent in the settlement:";
            List<(string, string)> options = new List<(string, string)>();
            options.Add(("Anarchy", ""));
            options.Add(("Clan/Tribal", ""));
            options.Add(("Caste", ""));
            options.Add(("Feudal", ""));
            options.Add(("Theocracy", ""));
            options.Add(("Dictatorship", ""));
            options.Add(("Representative Democracy", ""));
            options.Add(("Athenian Democracy", ""));
            options.Add(("Corporate State", ""));
            options.Add(("Technocracy", ""));

            InputRadio inDiag = new InputRadio(question, options);
            if (inDiag.ShowDialog() == true)
            {
                switch (inDiag.Answer.Item1)
                {
                    case "Anarchy":
                        return eSocietyType.Anarchy;
                    case "Clan/Tribal":
                        return eSocietyType.ClanTribal;
                    case "Caste":
                        return eSocietyType.Caste;
                    case "Feudal":
                        return eSocietyType.Feudal;
                    case "Theocracy":
                        return eSocietyType.Theocracy;
                    case "Dictatorship":
                        return eSocietyType.Dictatorship;
                    case "Representative Democracy":
                        return eSocietyType.RepresentativeDemocracy;
                    case "Athenian Democracy":
                        return eSocietyType.AthenianDemocracy;
                    case "Corporate State":
                        return eSocietyType.Corporate;
                    case "Technocracy":
                        return eSocietyType.Technocracy;
                }
            }

            // cancel was clicked
            return p.SocietyType;
        }

        public int GetControlRating(Planet p)
        {
            int minCR = RuleBook.SocietyTypeParams[p.SocietyType].MinControlRating;
            int maxCR = RuleBook.SocietyTypeParams[p.SocietyType].MaxControlRating;

            if (maxCR == minCR)
                // no option
                return minCR;

            string question = "Select the control rating from the range below:";
            List<(string, string)> options = new List<(string, string)>();
            for (int i = minCR; i <= maxCR; i++)
            {
                options.Add(("CR " + i.ToString(), RuleBook.ControlRatings[i]));
            }

            InputRadio inDiag = new InputRadio(question, options);
            if (inDiag.ShowDialog() == true)
                return inDiag.Selected + minCR;
            else
                return p.ControlRating;

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

            InputString inDiag = new InputString(question, "", true);
            if (inDiag.ShowDialog() == true)
            {
                double prop = double.Parse(inDiag.Answer) / 100;
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

            List<(string, string)> options = new List<(string, string)>();
            for (int i=5;i>=0;i--)
                options.Add(("Class " + ((i==0)?"0":RuleBook.ToRoman(i)), RuleBook.SpaceportName[i]));

            InputRadio inDiag = new InputRadio(question, options);
            if (inDiag.ShowDialog() == true)
                return 5 - inDiag.Selected;
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
