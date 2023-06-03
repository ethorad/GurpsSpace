using GurpsSpace.PlanetCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GurpsSpace
{
    /// <summary>
    /// Interaction logic for SelectInstallation.xaml
    /// </summary>
    public partial class SelectInstallation : Window
    {
        //private InstallationParameters instParams;
        //public InstallationParameters InstParams { get { return vmParameters.InstallationParameters; } }
        private int selected;
        public int Selected { get { return selected; } }
        public int PopulationRating { get { return vmParameters.PopulationRating; } }
        private List<RadioButton> radioButtons;

        public ViewModelInstallationParameters vmParameters;


        public SelectInstallation(InstallationParameters instParams)
        {
            vmParameters = new ViewModelInstallationParameters(instParams);
            radioButtons = new List<RadioButton>();

            InitializeComponent();

            lblInstallTypeQuestion.Content = "Select whether there is an installation of type " + vmParameters.Type.ToLower() + ", and any subtype options.";

            RadioButton rb;

            rb = new RadioButton();
            rb.Content = "None";
            rb.Tag = 0;
            radioButtons.Add(rb);
            typeChoice.Children.Add(rb);

            List<string> Names = instParams.Names;

            for (int i=0;i<Names.Count;i++)
            {
                rb = new RadioButton();
                rb.Content = Names[i];
                rb.Tag = i + 1;
                radioButtons.Add(rb);
                typeChoice.Children.Add(rb);

            }

            this.DataContext = vmParameters;
            vmParameters.PopulationRating = vmParameters.MinPR;

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            for (int i=0;i<radioButtons.Count;i++)
            {
                if (radioButtons[i].IsChecked == true)
                    selected = i;
            }
            this.DialogResult = true;
        }

    }
}
