using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    /// <summary>
    /// 簡單命令實作
    /// </summary>
    public class RelayCommand : IRelayCommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action execute;

        private readonly Func<bool>? canExecute;

        public RelayCommand(Action execute) : this(execute, null)
        {

        }
        public RelayCommand(Action execute, Func<bool>? canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object? parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            else
            {
                return this.canExecute();
            }
        }
        public void Execute(object? parameter)
        {
            this.execute.Invoke();
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
    /// <summary>
    /// 簡單命令實作
    /// </summary>
    public class RelayCommand<T> : IRelayCommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<T> execute;
        private readonly Func<bool>? canExecute;
        private readonly Func<T, bool>? canExecuteByParameter;
        public RelayCommand(Action<T> execute)
        {
            this.execute = execute;
        }
        public RelayCommand(Action<T> execute, Func<bool>? canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecuteByParameter)
        {
            this.execute = execute;
            this.canExecuteByParameter = canExecuteByParameter;
        }

        public bool CanExecute(object? parameter)
        {
            bool result = this.canExecute == null ? true : this.canExecute();

            if (this.canExecuteByParameter == null)
            {
                return result;
            }
            else
            {
                if (parameter is T t)
                {
                    return this.canExecuteByParameter(t);
                }
                else
                {
                    return false;
                }
            }
        }
        public void Execute(object? parameter)
        {
            if (parameter is T t)
            {
                this.execute.Invoke(t);
            }
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
