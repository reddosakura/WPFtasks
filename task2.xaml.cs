using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;


namespace WpfApp1
{

    public static class Serializer
    {
        private static string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static void Serialize<T>(List<T> notes, string file)
        {
            
            string json = JsonConvert.SerializeObject(notes, Formatting.Indented);
            File.WriteAllText(desktop + "//" + file, json);
        }
        public static List<T> Deserialize<T>(string file) { 
            string json = File.ReadAllText(desktop + "//" + file);
            List<T> notes = JsonConvert.DeserializeObject<List<T>>(json);
            return notes;
        }
    }

    public class Note
    {
        public DateTime date;
        public string preview;
        public string description;
        // private List<List<string>> notes_container;

        public Note(DateTime note_date, string note_preview, string note_description)
        {
            date = note_date;
            preview = note_preview;
            description = note_description;
        }
    }

    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private static int day_step = 0;
        //private static DateTime date = DateTime.Now;
        public static List<Note> main_notes;
        //private static List<string> note_names = new List<string>();
        //private static List<List<string>> main_notes = new List<List<string>>();



        public MainWindow()
        {
            InitializeComponent();
            var notes = Serializer.Deserialize<Note>("notes.json");
            //MessageBox.Show(string.Join(" ", notes));
            List<string> note_names = new List<string>();
            datepck.DisplayDate = DateTime.Now.Date;
            //UpdateList(noteslist, DateTime.Parse(datepck.Text));
            //if (notes != null)
            //{
            //    var currdate_notes = notes.Where(note => note.date == DateTime.Now.Date);
            //    foreach (var note in currdate_notes)
            //    {
            //        note_names.Add(note.preview);
            //    }
            //    noteslist.ItemsSource = note_names;
            //}

        }

        public static void UpdateList(ListBox lst, DateTime pckdate)
        {
            var notes = Serializer.Deserialize<Note>("notes.json");
            //MessageBox.Show(string.Join(" ", notes));
            List<string> note_names = new List<string>();
            if (notes != null)
            {
                var currdate_notes = notes.Where(note => note.date == pckdate.Date);
                foreach (var note in currdate_notes)
                {
                    note_names.Add(note.preview);
                }
                //List<string> items = new List<string>() { "1", "2", "3"};
                lst.ItemsSource = note_names;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "addbtn")
            {
                if ((nametxtbox.Text != "") && (desctxtbox.Text != "") && (datepck.Text != ""))
                {
                    //main_notes = new List<Note>();
                    var notes = Serializer.Deserialize<Note>("notes.json");
                    Note nwnt = new Note(DateTime.Parse(datepck.Text), nametxtbox.Text, desctxtbox.Text);
                    notes.Add(nwnt);
                    Serializer.Serialize(notes, "notes.json");
                    nametxtbox.Text = "";
                    desctxtbox.Text = "";
                    //MessageBox.Show(datepck.Text);
                    UpdateList(noteslist, DateTime.Parse(datepck.Text));

                }
                else
                {
                    MessageBox.Show("Не корректные данные. Проверьте поля названия заметки, описания и поле даты на заполненность");
                }
                //MessageBox.Show("Создать");
            }
            else if ((sender as Button).Name == "savebtn")
            {
                //List<Note> notes = Serializer.Deserialize<Note>("notes.json");
                //var onDate = notes.Where(note => note.date == DateTime.Parse(datepck.Text).Date).ToList();
                if ((noteslist.Items != null) && (noteslist.SelectedIndex != -1))
                {
                    //MessageBox.Show(noteslist.SelectedIndex.ToString());
                    try
                    {
                        List<Note> notes = Serializer.Deserialize<Note>("notes.json");
                        var onDate = notes.Where(note => note.date == DateTime.Parse(datepck.Text).Date).ToList();
                        //MessageBox.Show(noteslist.SelectedIndex.ToString());
                        var currnote = notes[notes.IndexOf(onDate[noteslist.SelectedIndex])];
                        currnote.preview = nametxtbox.Text;
                        currnote.description = desctxtbox.Text;
                        Serializer.Serialize(notes, "notes.json");
                        UpdateList(noteslist, DateTime.Parse(datepck.Text));
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Неверное значение для поля даты");
                    }
                }
            }
            else if ((sender as Button).Name == "delbtn")
            {
                if ((noteslist.Items != null) && (noteslist.SelectedIndex != -1))
                {
                    List<Note> notes = Serializer.Deserialize<Note>("notes.json");
                    var onDate = notes.Where(note => note.date == DateTime.Parse(datepck.Text).Date).ToList();
                    //MessageBox.Show(noteslist.SelectedIndex.ToString());
                    //var currnote = notes[notes.IndexOf(onDate[noteslist.SelectedIndex])];
                    notes.RemoveAt(notes.IndexOf(onDate[noteslist.SelectedIndex]));
                    Serializer.Serialize(notes, "notes.json");
                    UpdateList(noteslist, DateTime.Parse(datepck.Text));
                    nametxtbox.Text = "";
                    desctxtbox.Text = "";
                }
            }
        }

        private void ChangedDate(object sender, SelectionChangedEventArgs e)
        {
            nametxtbox.Text = "";
            desctxtbox.Text = "";
            UpdateList(noteslist, DateTime.Parse(datepck.Text));
        }

        private void noteslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Note> notes = Serializer.Deserialize<Note>("notes.json");
            var onDate = notes.Where(note => note.date == DateTime.Parse(datepck.Text).Date).ToList();
            if (onDate != null)
            {
                try
                {
                    nametxtbox.Text = onDate[noteslist.SelectedIndex].preview;
                    desctxtbox.Text = onDate[noteslist.SelectedIndex].description;
                }
                catch(ArgumentOutOfRangeException)
                {

                }               
            }
        }
    }
}
