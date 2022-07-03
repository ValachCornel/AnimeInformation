using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace AnimeInformation.MVVM
{

    public class Anime
    {
        public string AnimeName { get; set; }
        public int Seasons { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Link { get; set; }
        public Color ColorPick { get; set; }
    }

    public enum Color
    {
        [Description("Black")]
        [System.ComponentModel.DataAnnotations.Display(Order = 0)]
        Black = 0,
        [Description("Red")]
        [System.ComponentModel.DataAnnotations.Display(Order = 1)]
        Red = 1,
        [Description("Blue")]
        [System.ComponentModel.DataAnnotations.Display(Order = 2)]
        Blue = 2,
        [Description("Green")]
        [System.ComponentModel.DataAnnotations.Display(Order = 3)]
        Green = 3,
        [Description("Purple")]
        [System.ComponentModel.DataAnnotations.Display(Order = 4)]
        Purple = 4, 
        [Description("Orange")]
        [System.ComponentModel.DataAnnotations.Display(Order = 5)]
        Orange = 5

    }
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            AnimeList = new ObservableCollection<string>();
            DataGrid = new ObservableCollection<Anime>();
            Link = "https://putridparrot.com/blog/the-wpf-hyperlink/";
            ButtonText = "Info";
            LoadFromXml();           
            AddPanel = "Visible";            
            EditPanel = "Hidden";
            InitializeCommands();
        }

        public bool isEdit = false;
        string filepath = Directory.GetCurrentDirectory() + "\\Save.xml";

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
                OnComboBoxChanged();
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

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        private string _addPanel;
        public string AddPanel
        {
            get { return _addPanel; }
            set
            {
                _addPanel = value;
                OnPropertyChanged();
            }
        }
                
        private string _editPanel;
        public string EditPanel
        {
            get { return _editPanel; }
            set
            {
                _editPanel = value;
                OnPropertyChanged();
            }
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
        protected virtual void OnComboBoxChanged()
        {
            XDocument doc = null;
            if (File.Exists(filepath))
                doc = XDocument.Load(filepath);

            if (doc == null) return;

            XAttribute attribute = null;


            foreach (var item in doc.Root.Elements())
            {
                if(item.Attribute("name").Value == SelectedAnime)
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

        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> SwitchCommand { get; private set; }
        public DelegateCommand<object> DeleteCommand { get; private set; }
        protected virtual void InitializeCommands()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SwitchCommand = new DelegateCommand<object>(OnSwitch);
            DeleteCommand = new DelegateCommand<object>(OnDelete);
        }
        protected virtual void OnSave(object obj)
        {
            foreach (var item in DataGrid)
            {
                if(item.AnimeName == null || item.Seasons == null || item.Description == null || item.ImagePath == null || item.Link == null || item.ColorPick == null)
                {
                    MessageBox.Show("Please complete every cell form the table!");
                }
                else{
                    SaveToXml(item.AnimeName, item.Seasons, item.Description, item.ImagePath, item.Link, item.ColorPick.ToString());                   
                }
                
            }
            MessageBox.Show("The table has been saved!");
        }        
        protected virtual void OnSwitch(object obj)
        {
            if (isEdit){
                ButtonText = "Info";
                AddPanel = "Visible";
                EditPanel = "Hidden";
                isEdit = false;
                AnimeList.Clear();
            }
            else{
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
                ButtonText = "Grid";
                AddPanel = "Hidden";
                EditPanel = "Visible";
                isEdit = true;
            }
        }

        protected virtual void OnDelete(object obj)
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

        public void SaveToXml(string name, int seasons, string desc, string path, string link, string color)
        {
            XDocument doc = null;

            if (File.Exists(filepath))
                doc = XDocument.Load(filepath);
            else
                doc = new XDocument(new XElement("animes"));

            var target = doc.Root.Descendants("anime").FirstOrDefault(x => x.Attribute("name").Value == name);

            if(target == null)
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
