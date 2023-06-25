
using System.Windows;
using GurpsSpace.PlanetCreation;
using GurpsSpace.SpeciesCreation;
using GurpsSpace.ViewModels;

namespace GurpsSpace
{
    public partial class MainWindow : Window
    {
        ViewModelSetting vmSetting;
        Setting s;

        public MainWindow()
        {
            InitializeComponent();
            s = new();
            s.AddSpecies(new Species(s, "Humans", "Generic humans."));
            s.AddSpecies(new Species(s, "Eldar", "Noble and arrogant."));
            s.AddSpecies(new Species(s, "Orks", "Savage and warlike."));
            vmSetting = new ViewModelSetting(s);
            cmbPlanets.ItemsSource = vmSetting.PlanetList.Items;
            cmbSpecies.ItemsSource = vmSetting.SpeciesList.Items;
            this.DataContext = vmSetting;
        }


        private void btnCreatePlanet(object sender, RoutedEventArgs e)
        {
            PlanetCreator creator = new(s);
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

            PlanetCreator creator = new(p);
            if(creator.ShowDialog()==true)
            {
                selected.Planet = p;
            }
        }
        private void btnDeletePlanet(object sender, RoutedEventArgs e)
        {
            ViewModelPlanet selected = (ViewModelPlanet)cmbPlanets.SelectedItem;
            vmSetting.Remove(selected.Planet);
        }

        private void btnCreateSpecies(object sender, RoutedEventArgs e)
        {
            SpeciesCreator creator = new(s);
            if (creator.ShowDialog()==true)
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

            SpeciesCreator creator = new(s);
            if (creator.ShowDialog() == true)
            {
                selected.Species = s;
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
            if (testDiag.ShowDialog()==true)
            {
                MessageBox.Show(testDiag.Answer);
            }
        }

    }
}
