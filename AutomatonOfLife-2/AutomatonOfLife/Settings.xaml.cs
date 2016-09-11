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
using Rulesclass;
using System.IO;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;

namespace AutomatonOfLife
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public static List<Rule> rules = new List<Rule>();
        int buttons = 0;
        public static int rulescount = 0;
        public Settings()
        {
            rules.Clear();
            InitializeComponent();
            for(int i=0;i<5;i++)
            {
                for(int j=0;j<5;j++)
                {
                    PutCell(i, j, 0);
                }
            }
            PutRule1Cells();
            PutRule2Cells();
        }

        private void PutRule1Cells()
        {
            Button but1 = new Button();
            but1.Background = new SolidColorBrush(Colors.White);
            but1.Height = 50;
            but1.Width = 50;
            but1.Click += CellClicked;
            Canvas.SetLeft(but1, 50);
            Canvas.SetTop(but1, 0);
            Button but2 = new Button();
            but2.Background = new SolidColorBrush(Colors.White);
            but2.Height = 50;
            but2.Width = 50;
            but2.Click += CellClicked;
            Canvas.SetLeft(but2, 100);
            Canvas.SetTop(but2, 50);
            Button but3 = new Button();
            but3.Background = new SolidColorBrush(Colors.White);
            but3.Height = 50;
            but3.Width = 50;
            but3.Click += CellClicked;
            Canvas.SetLeft(but3, 0);
            Canvas.SetTop(but3, 50);
            Button but4 = new Button();
            but4.Background = new SolidColorBrush(Colors.White);
            but4.Height = 50;
            but4.Width = 50;
            but4.Click += CellClicked;
            Canvas.SetLeft(but4, 50);
            Canvas.SetTop(but4, 100);
            Button but5 = new Button();
            but5.Background = new SolidColorBrush(Colors.Turquoise);
            but5.Height = 50;
            //     but5.IsEnabled = false;
            but5.Width = 50;
            // but5.Click += CellClicked;
            Canvas.SetLeft(but5, 50);
            Canvas.SetTop(but5, 50);
            buttongrid1.Children.Add(but1);
            buttongrid1.Children.Add(but2);
            buttongrid1.Children.Add(but3);
            buttongrid1.Children.Add(but4);

            buttongrid1.Children.Add(but5);
        }

        private void PutRule2Cells()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button but1 = new Button();

                    if (i == 1 && j == 1)
                    {
                        but1.Background = new SolidColorBrush(Colors.Turquoise);
                    }
                    else
                    {
                        but1.Background = new SolidColorBrush(Colors.White);
                    }
                    but1.Height = 50;
                    but1.Width = 50;
                    but1.Click += CellClicked;
                    Canvas.SetLeft(but1, i * 50);
                    Canvas.SetTop(but1, j * 50);
                    buttongrid2.Children.Add(but1);
                }

            }
        }

        private void PutCell(int x, int y, int state)
        {
            Button but = new Button();

            if (state == 0)
            {
                but.Background = new SolidColorBrush(Colors.White);
            }
            else if (state == 1)
            {
                but.Background = new SolidColorBrush(Colors.Green);
            }
            else if (state == 2)
            {
                but.Background = new SolidColorBrush(Colors.Red);
            }
            if(buttons==12)
            {
                but.Background = new SolidColorBrush(Colors.Turquoise);
            }
            but.Width = 50;
            but.Height = 50;
            but.Click += CellClicked;
            Canvas.SetLeft(but, x * 50);
            Canvas.SetTop(but, y * 50);
            but.Name = "but" + x.ToString() + "" + y.ToString();
           // buttonlist.Add(but);
            buttongrid.Children.Add(but);
            but.Name = "but"+buttons.ToString();
            buttons++;
            // FF008000
        }

        private void CellClicked(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
       //     MessageBox.Show(clickedButton.Name,"", MessageBoxButton.OK);
            //     MessageBox.Show(clickedButton.Background.ToString(), "", MessageBoxButton.OK);
            if (clickedButton.Background.ToString() == "#FFFFFFFF")
            {
                clickedButton.Background = new SolidColorBrush(Colors.Green);
            }
            else if (clickedButton.Background.ToString() == "#FF008000")
            {
                clickedButton.Background = new SolidColorBrush(Colors.Red);
            }
            else if (clickedButton.Background.ToString() == "#FFFF0000")
            {
                clickedButton.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string moreless="";
            if(morelesscombo.SelectedIndex==0)
            {
                moreless = "more";
            }
            else if(morelesscombo.SelectedIndex==1)
            {
                moreless = "less";
            }
            else
            {
                moreless = "exactly";
            }
            int type = 10;
            if(typeecombo.SelectedIndex==0)
            {
                type = 1;
            }
            else if(typeecombo.SelectedIndex==1)
            {
                type = 2;
            }
            else if(typeecombo.SelectedIndex==2)
            {
                type = 3;
            }
            if(typeecombo.SelectedIndex==1&&qtycombo.SelectedIndex>4)
            {
                MessageBox.Show("Wrong input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (typeecombo.SelectedIndex == 2 && qtycombo.SelectedIndex > 9)
            {
                MessageBox.Show("Wrong input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                rules.Add(new Rule(qtycombo.SelectedIndex + 1, statecombo.SelectedIndex, choicecombo.SelectedIndex, moreless.ToString(), type));
                rulesgrid.ItemsSource = null;

                rulesgrid.ItemsSource = rules;
                rulescount++;
                rulesgrid.ScrollIntoView(rules[rules.Count - 1]);
            }
        }

        private void visualrulebut_Click(object sender, RoutedEventArgs e)
        {
            int state0count=0;
            int state1count=0;
            int state2count=0;
            for(int i=0;i<=11;i++)
            {
                Button but = new Button();
                but = (Button)buttongrid.Children[i];
                if (but.Background.ToString() == "#FFFFFFFF")
                {
                    state0count++;
                }
                else if (but.Background.ToString() == "#FF008000")
                {
                    state1count++;
                }
                else if (but.Background.ToString() == "#FFFF0000")
                {
                    state2count++;
                }
            }
           
            for (int i = 13; i <25 ; i++)
            {
                Button but = new Button();
                but = (Button)buttongrid.Children[i];
                if (but.Background.ToString() == "#FFFFFFFF")
                {
                    state0count++;
                }
                else if (but.Background.ToString() == "#FF008000")
                {
                    state1count++;
                }
                else if (but.Background.ToString() == "#FFFF0000")
                {
                    state2count++;
                }
                

            }
            if (state0count > 0)
            {
                rules.Add(new Rule(state0count, 0, choicecombo1.SelectedIndex, "exactly", 1));
            }
            if (state1count > 0)
            {
                rules.Add(new Rule(state1count, 1, choicecombo1.SelectedIndex, "exactly", 1));
            }
            if (state2count > 0)
            {
                rules.Add(new Rule(state2count, 2, choicecombo1.SelectedIndex, "exactly", 1));
            }
            rulesgrid.ItemsSource = null;

            rulesgrid.ItemsSource = rules;
            rulesgrid.ScrollIntoView(rules[rules.Count - 1]);
        }

        private void loadrulesbut_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            Nullable<bool> result = dlg.ShowDialog();
            string content = "";
            if (result == true)
            {
                try {
                    content = File.ReadAllText(dlg.FileName);
                    var firstWord = content.Substring(0, content.IndexOf("\n"));
                    int size = Int32.Parse(firstWord.ToString());

                    var splitted = content.Split('\n');
                    rules.Clear();

                    for (int i = 0; i < size; i++)
                    {
                        var splitted1 = splitted[i + 1].Split(' ');
                        int neighborstate = Int32.Parse(splitted1[0]);
                        int neighborcount = Int32.Parse(splitted1[1]);
                        int finalstate = Int32.Parse(splitted1[2]);
                        string moreless = splitted1[3];
                        int type = Int32.Parse(splitted1[4]);

                        Rule rule = new Rule(neighborcount, neighborstate, finalstate, moreless, type);
                        rules.Add(rule);

                    }


                    rulesgrid.ItemsSource = rules;
                    rulesgrid.ScrollIntoView(rules[rules.Count - 1]);
                }
                catch
                {
                    MessageBox.Show("Ops, something went wrong", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                }

            else
            {
                MessageBox.Show("Rules were not loaded correctly!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void applyrulesbut_Click(object sender, RoutedEventArgs e)
        {
            bool checkedrules = Checkrules();
            if (checkedrules)
            {
                MainWindow.changed = true;

                string createText = rules.Count.ToString() + Environment.NewLine;
                for (int i = 0; i < rulesgrid.Items.Count; i++)
                {
                    createText += rules[i].neighborstate.ToString() + " " + rules[i].neighborcount.ToString() + " " + rules[i].finalstate.ToString() + " " + rules[i].moreless + " " + rules[i].type + " " + Environment.NewLine;
                }


                using (StreamWriter writetext = new StreamWriter("appliedrules.txt"))
                {
                    writetext.WriteLine(createText);


                }
                this.Close();

            }
            else { MessageBox.Show("There have been some conflicts in the rules, but do not worry, the program will handle it.", "Problem occured", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
    }

        private bool Checkrules()
        {
            for(int i=0;i<rules.Count();i++)
            {
                for(int j=0;j<rules.Count();j++)
                {
                    if(i!=j)
                    {
                        if(rules[i].neighborcount==rules[j].neighborcount&&rules[i].neighborstate==rules[j].neighborstate)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void saverulesbut_Click(object sender, RoutedEventArgs e)
        {
            string createText = rules.Count.ToString() + Environment.NewLine;
            for (int i = 0; i < rulesgrid.Items.Count; i++)
            {
                createText += rules[i].neighborstate.ToString() + " " + rules[i].neighborcount.ToString() + " " + rules[i].finalstate.ToString() + " " + rules[i].moreless +" "+rules[i].type+" "+ Environment.NewLine;
            }
            SaveFileDialog dialog = new SaveFileDialog();
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string result1 = result.Value.ToString();
            }
            else
            {
                MessageBox.Show("Rules were not saved!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            using (StreamWriter writetext = new StreamWriter(dialog.FileName + ".txt"))
            {
                writetext.WriteLine(createText);

            }
            TextFileToBinary(dialog.FileName);

        }
        private void TextFileToBinary(string TexFilePath)
        {
            string fileContents;
            using (FileStream fileStream = new FileStream(TexFilePath + ".txt", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    fileContents = sr.ReadToEnd();
                }
            }
            using (FileStream fs = new FileStream(TexFilePath + "txt".Replace("txt", "bin"), FileMode.Create))
            {
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, fileContents);
            }
        }

        private void visualrule1but_Click(object sender, RoutedEventArgs e)
        {
            int state0count = 0;
            int state1count = 0;
            int state2count = 0;
            for (int i = 0; i <= 4; i++)
            {
                Button but = new Button();
                but = (Button)buttongrid1.Children[i];
                if (but.Background.ToString() == "#FFFFFFFF")
                {
                    state0count++;
                }
                else if (but.Background.ToString() == "#FF008000")
                {
                    state1count++;
                }
                else if (but.Background.ToString() == "#FFFF0000")
                {
                    state2count++;
                }
            }
            if (state0count > 0)
            {
                rules.Add(new Rule(state0count, 0, choicecombo1rule1.SelectedIndex, "exactly", 2));
            }
            if (state1count > 0)
            {
                rules.Add(new Rule(state1count, 1, choicecombo1rule1.SelectedIndex, "exactly", 2));
            }
            if (state2count > 0)
            {
                rules.Add(new Rule(state2count, 2, choicecombo1rule1.SelectedIndex, "exactly", 2));
            }

            rulesgrid.ItemsSource = null;

            rulesgrid.ItemsSource = rules;
            rulesgrid.ScrollIntoView(rules[rules.Count - 1]);
        }

        private void visualrule2but_Click(object sender, RoutedEventArgs e)
        {
            int state0count = 0;
            int state1count = 0;
            int state2count = 0;
            for (int i = 0; i <= 4; i++)
            {
                Button but = new Button();
                but = (Button)buttongrid2.Children[i];
                if (but.Background.ToString() == "#FFFFFFFF")
                {
                    state0count++;
                }
                else if (but.Background.ToString() == "#FF008000")
                {
                    state1count++;
                }
                else if (but.Background.ToString() == "#FFFF0000")
                {
                    state2count++;
                }
            }

            for (int j = 5; j < 9; j++)
            {
                Button but = new Button();
                but = (Button)buttongrid2.Children[j];
                if (but.Background.ToString() == "#FFFFFFFF")
                {
                    state0count++;
                }
                else if (but.Background.ToString() == "#FF008000")
                {
                    state1count++;
                }
                else if (but.Background.ToString() == "#FFFF0000")
                {
                    state2count++;
                }
            }
            if (state0count > 0)

            {
                rules.Add(new Rule(state0count, 0, choicecombo1rule1.SelectedIndex, "exactly", 3));
            }
            if (state1count > 0)
            {
                rules.Add(new Rule(state1count, 1, choicecombo1rule1.SelectedIndex, "exactly", 3));
            }
            if (state0count > 0)
            {
                rules.Add(new Rule(state2count, 2, choicecombo1rule1.SelectedIndex, "exactly", 3));
            }
            rulesgrid.ItemsSource = null;

            rulesgrid.ItemsSource = rules;
            rulesgrid.ScrollIntoView(rules[rules.Count - 1]);
        }
    }
}
