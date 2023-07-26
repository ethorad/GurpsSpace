using GurpsSpace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace
{
    public class Trait
    {
        public eTrait TraitType;
        public string Specialty;
        public int Level;
        public int VariableCost;
        public List<Trait> SubTraits;

        private TraitParameters parameters { get { return RuleBook.TraitParams[TraitType]; } }
        public string Name { get { return parameters.Name; } }
        public int Cost
        { 
            get 
            {
                int tot = 0;
                tot += parameters.BaseCost + parameters.LevelCost * Level + VariableCost; 
                foreach (Trait trait in SubTraits)
                {
                    tot += trait.Cost;
                }
                return tot;
            } 
        }

        public Trait(eTrait traitType, string specialty, int level, int variableCost)
        {
            TraitType = traitType;
            Specialty = specialty;
            Level = level;
            VariableCost = variableCost;
            SubTraits = new List<Trait>();
        }
        public Trait(eTrait traitType, string specialty)
        {
            TraitType = traitType;
            Specialty = specialty;
            Level = 0;
            VariableCost = 0;
            SubTraits = new List<Trait>();
        }
        public Trait(eTrait traitType)
        {
            TraitType = traitType;
            Specialty = "";
            Level = 0;
            VariableCost = 0;
            SubTraits = new List<Trait>();
        }
        public Trait(Trait t)
        {
            TraitType = t.TraitType;
            Specialty = t.Specialty;
            Level = t.Level;
            VariableCost = t.VariableCost;
            SubTraits = new List<Trait>();
            foreach (Trait subT in t.SubTraits)
            {
                SubTraits.Add(new Trait(subT));
            }
        }

    }
}
