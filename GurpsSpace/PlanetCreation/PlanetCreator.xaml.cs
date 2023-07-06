
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
        public ViewModelPlanetFactory vmPlanetFactory;
        private PlanetFactory planetFactory;
        public Planet Planet { get { return planetFactory.Planet; } }

// disabling warnings on non-nullable fields not being set up in the constructor
// This is because they are given values in the SetUp() call in the constructor but the compiler doesn't notice

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PlanetCreator(Setting setting)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            planetFactory = new(setting, new RandomPlanet(), new UserPlanet());
            SetUp();
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PlanetCreator(Planet planet)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            planetFactory = new(planet, new RandomPlanet(), new UserPlanet());
            SetUp();
        }
        private void SetUp()
        {
            vmPlanetFactory = new ViewModelPlanetFactory(planetFactory);

            InitializeComponent();
            SetUpInstallationGrid();

            this.DataContext = vmPlanetFactory;
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

            vmPlanetFactory.SelectInstallation(instType);

        }
        private void btnRandInstallation_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(((Button)sender).Tag.ToString() ?? "");
            string instType = RuleBook.InstallationParams[val].Type;

            vmPlanetFactory.RandomInstallation(instType);
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            vmPlanetFactory.RandomParameter(val);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            vmPlanetFactory.SelectParameter(val);
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnViewInstallation_Click(object sender, RoutedEventArgs e)
        {
            InstallationsList instWindow = new InstallationsList(vmPlanetFactory.InstallationsList);
            instWindow.ShowDialog();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(vmPlanetFactory.InstallationsList.Count.ToString());
        }
    }
}
