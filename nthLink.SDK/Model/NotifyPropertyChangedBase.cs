using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace nthLink.SDK.Model
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        protected bool SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if (!CheckIsChanged(oldValue, newValue))
            {
                return false;
            }

            if (OnPropertyChanging(propertyName, oldValue, newValue))
            {
                RaisePropertyChanging(propertyName);

                oldValue = newValue;

                OnPropertyChanged(propertyName);

                RaisePropertyChanged(propertyName);

                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnPropertyChanging(string name, object? oldValue, object? newValue)
        {
            return true;
        }
        protected virtual void OnPropertyChanged(string name)
        {

        }

        protected bool CheckIsChanged<T>(T? oldValue, T? newValue)
        {
            return !EqualityComparer<T>.Default.Equals(oldValue, newValue);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, propertyChangedEventArgs);
            }
        }

        protected void RaisePropertyChanging([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanging != null)
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    PropertyChanging.Invoke(this, new PropertyChangingEventArgs(propertyName));
                }
            }
        }

        protected void RaisePropertyChanging(PropertyChangingEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging.Invoke(this, propertyChangedEventArgs);
            }
        }
    }
}
