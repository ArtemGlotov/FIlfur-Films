using FilmsLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Labs_C_Sharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FAD _FAD;
        public delegate void ShowItem(object sender, MouseButtonEventArgs e);
        public ResourceDictionary _standartResources;
        public ResourceDictionary _currentResources;

        private MyCollection<Film> _currentFilmCollection;
        private MyCollection<Actor> _currentActorCollection;
        private MyCollection<Director> _currentDirectorCollection;

        private MyCollection<Film> _currentFilmPage = new MyCollection<Film>();
        private MyCollection<Film> _nextFilmPage = new MyCollection<Film>();
        private MyCollection<Film> _previousFilmPage = new MyCollection<Film>();
        private MyCollection<Film> _firstFilmPage = new MyCollection<Film>();
        private MyCollection<Film> _lastFilmPage = new MyCollection<Film>();

        private MyCollection<Actor> _currentActorPage = new MyCollection<Actor>();
        private MyCollection<Actor> _nextActorPage = new MyCollection<Actor>();
        private MyCollection<Actor> _previousActorPage = new MyCollection<Actor>();
        private MyCollection<Actor> _firstActorPage = new MyCollection<Actor>();
        private MyCollection<Actor> _lastActorPage = new MyCollection<Actor>();

        private MyCollection<Director> _currentDirectorPage = new MyCollection<Director>();
        private MyCollection<Director> _nextDirectorPage = new MyCollection<Director>();
        private MyCollection<Director> _previousDirectorPage = new MyCollection<Director>();
        private MyCollection<Director> _firstDirectorPage = new MyCollection<Director>();
        private MyCollection<Director> _lastDirectorPage = new MyCollection<Director>();

        private NumberPage _numCurrentFilmPage = new NumberPage();
        private NumberPage _numCurrentActorPage = new NumberPage();
        private NumberPage _numCurrentDirectorPage = new NumberPage();

        private int itemsPerPage = 2;

        public MainWindow()
        {
            InitializeComponent();
            LoadPluginsAsync();
            SetCommands();
            //SaveResources();
            GetResources();
            _FAD = new FAD();
            _currentFilmCollection = _FAD._allFilms;
            _currentActorCollection = _FAD._allActors;
            _currentDirectorCollection = _FAD._allDirectors;
            UpdateAll();
            _standartResources = new ResourceDictionary();
            foreach(var key in Resources.Keys)
            {
                _standartResources.Add(key, FindResource(key));
            }
            _currentResources = _standartResources;
            textBoxFilmPage.DataContext = _numCurrentFilmPage;
            textBoxActorPage.DataContext = _numCurrentActorPage;
            textBoxDirectorPage.DataContext = _numCurrentDirectorPage;
        }

        //OPEN
        private void OpenFilmWindow(object sender, MouseButtonEventArgs e)
        {
            Film tFilm = (sender as Panel).DataContext as Film;
            ShowFilmWindow nFilmWindow = new ShowFilmWindow(tFilm, _FAD, _currentResources);
            nFilmWindow.ShowDialog();
            UpdateFilms();
        }

        private void OpenActorWindow(object sender, MouseButtonEventArgs e)
        {
            Actor tActor = (sender as Panel).DataContext as Actor;
            ShowActorWindow nActorWindow = new ShowActorWindow(tActor, _FAD, _currentResources);
            nActorWindow.ShowDialog();
            UpdateActors();
        }

        private void OpenDirectorWindow(object sender, MouseButtonEventArgs e)
        {
            Director tDirector = (sender as Panel).DataContext as Director;
            ShowDirectorWindow nDirectorWindow = new ShowDirectorWindow(tDirector, _FAD, _currentResources);
            nDirectorWindow.ShowDialog();
            UpdateDirectors();
        }
        
        //ADD
        private void ButtonAddFilm_Click(object sender, RoutedEventArgs e)
        {
            OperationWithFilmWindow WindowNewFilm = new OperationWithFilmWindow(_FAD, "Add", null, _currentResources)
            {
                Title = "Add film"
            };
            WindowNewFilm.ShowDialog();
            UpdateCurrentFilmPageAsync();
        }

        private void ButtonAddActor_Click(object sender, RoutedEventArgs e)
        {
            OperationWithActorWindow WindowNewActor = new OperationWithActorWindow(_FAD, "Add", null, _currentResources)
            {
                Title = "Add film"
            };
            WindowNewActor.ShowDialog();
            System.Windows.Shapes.Path fil = new System.Windows.Shapes.Path();
            UpdateCurrentActorPageAsync();
        }

        private void ButtonAddDirector_Click(object sender, RoutedEventArgs e)
        {
            OperationWithDirectorWindow WindowNewDirector = new OperationWithDirectorWindow(_FAD, "Add", null, _currentResources)
            {
                Title = "Add film"
            };
            WindowNewDirector.ShowDialog();
            UpdateCurrentDirectorPageAsync();
        }

        //FIND
        private void ButtonFindFilms_Click(object sender, RoutedEventArgs e)
        {
            UpdateFilms();
        }

        private MyCollection<Film> FindFilms(MyCollection<Film> consideredCollection)
        {
            if (!Int32.TryParse(TextBoxFindedFilmRating.Text, out int nRating))
            {
                TextBoxFindedFilmRating.Clear();
            }
            if (!Int32.TryParse(TextBoxFindedFilmReleaseDate.Text, out int nReleaseDate))
            {
                TextBoxFindedFilmReleaseDate.Clear();
            }
            return FAD.FindFilms(consideredCollection, TextBoxFindedFilmTitle.Text, nRating, nReleaseDate);
        }

        private void ButtonFindActors_Click(object sender, RoutedEventArgs e)
        {
            UpdateActors();
        }

        private MyCollection<Actor> FindActors(MyCollection<Actor> consideredCollection)
        {
            if (!Int32.TryParse(TextBoxFindedActorAge.Text, out int nAge))
            {
                TextBoxFindedActorAge.Clear();
            }
            return FAD.FindActors(consideredCollection, TextBoxFindedFilmTitle.Text, TextBoxFindedActorLastname.Text, nAge);
        }

        private void ButtonFindDirectors_Click(object sender, RoutedEventArgs e)
        {
            UpdateDirectors();
        }

        private MyCollection<Director> FindDirectors(MyCollection<Director> consideredCollection)
        {
            return FAD.FindDirectors(consideredCollection, TextBoxFindedDirectorName.Text, TextBoxFindedDirectorLastname.Text);
        }

        //CLEAR TEXTBOXS
        private void ClearTextBoxsFilm()
        {
            TextBoxFindedFilmRating.Clear();
            TextBoxFindedFilmReleaseDate.Clear();
            TextBoxFindedFilmTitle.Clear();
        }

        private void ClearTextBoxsActor()
        {
            TextBoxFindedActorLastname.Clear();
            TextBoxFindedActorName.Clear();
            TextBoxFindedActorAge.Clear();
        }

        private void ClearTextBoxsDirector()
        {
            TextBoxFindedDirectorLastname.Clear();
            TextBoxFindedDirectorName.Clear();
        }

        //SHOWALL
        private void ShowAll()
        {
            ShowAllFilms();
            ShowAllActors();
            ShowAllDirectors();
        }

        private void ButtonShowAllFilms_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxsFilm();
            ShowAllFilms();
        }

        private void ShowAllFilms()
        {
            _currentFilmCollection = _FAD._allFilms;
            _numCurrentFilmPage.Number = 1;
            UpdateCurrentFilmPageAsync();
        }

        private void ButtonShowAllActors_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxsActor();
            ShowAllActors();
        }

        private void ShowAllActors()
        {
            _currentActorCollection = _FAD._allActors;
            _numCurrentActorPage.Number = 1;
            UpdateCurrentActorPageAsync();
        }

        private void ButtonShowAllDirectors_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxsDirector();
            ShowAllDirectors();
        }

        private void ShowAllDirectors()
        {
            _currentDirectorCollection = _FAD._allDirectors;
            _numCurrentDirectorPage.Number = 1;
            UpdateCurrentDirectorPageAsync();
        }
        
        //SORT
        private void ComboBoxSortDirectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDirectors();
        }

        private MyCollection<Director> SortDirectors(MyCollection<Director> consideredCollection)
        {
            if (ComboBoxSortDirectors.SelectedIndex == 0)
            {
                return FAD.SortDirectorsByName(consideredCollection);
            }
            else if (ComboBoxSortDirectors.SelectedIndex == 1)
            {
                return FAD.SortDirectorsByLastname(consideredCollection);
            }
            else return consideredCollection;
        }

        private void ComboBoxSortFilms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFilms();
        }

        private MyCollection<Film> SortFilms(MyCollection<Film> consideredCollection)
        {
            if (ComboBoxSortFilms.SelectedIndex == 0)
            {
                return FAD.SortFilmsByTitle(consideredCollection);
            }
            if (ComboBoxSortFilms.SelectedIndex == 1)
            {
                return FAD.SortFilmsByReleaseYear(consideredCollection);
            }
            if (ComboBoxSortFilms.SelectedIndex == 2)
            {
                return FAD.SortFilmsByRating(consideredCollection);
            }
            else return consideredCollection;
        }

        private void ComboBoxSortActors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateActors();
        }

        private MyCollection<Actor> SortActors(MyCollection<Actor> consideredCollection)
        {
            if (ComboBoxSortActors.SelectedIndex == 0)
            {
                return FAD.SortActorsByName(consideredCollection);
            }
            else if (ComboBoxSortActors.SelectedIndex == 1)
            {
                return FAD.SortActorsByLastname(consideredCollection);
            }
            else if (ComboBoxSortActors.SelectedIndex == 2)
            {
                return FAD.SortActorsByAge(consideredCollection);
            }
            else return consideredCollection;
        }

        //FILES
        private void MenuItemLoadNewFile_Click(object sender, RoutedEventArgs e)
        {
            LoadNewFile();
        }

        private void LoadNewFile()
        {
            if (MessageBox.Show("Do you want to save this data?", "Save", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SaveFile();
            }
            FAD nFAD = new FAD();
            OpenFileDialog nFileDialog = new OpenFileDialog
            {
                Filter = "Data files (*.BIN)|*.bin"
            };
            if (nFileDialog.ShowDialog() == true)
            {
                nFAD.LoadFADFromFile(nFileDialog.FileName);
            }
            nFileDialog.Reset();
            _FAD = nFAD;
            ShowAll();
            UpdateAll();
        }

        private void MenuItemSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            SaveFileDialog nSaveFileDialog = new SaveFileDialog
            {
                Filter = "Data files (*.BIN)|*.bin"
            };
            if (nSaveFileDialog.ShowDialog() == true)
            {
                _FAD.SaveFADToFile(nSaveFileDialog.FileName);
            }
            nSaveFileDialog.Reset();
        }

        private void MenuItemAddFromFile_Click(object sender, RoutedEventArgs e)
        {
            AddFromFile();
        }

        private void AddFromFile()
        {
            FAD nFAD = new FAD();
            OpenFileDialog nFileDialog = new OpenFileDialog
            {
                Filter = "Data files (*.BIN)|*.bin"
            };
            if (nFileDialog.ShowDialog() == true)
            {
                nFAD.LoadFADFromFile(nFileDialog.FileName);
            }
            nFileDialog.Reset();
            _FAD += nFAD;
            ShowAll();
            UpdateAll();
        }
        
        //PLUGINS
        private void MenuItemStandart_Click(object sender, RoutedEventArgs e)
        {
            _currentResources = _standartResources;
            Resources = _currentResources;
        }

        private void SetStyle(ResourceDictionary styleResourses)
        {
            _currentResources = styleResourses;
            Resources = _currentResources;   
            
        }

        private async void LoadPluginsAsync()
        {
            MenuItem plugins = new MenuItem();
            TextBlock headerPlugin = new TextBlock()
            {
                Text = "Plugins"
            };
            headerPlugin.SetResourceReference(StyleProperty, "firstTextStyle");
            plugins.Header = headerPlugin;

            string pluginsDirectory = ConfigurationManager.AppSettings["PluginsDirectory"];
            var dlls = Directory.EnumerateFiles(Path.Combine(Environment.CurrentDirectory, pluginsDirectory), "*.dll");
            List<Task> tasks = new List<Task>();
            Parallel.ForEach(dlls, new Action<string>(path => tasks.Add(LoadItem(path, plugins))));

            MenuItem standartStyle = new MenuItem()
            {
                Header = "Standart"
            };
            standartStyle.Click += MenuItemStandart_Click;
            plugins.Items.Add(standartStyle);

            await Task.WhenAll(tasks);
            
            MainMenu.Items.Add(plugins);
        }

        private Task LoadItem(string path, MenuItem plugins)
        {
            return Task.Run(() =>
            {
                var assembly = Assembly.LoadFile(path);
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.GetInterfaces().Contains(typeof(StyleInterface.IStyle)))
                    {
                        object objofclass = type.Assembly.CreateInstance(type.FullName);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            var newMenuItem = new MenuItem() { Header = ((StyleInterface.IStyle)objofclass).Name };
                            newMenuItem.Click += (object sender, RoutedEventArgs e) => SetStyle(((StyleInterface.IStyle)objofclass).ChangeStyle());
                            plugins.Items.Add(newMenuItem);
                        }));
                    }
                }
                Thread.Sleep(3000);
            });
        }

        //UPDATE
        private void UpdateAll()
        {
            UpdateActors();
            UpdateFilms();
            UpdateDirectors();
        }

        private void UpdateFilms()
        {
            _currentFilmCollection = _FAD._allFilms;
            _currentFilmCollection = FindFilms(_currentFilmCollection);
            _currentFilmCollection = SortFilms(_currentFilmCollection);
              
            UpdateCurrentFilmPageAsync();
        }

        private void UpdateActors()
        {
            _currentActorCollection = _FAD._allActors;
            _currentActorCollection = FindActors(_currentActorCollection);
            _currentActorCollection = SortActors(_currentActorCollection);

            UpdateCurrentActorPageAsync();
        }

        private void UpdateDirectors()
        {
            _currentDirectorCollection = _FAD._allDirectors;
            _currentDirectorCollection = FindDirectors(_currentDirectorCollection);
            _currentDirectorCollection = SortDirectors(_currentDirectorCollection);

            UpdateCurrentDirectorPageAsync();
        }

        //OTHER
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async Task<MyCollection<T>> SetPage<T>(MyCollection<T> source, int pageNumber) where T : IEquatable<T>
        {
            return await Task.Run(() =>
            {
                if (source.Count() == 0)
                    return new MyCollection<T>();
                int previousItemsCount = (pageNumber - 1) * itemsPerPage;
                if (itemsPerPage * pageNumber > source.Count())
                    return Enumerable.Range(previousItemsCount + 1, source.Count() - previousItemsCount).AsParallel().AsOrdered().Select(i => source[i - 1]).AsMyCollection();

                return Enumerable.Range(previousItemsCount + 1, itemsPerPage).AsParallel().AsOrdered().Select(i => source[i - 1]).ToList().AsMyCollection();
            });
        }

        //FILM PAGES
        private async void LoadFilmPagesAsync()
        {
            int maxPages = _currentFilmCollection.Count() / itemsPerPage + ((_currentFilmCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            
            if (_numCurrentFilmPage.Number != maxPages)
                _nextFilmPage = await SetPage(_currentFilmCollection, _numCurrentFilmPage.Number + 1);
            if (_numCurrentFilmPage.Number != 1)
                _previousFilmPage = await SetPage(_currentFilmCollection, _numCurrentFilmPage.Number - 1);
            _firstFilmPage = await SetPage(_currentFilmCollection, 1);
            _lastFilmPage = await SetPage(_currentFilmCollection, maxPages);
        }

        private async void UpdateCurrentFilmPageAsync()
        {
            int maxPages = _currentFilmCollection.Count() / itemsPerPage + ((_currentFilmCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_numCurrentFilmPage.Number > maxPages && _currentFilmCollection.Count != 0)
            {
                _numCurrentFilmPage.Number = maxPages;
            }
            _currentFilmPage = await SetPage(_currentFilmCollection, _numCurrentFilmPage.Number);
            FilmPanel.ItemsSource = _currentFilmPage;
            LoadFilmPagesAsync();
        }

        private void NextFilmPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentFilmCollection.Count() / itemsPerPage + ((_currentFilmCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_currentFilmCollection.Count == 0 || _numCurrentFilmPage.Number == maxPages) return;
            FilmPanel.ItemsSource = _nextFilmPage;
            _numCurrentFilmPage.Number++;
            LoadFilmPagesAsync();
        }

        private void PreviousFilmPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFilmCollection.Count == 0 || _numCurrentFilmPage.Number == 1) return;
            FilmPanel.ItemsSource = _previousFilmPage;
            _numCurrentFilmPage.Number--;
            LoadFilmPagesAsync();
        }

        private void FirstFilmPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFilmCollection.Count == 0 || _numCurrentFilmPage.Number == 1) return;
            FilmPanel.ItemsSource = _firstFilmPage;
            _numCurrentFilmPage.Number = 1;
            LoadFilmPagesAsync();
        }

        private void LastFilmPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentFilmCollection.Count() / itemsPerPage + ((_currentFilmCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_currentFilmCollection.Count == 0 || _numCurrentFilmPage.Number == maxPages) return;
            FilmPanel.ItemsSource = _lastFilmPage;
            _numCurrentFilmPage.Number = maxPages;
            LoadFilmPagesAsync();
        }

        //ACTOR PAGES
        private async void LoadActorPagesAsync()
        {
            int maxPages = _currentActorCollection.Count() / itemsPerPage + ((_currentActorCollection.Count() % itemsPerPage == 0) ? 0 : 1);

            if (_numCurrentActorPage.Number != maxPages)
                _nextActorPage = await SetPage(_currentActorCollection, _numCurrentFilmPage.Number + 1);
            if (_numCurrentFilmPage.Number != 1)
                _previousActorPage = await SetPage(_currentActorCollection, _numCurrentFilmPage.Number - 1);
            _firstActorPage = await SetPage(_currentActorCollection, 1);
            _lastActorPage = await SetPage(_currentActorCollection, maxPages);
        }

        private async void UpdateCurrentActorPageAsync()
        {
            int maxPages = _currentActorCollection.Count() / itemsPerPage + ((_currentActorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_numCurrentActorPage.Number > maxPages && _currentActorCollection.Count != 0)
            {
                _numCurrentActorPage.Number = maxPages;
            }
            _currentActorPage = await SetPage(_currentActorCollection, _numCurrentActorPage.Number);
            ActorPanel.ItemsSource = _currentActorPage;
            LoadActorPagesAsync();
        }

        private void NextActorPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentActorCollection.Count() / itemsPerPage + ((_currentActorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_currentActorCollection.Count == 0 || _numCurrentActorPage.Number == maxPages) return;
            ActorPanel.ItemsSource = _nextActorPage;
            _numCurrentActorPage.Number++;
            LoadActorPagesAsync();
        }

        private void PreviousActorPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentActorCollection.Count == 0 || _numCurrentActorPage.Number == 1) return;
            ActorPanel.ItemsSource = _previousActorPage;
            _numCurrentActorPage.Number--;
            LoadActorPagesAsync();
        }

        private void FirstActorPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentActorCollection.Count == 0 || _numCurrentActorPage.Number == 1) return;
            ActorPanel.ItemsSource = _firstActorPage;
            _numCurrentActorPage.Number = 1;
            LoadActorPagesAsync();
        }

        private void LastActorPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentActorCollection.Count() / itemsPerPage + ((_currentActorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_currentActorCollection.Count == 0 || _numCurrentActorPage.Number == maxPages) return;
            ActorPanel.ItemsSource = _lastActorPage;
            _numCurrentActorPage.Number = maxPages;
            LoadActorPagesAsync();
        }

        //FILM PAGES
        private async void LoadDirectorPagesAsync()
        {
            int maxPages = _currentDirectorCollection.Count() / itemsPerPage + ((_currentDirectorCollection.Count() % itemsPerPage == 0) ? 0 : 1);

            if (_numCurrentDirectorPage.Number != maxPages)
                _nextDirectorPage = await SetPage(_currentDirectorCollection, _numCurrentDirectorPage.Number + 1);
            if (_numCurrentDirectorPage.Number != 1)
                _previousDirectorPage = await SetPage(_currentDirectorCollection, _numCurrentDirectorPage.Number - 1);
            _firstDirectorPage = await SetPage(_currentDirectorCollection, 1);
            _lastDirectorPage = await SetPage(_currentDirectorCollection, maxPages);
        }

        private async void UpdateCurrentDirectorPageAsync()
        {
            int maxPages = _currentDirectorCollection.Count() / itemsPerPage + ((_currentDirectorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_numCurrentDirectorPage.Number > maxPages && _currentDirectorCollection.Count != 0)
            {
                _numCurrentDirectorPage.Number = maxPages;
            }
            _currentDirectorPage = await SetPage(_currentDirectorCollection, _numCurrentDirectorPage.Number);
            DirectorPanel.ItemsSource = _currentDirectorPage;
            LoadDirectorPagesAsync();
        }

        private void NextDirectorPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentDirectorCollection.Count() / itemsPerPage + ((_currentDirectorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_currentDirectorCollection.Count == 0 || _numCurrentDirectorPage.Number == maxPages) return;
            DirectorPanel.ItemsSource = _nextDirectorPage;
            _numCurrentDirectorPage.Number++;
            LoadDirectorPagesAsync();
        }

        private void PreviousDirectorPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentDirectorCollection.Count == 0 || _numCurrentDirectorPage.Number == 1) return;
            DirectorPanel.ItemsSource = _previousDirectorPage;
            _numCurrentDirectorPage.Number--;
            LoadDirectorPagesAsync();
        }

        private void FirstDirectorPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentDirectorCollection.Count == 0 || _numCurrentDirectorPage.Number == 1) return;
            DirectorPanel.ItemsSource = _firstDirectorPage;
            _numCurrentDirectorPage.Number = 1;
            LoadDirectorPagesAsync();
        }

        private void LastDirectorPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentDirectorCollection.Count() / itemsPerPage + ((_currentDirectorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            if (_currentDirectorCollection.Count == 0 || _numCurrentDirectorPage.Number == maxPages) return;
            DirectorPanel.ItemsSource = _lastDirectorPage;
            _numCurrentDirectorPage.Number = maxPages;
            LoadDirectorPagesAsync();
        }

        //BUTTON GO
        private void ButtonGoToFilmPage_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentFilmCollection.Count() / itemsPerPage + ((_currentFilmCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            string textPage = textBoxFilmPage.Text;
            if (CheckCorrectnessOfNumberOfPage(textPage, out int numPage, maxPages))
            {
                _numCurrentFilmPage.Number = numPage;
                UpdateCurrentFilmPageAsync();
            }
            else
            {
                MessageBox.Show("This page doesn't exist.", "Incorrect input", MessageBoxButton.OK);
                _numCurrentFilmPage.Number = _numCurrentFilmPage.Number;
            }
        }

        private void ButtonGoToActorPage_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentActorCollection.Count() / itemsPerPage + ((_currentActorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            string textPage = textBoxActorPage.Text;
            if (CheckCorrectnessOfNumberOfPage(textPage, out int numPage, maxPages))
            {
                _numCurrentActorPage.Number = numPage;
                UpdateCurrentActorPageAsync();
            }
            else
            {
                MessageBox.Show("This page doesn't exist.", "Incorrect input", MessageBoxButton.OK);
                _numCurrentActorPage.Number = _numCurrentActorPage.Number;
            }
        }

        private void ButtonGoToDirectorPage_Click(object sender, RoutedEventArgs e)
        {
            int maxPages = _currentDirectorCollection.Count() / itemsPerPage + ((_currentDirectorCollection.Count() % itemsPerPage == 0) ? 0 : 1);
            string textPage = textBoxDirectorPage.Text;
            if (CheckCorrectnessOfNumberOfPage(textPage, out int numPage, maxPages))
            {
                _numCurrentDirectorPage.Number = numPage;
                UpdateCurrentDirectorPageAsync();
            }
            else
            {
                MessageBox.Show("This page doesn't exist.", "Incorrect input", MessageBoxButton.OK);
                _numCurrentDirectorPage.Number = _numCurrentDirectorPage.Number;
            }
        }

        private bool CheckCorrectnessOfNumberOfPage(string textPage, out int numPage, int maxPages)
        {
            if(Int32.TryParse(textPage, out numPage) && numPage > 0 && numPage <= maxPages)
            {
                return true;
            }
            return false;
        }

        //HOTKEYS
        private void SetCommandBindings()
        {
            CommandBinding saveBinding = new CommandBinding();
            saveBinding.Command = HotKeyCommands.Save;
            saveBinding.Executed += SaveExecuted;
            CommandBinding openBinding = new CommandBinding();
            openBinding.Command = HotKeyCommands.Open;
            openBinding.Executed += OpenExecuted;
            CommandBinding closeBinding = new CommandBinding();
            closeBinding.Command = HotKeyCommands.Close;
            closeBinding.Executed += CloseExecuted;
            CommandBinding nextPageBinding = new CommandBinding();
            nextPageBinding.Command = HotKeyCommands.NextPage;
            nextPageBinding.Executed += NextPageExecuted;
            CommandBinding previousPageBinding = new CommandBinding();
            previousPageBinding.Command = HotKeyCommands.PreviousPage;
            previousPageBinding.Executed += PreviousPageExecuted;

            CommandBindings.Add(saveBinding);
            CommandBindings.Add(openBinding);
            CommandBindings.Add(closeBinding);
            CommandBindings.Add(nextPageBinding);
            CommandBindings.Add(previousPageBinding);
        }

        private void SaveExecuted(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void OpenExecuted(object sender, RoutedEventArgs e)
        {
            LoadNewFile();
        }

        private void CloseExecuted(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NextPageExecuted(object sender, RoutedEventArgs e)
        {
            if(tTabControl.SelectedIndex == 0)
            {
                NextFilmPageButton_Click(sender, e);
            }
            else if(tTabControl.SelectedIndex == 1)
            {
                NextActorPageButton_Click(sender, e);
            }
            else if(tTabControl.SelectedIndex == 2)
            {
                NextDirectorPageButton_Click(sender, e);
            }
        }

        private void PreviousPageExecuted(object sender, RoutedEventArgs e)
        {
            if (tTabControl.SelectedIndex == 0)
            {
                PreviousFilmPageButton_Click(sender, e);
            }
            else if (tTabControl.SelectedIndex == 1)
            {
                PreviousActorPageButton_Click(sender, e);
            }
            else if (tTabControl.SelectedIndex == 2)
            {
                PreviousDirectorPageButton_Click(sender, e);
            }
        }

        private void SetCommands()
        {
            SetCommandBindings();
            try
            {
                HotKeySection customSection = (HotKeySection)ConfigurationManager.GetSection("HotKeySection");
                foreach (HotKeyElement element in customSection.HotKeys)
                {
                    try
                    {
                        RoutedCommand elementCommand = GetCommand(element.Command);
                        KeyGesture elementGesture = GetKeyGesture(element.Gesture);
                        InputBindings.Add(new KeyBinding(elementCommand, elementGesture));
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                return;
            }

        }

        private RoutedCommand GetCommand(string command)
        {
            switch (command)
            {
                case "Save": return HotKeyCommands.Save;
                case "Open": return HotKeyCommands.Open;
                case "Close": return HotKeyCommands.Close;
                case "NextPage": return HotKeyCommands.NextPage;
                case "PreviousPage": return HotKeyCommands.PreviousPage;
            }
            throw new Exception();
        }

        private KeyGesture GetKeyGesture(string gesture)
        {
            switch (gesture)
            {
                case "CTRL+S": return new KeyGesture(Key.S, ModifierKeys.Control);
                case "CTRL+O": return new KeyGesture(Key.O, ModifierKeys.Control);
                case "CTRL+Q": return new KeyGesture(Key.Q, ModifierKeys.Control);
                case "RIGHT": return new KeyGesture(Key.Right);
                case "LEFT": return new KeyGesture(Key.Left);
            }
            throw new Exception();
        }

        //IMAGE RESOURCES
        private void GetResources()
        {
            Application.Current.Resources.Add("LoupeImage", GetImage("LoupeImage").Source);
            Application.Current.Resources.Add("ToNextImage", GetImage("ToNextImage").Source);
            Application.Current.Resources.Add("ToPreviousImage", GetImage("ToPreviousImage").Source);
            Application.Current.Resources.Add("ToLastImage", GetImage("ToLastImage").Source);
            Application.Current.Resources.Add("ToFirstImage", GetImage("ToFirstImage").Source);
            Application.Current.Resources.Add("BlankImage", GetImage("BlankImage").Source);
            Application.Current.Resources.Add("ChangeImage", GetImage("ChangeImage").Source);
            Application.Current.Resources.Add("LoadImage", GetImage("LoadImage").Source);
            Application.Current.Resources.Add("TrashImage", GetImage("TrashImage").Source);
        }

        private void SaveResources()
        {
            string path = @"D:\BSUIR\2 курс\C_Sharp\Labs_C_Sharp\Labs_C_Sharp\ImagesResources\";
            using (ResXResourceWriter resx = new ResXResourceWriter("pictogramms.resx"))
            {
                resx.AddResource("BlankImage", GetImageBuffer(path + "blankImage.jpg"));
                resx.AddResource("ChangeImage", GetImageBuffer(path + "change.png"));
                resx.AddResource("LoupeImage", GetImageBuffer(path + "loupe.png"));
                resx.AddResource("ToFirstImage", GetImageBuffer(path + "tofirst.png"));
                resx.AddResource("ToLastImage", GetImageBuffer(path + "tolast.png"));
                resx.AddResource("ToNextImage", GetImageBuffer(path + "tonext.png"));
                resx.AddResource("LoadImage", GetImageBuffer(path + "top.png"));
                resx.AddResource("ToPreviousImage", GetImageBuffer(path + "toprevious.png"));
                resx.AddResource("TrashImage", GetImageBuffer(path + "trash.png"));
            }
        }

        private byte[] GetImageBuffer(string path)
        {
            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(File.ReadAllBytes(path));
            source.EndInit();

            byte[] image_buffer = null;
            using (var stream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(stream);
                image_buffer = stream.ToArray();

            }
            return image_buffer;
        }

        private Image GetImage(string resource_name)
        {
            byte[] image_buff = null;

            using (ResXResourceSet resxSet = new ResXResourceSet(@".\pictogramms.resx"))
            {
                image_buff = (byte[])resxSet.GetObject(resource_name);
            }


            var icon = new Image();
            using (var stream = new MemoryStream(image_buff))
            {
                var decoder = PngBitmapDecoder.Create(stream,
                        BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                var image_source = decoder.Frames[0];
                icon.Source = image_source;
            }

            return icon;
        }
    }
}
