
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            randomiser = new RandomPlanet();
            userInput = new UserPlanet();

            InitializeComponent();
            SetUpInstallationGrid();

            this.DataContext = vmPlanet;
        }
        private void SetUpInstallationGrid()
        {
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Auto);
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Auto);
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[4].Width = new GridLength(1, GridUnitType.Auto);

            Label lbl;
            Button but;
            Binding bind;

            for (int i=0;i<RuleBook.InstallationParams.Count;i++)
            {
                InstallationGrid.RowDefinitions.Add(new RowDefinition());

                lbl = new Label();
                lbl.Content = RuleBook.InstallationParams[i].Type;
                Grid.SetRow(lbl,i);
                Grid.SetColumn(lbl, 0);
                InstallationGrid.Children.Add(lbl);

                lbl = new Label();
                bind = new Binding("SpaceportClassString"); // ("InstallSummary" + RuleBook.InstallationParams[i].Type);
                bind.Mode = BindingMode.OneWay;
                BindingOperations.SetBinding(lbl, Label.ContentProperty, bind);
                Grid.SetRow(lbl,i);
                Grid.SetColumn(lbl, 1);
                InstallationGrid.Children.Add(lbl);

                but = new Button();
                but.Content = "Add";
                but.Tag = i;
                but.Click += btnAddInstallation_Click;
                Grid.SetRow(but, i);
                Grid.SetColumn(but, 2);
                InstallationGrid.Children.Add(but);

                but = new Button();
                but.Content = "Delete";
                but.Tag = i;
                but.Click += btnDeleteInstallation_Click;
                Grid.SetRow(but, i);
                Grid.SetColumn(but, 3);
                InstallationGrid.Children.Add(but);

                but = new Button();
                but.Content = "Rand";
                but.Tag = i;
                but.Click += btnRandInstallation_Click;
                Grid.SetRow(but, i);
                Grid.SetColumn(but, 4);
                InstallationGrid.Children.Add(but);
            }
        }

        private void btnAddInstallation_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(((Button)sender).Tag.ToString() ?? "");
            MessageBox.Show("Add " + RuleBook.InstallationParams[val].Type);
        }
        private void btnDeleteInstallation_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(((Button)sender).Tag.ToString() ?? "");
            MessageBox.Show("Delete " + RuleBook.InstallationParams[val].Type);
        }
        private void btnRandInstallation_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(((Button)sender).Tag.ToString() ?? "");
            MessageBox.Show("Rand " + RuleBook.InstallationParams[val].Type);
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
