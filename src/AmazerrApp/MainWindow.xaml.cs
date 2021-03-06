﻿using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Amazerrr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
                var sb = new StringBuilder();
                var imageData = await File.ReadAllBytesAsync(openFileDialog.FileName);
                var imageAnalyzer = new ImageAnalyzer(NullLogger.Instance);
                var input = imageAnalyzer.Analyze(imageData);

                sb.AppendLine("Board:");
                sb.AppendLine(input);
                sb.AppendLine();

                var parser = new Parser(NullLogger.Instance);
                var board = parser.Parse(input);

                var solver = new Solver(NullLogger.Instance);
                var output = solver.Solve(board);

                sb.AppendLine($"Solution in {output.Count} moves:");
                foreach (var item in output)
                {
                    sb.AppendLine(item.ToString());
                }

                Swipes.Text = sb.ToString();

                BoardImage.Source = new BitmapImage(new Uri($"file://{openFileDialog.FileName}"));
            }
        }
    }
}
