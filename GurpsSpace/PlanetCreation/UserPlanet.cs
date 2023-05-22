using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Annotations;

namespace GurpsSpace.PlanetCreation
{
    internal class UserPlanet : IPlanetCreator
    {


        public string SetName(ViewModelPlanet p)
        {
            InputString inStr = new("Enter planet's name:", p.Name);
            if (inStr.ShowDialog()==true)
            {
                p.Name = inStr.Answer;
            }
            return p.Name;
        }

        public (eSize, eSubtype) SetSizeAndSubtype(ViewModelPlanet p)
        {
            PlanetTypeSelection typeDiag = new();
            if (typeDiag.ShowDialog() == true)
            {
                p.Size = typeDiag.Size;
                p.Subtype = typeDiag.Subtype;
            }
            return (p.Size, p.Subtype);
        }

        public eResourceValueCategory SetResourceValueCategory(ViewModelPlanet p)
        {
            List<(string, string)> options = new List<(string, string)>();
            List<int> vals = ((int[])Enum.GetValues(typeof(eResourceValueCategory))).ToList<int>();
            vals.Sort();
            foreach (int i in vals)
            {
                options.Add((i.ToString(), ((eResourceValueCategory)i).ToString()));
            }
            InputRadio radioDiag = new InputRadio("Select the resource value for this " + ((p.IsPlanet) ? "planet:" : "asteroid belt:"), options);
            if (radioDiag.ShowDialog() == true)
            {
                p.ResourceValueCategory = radioDiag.Answer.Item2.ToEnum<eResourceValueCategory>();
            }
            return p.ResourceValueCategory;
        }

        public double SetAtmosphericMass(ViewModelPlanet p)
        {
            throw new NotImplementedException();
        }

        public (fAtmosphericConditions, string) SetAtmosphericConditions(ViewModelPlanet p)
        {
            if (!p.HasAtmosphericOptions)
            {
                (p.AtmosphericConditions, p.AtmosphericDescription) = RuleBook.PlanetParams[(p.Size, p.Subtype)].AtmosphereA;
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
                    p.AtmosphericConditions = radioDiag.Answer.Item1.ToEnum<fAtmosphericConditions>();
                    p.AtmosphericDescription = radioDiag.Answer.Item2;
                }
            }
            return (p.AtmosphericConditions, p.AtmosphericDescription);
        }

        public double SetHydrographicCoverage(ViewModelPlanet p)
        {
            throw new NotImplementedException();
        }

        public int SetAverageSurfaceTempK(ViewModelPlanet p)
        { 
            throw new NotImplementedException(); 
        }

        public double SetDensity(ViewModelPlanet p)
        {
            throw new NotImplementedException();
        }

        public double SetGravity(ViewModelPlanet p)
        {
            throw new NotImplementedException();
        }

        public eSettlementType SetSettlementType(ViewModelPlanet p)
        {
            string question = "Select the settlement type to be present:";
            List<(string, string)> options = new List<(string, string)>();
            options.Add(("None", "No settlement is present."));
            options.Add(("Outpost", "A minor outpost is present.  For example, a military base or research station."));
            options.Add(("Colony", "A full fledged colony is present.  This will be part of a larger interstellar civilisation.  It will usually have at least positive affinity, i.e. either attractive resource level or a good habitability."));
            options.Add(("Homeworld", "This is the homeworld for a species.  This may be part of an interstellar civilisation.  This wll generally have high habitability for the selected species, as it will have evolved to live here."));
            InputRadio radioDiag = new InputRadio(question, options);
            if (radioDiag.ShowDialog()==true)
            {
                switch(radioDiag.Answer.Item1)
                {
                    case "Outpost":
                        p.SettlementType = eSettlementType.Outpost;
                        break;
                    case "Colony":
                        p.SettlementType = eSettlementType.Colony;
                        break;
                    case "Homeworld":
                        p.SettlementType = eSettlementType.Homeworld;
                        break;
                    default:
                        p.SettlementType = eSettlementType.None;
                        break;
                }
            }

            if(p.SettlementType==eSettlementType.Colony)
            {
                // get age
                InputString inStr = new InputString("Enter colony age.  This is used to aid with the population count.", "", true);
                if (inStr.ShowDialog()==true)
                {
                    p.ColonyAge = int.Parse(inStr.Answer);
                }
            }
            else
            {
                // not a colony
                p.ColonyAge = 0;
            }

            if(p.SettlementType == eSettlementType.Homeworld)
            {
                // get interstellar or not
                InputRadio inRadio = new InputRadio("Is the homeworld part of an interstellar civilisation?", new List<(string, string)>
                {
                    ("Interstellar","The homeworld is part of an interstellar civilisation."),
                    ("Uncontacted","The homeworld has not spread outside of its system, and has not been contacted.")
                });
                if (inRadio.ShowDialog()==true)
                {
                    if (inRadio.Selected == 0)
                        p.Interstellar = true;
                    if (inRadio.Selected==1)
                        p.Interstellar = false;
                }
            }
            else
            {
                // not a homeworld - colonies and outposts are assumed to be interstellar
                p.Interstellar = true;
            }

            return p.SettlementType;

        }

        public Species SetLocalSpecies(ViewModelPlanet p)
        {
            List<(string, string)> options = new List<(string, string)>();
            foreach (Species s in p.Setting.Species)
            {
                options.Add((s.Name, s.Description));
            }
            InputRadio radioDiag = new InputRadio("Select the main race inhabiting this " + ((p.IsPlanet) ? "planet:" : "asteroid belt:"), options);
            if (radioDiag.ShowDialog()==true)
            {
                p.LocalSpecies = p.Setting.Species[radioDiag.Selected];
            }
            return p.LocalSpecies;

        }

        public int SetLocalTechLevel(ViewModelPlanet p)
        {
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
                p.LocalTechLevel = radioDiag.Selected; // since in order of TL starting from zero can just use the selected index
            }


            question = "Select whether the settlement is delayed or advanced relative to TL" + p.LocalTechLevel.ToString() + ".";
            options.Clear();
            options.Add(("Delayed", "Settlement is behind normal for TL" + p.LocalTechLevel.ToString() + "."));
            options.Add(("Normal", "Settlement has a normal level of development for TL" + p.LocalTechLevel.ToString() + "."));
            options.Add(("Advanced", "Settlement is ahead of normal for TL" + p.LocalTechLevel.ToString() + "."));
            radioDiag = new InputRadio(question, options);
            if (radioDiag.ShowDialog()== true)
            {
                switch (radioDiag.Answer.Item1)
                {
                    case "Delayed":
                        p.LocalTechLevelRelativity = eTechLevelRelativity.Delayed;
                        break;
                    case "Normal":
                        p.LocalTechLevelRelativity = eTechLevelRelativity.Normal;
                        break;
                    case "Advanced":
                        p.LocalTechLevelRelativity = eTechLevelRelativity.Advanced;
                        break;
                }
            }
            return p.LocalTechLevel;
        }

        public double SetPopulation(ViewModelPlanet p)
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
        private double SetPopulationHomeworld(ViewModelPlanet p)
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
                p.Population = pop;
            }

            return p.Population;
        }
        private double SetPopulationColony(ViewModelPlanet p)
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
            if (population > s.CarryingCapacity(p.Planet))
                population = s.CarryingCapacity(p.Planet);

            string question = "Enter the colony population below. ";
            question += "For a " + s.Name + " colony, the minimum size is " + s.StartingColonyPopulation.ToString("N0") + ". ";
            question += "After " + p.ColonyAge + " years of growth, this is expected to have reached " + population.ToString("N0") + ". ";

            InputString inDiag = new InputString(question, "", true);
            if (inDiag.ShowDialog() == true)
            {
                if (inDiag.Answer != "")
                    p.Population = double.Parse(inDiag.Answer);
            }

            return p.Population;
        }
        private double SetPopulationOutpost(ViewModelPlanet p)
        {
            string question = "Enter the outpost population below. This will generally be in the range 100 to 100,000.";
            InputString inDiag = new InputString(question, "", true);
            if(inDiag.ShowDialog()==true)
            {
                p.Population = double.Parse(inDiag.Answer);
            }
            return p.Population;
        }

        public (eWorldUnityLevel, fGovernmentSpecialConditions) SetWorldGovernance(ViewModelPlanet p)
        {
            string question = "Select the degree to which the settlement is socially unified. " +
                "Higher technology levels, and smaller populations, tend to be more unified.";

            List<(string, string)> answers = new List<(string, string)>();
            answers.Add(("Diffuse", "A variety of social structure, none dominating."));
            answers.Add(("Factionalised", "A few competing social structures."));
            answers.Add(("Coalition", "A few cooperating social structures."));
            answers.Add(("World Government", "A single world government, which may have special conditions."));

            InputRadio inDiag = new InputRadio(question, answers);
            if (inDiag.ShowDialog()==true)
            {
                switch(inDiag.Answer.Item1)
                {
                    case "Diffuse":
                        p.WorldUnityLevel = eWorldUnityLevel.Diffuse;
                        p.GovernmentSpecialConditions = fGovernmentSpecialConditions.None;
                        break;
                    case "Factionalised":
                        p.WorldUnityLevel = eWorldUnityLevel.Factionalised;
                        p.GovernmentSpecialConditions = fGovernmentSpecialConditions.None;
                        break;
                    case "Coalition":
                        p.WorldUnityLevel = eWorldUnityLevel.Coalition;
                        p.GovernmentSpecialConditions = fGovernmentSpecialConditions.None;
                        break;
                    case "World Government":
                        p.WorldUnityLevel = eWorldUnityLevel.WorldGovernment;
                        p.GovernmentSpecialConditions = GetGovernmentSpecialConditions(p);
                        break;
                }
            }

            return (p.WorldUnityLevel, p.GovernmentSpecialConditions);
        }
        private fGovernmentSpecialConditions GetGovernmentSpecialConditions(ViewModelPlanet p)
        {
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
            options.Add(("Matriarchy", "Ruled by women (or equivalent alien gender)."));
            answers.Add((fGovernmentSpecialConditions.Matriarchy, false));
            options.Add(("Patriarchy", "Ruled by men (or equivalent alien gender)."));
            answers.Add((fGovernmentSpecialConditions.Patriarchy, false));
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
                p.GovernmentSpecialConditions = answers[inDiag.Selected].Item1;
            }

            if (answers[inDiag.Selected].Item2)
            {
                // give option for a second condition
                // can't seem to reopen the same dialog box, so refreshing it with new
                inDiag = new InputRadio(question, options);
                if (inDiag.ShowDialog() == true)
                    p.GovernmentSpecialConditions |= answers[inDiag.Selected].Item1;
            }

            return p.GovernmentSpecialConditions;
        }

        public eSocietyType SetSocietyType(ViewModelPlanet p)
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
                switch(inDiag.Answer.Item1)
                {
                    case "Anarchy":
                        p.SocietyType = eSocietyType.Anarchy;
                        break;
                    case "Clan/Tribal":
                        p.SocietyType = eSocietyType.ClanTribal;
                        break;
                    case "Caste":
                        p.SocietyType = eSocietyType.Caste;
                        break;
                    case "Feudal":
                        p.SocietyType = eSocietyType.Feudal;
                        break;
                    case "Theocracy":
                        p.SocietyType = eSocietyType.Theocracy;
                        break;
                    case "Dictatorship":
                        p.SocietyType = eSocietyType.Dictatorship;
                        break;
                    case "Representative Democracy":
                        p.SocietyType = eSocietyType.RepresentativeDemocracy;
                        break;
                    case "Athenian Democracy":
                        p.SocietyType = eSocietyType.AthenianDemocracy;
                        break;
                    case "Corporate State":
                        p.SocietyType = eSocietyType.Corporate;
                        break;
                    case "Technocracy":
                        p.SocietyType = eSocietyType.Technocracy;
                        break;
                }
            }

            return p.SocietyType;
        }

        public int SetControlRating(ViewModelPlanet p)
        {
            int minCR = RuleBook.SocietyTypeParams[p.SocietyType].MinControlRating;
            int maxCR = RuleBook.SocietyTypeParams[p.SocietyType].MaxControlRating;

            if (maxCR == minCR)
            {
                // no option
                p.ControlRating = minCR;
            }
            else
            {
                string question = "Select the control rating from the range below:";
                List<(string, string)> options = new List<(string, string)>();
                for (int i=minCR; i<=maxCR; i++)
                {
                    options.Add(("CR " + i.ToString(), RuleBook.ControlRatings[i]));
                }
                
                InputRadio inDiag = new InputRadio(question, options);
                if (inDiag.ShowDialog()==true)
                {
                    p.ControlRating = inDiag.Selected + minCR;
                }
                
            }

            return p.ControlRating;
        }

        public double SetTradeVolume(ViewModelPlanet p)
        {
            if (!p.Interstellar)
            {
                // no interstellar trade if uncontacted
                p.TradeVolume = 0;
            }
            else
            {
                string question = "Enter the trade volume as a percentage of the total economic volume (" + p.EconomicVolumeString + "). ";
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
                    p.TradeVolume = trade;
                }
            }
            return p.TradeVolume;
        }
    }
}
