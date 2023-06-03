using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.PlanetCreation
{
    public class ViewModelInstallationParameters : ViewModel
    {
        private InstallationParameters installationParameters;
        public InstallationParameters InstallationParameters { get { return installationParameters; } }
        
        public string Type { get { return InstallationParameters.Type; } }
        public int MinPR { get { return InstallationParameters.MinimumPopulationRating(); } }
        public int MaxPR { get { return InstallationParameters.MaximumPopulationRating(); } }

        private int populationRating;
        public int PopulationRating
        {
            get { return populationRating; }
            set
            {
                populationRating = value;
                MemberUpdated();
            }
        }

        public bool HasPopulationRating { get { return MaxPR > 0; } }

        public ViewModelInstallationParameters(InstallationParameters installationParameters)
        {
            this.installationParameters = installationParameters;
            MemberUpdated();
        }
    }
}
