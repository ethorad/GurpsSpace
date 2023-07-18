
using System;
using System.Windows;
using GurpsSpace.PlanetCreation;
using GurpsSpace.SpeciesCreation;
using GurpsSpace.ViewModels;

namespace GurpsSpace
{
    public partial class MainWindow : Window
    {
        ViewModelSetting vmSetting;
        Setting setting;

        public MainWindow()
        {
            InitializeComponent();
            setting = new();
            vmSetting = new ViewModelSetting(setting);

            Species sp;

            sp = new Species(setting, "Humans", "Generic humans.");
            vmSetting.Add(sp);

            sp = new Species(setting, "Eldar", "Noble and arrogant.");
            sp.AddTrait(eTrait.IncreasedConsumption, 1);
            vmSetting.Add(sp);

            sp = new Species(setting, "Orks", "Savage and warlike.");
            sp.AddTrait(eTrait.ReducedConsumption, 1);
            vmSetting.Add(sp);

            sp = new Species(setting, "Spirits", "Ethereal ghosts.");
            sp.AddTrait(eTrait.DoesntEatOrDrink);
            vmSetting.Add(sp);

            cmbPlanets.ItemsSource = vmSetting.PlanetList.Items;
            cmbSpecies.ItemsSource = vmSetting.SpeciesList.Items;
            cmbSpecies.SelectedIndex = 0;
            this.DataContext = vmSetting;
        }
        private void btnExitApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCreatePlanet(object sender, RoutedEventArgs e)
        {
            PlanetCreator creator = new(setting);
            if (creator.ShowDialog()==true)
            {
                vmSetting.Add(creator.Planet);
                cmbPlanets.SelectedIndex = cmbPlanets.Items.Count - 1;
            }
        }
        private void btnRandomPlanet(object sender, RoutedEventArgs e)
        {
            PlanetCreator creator = new PlanetCreator(setting, true);
            if (creator.ShowDialog()==true)
            {
                vmSetting.Add(creator.Planet);
                cmbPlanets.SelectedIndex = cmbPlanets.Items.Count - 1;
            }
        }
        private void btnEditPlanet(object sender, RoutedEventArgs e)
        {
            // take a copy of the selected planet, and only replace it if we clicked OK
            // as otherwise changes on the creator screen immediately flow back to the item
            ViewModelPlanet selected = (ViewModelPlanet)cmbPlanets.SelectedItem;
            Planet p = new Planet(selected.Planet);
            int index = cmbPlanets.SelectedIndex;

            PlanetCreator creator = new(p);
            if (creator.ShowDialog() == true)
            {
                vmSetting.Replace(p, index);
                cmbPlanets.SelectedIndex = index;
            }
        }
        private void btnDeletePlanet(object sender, RoutedEventArgs e)
        {
            ViewModelPlanet selected = (ViewModelPlanet)cmbPlanets.SelectedItem;
            vmSetting.Remove(selected.Planet);
        }

        private void btnCreateSpecies(object sender, RoutedEventArgs e)
        {
            SpeciesCreator creator = new(setting);
            if (creator.ShowDialog()==true)
            {
                vmSetting.Add(creator.Species);
                cmbSpecies.SelectedIndex = cmbSpecies.Items.Count - 1;
            }
        }
        private void btnRandomSpecies(object sender, RoutedEventArgs e)
        {
            SpeciesCreator creator = new SpeciesCreator(setting, true);
            if (creator.ShowDialog() == true)
            {
                vmSetting.Add(creator.Species);
                cmbSpecies.SelectedIndex = cmbSpecies.Items.Count - 1;
            }
        }
        private void btnEditSpecies(object sender, RoutedEventArgs e)
        {
            // take a copy of the selected species, and only replace it if we clicked OK
            // as otherwise changes on the creator screen immediately flow back to the item
            ViewModelSpecies selected = (ViewModelSpecies)cmbSpecies.SelectedItem;
            Species s = new Species(selected.Species);
            int index = cmbSpecies.SelectedIndex;

            SpeciesCreator creator = new(s);
            if (creator.ShowDialog() == true)
            {
                vmSetting.Replace(s, index);
                cmbSpecies.SelectedIndex = index;
            }
        }
        private void btnDeleteSpecies(object sender, RoutedEventArgs e)
        {
            ViewModelSpecies selected = (ViewModelSpecies)cmbSpecies.SelectedItem;
            vmSetting.Remove(selected.Species);
        }


        private void btnTestClick(object sender, RoutedEventArgs e)
        {
            string question = "Some really long text which will wrap over the line.  This is so that we can test that the " +
                "wrapping functionality in the string input dialog box is working correctly.  I should perhaps have copied " +
                "the lorem ipsum text rather than writing my own, but there we go.  This is probably long enough ...";
            InputString testDiag = new InputString(question);
            if (testDiag.ShowDialog() == true)
            {
                MessageBox.Show(testDiag.Answer);
            }
        }
    }
}
