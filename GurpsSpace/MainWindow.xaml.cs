
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
            cmbPlanets.ItemsSource = vmSetting.PlanetList.Planets;
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
            Planet p = ((ViewModelPlanet)cmbPlanets.SelectedItem).Planet;
            PlanetCreator creator = new(p);
            if(creator.ShowDialog()==true)
            {
                // do something
                // at the moment, the creator window edits the actual planet
                // so even if you cancel the changes are made
                // also the drop down list doesn't update
            }
        }
        private void btnDeletePlanet(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete");
        }

        private void btnCreateSpecies(object sender, RoutedEventArgs e)
        {
            SpeciesCreator creator = new(s);
            if (creator.ShowDialog()==true)
            {
                vmSetting.Add(creator.Species);
            }
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
