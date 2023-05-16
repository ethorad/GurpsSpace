using System;
using System.Windows;
using System.Windows.Controls;

namespace GurpsSpace.PlanetCreation
{
    /// <summary>
    /// Interaction logic for PlanetCreator.xaml
    /// </summary>
    public partial class PlanetCreator : Window
    {
        public Planet Planet;
        private IPlanetCreator randomiser;
        private IPlanetCreator userInput;

        public PlanetCreator(Setting setting)
        {
            Planet = new(setting);
            InitializeComponent();
            randomiser = new RandomPlanetNameAndType();
            userInput = new UserPlanet();

            this.DataContext = Planet;
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            string? val = ((Button)sender).Tag.ToString();

            switch (val)
            {
                case "Name":
                    randomiser.SetName(Planet);
                    break;
                case "Type":
                    randomiser.SetSizeAndSubtype(Planet);
                    break;
                case "ResourceValueCategory":
                    randomiser.SetResourceValueCategory(Planet);
                    break;
                case "AtmosphericMass":
                    randomiser.SetAtmosphericMass(Planet);
                    break;
                case "AtmosphericConditions":
                    randomiser.SetAtmosphericConditions(Planet);
                    break;
                case "HydrographicCoverage":
                    randomiser.SetHydrographicCoverage(Planet);
                    break;
                case "AverageSurfaceTempK":
                    randomiser.SetAverageSurfaceTempK(Planet);
                    break;
                case "Density":
                    randomiser.SetDensity(Planet);
                    break;
                case "Gravity":
                    randomiser.SetGravity(Planet);
                    break;
                case "SettlementType":
                    randomiser.SetSettlementType(Planet);
                    break;
                case "Species":
                    randomiser.SetLocalSpecies(Planet);
                    break;
                case "TechLevel":
                    randomiser.SetLocalTechLevel(Planet);
                    break;

            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string? val = ((Button)sender).Tag.ToString();

            switch (val)
            {
                case "Type":
                    userInput.SetSizeAndSubtype(Planet);
                    break;
                case "ResourceValueCategory":
                    userInput.SetResourceValueCategory(Planet);
                    break;
                case "AtmosphericConditions":
                    userInput.SetAtmosphericConditions(Planet);
                    break;
                case "SettlementType":
                    userInput.SetSettlementType(Planet);
                    break;
                case "Species":
                    userInput.SetLocalSpecies(Planet);
                    break;
                case "TechLevel":
                    userInput.SetLocalTechLevel(Planet);
                    break;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Planet.LocalTechLevelDescription);
        }

    }
}
