using System.Windows;
using Microsoft.Win32;

namespace AmazerrApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png)|*.PNG|All files (*.*)|*.*"
            };

            var result = openFileDialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                Swipes.Text = openFileDialog.FileName;
            }
        }
    }
}
