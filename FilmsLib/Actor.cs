using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FilmsLib
{
    public class Actor : IEquatable<Actor>, INotifyPropertyChanged, IImage
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

        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
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

        private string _biography;
        public string Biography
        {
            get
            {
                return _biography;
            }
            set
            {
                _biography = value;
                OnPropertyChanged("Biography");
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

        public Actor(string nName, string nLastname, int nAge, string nImage)
        {
            _name = nName;
            _lastname = nLastname;
            _age = nAge;
            _films = new MyCollection<Film>();
            Image = nImage ?? "ImagesResources\\blankImage.jpg";
        }

        public bool Equals(Actor other) => _name == other._name && _lastname == other._lastname && _age == other._age;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            return Name + "." + Lastname + "." + Age.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
