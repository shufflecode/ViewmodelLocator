using System.Collections.Generic;

namespace ViewModelLocator.ViewModels
{
    internal interface IMainWindowViewModel
    {
        List<string> Items { get; set; }
        string WelcomeMessage { get; set; }
    }
}