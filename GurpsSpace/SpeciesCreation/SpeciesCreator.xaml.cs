using System.Windows;
using System.Windows.Controls;

namespace GurpsSpace.SpeciesCreation
{
    /// <summary>
    /// Interaction logic for SpeciesCreator.xaml
    /// </summary>
    public partial class SpeciesCreator : Window
    {
        public ViewModelSpeciesFactory vmSpeciesFactory;
        private SpeciesFactory speciesFactory;
        public Species Species { get { return speciesFactory.Species; } }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SpeciesCreator(Setting s, bool fullRandom = false)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            speciesFactory = new SpeciesFactory(s, new RandomSpecies(), new UserSpecies());
            vmSpeciesFactory = new ViewModelSpeciesFactory(speciesFactory);
            if (fullRandom)
                vmSpeciesFactory.FullRandom();
            this.DataContext = vmSpeciesFactory;
            InitializeComponent();
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SpeciesCreator(Species s, bool fullRandom = false)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            speciesFactory = new SpeciesFactory(s, new RandomSpecies(), new UserSpecies());
            vmSpeciesFactory = new ViewModelSpeciesFactory(speciesFactory);
            if (fullRandom)
                vmSpeciesFactory.FullRandom();
            this.DataContext = vmSpeciesFactory;
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            vmSpeciesFactory.SelectParameter(val);
        }
        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            vmSpeciesFactory.RandomParameter(val);
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Number of traits = " + vmSpeciesFactory.TraitList.Count);
        }
    }
}
