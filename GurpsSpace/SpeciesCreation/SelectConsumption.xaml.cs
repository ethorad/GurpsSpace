using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SelectConsumption.xaml
    /// </summary>
    public partial class SelectConsumption : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int consumption;
        public int Consumption 
        { 
            get { return consumption; }
            set 
            { 
                consumption = value;
                if (consumption != 0)
                    doesNotEatOrDrink = false;
            }
        }
        public int IncreasedConsumption
        {
            get { return (Consumption > 0) ? Consumption : 0; }
            set { Consumption = value; MemberUpdated(); }
        }
        public int ReducedConsumption
        {
            get { return (Consumption < 0) ? -Consumption : 0; }
            set { Consumption = -value; MemberUpdated(); }
        }

        private bool doesNotEatOrDrink;
        public bool DoesNotEatOrDrink
        {
            get { return doesNotEatOrDrink; }
            set 
            { 
                doesNotEatOrDrink = value;
                if (doesNotEatOrDrink == true)
                    Consumption = 0;
                MemberUpdated(); 
            }
        }

        public SelectConsumption(Species s)
        {
            consumption = s.Consumption ?? 0;
            doesNotEatOrDrink = s.DoesNotEatOrDrink ?? false;
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        protected void MemberUpdated(string property = "")
        {
            // by default set the property to String.Empty so that all properties count as updated
            if (property == "")
                property = String.Empty;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
