using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GurpsSpace
{

    public partial class InputRadio : Window
    {
        private int selected; public int Selected { get { return selected; } }
        List<(string, string)> answerList;
        public (string, string) Answer { get { return answerList[selected]; } }
        private List<RadioButton> radioButtons;

        public InputRadio(string question, List<(string, string)> answers)
        {
            InitializeComponent();
            lblQuestion.Text = question;
            selected = 0;
            answerList = answers;
            radioButtons = new List<RadioButton>();

            Grid g = new();
            for (int i = 0; i < answerList.Count; i++)
            {
                g.RowDefinitions.Add(new RowDefinition()); // one row for each option
                g.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Auto);
            }
            g.ColumnDefinitions.Add(new ColumnDefinition()); // radio buttons
            g.ColumnDefinitions.Add(new ColumnDefinition()); // descriptions

            g.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
            g.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

            for (int i=0;i<answers.Count; i++)
            {
                if (i % 2 == 0)
                {
                    Border bd = new Border();
                    bd.Background = Brushes.LightGray;
                    Grid.SetRow(bd, i);
                    Grid.SetColumn(bd, 0);
                    Grid.SetColumnSpan(bd, 2);
                    g.Children.Add(bd);
                }


                RadioButton rb = new();
                rb.Content = answerList[i].Item1;
                Grid.SetRow(rb, i);
                Grid.SetColumn(rb, 0);
                g.Children.Add(rb);
                radioButtons.Add(rb);

                TextBlock tb = new();
                tb.Text = answerList[i].Item2;
                tb.TextWrapping = TextWrapping.WrapWithOverflow;
                tb.Margin = new Thickness(5, 5, 5, 5); // to match the margin on the radiobuttons
                Grid.SetColumn(tb, 1);
                Grid.SetRow(tb, i);
                g.Children.Add(tb);
            }

            Grid.SetColumn(g, 1);
            Grid.SetRow(g, 1);
            radioButtonPanel.Children.Add(g);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            for (int i=0; i<radioButtons.Count; i++)
            {
                if (radioButtons[i].IsChecked==true)
                {
                    selected = i;
                }
            }
            this.DialogResult = true;
        }
    }
}
