using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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

        private Brush _textColor;
        public Brush TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
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
            string color = SelectedAnime.ColorPick.ToString();
            SolidColorBrush brush = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            TextColor = brush;
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
