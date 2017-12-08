using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FilmsLib
{
    public class Film : IEquatable<Film>, INotifyPropertyChanged, IImage//, ICloneable
    {
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title= value;
                OnPropertyChanged("Name");
            }
        }

        private int _rating;
        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
                OnPropertyChanged("Rating");
            }
        }

        public int _releaseYear;
        public int ReleaseYear
        {
            get
            {
                return _releaseYear;
            }
            set
            {
                _releaseYear = value;
                OnPropertyChanged("ReleaseYear");
            }
        }

        private Director _director;
        public Director Director
        {
            get
            {
                return _director;
            }
            set
            {
                _director = value;
                OnPropertyChanged("Director");
            }
        }

        private string _image;
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private MyCollection<Genre> _genres;
        public MyCollection<Genre> Genres
        {
            get
            {
                return _genres;
            }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }

        private MyCollection<Actor> _actors;
        public MyCollection<Actor> Actors
        {
            get
            {
                return _actors;
            }
            set
            {
                _actors = value;
                OnPropertyChanged("Actors");
            }
        }

        public Film(string nTitle, int nRating, int nReliseYear, Director nDirector, string nImage)
        {
            Title = nTitle;
            Rating = nRating;
            ReleaseYear = nReliseYear;
            Genres = new MyCollection<Genre>();
            AddGenres();
            Director = nDirector;
            Actors = new MyCollection<Actor>();
            Image =  nImage ?? "ImagesResources\\blankImage.jpg";
        }

        private void AddGenres()
        {
            Genres.Add(new Genre("action", false));
            Genres.Add(new Genre("comedy", false));
            Genres.Add(new Genre("adventure", false));
            Genres.Add(new Genre("drama", false));
            Genres.Add(new Genre("crime", false));
            Genres.Add(new Genre("horror", false));
            Genres.Add(new Genre("fantasy", false));
            Genres.Add(new Genre("thriller", false));
            Genres.Add(new Genre("animation", false));
            Genres.Add(new Genre("biography", false));
            Genres.Add(new Genre("war", false));
            Genres.Add(new Genre("sci-fi", false));
            Genres.Add(new Genre("romance", false));

            Genres.Add(new Genre("biography", false));
        }

        public bool Equals(Film other) => Title == other.Title && Rating == other.Rating 
            && ReleaseYear == other.ReleaseYear;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            return Title + "." + Rating.ToString() + "." + ReleaseYear.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
