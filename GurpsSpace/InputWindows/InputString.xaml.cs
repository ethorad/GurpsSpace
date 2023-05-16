using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GurpsSpace
{

    public partial class InputString : Window
    {
        bool onlyNumbers;

        public InputString(string question, string defaultAnswer="", bool onlyNumbers=false)
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
            this.onlyNumbers = onlyNumbers;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        private void textboxNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]");
            if (onlyNumbers)
                e.Handled = reg.IsMatch(e.Text);
            else
                e.Handled = true;
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }

    }
}
