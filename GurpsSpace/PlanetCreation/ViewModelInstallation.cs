using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.PlanetCreation
{
    public class ViewModelInstallation : ViewModel
    {
        private Installation installation;
        public Installation Installation
        { 
            get { return installation; }
        }

        public ViewModelInstallation(Installation installation)
        {
            this.installation = installation;
            MemberUpdated();
        }

        public string Type { get { return Installation.Type; } }
        public string Subtype { get { return Installation.Subtype; } }
        public string Name { get { return  Installation.Name; } }

        public int PR { get { return Installation.PR; } }

        public string PRstring { get { return (PR == 0) ? "n/a" : PR.ToString("N0"); } }
    }
}
