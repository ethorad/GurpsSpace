using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.Helpers
{
    internal class TraitParameters
    {
        public eTrait Trait;
        public string Name;
        public string Specialty;

        public int BaseCost;
        public int LevelCost;
        public int MaxLevel;

        public List<eTrait> BannedTraits;

        public TraitParameters(eTrait trait, string name, string specialty, int baseCost, int levelCost, int maxLevel)
        {
            Trait = trait;
            Name = name;
            Specialty = specialty;
            BaseCost = baseCost;
            LevelCost = levelCost;
            MaxLevel = maxLevel;
            BannedTraits = new List<eTrait>();
        }
    }
}
