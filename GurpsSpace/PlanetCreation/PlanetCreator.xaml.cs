
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
            string? val = ((Button)sender).Tag.ToString();

            switch (val)
            {
                case "Name":
                    randomiser.SetName(vmPlanet);
                    break;
                case "Type":
                    randomiser.SetSizeAndSubtype(vmPlanet);
                    break;
                case "ResourceValueCategory":
                    randomiser.SetResourceValueCategory(vmPlanet);
                    break;
                case "AtmosphericMass":
                    randomiser.SetAtmosphericMass(vmPlanet);
                    break;
                case "AtmosphericConditions":
                    randomiser.SetAtmosphericConditions(vmPlanet);
                    break;
                case "HydrographicCoverage":
                    randomiser.SetHydrographicCoverage(vmPlanet);
                    break;
                case "AverageSurfaceTempK":
                    randomiser.SetAverageSurfaceTempK(vmPlanet);
                    break;
                case "Density":
                    randomiser.SetDensity(vmPlanet);
                    break;
                case "Gravity":
                    randomiser.SetGravity(vmPlanet);
                    break;
                case "SettlementType":
                    randomiser.SetSettlementType(vmPlanet);
                    break;
                case "Species":
                    randomiser.SetLocalSpecies(vmPlanet);
                    break;
                case "TechLevel":
                    randomiser.SetLocalTechLevel(vmPlanet);
                    break;
                case "Population":
                    randomiser.SetPopulation(vmPlanet);
                    break;
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string? val = ((Button)sender).Tag.ToString();

            switch (val)
            {
                case "Type":
                    userInput.SetSizeAndSubtype(vmPlanet);
                    break;
                case "ResourceValueCategory":
                    userInput.SetResourceValueCategory(vmPlanet);
                    break;
                case "AtmosphericConditions":
                    userInput.SetAtmosphericConditions(vmPlanet);
                    break;
                case "SettlementType":
                    userInput.SetSettlementType(vmPlanet);
                    break;
                case "Species":
                    userInput.SetLocalSpecies(vmPlanet);
                    break;
                case "TechLevel":
                    userInput.SetLocalTechLevel(vmPlanet);
                    break;
                case "Population":
                    userInput.SetPopulation(vmPlanet);
                    break;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(vmPlanet.LocalTechLevelString);
        }

    }
}
