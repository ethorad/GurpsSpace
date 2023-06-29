
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;

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

// disabling warnings on non-nullable fields not being set up in the constructor
// This is because they are given values in the SetUp() call in the constructor but the compiler doesn't notice

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PlanetCreator(Setting setting)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Planet = new(setting);
            SetUp();
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PlanetCreator(Planet p)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Planet = p;
            SetUp();
        }
        private void SetUp()
        {
            vmPlanet = new ViewModelPlanet(Planet);
            randomiser = new RandomPlanet();
            userInput = new UserPlanet();

            InitializeComponent();
            SetUpInstallationGrid();

            this.DataContext = vmPlanet;
        }
        private void SetUpInstallationGrid()
        {
            // Type
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            // Count
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Auto);
            // button view / select
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Auto);
            // button rand
            InstallationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            InstallationGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Auto);

            Label lbl;
            Button but;
            Binding bind;

            // header row
            InstallationGrid.RowDefinitions.Add(new RowDefinition());

            lbl = new Label();
            lbl.Content = "Type";
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 0);
            InstallationGrid.Children.Add(lbl);

            lbl = new Label();
            lbl.Content = "Count";
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 1);
            InstallationGrid.Children.Add(lbl);

            // totals row
            InstallationGrid.RowDefinitions.Add(new RowDefinition());

            lbl = new Label();
            lbl.Content = "All installations";
            Grid.SetRow(lbl, 1);
            Grid.SetColumn(lbl, 0);
            InstallationGrid.Children.Add(lbl);

            lbl = new Label();
            bind = new Binding("InstallationsList[all]");
            bind.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(lbl, Label.ContentProperty, bind);
            Grid.SetRow(lbl, 1);
            Grid.SetColumn(lbl, 1);
            InstallationGrid.Children.Add(lbl);

            but = new Button();
            but.Content = "View";
            but.Click += btnViewInstallation_Click;
            Grid.SetRow(but, 1);
            Grid.SetColumn(but, 2);
            InstallationGrid.Children.Add(but);

            but = new Button();
            but.Content = "Rand";
            but.Tag = "Installations";
            but.Click += btnRandom_Click;
            Grid.SetRow(but, 1);
            Grid.SetColumn(but, 3);
            InstallationGrid.Children.Add(but);

            for (int i=0;i<RuleBook.InstallationParams.Count;i++)
            {
                InstallationGrid.RowDefinitions.Add(new RowDefinition());

                lbl = new Label();
                lbl.Content = RuleBook.InstallationParams[i].Type;
                Grid.SetRow(lbl, i + 2);
                Grid.SetColumn(lbl, 0);
                InstallationGrid.Children.Add(lbl);

                lbl = new Label();
                bind = new Binding("InstallationsList[" + RuleBook.InstallationParams[i].Type+"]");
                bind.Mode = BindingMode.OneWay;
                BindingOperations.SetBinding(lbl, Label.ContentProperty, bind);
                Grid.SetRow(lbl, i + 2);
                Grid.SetColumn(lbl, 1);
                InstallationGrid.Children.Add(lbl);

                but = new Button();
                but.Content = "Select";
                but.Tag = i;
                but.Click += btnSelectInstallation_Click;
                Grid.SetRow(but, i + 2);
                Grid.SetColumn(but, 2);
                InstallationGrid.Children.Add(but);

                but = new Button();
                but.Content = "Rand";
                but.Tag = i;
                but.Click += btnRandInstallation_Click;
                Grid.SetRow(but, i + 2);
                Grid.SetColumn(but, 3);
                InstallationGrid.Children.Add(but);
            }
        }

        private void btnSelectInstallation_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(((Button)sender).Tag.ToString() ?? "");
            string instType = RuleBook.InstallationParams[val].Type;
            List<Installation>? lst = userInput.GetInstallation(vmPlanet.Planet, instType);
            if (lst != null)
            {
                vmPlanet.ClearInstallations(instType);
                vmPlanet.AddInstallations(lst);
            }
        }
        private void btnRandInstallation_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(((Button)sender).Tag.ToString() ?? "");
            string instType = RuleBook.InstallationParams[val].Type;
            List<Installation>? lst = randomiser.GetInstallation(vmPlanet.Planet, instType);
            if (lst != null)
            {
                vmPlanet.ClearInstallations(instType);
                vmPlanet.AddInstallations(lst);
            }
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
                    string? name = pc.GetName(vmPlanet.Planet);
                    if (name != null)
                        vmPlanet.Name = name;
                    break;
                case "Type":
                    eSize? size;
                    eSubtype? subtype;
                    (size, subtype) = pc.GetSizeAndSubtype(vmPlanet.Planet);
                    if (size != null)
                        vmPlanet.Size = size ?? eSize.None;
                    if (subtype != null)
                        vmPlanet.Subtype = subtype ?? eSubtype.None;
                    break;
                case "ResourceValueCategory":
                    eResourceValueCategory? res = pc.GetResourceValueCategory(vmPlanet.Planet);
                    if (res != null)
                        vmPlanet.ResourceValueCategory = res ?? eResourceValueCategory.Average;
                    break;
                case "AtmosphericMass":
                    double? atmMass = pc.GetAtmosphericMass(vmPlanet.Planet);
                    if (atmMass != null)
                        vmPlanet.AtmosphericMass = atmMass ?? 0;
                    break;
                case "AtmosphericConditions":
                    fAtmosphericConditions? cond;
                    string? condDesc;
                    (cond, condDesc) = pc.GetAtmosphericConditions(vmPlanet.Planet);
                    if (cond != null)
                        vmPlanet.AtmosphericConditions = cond ?? fAtmosphericConditions.None;
                    if (condDesc != null)
                        vmPlanet.AtmosphericDescription = condDesc ?? "tbc";
                    break;
                case "HydrographicCoverage":
                    double? hydro = pc.GetHydrographicCoverage(vmPlanet.Planet);
                    if (hydro != null)
                        vmPlanet.HydrographicCoverage = hydro ?? 0;
                    break;
                case "AverageSurfaceTempK":
                    int? tempK = pc.GetAverageSurfaceTempK(vmPlanet.Planet);
                    if (tempK != null)
                        vmPlanet.AverageSurfaceTempK = tempK ?? 0;
                    break;
                case "Density":
                    double? density = pc.GetDensity(vmPlanet.Planet);
                    if (density != null)
                        vmPlanet.Density = density ?? 0;
                    break;
                case "Gravity":
                    double? grav = pc.GetGravity(vmPlanet.Planet);
                    if (grav != null)
                        vmPlanet.Gravity = grav ?? 0;
                    break;
                case "SettlementType":
                    eSettlementType? settType;
                    int? colonyAge;
                    bool? interstellar;
                    (settType, colonyAge, interstellar) = pc.GetSettlementType(vmPlanet.Planet);
                    if (settType != null)
                    {
                        vmPlanet.SettlementType = settType ?? eSettlementType.None;
                        vmPlanet.ColonyAge = colonyAge ?? 0;
                        vmPlanet.Interstellar = interstellar ?? true;
                    }
                    break;
                case "Species":
                    Species? s = pc.GetLocalSpecies(vmPlanet.Planet);
                    if (s != null)
                        vmPlanet.LocalSpecies = s;
                    break;
                case "TechLevel":
                    int? tl;
                    eTechLevelRelativity? adj;
                    (tl, adj) = pc.GetLocalTechLevel(vmPlanet.Planet);
                    if (tl != null)
                        vmPlanet.LocalTechLevel = tl ?? 0;
                    if (adj != null)
                        vmPlanet.LocalTechLevelRelativity = adj ?? eTechLevelRelativity.Normal;
                    break;
                case "Population":
                    double? pop = pc.GetPopulation(vmPlanet.Planet);
                    if (pop != null)
                        vmPlanet.Population = pop ?? 0;
                    break;
                case "WorldGovernance":
                    eWorldUnityLevel? unity;
                    fGovernmentSpecialConditions? specCond;
                    (unity,specCond) = pc.GetWorldGovernance(vmPlanet.Planet);
                    if (unity != null)
                        vmPlanet.WorldUnityLevel = unity ?? eWorldUnityLevel.Diffuse;
                    if (specCond != null)
                        vmPlanet.GovernmentSpecialConditions = specCond ?? fGovernmentSpecialConditions.None;
                    break;
                case "SocietyType":
                    eSocietyType? soc = pc.GetSocietyType(vmPlanet.Planet);
                    if (soc != null)
                        vmPlanet.SocietyType = soc ?? eSocietyType.Anarchy;
                    break;
                case "ControlRating":
                    int? cr = pc.GetControlRating(vmPlanet.Planet);
                    if (cr != null)
                        vmPlanet.ControlRating = cr ?? 0;
                    break;
                case "TradeVolume":
                    double? trade = pc.GetTradeVolume(vmPlanet.Planet);
                    if (trade != null)
                        vmPlanet.TradeVolume = trade ?? 0;
                    break;
                case "SpaceportClass":
                    int? spacepostClass = pc.GetSpaceportClass(vmPlanet.Planet);
                    if (spacepostClass != null)
                        vmPlanet.SpaceportClass = spacepostClass ?? 0;
                    break;
                case "Installations":
                    List<Installation>? newInst = pc.GetInstallations(vmPlanet.Planet);
                    if (newInst != null)
                        vmPlanet.InstallationsList = new ViewModelInstallationList(newInst);
                    break;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnViewInstallation_Click(object sender, RoutedEventArgs e)
        {
            InstallationsList instWindow = new InstallationsList(new ViewModelInstallationList(Planet.Installations));
            instWindow.ShowDialog();
        }
    }
}
