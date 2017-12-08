using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FilmsLib
{
    public class FAD
    {
        public MyCollection<Film> _allFilms;
        public MyCollection<Actor> _allActors;
        public MyCollection<Director> _allDirectors;
        public FAD()
        {
            _allFilms = new MyCollection<Film>();
            _allActors = new MyCollection<Actor>();
            _allDirectors = new MyCollection<Director>();
        }

        //ADD
        public void AddFilm(Film nFilm)
        {
            _allFilms.Add(nFilm);
            foreach(Actor tActor in nFilm.Actors)
            {
                tActor.Films.Add(nFilm);
            }
            nFilm.Director?.Films.Add(nFilm);
        }

        public void AddActor(Actor nActor)
        {
            _allActors.Add(nActor);
            foreach(Film tFilm in nActor.Films)
            {
                tFilm.Actors.Add(nActor);
            }
        }

        public void AddDirector(Director nDirector)
        {
            _allDirectors.Add(nDirector);
            foreach(Film tFilm in nDirector.Films)
            {
                tFilm.Director = nDirector;
            }
        }

        //DELETE
        public void DeleteFilm(Film deletedFilm)
        {
            foreach(Actor tActor in _allActors)
            {
                foreach(Film tFilm in tActor.Films)
                {
                    if(deletedFilm.Equals(tFilm))
                    {
                        tActor.Films.Remove(tFilm);
                    }
                }
            }
            deletedFilm.Director?.Films.Remove(deletedFilm);
            _allFilms.Remove(deletedFilm);
        }

        public void DeleteActor(Actor deletedActor)
        {
            foreach(Film tFilm in _allFilms)
            {
                foreach(Actor tActor in tFilm.Actors)
                {
                    if(deletedActor.Equals(tActor))
                    {
                        tFilm.Actors.Remove(tActor);
                    }
                }      
            }
            _allActors.Remove(deletedActor);
        }

        public void DeleteDirector(Director deletedDirector)
        {
            foreach(Film tFilm in deletedDirector.Films)
            {
                tFilm.Director = null;
            }
            _allDirectors.Remove(deletedDirector);
        }

        //FIND
        public static MyCollection<Actor> FindActors(MyCollection<Actor> tItems, string byName, string byLastname, int byAge)
        {
            if (byName != "")
            {
                tItems = (from actor in tItems.AsParallel().AsOrdered()
                         where actor.Name.ToLower().Contains(byName.ToLower())
                         select actor).AsMyCollection();
            }

            if (byLastname != "")
            {
                tItems = (from actor in tItems.AsParallel().AsOrdered()
                          where actor.Lastname.ToLower().Contains(byLastname.ToLower())
                          select actor).AsMyCollection();
            }

            if (byAge != 0)
            {
                tItems = (from actor in tItems.AsParallel().AsOrdered()
                          where actor.Age == byAge
                          select actor).AsMyCollection();
            }
            return tItems;
        }

        public static MyCollection<Film> FindFilms(MyCollection<Film> tItems, string byTitle, int byRating, int byReliseYear)
        {   
            if (byTitle != "")
            {
                tItems = (from film in tItems.AsParallel().AsOrdered()
                          where film.Title.ToLower().Contains(byTitle.ToLower())
                          select film).AsMyCollection();
            }

            if (byRating != 0)
            {
                tItems = (from film in tItems.AsParallel().AsOrdered()
                          where film.Rating >= byRating
                          select film).AsMyCollection();
            }

            if (byReliseYear != 0)
            {
                tItems = (from film in tItems.AsParallel().AsOrdered()
                          where film.ReleaseYear == byReliseYear
                          select film).AsMyCollection();
            }
            return tItems;
        }

        public static MyCollection<Director> FindDirectors(MyCollection<Director> tItems, string byName, string byLastname)
        {            
            if (byName != "")
            {
                tItems = (from director in tItems.AsParallel().AsOrdered()
                          where director.Name.ToLower().Contains(byName.ToLower())
                          select director).AsMyCollection();
            }

            if (byLastname != "")
            {
                tItems = (from director in tItems.AsParallel().AsOrdered()
                          where director.Lastname.ToLower().Contains(byLastname.ToLower())
                          select director).AsMyCollection();
            }
            return tItems;
        }

        //SORT
        public static MyCollection<Director> SortDirectorsByName(MyCollection<Director> tItems)
        {
            return (from director in tItems
                    orderby director.Name
                    select director).AsMyCollection();
        }

        public static MyCollection<Director> SortDirectorsByLastname(MyCollection<Director> tItems)
        {
            return (from director in tItems
                    orderby director.Lastname
                    select director).AsMyCollection();
        }

        public static MyCollection<Film> SortFilmsByTitle(MyCollection<Film> tItems)
        {
            return (from film in tItems
                    orderby film.Title
                    select film).AsMyCollection();
        }

        public static MyCollection<Film> SortFilmsByRating(MyCollection<Film> tItems)
        {
            return (from film in tItems
                    orderby film.Rating
                    select film).AsMyCollection();
        }

        public static MyCollection<Film> SortFilmsByReleaseYear(MyCollection<Film> tItems)
        {
            return (from film in tItems
                    orderby film.ReleaseYear
                    select film).AsMyCollection();
        }

        public static MyCollection<Actor> SortActorsByName(MyCollection<Actor> tItems)
        {
            return (from actor in tItems
                    orderby actor.Name
                    select actor).AsMyCollection();
        }

        public static MyCollection<Actor> SortActorsByLastname(MyCollection<Actor> tItems)
        {
            return (from actor in tItems
                    orderby actor.Lastname
                    select actor).AsMyCollection();
        }

        public static MyCollection<Actor> SortActorsByAge(MyCollection<Actor> tItems)
        {
            return (from actor in tItems
                    orderby actor.Age
                    select actor).AsMyCollection();
        }

        //SAVE
        public void SaveFADToFile(string pathToFile)
        {
            using (Stream s = File.Create(pathToFile))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Compress))
                {
                    using (StreamWriter w = new StreamWriter(ds))
                    {
                        w.WriteLine("<Films>");
                        foreach(Film tFilm in _allFilms)
                        {
                            SaveFilm(w, tFilm);
                        }
                        w.WriteLine("</Films>");
                        w.WriteLine("<Actors>");
                        foreach(Actor tActor in _allActors)
                        {
                            SaveActor(w, tActor);
                        }
                        w.WriteLine("</Actors>");
                        w.WriteLine("<Directors>");
                        foreach (Director tDirector in _allDirectors)
                        {
                            SaveDirector(w, tDirector);
                        }
                        w.WriteLine("</Directors>");
                        w.WriteLine("<PairsFilmActor>");
                        foreach (Film tFilm in _allFilms)
                        {
                            foreach(Actor tActor in tFilm.Actors)
                            {
                                SavePair(w, tFilm, tActor);
                            }
                        }
                        w.WriteLine("</PairsFilmActor>");
                        w.WriteLine("<PairsFilmDirector>");
                        foreach (Director tDirector in _allDirectors)
                        {
                            foreach(Film tFilm in tDirector.Films)
                            {
                                SavePair(w, tFilm, tDirector);
                            }
                        }
                        w.WriteLine("</PairsFilmDirector>");
                    }
                }
            }
        }

        private void SaveFilm(TextWriter w, Film tFilm)
        {
            w.WriteLine("<Film>");
            w.WriteLine(tFilm.Title);
            w.WriteLine(tFilm.ReleaseYear);
            w.WriteLine(tFilm.Rating);
            if(tFilm.Image != null)
            {
                w.WriteLine(tFilm.Image);
            }
            else
            {
                w.WriteLine("null");
            }
            foreach(Genre tGenre in tFilm.Genres)
            {
                w.WriteLine(tGenre.Name);
                w.WriteLine(tGenre.Checked);
            }
            w.WriteLine("</Film>");
        }

        private void SaveActor(TextWriter w, Actor tActor)
        {
            w.WriteLine("<Actor>");
            w.WriteLine(tActor.Name);
            w.WriteLine(tActor.Lastname);
            w.WriteLine(tActor.Age);
            w.WriteLine(tActor.Biography);
            if(tActor.Image != null)
            {
                w.WriteLine(tActor.Image);
            }
            else
            {
                w.WriteLine("null");
            }
            w.WriteLine("</Actor>");
        }

        private void SaveDirector(TextWriter w, Director tDirector)
        {
            w.WriteLine("<Director>");
            w.WriteLine(tDirector.Name);
            w.WriteLine(tDirector.Lastname);
            if(tDirector.Image != null)
            {
                w.WriteLine(tDirector.Image);
            }
            else
            {
                w.WriteLine("null");
            }
            w.WriteLine("</Director>");
        }
            
        private void SavePair<T,U>(TextWriter w, T firstItem, U secondItem)
        {
            w.WriteLine("<Pair>");
            w.WriteLine(firstItem.ToString());
            w.WriteLine(secondItem.ToString());
            w.WriteLine("</Pair>");
        }

        //LOAD
        public void LoadFADFromFile(string pathToFile)
        {
            using (Stream s = File.OpenRead(pathToFile))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Decompress))
                {
                    using (StreamReader r = new StreamReader(ds))
                    {
                        while(!r.EndOfStream)
                        {
                            string currentStr;
                            currentStr = r.ReadLine();
                            if(currentStr == "<Films>")
                            {
                                currentStr = r.ReadLine();
                                while(currentStr != "</Films>")
                                {
                                    if(currentStr == "<Film>")
                                    {
                                        LoadFilm(r);
                                    }
                                    currentStr = r.ReadLine();
                                }
                            }
                            else if (currentStr == "<Actors>")
                            {
                                currentStr = r.ReadLine();
                                while(currentStr != "</Actors>")
                                {
                                    if(currentStr == "<Actor>")
                                    {
                                        LoadActor(r);
                                    }
                                    currentStr = r.ReadLine();
                                }
                            }
                            else if (currentStr == "<Directors>")
                            {
                                currentStr = r.ReadLine();
                                while(currentStr != "</Directors>")
                                {
                                    if(currentStr == "<Director>")
                                    {
                                        LoadDirector(r);
                                    }
                                    currentStr = r.ReadLine();
                                }
                            }
                            else if (currentStr == "<PairsFilmActor>")
                            {
                                currentStr = r.ReadLine();
                                while (currentStr != "</PairsFilmActor>")
                                {
                                    if (currentStr == "<Pair>")
                                    {
                                        LoadPair(r, _allFilms, _allActors);
                                    }
                                    currentStr = r.ReadLine();
                                }
                            }
                            else if (currentStr == "<PairsFilmDirector>")
                            {
                                currentStr = r.ReadLine();
                                while (currentStr != "</PairsFilmDirector>")
                                {
                                    if (currentStr == "<Pair>")
                                    {
                                        LoadPair(r, _allFilms, _allDirectors);
                                    }
                                    currentStr = r.ReadLine();
                                }
                            }
                        }
                    }
                }
            }

        }

        private void LoadFilm(StreamReader r)
        {
            string title = r.ReadLine();
            int releaseYear = Int32.Parse(r.ReadLine());
            int rating = Int32.Parse(r.ReadLine());
            string image = r.ReadLine();
            if (image == "null")
            {
                image = null;
            }
            Film nFilm = new Film(title, rating, releaseYear, null, image);
            string genreName;
            while ((genreName = r.ReadLine()) != "</Film>")
            {
                Boolean.TryParse(r.ReadLine(), out bool genreChecked);
                foreach (Genre tGenre in nFilm.Genres)
                {
                    if (tGenre.Name == genreName)
                    {
                        tGenre.Checked = genreChecked;
                    }
                }
            }
            _allFilms.Add(nFilm);
        }

        private void LoadActor(StreamReader r)
        {
            string name = r.ReadLine();
            string lastname = r.ReadLine();
            int age = Int32.Parse(r.ReadLine());
            string biography = r.ReadLine();
            string image = r.ReadLine();
            if (image == "null")
            {
                image = null;
            }
            Actor nActor = new Actor(name, lastname, age, image)
            {
                Biography = biography
            };
            _allActors.Add(nActor);
        }

        private void LoadDirector(StreamReader r)
        {
            string name = r.ReadLine();
            string lastname = r.ReadLine();
            string image = r.ReadLine();
            if (image == "null")
            {
                image = null;
            }
            Director nDirector = new Director(name, lastname, image);
            _allDirectors.Add(nDirector);
        }

        private void LoadPair<T,U>(StreamReader r, MyCollection<T> first, MyCollection<U> second) 
            where T: class, IEquatable<T> where U: class, IEquatable<U>
        {
            T objFirst = null;
            U objSecond = null;
            string sFirst = r.ReadLine();
            string sSecond = r.ReadLine();
            foreach(T itemInFirst in first)
            {
                if(itemInFirst.ToString() == sFirst)
                {
                    objFirst = itemInFirst;
                    break;
                }
            }
            if (objFirst == null) return;
            foreach(U itemInSecond in second)
            {
                if(itemInSecond.ToString() == sSecond)
                {
                    objSecond = itemInSecond;
                    break;
                }
            }
            if(objSecond != null)
            {
                MakePair(objFirst, objSecond);
            }         
        }

        private void MakePair(object first, object second)
        {
            if(!(first is Film))
            {
                throw new Exception("Loading error: incorrect pair");
            }
            else
            {
                if(second is Actor)
                {
                    MakePair(first as Film, second as Actor);
                }
                else if(second is Director)
                {
                    MakePair(first as Film, second as Director);
                }
                else
                {
                    throw new Exception("Loading error: incorrect pair");
                }
            }
        }

        private void MakePair(Film tFilm, Actor tActor)
        {
            tFilm.Actors.Add(tActor);
            tActor.Films.Add(tFilm);
        }

        private void MakePair(Film tFilm, Director tDirector)
        {
            tFilm.Director = tDirector;
            tDirector.Films.Add(tFilm);
        }

        //OPERATORS
        public static FAD operator +(FAD first, FAD second)
        {
            FAD nFAD = new FAD();
            nFAD._allActors.AddRange(first._allActors);
            nFAD._allActors.AddRange(second._allActors);
            nFAD._allDirectors.AddRange(first._allDirectors);
            nFAD._allDirectors.AddRange(second._allDirectors);
            nFAD._allFilms.AddRange(first._allFilms);
            nFAD._allFilms.AddRange(second._allFilms);
            return nFAD;
        }
    }
}
