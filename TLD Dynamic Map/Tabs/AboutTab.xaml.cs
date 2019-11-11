using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace TLD_Dynamic_Map.Tabs
{
    public partial class AboutTab : UserControl
    {
        public AboutTab()
        {
            InitializeComponent();
        }

        private void ViewOnGithubClicked(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/FINDarkside/TLD-Save-Editor");
        }
    }
}
