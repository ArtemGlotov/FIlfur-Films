using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using FilmsLib;

namespace Labs_C_Sharp
{
    /// <summary>
    /// Логика взаимодействия для OperationWithFilm.xaml
    /// </summary>
    public partial class OperationWithFilmWindow : Window
    {
        private FAD _FAD;
        private Film _tempFilm, _oldFilm;
        private bool isNewFilm;
        public OperationWithFilmWindow(FAD fad, string textOnWindow, Film oldFilm, ResourceDictionary style)
        {
            InitializeComponent();
            ButtonAddOrChange.Content = textOnWindow;
            Title = textOnWindow + " film";
            _FAD = fad;
            Resources.Clear();
            Resources.MergedDictionaries.Add(style);
            if (oldFilm == null)
            {
                _tempFilm = new Film(null, 0, 0, null, null);
                isNewFilm = true;
            }
            else
            {
                _oldFilm = oldFilm;
                _tempFilm = new Film(oldFilm.Title, oldFilm.Rating, oldFilm.ReleaseYear, oldFilm.Director, oldFilm.Image);
                foreach(Actor tActor in oldFilm.Actors)
                {
                    _tempFilm.Actors.Add(tActor);
                }
                _tempFilm.Genres.Clear();
                foreach(Genre tGenre in oldFilm.Genres)
                {
                    _tempFilm.Genres.Add(new Genre(tGenre.Name, tGenre.Checked));
                }
                isNewFilm = false;
            }
            Resources.Add("Film", _tempFilm);
            SetResourceReference(DataContextProperty, "Film");

            ComboBoxActors.ItemsSource = _FAD._allActors;
            ComboBoxDirectors.ItemsSource = _FAD._allDirectors;
        }
       
        private bool TestCorrectness()
        {
            bool result = true;
            string errorString = "";
            if (TextBoxTitleOfFilm.Text == "")
            {
                TextBoxTitleOfFilm.Background = Brushes.LightPink;
                errorString += "\nThe film's title can't be blank";
                result = false;
            }
            else
            {
                TextBoxTitleOfFilm.Background = Brushes.White;
            }

            if (!Int32.TryParse(TextBoxRating.Text, out int checkNumber) || checkNumber < 0 || checkNumber > 21)
            {
                TextBoxRating.Background = Brushes.LightPink;
                errorString += "\nIncorrect rating";
                result = false;
            }
            else
            {
                TextBoxRating.Background = Brushes.White;
            }
            if (!Int32.TryParse(TextBoxReleaseYear.Text, out checkNumber) || checkNumber < 1896 || checkNumber > 2017)
            {
                TextBoxReleaseYear.Background = Brushes.LightPink;
                errorString += "\nIncorrect release year";
                result = false;
            }
            else
            {
                TextBoxReleaseYear.Background = Brushes.White;
            }
            if (!result)
            {
                errorString = errorString.Substring(1);
                ErrorLabel.Content = errorString;
            }
            else
            {
                ErrorLabel.Content = "";
            }
            return result;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAddOrChange_Click(object sender, RoutedEventArgs e)
        {
            if(TestCorrectness())
            {
                bool isFilmInBase = false;
                foreach(Film tFilm in _FAD._allFilms)
                {
                    if (tFilm.Equals(_tempFilm))
                    {
                        isFilmInBase = true;
                    }
                }

                if (!isNewFilm)
                {
                    _FAD.DeleteFilm(_oldFilm);
                    _FAD.AddFilm(_tempFilm);
                    Close();
                }
                else
                {
                    if (!isFilmInBase)
                    {
                        _FAD.AddFilm(_tempFilm);
                        Close();
                    }
                    else
                    {
                        ErrorLabel.Content += "This film has already been in base";
                    }
                }
            }
        }

        private void ComboBoxActors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Actor selectedActor = _FAD._allActors[ComboBoxActors.SelectedIndex];
            foreach (Actor tActor in _tempFilm.Actors)
            {
                if(tActor == selectedActor)
                {
                    return;
                }
            }
            _tempFilm.Actors.Add(selectedActor);
        }

        private void Image_MouseEnter(object sender, RoutedEventArgs e)
        {
            ButtonChangeImage.Visibility = Visibility.Visible;
        }

        private void Image_MouseLeave(object sender, RoutedEventArgs e)
        {
            ButtonChangeImage.Visibility = Visibility.Hidden;
        }

        private void ButtonChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog nFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.BMP, *.JPG, *.PNG)|*.bmp;*.jpg;*.png"
            };
            if (nFileDialog.ShowDialog() == true)
            {
                _tempFilm.Image = nFileDialog.FileName;
            }
            nFileDialog.Reset();  
        }

        private void DeleteLabelActor(object sender, MouseButtonEventArgs e)
        {
            foreach(Actor tActor in _tempFilm.Actors)
            {
                if(tActor == (Actor)((StackPanel)sender).DataContext)
                {
                    _tempFilm.Actors.Remove(tActor);
                    return;
                }
            }
        }
    }
}
