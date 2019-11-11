using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using TLD_Dynamic_Map.Helpers.Helpers;

namespace TLD_Dynamic_Map.ViewModels
{
    public class SaveFileUpdatedViewModel
    {
        public ICommand RefreshCommand { get; set; }

        public SaveFileUpdatedViewModel()
        {
            RefreshCommand = new CommandHandler(() =>
            {
                MainWindow.Instance.CurrentSaveSelectionChanged(null, null);
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
    }
}
