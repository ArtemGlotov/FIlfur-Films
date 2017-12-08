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
    /// Логика взаимодействия для ShowActorWindow.xaml
    /// </summary>
    public partial class ShowActorWindow : Window
    {
        private FAD _FAD;
        private Actor _tActor;
       

        public ShowActorWindow(Actor nActor, FAD nFAD, ResourceDictionary style)
        {
            InitializeComponent();
            Title = nActor.Name + " " + nActor.Lastname;
            _tActor = nActor;
            _FAD = nFAD;
            Resources.Clear();
            Resources.MergedDictionaries.Add(style);


            PanelFilms.ItemsSource = _tActor.Films;
            Resources.Add("Actor", _tActor);
            SetResourceReference(DataContextProperty, "Actor");
        }

        private void OpenFilmWindow(object sender, MouseButtonEventArgs e)
        {
            ShowFilmWindow nFilmWindow = new ShowFilmWindow((sender as Label).DataContext as Film, _FAD, Resources);
            Close();
            nFilmWindow.ShowDialog();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            _FAD.DeleteActor(_tActor);
            Close();
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            OperationWithActorWindow changeActorWindow = new OperationWithActorWindow(_FAD, "Change", _tActor, Resources);
            Close();
            changeActorWindow.ShowDialog();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
