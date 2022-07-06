using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Linq;

namespace AnimeInformation.MVVM
{
    class GridViewModel : INotifyPropertyChanged
    {       
        public GridViewModel()
        {
            InitializeCommands();
            DataGrid = new ObservableCollection<Anime>();
            LoadFromXml();
        }

        private ObservableCollection<Anime> _dataGrid;
        public ObservableCollection<Anime> DataGrid
        {
            get { return _dataGrid; }
            set
            {
                _dataGrid = value;
                OnPropertyChanged();
            }
        }

        private Anime _selectedGrid;
        public Anime SelectedGrid
        {
            get { return _selectedGrid; }
            set
            {
                _selectedGrid = value;
                OnPropertyChanged();
            }
        }

        string filepath = Directory.GetCurrentDirectory() + "\\Save.xml";


        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> DeleteCommand { get; private set; }

        protected virtual void InitializeCommands()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            DeleteCommand = new DelegateCommand<object>(OnDelete);
        }

        protected virtual void OnSave(object obj)
        {
            Save();
        }
        protected virtual void OnDelete(object obj)
        {
           Delete();
        }

        public void LoadFromXml()
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

                attribute = item.Attribute("seasons");
                if (attribute != null)
                    anime.Seasons = Convert.ToInt32(attribute.Value);

                attribute = item.Attribute("desc");
                if (attribute != null)
                    anime.Description = attribute.Value;

                attribute = item.Attribute("path");
                if (attribute != null)
                    anime.ImagePath = attribute.Value;

                attribute = item.Attribute("link");
                if (attribute != null)
                    anime.Link = attribute.Value;

                attribute = item.Attribute("color");
                if (attribute != null)
                {
                    Enum.TryParse(attribute.Value, out Color color);
                    anime.ColorPick = color;
                }

                DataGrid.Add(anime);

            }
        }

        public void SaveToXml(string name, int seasons, string desc, string path, string link, string color)
        {
            XDocument doc = null;

            if (File.Exists(filepath))
                doc = XDocument.Load(filepath);
            else
                doc = new XDocument(new XElement("animes"));

            var target = doc.Root.Descendants("anime").FirstOrDefault(x => x.Attribute("name").Value == name);

            if (target == null)
            {
                XElement root = new XElement("anime");
                root.Add(new XAttribute("name", name));
                root.Add(new XAttribute("seasons", seasons));
                root.Add(new XAttribute("desc", desc));
                root.Add(new XAttribute("path", path));
                root.Add(new XAttribute("link", link));
                root.Add(new XAttribute("color", color));
                doc.Element("animes").Add(root);
            }
            else
            {
                if (target.Attribute("name").Value != name)
                    target.Attribute("name").Value = name;
                else if (target.Attribute("seasons").Value != seasons.ToString())
                    target.Attribute("seasons").Value = seasons.ToString();
                else if (target.Attribute("desc").Value != desc)
                    target.Attribute("desc").Value = desc;
                else if (target.Attribute("path").Value != path)
                    target.Attribute("path").Value = path;
                else if (target.Attribute("link").Value != link)
                    target.Attribute("link").Value = link;
                else if (target.Attribute("color").Value != color)
                    target.Attribute("color").Value = color;
            }
            doc.Save(filepath);
        }

        public void Delete()
        {
            if (SelectedGrid != null)
            {
                var doc = XDocument.Load(filepath);
                var target = doc.Root.Descendants("anime").FirstOrDefault(x => x.Attribute("name").Value == SelectedGrid.AnimeName.ToString());
                target.Remove();
                int index = DataGrid.IndexOf(SelectedGrid);
                DataGrid.RemoveAt(index);
                doc.Save(filepath);
            }
            else
                MessageBox.Show("Please select a animal!");
        }

        public void Save()
        {
            foreach (var item in DataGrid)
            {
                if (item.AnimeName == null || item.Seasons == null || item.Description == null || item.ImagePath == null || item.Link == null || item.ColorPick == null)
                {
                    MessageBox.Show("Please complete every cell form the table!");
                }
                else
                {
                    SaveToXml(item.AnimeName, item.Seasons, item.Description, item.ImagePath, item.Link, item.ColorPick.ToString());
                }

            }
            MessageBox.Show("The table has been saved!");
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


    
