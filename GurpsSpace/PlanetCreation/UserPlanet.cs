using System;
using System.Collections.Generic;
using System.Linq;

namespace GurpsSpace.PlanetCreation
{
    internal class UserPlanet : IPlanetCreator
    {


        public string SetName(Planet p)
        {
            InputString inStr = new("Enter planet's name:", p.Name);
            if (inStr.ShowDialog()==true)
            {
                p.Name = inStr.Answer;
            }
            return p.Name;
        }

        public (eSize, eSubtype) SetSizeAndSubtype(Planet p)
        {
            PlanetTypeSelection typeDiag = new();
            if (typeDiag.ShowDialog() == true)
            {
                p.Size = typeDiag.Size;
                p.Subtype = typeDiag.Subtype;
            }
            return (p.Size, p.Subtype);
        }

        public eResourceValueCategory SetResourceValueCategory(Planet p)
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

        public double SetAtmosphericMass(Planet p)
        {
            throw new NotImplementedException();
        }

        public (fAtmosphericConditions, string) SetAtmosphericConditions(Planet p)
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

        public double SetHydrographicCoverage(Planet p)
        {
            throw new NotImplementedException();
        }

        public int SetAverageSurfaceTempK(Planet p)
        { 
            throw new NotImplementedException(); 
        }

        public double SetDensity(Planet p)
        {
            throw new NotImplementedException();
        }

        public double SetGravity(Planet p)
        {
            throw new NotImplementedException();
        }

        public eSettlementType SetSettlementType(Planet p)
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

        public Species SetLocalSpecies(Planet p)
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

        public int SetLocalTechLevel(Planet p)
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

    }
}
