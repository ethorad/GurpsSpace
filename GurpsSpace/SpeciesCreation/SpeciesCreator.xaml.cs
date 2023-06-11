using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GurpsSpace.SpeciesCreation
{
    /// <summary>
    /// Interaction logic for SpeciesCreator.xaml
    /// </summary>
    public partial class SpeciesCreator : Window
    {
        public ViewModelSpecies vmSpecies;
        public Species Species;

        private ISpeciesCreator randomiser;
        private ISpeciesCreator userInput;

        public SpeciesCreator(Setting s)
        {
            Species = new Species(s);
            vmSpecies = new ViewModelSpecies(Species);

            randomiser = new RandomSpecies();
            userInput = new UserSpecies();

            InitializeComponent();
            this.DataContext = vmSpecies;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            SetParameter(val, userInput);
        }
        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            string val = ((Button)sender).Tag.ToString() ?? "";
            SetParameter(val, randomiser);
        }
        private void SetParameter(string param, ISpeciesCreator sc)
        {
            switch (param)
            {
                case "Name":
                    string? name = sc.GetName(vmSpecies.Species);
                    if (name != null)
                        vmSpecies.Name = name;
                    break;
                case "Diet":
                    eSpeciesDiet? diet = sc.GetDiet(vmSpecies.Species);
                    if (diet != null)
                        vmSpecies.Diet = diet;
                    break;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Testing");
        }
    }
}
