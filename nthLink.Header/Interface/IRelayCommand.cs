using System.Windows.Input;

namespace nthLink.Header.Interface
{
    public interface IRelayCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
