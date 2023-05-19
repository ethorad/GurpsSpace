using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach (TechLevelParameters tlp in RuleBook.TechLevelParams)
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
                switch (radioDiag.Selected)
                {
                    case 0:
                        p.LocalTechLevelIsDelayed = true; 
                        break;
                    case 1:
                        p.LocalTechLevelIsDelayed = false;
                        p.LocalTechLevelIsAdvanced = false;
                        break;
                    case 2:
                        p.LocalTechLevelIsAdvanced = true;
                        break;
                }
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
                ulong pop = (ulong)((double)p.CarryingCapacity * perc);
                pop = RuleBook.RoundToSignificantFigures(pop, 2);
                p.Population = pop;
            }

            return p.Population;
        }
        private ulong SetPopulationColony(ViewModelPlanet p)
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
            ulong population = (ulong)(s.StartingColonyPopulation * Math.Pow(1 + s.AnnualGrowthRate, effectiveDecadesOfGrowth * 10));
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
                    p.Population = ulong.Parse(inDiag.Answer);
            }

            return p.Population;
        }
        private ulong SetPopulationOutpost(ViewModelPlanet p)
        {
            string question = "Enter the outpost population below. This will generally be in the range 100 to 100,000.";
            InputString inDiag = new InputString(question, "", true);
            if(inDiag.ShowDialog()==true)
            {
                p.Population = ulong.Parse(inDiag.Answer);
            }
            return p.Population;
        }

        public eWorldUnityLevel SetWorldUnityLevel(ViewModelPlanet p)
        {
            throw new NotImplementedException();
        }

    }
}
