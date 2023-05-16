
using System.Windows;
using GurpsSpace.PlanetCreation;

namespace GurpsSpace
{
    public partial class MainWindow : Window
    {
        Setting s;

        public MainWindow()
        {
            InitializeComponent();
            s = new();
            s.AddSpecies(new Species(s, "Humans", "Generic humans."));
            s.AddSpecies(new Species(s, "Eldar", "Noble and arrogant."));
            s.AddSpecies(new Species(s, "Orks", "Savage and warlike."));
            this.DataContext = s;
        }


        private void btnCreatePlanet(object sender, RoutedEventArgs e)
        {
            PlanetCreator creator = new(s);
            if (creator.ShowDialog()==true)
            {
                s.AddPlanet(creator.Planet);
            }
        }

        private void btnNameClick(object sender, RoutedEventArgs e)
        {
            InputString inDialog = new("Enter the planet name", s.Planets[0].Name);
            if (inDialog.ShowDialog() == true)
                s.Planets[0].Name = inDialog.Answer;
        }

        private void btnTestClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(s.MainSpecies.Name);
        }

    }
}
