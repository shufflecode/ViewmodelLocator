using System.Collections.Generic;
using ViewModelLocator.Locators;

namespace ViewModelLocator.ViewModels
{
    [ViewModel("MainWindow", designDataType:typeof(MainWindowDesignData))]
    internal class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
          
            WelcomeMessage = "initialized during Runtime";
            Items = new List<string> { "Marc", "Peter", "Paul" ,"Alice", "Bob"};
                        
        }

        private string _welcomeMessage;

        public string WelcomeMessage
        {
            get => _welcomeMessage;

            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        private List<string> _items;

        public List<string> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }

    internal class MainWindowDesignData : ViewModelBase, IMainWindowViewModel
    {
        public MainWindowDesignData()
        {
            WelcomeMessage = "initialized during DesignTime via XDescProc.exe";
            Items = new List<string> { "Foo", "Bar ", "Baz" , "Boo", "Lorem Ipsum"};
        }

        private string _welcomeMessage;

        public string WelcomeMessage
        {
            get => _welcomeMessage;

            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        private List<string> _items;

        public List<string> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}
