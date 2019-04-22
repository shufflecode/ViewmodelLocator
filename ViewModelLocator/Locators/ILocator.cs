using ViewModelLocator.ViewModels;

namespace ViewModelLocator.Locators
{
    public interface ILocator
    {        
        void Register(string name, object o);
        object Locate(string name);
        object this[string name] { get; }
    }
}
