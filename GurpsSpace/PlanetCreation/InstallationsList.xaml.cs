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

namespace GurpsSpace.PlanetCreation
{
    /// <summary>
    /// Interaction logic for InstallationsList.xaml
    /// </summary>
    public partial class InstallationsList : Window
    {
        public InstallationsList(ViewModelInstallationList lst)
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
