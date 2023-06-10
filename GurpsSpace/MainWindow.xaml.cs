
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
            this.DataContext = vmSetting;
        }


        private void btnCreatePlanet(object sender, RoutedEventArgs e)
        {
            PlanetCreator creator = new(s);
            if (creator.ShowDialog()==true)
            {
                vmSetting.Add(creator.Planet);
            }
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
