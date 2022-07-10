using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
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

        public static Anime FromXml(XmlReader element)
        {
            Anime anime = new Anime();

            element.MoveToContent();
            while (element.MoveToNextAttribute())
            {
                if (element.NodeType == XmlNodeType.Attribute)
                {
                    switch (element.Name)
                    {
                        case "name":
                            anime.AnimeName = element.Value;
                            break;
                        case "seasons":
                            anime.Seasons = Convert.ToInt32(element.Value);
                            break;
                        case "desc":
                            anime.Description = element.Value;
                            break;
                        case "path":
                            anime.ImagePath = element.Value;
                            break;
                        case "link":
                            anime.Link = element.Value;
                            break;
                        case "color":
                            Enum.TryParse(element.Value, out Color color);
                            anime.ColorPick = color;
                            break;

                    }
                }

            }
            return anime;
        }
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
            ButtonText = "Info";                    
            AddPanel = true;            
            EditPanel = false;
            InitializeCommands();
        }

        public bool isEdit = false;

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

        private bool _addPanel;
        public bool AddPanel
        {
            get { return _addPanel; }
            set
            {
                _addPanel = value;
                OnPropertyChanged();
            }
        }
                
        private bool _editPanel;
        public bool EditPanel
        {
            get { return _editPanel; }
            set
            {
                _editPanel = value;
                OnPropertyChanged();
            }
        }

        public MVVM.GridViewModel GridViewModel { get; set; }
        public MVVM.InfoViewModel InfoViewModel { get; set; }

        public DelegateCommand<object> SwitchCommand { get; private set; }
        protected virtual void InitializeCommands()
        {
            SwitchCommand = new DelegateCommand<object>(OnSwitch);
        }
        protected virtual void OnSwitch(object obj)
        {
            if (isEdit){
                ButtonText = "Info";
                AddPanel = true;
                EditPanel = false;
                isEdit = false;
                
            }
            else{
                
                InfoViewModel.AddToCombo();
                ButtonText = "Grid";
                AddPanel = false;
                EditPanel = true;
                isEdit = true;
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
