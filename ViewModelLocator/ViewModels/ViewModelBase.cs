using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModelLocator.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}