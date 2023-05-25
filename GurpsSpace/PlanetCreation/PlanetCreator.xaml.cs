
using System.Windows;
using System.Windows.Controls;

namespace GurpsSpace.PlanetCreation
{
    /// <summary>
    /// Interaction logic for PlanetCreator.xaml
    /// </summary>
    public partial class PlanetCreator : Window
    {
        public ViewModelPlanet vmPlanet;
        public Planet Planet;
        private IPlanetCreator randomiser;
        private IPlanetCreator userInput;

        public PlanetCreator(Setting setting)
        {
            Planet = new(setting);
            vmPlanet = new ViewModelPlanet(Planet);
            InitializeComponent();
            randomiser = new RandomPlanet();
            userInput = new UserPlanet();

            this.DataContext = vmPlanet;
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            SetParameter(val, randomiser);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            SetParameter(val, userInput);
        }

        private void SetParameter(string param, IPlanetCreator pc)
        {
            switch (param)
            {
                case "Name":
                    pc.SetName(vmPlanet);
                    break;
                case "Type":
                    pc.SetSizeAndSubtype(vmPlanet);
                    break;
                case "ResourceValueCategory":
                    pc.SetResourceValueCategory(vmPlanet);
                    break;
                case "AtmosphericMass":
                    pc.SetAtmosphericMass(vmPlanet);
                    break;
                case "AtmosphericConditions":
                    pc.SetAtmosphericConditions(vmPlanet);
                    break;
                case "HydrographicCoverage":
                    pc.SetHydrographicCoverage(vmPlanet);
                    break;
                case "AverageSurfaceTempK":
                    pc.SetAverageSurfaceTempK(vmPlanet);
                    break;
                case "Density":
                    pc.SetDensity(vmPlanet);
                    break;
                case "Gravity":
                    pc.SetGravity(vmPlanet);
                    break;
                case "SettlementType":
                    pc.SetSettlementType(vmPlanet);
                    break;
                case "Species":
                    pc.SetLocalSpecies(vmPlanet);
                    break;
                case "TechLevel":
                    pc.SetLocalTechLevel(vmPlanet);
                    break;
                case "Population":
                    pc.SetPopulation(vmPlanet);
                    break;
                case "WorldGovernance":
                    pc.SetWorldGovernance(vmPlanet);
                    break;
                case "SocietyType":
                    pc.SetSocietyType(vmPlanet);
                    break;
                case "ControlRating":
                    pc.SetControlRating(vmPlanet);
                    break;
                case "TradeVolume":
                    pc.SetTradeVolume(vmPlanet);
                    break;
                case "SpaceportClass":
                    pc.SetSpaceportClass(vmPlanet);
                    break;
                case "Installations":
                    pc.SetInstallations(vmPlanet);
                    break;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(vmPlanet.PopulationRating.ToString());
        }

        private void btnViewInstallation_Click(object sender, RoutedEventArgs e)
        {
            InstallationsList instWindow = new InstallationsList(new ViewModelInstallationList(Planet.Installations));
            instWindow.ShowDialog();
        }
    }
}
