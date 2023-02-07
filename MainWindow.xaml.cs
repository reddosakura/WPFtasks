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

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static string marker = "X";
        public static string marker2 = "O";
        public static List<string> emptyBtns = new List<string>() { "btn1", "btn2", "btn3", "btn4", "btn5", "btn6", "btn7", "btn8", "btn9" };
        private static void SimulateGame(List<Button> btns)
        {
            var rand = new Random();
            string buttonChoice = emptyBtns[rand.Next(emptyBtns.Count)];
            foreach (Button b in btns)
            {
                if ((b.Name == buttonChoice) & (b.Content == ""))
                {
                    b.Content = marker2;
                    b.IsEnabled = false;
                    emptyBtns.Remove(buttonChoice);
                }
            }
            

        }
        private static bool checkLine(string item1, string item2, string item3)
        {
            if (((item1 == item2) & (item1 == item3) & (item1 =="X")) || ((item1 == item2) & (item1 == item3) & (item1 == "O")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string WhoWin(List<List<Button>> btns)
        {
            for (int i = 0; i < 3; i++)
            {
                if (checkLine((string)btns[i][0].Content, (string)btns[i][1].Content, (string)btns[i][2].Content))
                {
                    return (string)btns[i][0].Content;
                    break;
                }
                else if (checkLine((string)btns[0][i].Content, (string)btns[1][i].Content, (string)btns[2][i].Content))
                {
                    return (string)btns[0][i].Content;
                    break;
                }
            }
            if (checkLine((string)btns[0][0].Content, (string)btns[1][1].Content, (string)btns[2][2].Content))
            {
                return (string)btns[0][0].Content;
            }
            else if (checkLine((string)btns[2][0].Content, (string)btns[1][1].Content, (string)btns[0][2].Content))
            {
                return (string)btns[2][0].Content;
            }
            return "";
            //else
            //{
              //  return "N";
            //}
            //for(int i = 0; i < btns.Count; i++)
            //{
              //  for (int j = 0; j < btns[0].Count; j++)
                //{
                  //  btns[i][j].IsEnabled = false;
                //}
            //}
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as Button).Content = marker;
                List<Button> btns = new List<Button>() { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };
                emptyBtns.Remove((sender as Button).Name);
                SimulateGame(btns);
                List<List<Button>> matrix = new List<List<Button>>() { new List<Button> {btn1, btn2, btn3}, new List<Button> { btn4, btn5, btn6 }, new List<Button> { btn7, btn8, btn9 } };

                if (WhoWin(matrix) == "X") 
                {
                    MessageBox.Show("Победил Х");
                    foreach (var b in btns)
                    {
                        b.IsEnabled = false;
                    }
                }
                else if (WhoWin(matrix) == "O")
                {
                    MessageBox.Show("Победил О");
                    foreach (var b in btns)
                    {
                        b.IsEnabled = false;
                    }
                }
                
            }
            catch(Exception ex)
            {
                List<Button> btns = new List<Button>() { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };
                MessageBox.Show("Ничья");
                foreach(var b in btns)
                    {
                        b.IsEnabled = false;
                    }
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            emptyBtns = new List<string>() { "btn1", "btn2", "btn3", "btn4", "btn5", "btn6", "btn7", "btn8", "btn9" };
            if (marker == "X")
            {
                marker = "O";
                marker2 = "X";
            }
            else
            {
                marker = "X";
                marker2 = "O";
            }
            btn1.IsEnabled = true;
            btn2.IsEnabled = true;
            btn3.IsEnabled = true;
            btn4.IsEnabled = true;
            btn5.IsEnabled = true;
            btn6.IsEnabled = true;
            btn7.IsEnabled = true;
            btn8.IsEnabled = true;
            btn9.IsEnabled = true;
            btn1.Content = "";
            btn2.Content = "";
            btn3.Content = "";
            btn4.Content = "";
            btn5.Content = "";
            btn6.Content = "";
            btn7.Content = "";
            btn8.Content = "";
            btn9.Content = "";

        }
    }
}
