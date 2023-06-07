using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace GurpsSpace
{

    public partial class InputString : Window
    {
        bool onlyNumbers;

        public InputString(string question, string defaultAnswer="", bool onlyNumbers=false)
        {
            InitializeComponent();
            lblQuestion.Text = question;
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
            Regex reg = new Regex("[^0-9,]");
            if (onlyNumbers)
                e.Handled = reg.IsMatch(e.Text);
            else
                e.Handled = false;
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }

    }
}
