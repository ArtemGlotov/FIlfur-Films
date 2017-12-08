using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FilmsLib
{
    public class Director : IEquatable<Director>, INotifyPropertyChanged, IImage
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _lastname;
        public string Lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
                OnPropertyChanged("Lastname");
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

        private MyCollection<Film> _films;
        public MyCollection<Film> Films
        {
            get
            {
                return _films;
            }
            set
            {
                _films = value;
            }
        }

        public Director(string nName, string nLastname, string nImage)
        {
            _name = nName;
            _lastname = nLastname;
            Image = nImage ?? "ImagesResources\\blankImage.jpg";
            _films = new MyCollection<Film>();
        }
        
        public bool Equals(Director other) => _name == other._name && _lastname == other._lastname;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            return Name + "." + Lastname;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
