using System.Windows.Input;

namespace railway_monitor.Bases {
    public class RelayCommand : ICommand {
        public Action ExecuteDelegate { get; set; } = () => { };

        public RelayCommand(Action executeDelegate) {
            ExecuteDelegate = executeDelegate;
        }

        public event EventHandler? CanExecuteChanged;
        public virtual bool CanExecute(object? parameter) {
            return true;
        }

        public virtual void Execute(object? parameter) {
            ExecuteDelegate.Invoke();
        }

        protected void OnCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
