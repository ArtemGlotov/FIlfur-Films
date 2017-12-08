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
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FilmsLib;

namespace Labs_C_Sharp
{
    /// <summary>
    /// Логика взаимодействия для OperationWithDirectorWindow.xaml
    /// </summary>
    public partial class OperationWithDirectorWindow : Window
    {
        private Director _tempDirector, _oldDirector;
        private FAD _FAD;
        private bool isNewDirector;

        public OperationWithDirectorWindow(FAD fad, string textOnWindow, Director oldDirector, ResourceDictionary style)
        {
            InitializeComponent();
            ButtonAddOrChange.Content = textOnWindow;
            Title = textOnWindow + " director";
            _FAD = fad;
            Resources.Clear();
            Resources.MergedDictionaries.Add(style);

            if (oldDirector == null)
            {
                _tempDirector = new Director(null, null, null);
                isNewDirector = true;
            }
            else
            {
                _oldDirector = oldDirector;
                _tempDirector = new Director(oldDirector.Name, oldDirector.Lastname, oldDirector.Image);
                foreach (Film tFilm in oldDirector.Films)
                {
                    _tempDirector.Films.Add(tFilm);
                }
                isNewDirector = false;
            }
            ComboBoxFilms.ItemsSource = _FAD._allFilms;
            Resources.Add("Director", _tempDirector);
            SetResourceReference(DataContextProperty, "Director");
        }

        private bool TestCorrectness()
        {
            bool result = true;
            string errorString = "";

            if (TextBoxName.Text == "")
            {
                TextBoxName.Background = Brushes.LightPink;
                errorString += "\nThe director's name can't be blank";
                result = false;
            }
            else
            {
                TextBoxName.Background = Brushes.White;
            }

            if (TextBoxLastname.Text == "")
            {
                TextBoxLastname.Background = Brushes.LightPink;
                errorString += "\nThe director's lastname can't be blank";
                result = false;
            }
            else
            {
                TextBoxLastname.Background = Brushes.White;
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
                bool isDirectorInBase = false;
                foreach (Director tDirector in _FAD._allDirectors)
                {
                    if (tDirector.Equals(_tempDirector))
                    {
                        isDirectorInBase = true;
                    }
                }

                if (!isNewDirector)
                {
                    _FAD.DeleteDirector(_oldDirector);
                    _FAD.AddDirector(_tempDirector);
                    Close();
                }
                else
                {
                    if (!isDirectorInBase)
                    {
                        _FAD.AddDirector(_tempDirector);
                        Close();
                    }
                    else
                    {
                        ErrorLabel.Content += "This director has already been in base";
                    }
                }
            }
        }

        private void ComboBoxFilms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Film selectedFilm = _FAD._allFilms[ComboBoxFilms.SelectedIndex];
            foreach (Film tFilm in _tempDirector.Films)
            {
                if (tFilm == selectedFilm)
                {
                    return;
                }
            }
            _tempDirector.Films.Add(selectedFilm);
        }

        private void DeleteLabelFilm(object sender, MouseButtonEventArgs e)
        {
            foreach (Film tFilm in _tempDirector.Films)
            {
                if (tFilm == (Film)((TextBlock)sender).DataContext)
                {
                    _tempDirector.Films.Remove(tFilm);
                    return;
                }
            }
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
            OpenFileDialog nFileDialog = new OpenFileDialog();
            nFileDialog.Filter = "Image files (*.BMP, *.JPG, *.PNG)|*.bmp;*.jpg;*.png";
            if (nFileDialog.ShowDialog() == true)
            {
                _tempDirector.Image = nFileDialog.FileName;
            }
            nFileDialog.Reset();
        }
    }
}
