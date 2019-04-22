using System.Collections.Generic;

namespace ViewModelLocator.ViewModels
{
    public interface IMainWindowViewModel
    {
        List<string> Items { get; set; }
        string WelcomeMessage { get; set; }
    }
}