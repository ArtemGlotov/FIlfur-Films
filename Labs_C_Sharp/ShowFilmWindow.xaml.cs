using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FilmsLib;

namespace Labs_C_Sharp
{
    /// <summary>
    /// Логика взаимодействия для ShowFilmWindow.xaml
    /// </summary>
    public partial class ShowFilmWindow : Window
    {
        private FAD _FAD;
        private Film _tFilm;
        
        public ShowFilmWindow(Film nFilm, FAD nFAD, ResourceDictionary style)
        {
            InitializeComponent();
            Title = nFilm.Title;
            _tFilm = nFilm;
            _FAD = nFAD;
            Resources.Clear();
            Resources.MergedDictionaries.Add(style);
  
            List<Genre> genres = new List<Genre>(); 
            foreach(Genre tGenre in _tFilm.Genres)
            {
                if(tGenre.Checked == true)
                {
                    genres.Add(tGenre);
                }
            }

            PanelActors.ItemsSource = _tFilm.Actors;
            PanelGenres.ItemsSource = genres;
            Resources.Add("Film", _tFilm);
            SetResourceReference(DataContextProperty, "Film");
        }

        private void OpenActorWindow(object sender, MouseButtonEventArgs e)
        {
            Close();
            ShowActorWindow nActorWondow = new ShowActorWindow((sender as StackPanel).DataContext as Actor, _FAD, Resources);
            nActorWondow.ShowDialog();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            _FAD.DeleteFilm(_tFilm);
            Close();
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            Close();
            OperationWithFilmWindow changeFilmWindow = new OperationWithFilmWindow(_FAD, "Change", _tFilm, Resources);
            changeFilmWindow.ShowDialog();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenDirectorWindow(object sender, MouseButtonEventArgs e)
        {
            Close();
            ShowDirectorWindow nDirectorWindow = new ShowDirectorWindow((sender as StackPanel).DataContext as Director, _FAD, Resources);
            nDirectorWindow.ShowDialog();
        }
    }
}
