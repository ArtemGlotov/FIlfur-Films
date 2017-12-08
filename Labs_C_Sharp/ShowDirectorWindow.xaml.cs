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
using FilmsLib;

namespace Labs_C_Sharp
{
    /// <summary>
    /// Логика взаимодействия для ShowDirectorWindow.xaml
    /// </summary>
    public partial class ShowDirectorWindow : Window
    {
        private FAD _FAD;
        private Director _tDirector;
        
        public ShowDirectorWindow(Director nDirector, FAD nFAD, ResourceDictionary style)
        {
            InitializeComponent();
            Title = nDirector.Name + " " + nDirector.Lastname;
            _tDirector = nDirector;
            _FAD = nFAD;
            Resources.Clear();
            Resources.MergedDictionaries.Add(style);

            PanelFilms.ItemsSource = _tDirector.Films;
            Resources.Add("Director", _tDirector);
            SetResourceReference(DataContextProperty, "Director");
        }

        private void OpenFilmWindow(object sender, MouseButtonEventArgs e)
        {
            ShowFilmWindow nFilmWindow = new ShowFilmWindow((sender as Label).DataContext as Film, _FAD, Resources);
            Close();
            nFilmWindow.ShowDialog();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            _FAD.DeleteDirector(_tDirector);
            Close();
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            OperationWithDirectorWindow changeDirectorWindow = new OperationWithDirectorWindow(_FAD, "Change", _tDirector, Resources);
            Close();
            changeDirectorWindow.ShowDialog();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
