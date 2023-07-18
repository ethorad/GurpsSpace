﻿using GurpsSpace.Helpers;
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

        private TraitParameters parameters { get { return RuleBook.TraitParams[TraitType]; } }
        public string Name { get { return parameters.Name; } }
        public int Cost { get { return parameters.BaseCost + parameters.LevelCost * Level + VariableCost; } }

        public Trait(eTrait traitType, string specialty, int level, int variableCost)
        {
            TraitType = traitType;
            Specialty = specialty;
            Level = level;
            VariableCost = variableCost;
        }
        public Trait(eTrait traitType, string specialty)
        {
            TraitType = traitType;
            Specialty = specialty;
            Level = 0;
            VariableCost = 0;
        }
        public Trait(Trait t)
        {
            TraitType = t.TraitType;
            Specialty = t.Specialty;
            Level = t.Level;
            VariableCost = t.VariableCost;
        }

    }
}