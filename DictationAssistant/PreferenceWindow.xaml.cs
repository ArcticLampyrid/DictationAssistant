using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WK.Libraries.BetterFolderBrowserNS;

namespace DictationAssistant
{
    /// <summary>
    /// PreferenceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PreferenceWindow : Window
    {
        public PreferenceViewModel ViewModel { get; } = new PreferenceViewModel();

        public PreferenceWindow()
        {
            InitializeComponent();
            MainStackPanel.DataContext = ViewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
            }
            catch (InvalidOperationException)
            {
            }
            this.Close();
        }

        private void SetPathForImprovedResourceButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BetterFolderBrowser()
            {
                RootFolder = ViewModel.PathForImprovedResource
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel.PathForImprovedResource = dialog.SelectedPath;
            }
        }
    }
}
