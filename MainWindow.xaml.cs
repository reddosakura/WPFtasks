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
using Microsoft.WindowsAPICodePack.Dialogs;
//using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Windows.Markup;

namespace task3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            playBtn.IsEnabled = false;
            nextBtn.IsEnabled = false;
            prevBtn.IsEnabled = false;
            musicSlider.Value = 0;
            volSlider.Value = 0.5;
        }
        private static bool isPlaying = false;
        private static List<string> paths = new List<string>();
        private static List<string> songNames = new List<string>();
        private static Random rnd = new Random();
        private static bool isRepeated = false;
        private static bool isMixed = false;
        private void playMusic(string path)
        {
            mediaEl.Source = new Uri(path);
            mediaEl.Position = new TimeSpan((long)(musicSlider.Value));
            //MessageBox.Show($"{(long)(musicSlider.Value)} {mediaEl.Position}");
            mediaEl.Play();
        }

        private void OpenFiles(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            songNames.Clear();
            if (result == CommonFileDialogResult.Ok)
            {
                var files = Directory.GetFiles(dialog.FileName).ToList().Where(file => System.IO.Path.GetExtension(file) == ".mp3" ||
                                                                                                     System.IO.Path.GetExtension(file) == ".wav" ||
                                                                                                     System.IO.Path.GetExtension(file) == ".ogg" ||
                                                                                                     System.IO.Path.GetExtension(file) == ".aiff" ||
                                                                                                     System.IO.Path.GetExtension(file) == ".flac");
                //List<string> fileNames = new List<string>();
                foreach (string file in files)
                {
                    paths.Add(file);
                    songNames.Add(System.IO.Path.GetFileName(file));
                }
                playlist.ItemsSource = songNames;
            }
        }
        private void duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaEl != null)
            {
                mediaEl.Position = new TimeSpan((long)(musicSlider.Value));
                //MessageBox.Show($"{musicSlider.Value}  {span}");
                timeDur.Text = $"{mediaEl.Position.Minutes}:{mediaEl.Position.Seconds}";
            }
        }
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            musicSlider.Maximum = mediaEl.NaturalDuration.TimeSpan.Ticks;
            //MessageBox.Show($"{musicSlider.Maximum}");
        }

        //private void Timer()
        //{
        //    Thread.Sleep(1000);

        //}

        private void playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                songName.Text = playlist.SelectedItem.ToString();
                //if (isPlaying)
                //{
                musicSlider.Value = 0;
                playMusic(paths[playlist.SelectedIndex]);
                isPlaying = true;
                Thread tr = new Thread(() => { updateSlider(this); });
                playBtn.IsEnabled = true;
                nextBtn.IsEnabled = true;
                prevBtn.IsEnabled = true;
                tr.Start();
                //}
            }
            catch (NullReferenceException)
            {
                playlist.ItemsSource = new List<string>();
            }


        }

        private void nextSong(object sender, RoutedEventArgs e)
        {
            try
            {
                musicSlider.Value = 0;
                songName.Text = System.IO.Path.GetFileName(paths[songNames.IndexOf(songName.Text) + 1]);
                playMusic(paths[songNames.IndexOf(songName.Text)]);
                //MessageBox.Show($"{paths[((List<string>)playlist.ItemsSource).IndexOf(songName.Text) + 1]}\n{((List<string>)playlist.ItemsSource).IndexOf(songName.Text)}");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        private void prevSong(object sender, RoutedEventArgs e)
        {
            try
            {
                musicSlider.Value = 0;
                songName.Text = System.IO.Path.GetFileName(paths[songNames.IndexOf(songName.Text) - 1]);
                playMusic(paths[songNames.IndexOf(songName.Text)]);
                //MessageBox.Show($"{paths[((List<string>)playlist.ItemsSource).IndexOf(songName.Text) - 1]}\n{((List<string>)playlist.ItemsSource).IndexOf(songName.Text)}");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                isPlaying = false;
                mediaEl.Pause();
                mediaEl.Position = new TimeSpan((long)(musicSlider.Value));

            }
            else
            {
                isPlaying = true;
                mediaEl.Position = new TimeSpan((long)(musicSlider.Value));
                //mediaEl.Source = new Uri(paths[playlist.SelectedIndex]);
                mediaEl.Play();
                //playMusic(paths[playlist.SelectedIndex]);
            }
            //Thread tr = new Thread(() => { updateSlider(this); });
            //tr.Start();
            //mediaEl.Source = new Uri(paths[playlist.SelectedIndex]);
            //mediaEl.Play();
        }
        //private void updateSlider()
        //{
        //    Thread.Sleep(1000);
        //    musicSlider.Value += 1;
        //}
        private void updateSlider(MainWindow win)
        {
            //bool flag = true;
            while (true)
            {
                win.Dispatcher.Invoke(() =>
                {
                    win.musicSlider.Value = Convert.ToDouble(win.mediaEl.Position.Ticks);
                    if((win.musicSlider.Maximum == Convert.ToDouble(win.mediaEl.Position.Ticks)) && (isRepeated == false))
                    {
                        try
                        {
                            musicSlider.Value = 0;
                            songName.Text = System.IO.Path.GetFileName(paths[((List<string>)playlist.ItemsSource).IndexOf(songName.Text) + 1]);
                            playMusic(paths[songNames.IndexOf(songName.Text)]);
                            //MessageBox.Show($"{paths[((List<string>)playlist.ItemsSource).IndexOf(songName.Text) + 1]}\n{((List<string>)playlist.ItemsSource).IndexOf(songName.Text)}");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                    else if ((win.musicSlider.Maximum == Convert.ToDouble(win.mediaEl.Position.Ticks)) && (isRepeated))
                    {
                        win.musicSlider.Value = 0;
                    }
                });
                Thread.Sleep(1000);
                //MessageBox.Show($"{win.mediaEl.Position.Ticks}    {win.musicSlider.Value}");
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaEl.Volume = (double)(volSlider.Value);
        }

        private void repeatBtn_Click(object sender, RoutedEventArgs e)
        {
            if(isRepeated)
            {
                isRepeated = false;
                
            }
            isRepeated = true;
        }

        private void mixBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isMixed)
            {
                isMixed = false;
                playlist.ItemsSource = songNames;
            }
            else
            {
                isMixed = true;
                playlist.ItemsSource = ((List<string>)playlist.ItemsSource).OrderBy(x => rnd.Next()).ToList();
            }

            //paths = paths.OrderBy(x => rnd.Next()).ToList();
        }
    }
}
