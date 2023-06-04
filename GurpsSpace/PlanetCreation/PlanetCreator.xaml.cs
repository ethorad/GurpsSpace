﻿
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
            List<Installation> lst = userInput.GetInstallation(vmPlanet.Planet, instType);
            vmPlanet.ClearInstallations(instType);
            vmPlanet.AddInstallations(lst);
        }
        private void btnRandInstallation_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(((Button)sender).Tag.ToString() ?? "");
            string instType = RuleBook.InstallationParams[val].Type;
            List<Installation> lst = randomiser.GetInstallation(vmPlanet.Planet, instType);
            vmPlanet.ClearInstallations(instType);
            vmPlanet.AddInstallations(lst);
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
                    int spacepostClass = pc.GetSpaceportClass(vmPlanet.Planet);
                    vmPlanet.SpaceportClass = spacepostClass;
                    break;
                case "Installations":
                    List<Installation> newInst = pc.GetInstallations(vmPlanet.Planet);
                    vmPlanet.InstallationsList = new ViewModelInstallationList(newInst);
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
