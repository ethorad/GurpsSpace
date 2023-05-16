using System.Windows;
using System.Windows.Controls;

namespace GurpsSpace.PlanetCreation
{
    /// <summary>
    /// Interaction logic for PlanetTypeSelection.xaml
    /// </summary>
    public partial class PlanetTypeSelection : Window
    {
        private eSize size; public eSize Size { get { return size; } }
        private eSubtype subtype; public eSubtype Subtype { get { return subtype; } }

        public PlanetTypeSelection()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string? val = ((Button)sender).Tag.ToString();
            if (val is null)
                return;
            string[] selected = val.Split('|');
            size = selected[0].ToEnum<eSize>();
            subtype = selected[1].ToEnum<eSubtype>();

            this.DialogResult = true;
        }

    }
}
