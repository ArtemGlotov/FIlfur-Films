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
    /// Логика взаимодействия для OperationWithActorWindow.xaml
    /// </summary>
    public partial class OperationWithActorWindow : Window
    {
        private Actor _tempActor, _oldActor;
        private FAD _FAD;
        private bool isNewActor;

        public OperationWithActorWindow(FAD fad, string textOnWindow, Actor oldActor, ResourceDictionary style)
        {
            InitializeComponent();
            ButtonAddOrChange.Content = textOnWindow;
            Title = textOnWindow + " actor";
            _FAD = fad;
            Resources.Clear();
            Resources.MergedDictionaries.Add(style);

            if (oldActor == null)
            {
                _tempActor = new Actor(null, null, 0, null);
                isNewActor = true;
            }
            else
            {
                _oldActor = oldActor;
                _tempActor = new Actor(oldActor.Name, oldActor.Lastname, oldActor.Age, oldActor.Image);
                _tempActor.Biography = oldActor.Biography;
                foreach(Film tFilm in oldActor.Films)
                {
                    _tempActor.Films.Add(tFilm);
                }
                isNewActor = false;
            }
            ComboBoxFilms.ItemsSource = _FAD._allFilms;
            Resources.Add("Actor", _tempActor);
            SetResourceReference(DataContextProperty, "Actor");
        }

        private bool TestCorrectness()
        {
            bool result = true;
            string errorString = "";

            if (TextBoxName.Text == "")
            {
                TextBoxName.Background = Brushes.LightPink;
                errorString += "\nThe actor's name can't be blank";
                result = false;
            }
            else
            {
                TextBoxName.Background = Brushes.White;
            }

            if (TextBoxLastname.Text == "")
            {
                TextBoxLastname.Background = Brushes.LightPink;
                errorString += "\nThe actor's lastname can't be blank";
                result = false;
            }
            else
            {
                TextBoxLastname.Background = Brushes.White;
            }

            if (!Int32.TryParse(TextBoxAge.Text, out int checkNumber) || checkNumber < 0 || checkNumber > 120)
            {
                TextBoxAge.Background = Brushes.LightPink;
                errorString += "\nIncorrect age";
                result = false;
            }
            else
            {
                TextBoxAge.Background = Brushes.White;
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
            if (TestCorrectness())
            {
                bool isActorInBase = false;
                foreach(Actor tActor in _FAD._allActors)
                {
                    if(tActor.Equals(_tempActor))
                    {
                        isActorInBase = true;
                    }
                }

                if (!isNewActor)
                {
                    _FAD.DeleteActor(_oldActor);
                    _FAD.AddActor(_tempActor);
                    Close();
                }
                else
                {
                    if(!isActorInBase)
                    {
                        _FAD.AddActor(_tempActor);
                        Close();
                    }
                    else
                    {
                        ErrorLabel.Content += "This actor has already been in base";
                    }
                }
            }
        }

        private void ComboBoxFilms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Film selectedFilm = _FAD._allFilms[ComboBoxFilms.SelectedIndex];
            foreach (Film tFilm in _tempActor.Films)
            {
                if (tFilm == selectedFilm)
                {
                    return;
                }
            }
            _tempActor.Films.Add(selectedFilm);
        }

        private void DeleteLabelFilm(object sender, MouseButtonEventArgs e)
        {
            foreach (Film tFilm in _tempActor.Films)
            {
                if (tFilm == (Film)((TextBlock)sender).DataContext)
                {
                    _tempActor.Films.Remove(tFilm);
                    return;
                }
            }
        }

        private void ButtonChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog nFileDialog = new OpenFileDialog();
            nFileDialog.Filter = "Image files (*.BMP, *.JPG, *.PNG)|*.bmp;*.jpg;*.png";
            if (nFileDialog.ShowDialog() == true)
            {
                _tempActor.Image = nFileDialog.FileName;
            }
            nFileDialog.Reset();
        }

        private void Image_MouseEnter(object sender, RoutedEventArgs e)
        {
            ButtonChangeImage.Visibility = Visibility.Visible;
        }

        private void Image_MouseLeave(object sender, RoutedEventArgs e)
        {
            ButtonChangeImage.Visibility = Visibility.Hidden;
        }
    }
}
