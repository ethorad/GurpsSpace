
using System.Windows;
using GurpsSpace.ViewModels;

namespace GurpsSpace.PlanetCreation
{
    /// <summary>
    /// Interaction logic for InstallationsList.xaml
    /// </summary>
    public partial class InstallationsList : Window
    {
        public InstallationsList(ViewModelList<ViewModelInstallation> lst)
        {
            this.DataContext = lst;
            InitializeComponent();

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
