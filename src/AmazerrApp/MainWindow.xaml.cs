using System.IO;
using System.Text;
using System.Windows;
using Amazerrr;
using Microsoft.Win32;

namespace AmazerrApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            Swipes.Text = string.Empty;
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png)|*.PNG|All files (*.*)|*.*"
            };

            var result = openFileDialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                var imageData = await File.ReadAllBytesAsync(openFileDialog.FileName);
                
                var imageAnalyzer = new ImageAnalyzer();
                var input = imageAnalyzer.Analyze(imageData);

                var parser = new Parser();
                var board = parser.Parse(input);

                var solver = new Solver();
                var output = solver.Solve(board);

                var sb = new StringBuilder();
                foreach (var item in output)
                {
                    sb.AppendLine(item.ToString());
                }

                Swipes.Text = sb.ToString();
            }
        }
    }
}
