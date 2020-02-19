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

namespace DictationAssistant
{
    /// <summary>
    /// SaveAudioDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SaveAudioDialog : Window
    {
        public SaveAudioViewModel ViewModel { get; } = new SaveAudioViewModel();

        public SaveAudioDialog()
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

        private void SetTargetPath_Click(object sender, RoutedEventArgs e)
        {
            var name = ViewModel.EncoderInfo?.Name;
            var extension = ViewModel.EncoderInfo?.Extension;
            var dialog = new System.Windows.Forms.SaveFileDialog
            {
                DefaultExt = ViewModel.EncoderInfo?.Extension,
                Filter = $"{name}(*.{extension})|*.{extension}|All files(*.*)|*.*"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel.TargetPath = dialog.FileName;
            }
        }

        private void EncoderInfoCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.TargetPath = System.IO.Path.ChangeExtension(ViewModel.TargetPath, ViewModel.EncoderInfo.Extension);
        }
    }
}
