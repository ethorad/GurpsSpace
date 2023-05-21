using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace
{
    internal class SocietyTypeParameters
    {
        private eSocietyType societyType;
        public eSocietyType SocietyType { get { return  societyType; } }

        private int minControlRating;
        public int MinControlRating { get { return minControlRating; } }

        private int maxControlRating;
        public int MaxControlRating { get { return maxControlRating; } }

        public SocietyTypeParameters(eSocietyType societyType, int minControlRating, int maxControlRating)
        {
            this.societyType = societyType;
            this.minControlRating = minControlRating;
            this.maxControlRating = maxControlRating;
        }
    }
}
