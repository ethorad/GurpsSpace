using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.PlanetCreation
{
    internal class ViewModelPlanet : ViewModel
    {
        private Planet planet;
        public Planet Planet
        {
            get { return planet; }
            set
            {
                planet = value;
                MemberUpdated();
            }
        }

        public ViewModelPlanet(Planet p)
        {
            planet = p;
        }
    }
}
