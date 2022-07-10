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
    class InfoViewModel : INotifyPropertyChanged
    {
        public InfoViewModel()
        {
            _animeList = new ObservableCollection<Anime>();

        }

        private ObservableCollection<Anime> _animeList;
        public ObservableCollection<Anime> AnimeList
        {
            get { return _animeList; }
            set
            {
                _animeList = value;

                OnPropertyChanged();
            }
        }

        private Anime _selectedAnime;
        public Anime SelectedAnime
        {
            get { return _selectedAnime; }
            set
            {
                _selectedAnime = value;
                OnPropertyChanged();
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

        public void AddToCombo()
        {
            AnimeList?.Clear();

            XDocument doc = null;
            if (File.Exists(filepath))
                doc = XDocument.Load(filepath);

            if (doc == null) return;

            XAttribute attribute = null;

            foreach (var item in doc.Root.Elements())
            {
                _animeList.Add(Anime.FromXml(item.CreateReader()));
            }

            OnPropertyChanged(nameof(AnimeList));
            SelectedAnime = AnimeList.FirstOrDefault();
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
