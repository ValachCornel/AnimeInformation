using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnimeInformation.MVVM
{
    class InfoViewModel
    {
        public InfoViewModel()
        {
            AnimeList = new ObservableCollection<string>();
            AddToCombo();
        }

        private ObservableCollection<string> _animeList;
        public ObservableCollection<string> AnimeList
        {
            get { return _animeList; }
            set
            {
                _animeList = value;

                OnPropertyChanged();
            }
        }

        private string _selectedAnime;
        public string SelectedAnime
        {
            get { return _selectedAnime; }
            set
            {
                _selectedAnime = value;                
                OnPropertyChanged();
                OnComboBoxChanged();
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private int _seasons;
        public int Seasons
        {
            get { return _seasons; }
            set
            {
                _seasons = value;
                OnPropertyChanged();
            }
        }

        private string _link;
        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged();
            }
        }
        string filepath = Directory.GetCurrentDirectory() + "\\Save.xml";
        protected virtual void OnComboBoxChanged()
        {
            XDocument doc = null;
            if (File.Exists(filepath))
                doc = XDocument.Load(filepath);

            if (doc == null) return;

            XAttribute attribute = null;


            foreach (var item in doc.Root.Elements())
            {
                if (item.Attribute("name").Value == SelectedAnime)
                {
                    var anime = new Anime();

                    attribute = item.Attribute("path");
                    if (attribute != null)
                        anime.ImagePath = attribute.Value;

                    attribute = item.Attribute("desc");
                    if (attribute != null)
                        anime.Description = attribute.Value;

                    attribute = item.Attribute("seasons");
                    if (attribute != null)
                        anime.Seasons = Convert.ToInt32(attribute.Value);

                    attribute = item.Attribute("link");
                    if (attribute != null)
                        anime.Link = attribute.Value;

                    ImagePath = anime.ImagePath;
                    Description = anime.Description;
                    Seasons = anime.Seasons;
                    Link = anime.Link;
                }
            }
        }


        private void AddToCombo()
        {
            MainViewModel mainViewModel = new MainViewModel();
            if (!mainViewModel.isEdit)
            {
                XDocument doc = null;
                if (File.Exists(filepath))
                    doc = XDocument.Load(filepath);

                if (doc == null) return;

                XAttribute attribute = null;

                foreach (var item in doc.Root.Elements())
                {
                    var anime = new Anime();
                    attribute = item.Attribute("name");
                    if (attribute != null)
                        anime.AnimeName = attribute.Value;

                    AnimeList.Add(anime.AnimeName);
                }

                SelectedAnime = AnimeList[0];
            }
            else
            {
                AnimeList.Clear();
            }
            
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
