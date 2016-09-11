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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rulesclass;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace AutomatonOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Button> buttonlist = new List<Button>();
        List<Cell> cellist = new List<Cell>();
        List<Cell> cellistcpy = new List<Cell>();
        List<Cell> cellistcpy1 = new List<Cell>();
        List<Cell> oscilcheck1 = new List<Cell>();
        List<Cell> oscilcheck2 = new List<Cell>();
        List<Cell> oscilcheck3 = new List<Cell>();
        List<Cell> oscilcheck4 = new List<Cell>();

        public static bool changed = false;
        List<Rule> rules = new List<Rule>();
        int zoom = 0;
        public MainWindow()
        {
            InitializeComponent();
            for(int i=0;i<10;i++)
            {
                for(int j=0;j<10;j++)
                {
                    PutCell(i, j, State.idle, zoom);
                }
            }
       
        }

        private void PutCell(int x, int y, State state, int zoom)
        {
            int increase = 0;
            if(zoom>=0)
            {
                increase = zoom + 1;
            }
            if(zoom<0)
            {
                increase = -1*(1/zoom - 1);
            }
            Button but = new Button();
            if (state == State.idle)
            {
                but.Background = new SolidColorBrush(Colors.White);
            }
            else if(state==State.alive)
            {
                but.Background = new SolidColorBrush(Colors.Green);
            }
            else if(state==State.dead)
            {
                but.Background = new SolidColorBrush(Colors.Red);
            }
            but.Width = 7*increase;
            but.Height = 7*increase;
            but.Click += CellClicked;
            Canvas.SetLeft(but, x*7*increase);
            Canvas.SetTop(but, y*7*increase);
            Cell cell = new Cell();
            cell.color = but.Background.ToString();
            cell.state = state;
            cell.positionx = x+1;
            cell.positiony = y+1;
            but.Name = "but_"+cell.positionx .ToString() + "_" + cell.positiony.ToString();
            buttonlist.Add(but);
            cellist.Add(cell);
            cellcanvas.Children.Add(but);
            cellcanvas.Height = x * 7 * increase+but.Height;
            cellcanvas.Width = cellcanvas.Height;
        }

        private void CellClicked(object sender, RoutedEventArgs e)
        {
            
            Button clickedButton = sender as Button;
            string name = clickedButton.Name;
            string[] splitted=name.Split('_');
            int posx = Int32.Parse(splitted[1]);
            int posy = Int32.Parse(splitted[2]);
            if (clickedButton.Background.ToString() == "#FFFFFFFF")
            {
                clickedButton.Background = new SolidColorBrush(Colors.Green);
                foreach(Cell cell in cellist)
                {
                    if(cell.positionx==posx&&cell.positiony==posy)
                    {
                        cell.state = State.alive;
                    }
                }
            }
            else if(clickedButton.Background.ToString()== "#FF008000")
            {
                clickedButton.Background = new SolidColorBrush(Colors.Red);
                foreach (Cell cell in cellist)
                {
                    if (cell.positionx == posx && cell.positiony == posy)
                    {
                        cell.state = State.dead;
                    }
                }
            }
            else if (clickedButton.Background.ToString() == "#FFFF0000")
            {
                clickedButton.Background = new SolidColorBrush(Colors.White);
                foreach (Cell cell in cellist)
                {
                    if (cell.positionx == posx && cell.positiony == posy)
                    {
                        cell.state = State.idle;
                    }
                }
            }
        }

        private void randgreenbut_Click(object sender, RoutedEventArgs e)
        {
           // int howmany = Int32.Parse(randgreen.Text.ToString());
            int howmany;
            bool isNumeric = int.TryParse(randgreen.Text, out howmany);

            if (isNumeric)
            {
                Random rnd = new Random();

                for (int i = 0; i < howmany; i++)
                {

                    int rand = rnd.Next(buttonlist.Count);
              
                    buttonlist[rand].Background = new SolidColorBrush(Colors.Green);
                    cellist[rand].state = State.alive;
                }
            }
            else
            {
                MessageBox.Show("Wrong input!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void randredbut_Click(object sender, RoutedEventArgs e)
        {
            int howmany;
            bool isNumeric = int.TryParse(randgreen.Text, out howmany);
            if (isNumeric)
            {
                Random rnd = new Random();

                for (int i = 0; i < howmany; i++)
                {

                    int rand = rnd.Next(buttonlist.Count);
                    buttonlist[rand].Background = new SolidColorBrush(Colors.Red);
                    cellist[rand].state = State.dead;
                }
            }
            else
            {
                MessageBox.Show("Wrong input!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void resetgridbut_Click(object sender, RoutedEventArgs e)
        {
            zoom = 0;
            cellcanvas.Children.Clear();
            buttonlist.Clear();
            foreach(Cell cell in cellist)
            {
                cellistcpy1.Add(cell);
            }
            cellist.Clear();
          for(int i=0;i<Math.Sqrt(cellistcpy1.Count);i++)
            {
                for(int j=0;j<Math.Sqrt(cellistcpy1.Count);j++)
                {
                    PutCell(i, j, State.idle, zoom);
                }
            }

            cellistcpy1.Clear();
            
            }

        private void setiingsbut_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();

            settings.Show();
            this.Hide();
            settings.Closed += Settings_Closed;
        }

        private void Settings_Closed(object sender, EventArgs e)
        {
            this.Show();
            if (changed)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                string content = "";

                content = File.ReadAllText("appliedrules.txt");

                var firstWord = content.Substring(0, content.IndexOf("\n"));
                int size = Int32.Parse(firstWord.ToString());
                var splitted = content.Split('\n');
                for (int i = 0; i < size; i++)
                {
                    var splitted1 = splitted[i + 1].Split(' ');
                    Rule rule = new Rule(Int32.Parse(splitted1[1]), Int32.Parse(splitted1[0]), Int32.Parse(splitted1[2]), splitted1[3], Int32.Parse(splitted1[4]));
                    rules.Add(rule);
                }
            }
        }

        private void loadgridbut_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            Nullable<bool> result = dlg.ShowDialog();
            string content = "";
            if (result == true)
            {
                try
                {
                    content = File.ReadAllText(dlg.FileName);
                    var firstWord = content.Substring(0, content.IndexOf("\n"));
                    int size = Int32.Parse(firstWord.ToString());

                    var splitted = content.Split('\n');
                    cellcanvas.Children.Clear();
                    List<State> newstates = new List<State>();
                    cellist.Clear();
                    buttonlist.Clear();
                    cellistcpy.Clear();
                    for (int i = 0; i < size; i++)
                    {
                        var splitted1 = splitted[i + 1].Split(' ');
                        int posx = Int32.Parse(splitted1[0]);
                        int posy = Int32.Parse(splitted1[1]);
                        State state = new State();
                        Cell cell = new Cell();

                        if (splitted1[2].ToString().Contains("idle"))
                        {
                            state = State.idle;
                            cell.color = "#FFFFFFFF";
                        }
                        else if (splitted1[2].ToString().Contains("alive"))
                        {
                            state = State.alive;
                            cell.color = "#FF008000";
                        }
                        else if (splitted1[2].ToString().Contains("dead"))
                        {
                            state = State.dead;
                            cell.color = "#FFFF0000";
                        }
                        cell.positionx = posx;
                        cell.positiony = posy;
                        cell.state = state;
                        PutCell(posx - 1, posy - 1, state, zoom);

                        newstates.Add(state);

                    }
                }
                catch
                {
                    MessageBox.Show("Ops, Something went wrong", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else {
                MessageBox.Show("Grid not loaded!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            

        }

        private void savegridbut_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0;i<cellcanvas.Children.Count; i++)
            {
                Cell cell = new Cell();
                Button but = new Button();
                but = (Button)cellcanvas.Children[i];
                cell.color = but.Background.ToString();
                cell.positionx = cellist[i].positionx;
                cell.positiony = cellist[i].positiony;
                if (but.Background.ToString() == "#FFFFFFFF")
                {
                    cell.state = State.idle;
                }
                else if (but.Background.ToString() == "#FF008000")
                {
                    cell.state = State.alive;
                }
                else if (but.Background.ToString() == "#FFFF0000")
                {
                    cell.state = State.dead;
                }
                cellistcpy.Add(cell);
            }
            string createText = cellcanvas.Children.Count.ToString() + Environment.NewLine;
            for(int j=0;j<cellistcpy.Count;j++)
            {
                createText += cellistcpy[j].positionx.ToString()+" "+cellistcpy[j].positiony.ToString()+" "+cellistcpy[j].state.ToString() + Environment.NewLine;
            }


            SaveFileDialog dialog = new SaveFileDialog();
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                 string result1=result.Value.ToString();
            }
            

            using (StreamWriter writetext = new StreamWriter(dialog.FileName+".txt"))
            {
                writetext.WriteLine(createText);

            }
            TextFileToBinary(dialog.FileName);
        }

        private void TextFileToBinary(string TexFilePath)
        {
            string fileContents;
            using (FileStream fileStream = new FileStream(TexFilePath+".txt", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    fileContents = sr.ReadToEnd();
                }
            }
            using (FileStream fs = new FileStream(TexFilePath+"txt".Replace("txt", "bin"), FileMode.Create))
            {
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, fileContents);
            }
        }

        private void startbut_Click(object sender, RoutedEventArgs e)
        {
            int compq;
            
            if (compqty.Text == "") { compq = 1; }
       //     else { compq = Int32.Parse(compqty.Text); }
          //  int n;
            bool isNumeric = int.TryParse(compqty.Text, out compq);
            if (isNumeric || compqty.Text == "")
            {
                if (compqty.Text == "") { compq = 1; }
                for (int cc = 0; cc < compq; cc++)
                {
                    bool modified = false;
                    cellistcpy1.Clear();

                    for (int i = 0; i < cellist.Count; i++)
                    {
                        if (oscilcheck4.Count > 0) { oscilcheck4.Clear(); }
                        if (oscilcheck1.Count == 0)
                        {
                            oscilcheck1.Add( cellist[i]);
                        }
                        if (oscilcheck2.Count == 0)
                        {
                            oscilcheck2.Add(cellist[i]);
                        }
                        if (oscilcheck3.Count == 0)
                        {
                            oscilcheck3.Add(cellist[i]);
                        }
                        if (oscilcheck4.Count == 0)
                        {
                            oscilcheck4.Add(cellist[i]);
                        }

                        modified = false;
                        Cell cell = cellist[i];
                        int state01count = ChechNeighbor(cell, State.idle);
                        int state11count = ChechNeighbor(cell, State.alive);
                        int state21count = ChechNeighbor(cell, State.dead);

                        int state02count = ChechNeighbor2(cell, State.idle);
                        int state12count = ChechNeighbor2(cell, State.alive);
                        int state22count = ChechNeighbor2(cell, State.dead);

                        int state03count = ChechNeighbor3(cell, State.idle);
                        int state13count = ChechNeighbor3(cell, State.alive);
                        int state23count = ChechNeighbor3(cell, State.dead);

                        int state = 100;
                        if (cell.state == State.idle) { state = 0; }
                        else if (cell.state == State.alive) { state = 1; }
                        else if (cell.state == State.dead) { state = 2; }
                        if(rules.Count==0)
                        {
                            
                            MessageBox.Show("Default rules will now be presented.", "", MessageBoxButton.OK);
                            Rule rule1 = new Rule(8, 1, 2, "exactly",3);
                            Rule rule2 = new Rule(1, 2, 1, "exactly",3);
                            Rule rule3 = new Rule(1, 2, 1, "more",3);
                            Rule rule4 = new Rule(1, 1, 2, "more", 3);

                            rules.Add(rule1);
                            rules.Add(rule2);
                            rules.Add(rule3);
                            rules.Add(rule4);
                        }
                        for (int j = 0; j < rules.Count; j++)
                        {
                            if (rules[j].type == 1)
                            {
                                if (rules[j].moreless == "exactly")
                                {

                                    if (!modified && rules[j].neighborstate == 0 && state01count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state11count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state21count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);

                                    }
                                }
                                else if (rules[j].moreless == "more")
                                {
                                    if (!modified && rules[j].neighborstate == 0 && state01count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state11count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state21count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);

                                    }
                                }
                                else if (rules[j].moreless == "less")
                                {
                                    if (!modified && rules[j].neighborstate == 0 && state01count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state11count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state21count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state01count, state11count, state21count);

                                    }
                                }
                            }
                            else if(rules[j].type==2)
                            {
                                if (rules[j].moreless == "exactly")
                                {

                                    if (!modified && rules[j].neighborstate == 0 && state02count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state12count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state22count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);

                                    }
                                }
                                else if (rules[j].moreless == "more")
                                {
                                    if (!modified && rules[j].neighborstate == 0 && state02count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state12count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state22count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);

                                    }
                                }
                                else if (rules[j].moreless == "less")
                                {
                                    if (!modified && rules[j].neighborstate == 0 && state02count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state12count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state22count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state02count, state12count, state22count);

                                    }
                                }
                            }
                            else if(rules[j].type==3)
                            {
                                if (rules[j].moreless == "exactly")
                                {

                                    if (!modified && rules[j].neighborstate == 0 && state03count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state13count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state23count == rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);

                                    }
                                }
                                else if (rules[j].moreless == "more")
                                {
                                    if (!modified && rules[j].neighborstate == 0 && state03count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state13count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state23count > rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);

                                    }
                                }
                                else if (rules[j].moreless == "less")
                                {
                                    if (!modified && rules[j].neighborstate == 0 && state03count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);
                                    }
                                    if (!modified && rules[j].neighborstate == 1 && state13count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);

                                    }
                                    if (!modified && rules[j].neighborstate == 2 && state23count < rules[j].neighborcount && state != rules[j].finalstate)
                                    {
                                        modified = true;
                                        ApplyRule(cell, rules[j], state03count, state13count, state23count);

                                    }
                                }
                            }
                        }
                        if (!modified)
                        {


                            Cell cell1 = new Cell();
                            cell1.positionx = cell.positionx;
                            cell1.positiony = cell.positiony;
                            cell1.state = cell.state;
                            cell1.color = cell.color;
                            cellistcpy1.Add(cell1);

                        }
                      
                    }

                    cellist.Clear();
                    buttonlist.Clear();
                    cellcanvas.Children.Clear();

                    for (int a = 0; a < cellistcpy1.Count; a++)
                    {
                        PutCell(cellistcpy1[a].positionx - 1, cellistcpy1[a].positiony - 1, cellistcpy1[a].state, zoom);
                    }
                    //TU BYŁ PUT CELL
                    if (cellist==oscilcheck1||cellist==oscilcheck2||cellist==oscilcheck3||cellist==oscilcheck4)
                    {
                        MessageBox.Show("Oscilation found.", "", MessageBoxButton.OK);
                    }
                    cellistcpy1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Wrong input!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private int ChechNeighbor3(Cell cell, State v)
        {
            int sum = 0;
            foreach (Cell cell1 in cellist)
            {

                for (int i = -1; i < 2; i++)
                {

                    if (cell1.positionx == cell.positionx + i && cell1.positiony == cell.positiony - 1 && cell1.state == v)
                    {
                        sum++;
                    }
                    if (cell1.positionx == cell.positionx + i && cell1.positiony == cell.positiony + 1 && cell1.state == v)
                    {
                        sum++;
                    }

                }

                if (cell1.positionx == cell.positionx - 1 && cell1.positiony == cell.positiony && cell1.state == v)
                {
                    sum++;
                }

                if (cell1.positionx == cell.positionx + 1 && cell1.positiony == cell.positiony && cell1.state == v)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int ChechNeighbor2(Cell cell, State v)
        {
            int sum = 0;
            foreach (Cell cell1 in cellist)
            {



                if (cell1.positionx == cell.positionx && cell1.positiony == cell.positiony - 1 && cell1.state == v)
                {
                    sum++;
                }
                if (cell1.positionx == cell.positionx && cell1.positiony == cell.positiony + 1 && cell1.state == v)
                {
                    sum++;
                }



                if (cell1.positionx == cell.positionx - 1 && cell1.positiony == cell.positiony && cell1.state == v)
                {
                    sum++;
                }

                if (cell1.positionx == cell.positionx + 1 && cell1.positiony == cell.positiony && cell1.state == v)
                {
                    sum++;
                }
            }
            return sum;
        }

        private void ApplyRule(Cell cell, Rule rule, int state0count, int state1count, int state2count)
        {
            Cell cell1 = new Cell();
            cell1.positionx = cell.positionx;
            cell1.positiony = cell.positiony;
            
            if (rule.moreless == "exactly")
            {
                if (rule.neighborstate == 0 && rule.neighborcount == state0count)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
                if (rule.neighborstate == 1 && rule.neighborcount == state1count)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
                if (rule.neighborstate == 2 && rule.neighborcount == state2count)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
            }
            else if(rule.moreless=="more")
            {
                if (rule.neighborstate == 0 && state0count > rule.neighborcount)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
                if (rule.neighborstate == 1 && state1count > rule.neighborcount)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
                if (rule.neighborstate == 2 && state2count > rule.neighborcount)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
            }
            else if(rule.moreless=="less")
            {
                if (rule.neighborstate == 0 && state0count < rule.neighborcount)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
                if (rule.neighborstate == 1 && state1count < rule.neighborcount)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
                if (rule.neighborstate == 2 && state2count < rule.neighborcount)
                {
                    if (rule.finalstate == 0)
                    {
                        cell1.state = State.idle;
                    }
                    if (rule.finalstate == 1)
                    {
                        cell1.state = State.alive;
                    }
                    if (rule.finalstate == 2)
                    {
                        cell1.state = State.dead;
                    }

                }
            }
            cellistcpy1.Add(cell1);
        }

        private int ChechNeighbor(Cell cell,  State v)
        {
            int sum = 0;
           foreach(Cell cell1 in cellist)
            {
                
              for(int i=-2;i<3;i++)
                {
                    if(cell1.positionx==cell.positionx+i&&cell1.positiony==cell.positiony-2&&cell1.state==v)
                    {
                        sum++;
                    }
                    if (cell1.positionx == cell.positionx + i && cell1.positiony == cell.positiony - 1 && cell1.state == v)
                    {
                        sum++;
                    }
                    if (cell1.positionx == cell.positionx + i && cell1.positiony == cell.positiony + 1 && cell1.state == v)
                    {
                        sum++;
                    }
                    if (cell1.positionx == cell.positionx + i && cell1.positiony == cell.positiony + 2 && cell1.state == v)
                    {
                        sum++;
                    }
                }
                if (cell1.positionx == cell.positionx -2 && cell1.positiony == cell.positiony  && cell1.state == v)
                {
                    sum++;
                }
                if (cell1.positionx == cell.positionx -1 && cell1.positiony == cell.positiony && cell1.state == v)
                {
                    sum++;
                }
                if (cell1.positionx == cell.positionx + 2 && cell1.positiony == cell.positiony && cell1.state == v)
                {
                    sum++;
                }
                if (cell1.positionx == cell.positionx + 1 && cell1.positiony == cell.positiony && cell1.state == v)
                {
                    sum++;
                }
            }
            return sum;
        }

        private void resizebut_Click(object sender, RoutedEventArgs e)
        {
           

            int count;
            bool isNumeric = int.TryParse(sizetxt.Text, out count);
            if (isNumeric)
            {
                if (count <= 100)
                {
                    cellcanvas.Children.Clear();
                    buttonlist.Clear();
                    cellist.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            PutCell(i, j, State.idle, zoom);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please, I'm not the fastest computer in the world. Respect me and my hardware.\nResize refused.", "pls", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Wrong input!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void zoominbut_Click(object sender, RoutedEventArgs e)
        {
            zoom++;
            cellcanvas.Children.Clear();
            buttonlist.Clear();
            foreach(Cell cell1 in cellist)
            {
                cellistcpy1.Add(cell1);
            }
            cellist.Clear();
            foreach(Cell cell in cellistcpy1)
            {
                PutCell(cell.positionx, cell.positiony, cell.state, zoom);
            }
            cellistcpy1.Clear();
        }

        private void zoomoutbut_Click(object sender, RoutedEventArgs e)
        {
            zoom--;
            cellcanvas.Children.Clear();
            buttonlist.Clear();

            foreach (Cell cell1 in cellist)
            {
                cellistcpy1.Add(cell1);
            }
            cellist.Clear();
            foreach (Cell cell in cellistcpy1)
            {
                PutCell(cell.positionx, cell.positiony, cell.state, zoom);
            }
            cellistcpy1.Clear();
        }
    }
    
}
