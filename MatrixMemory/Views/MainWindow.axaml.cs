using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MatrixMemory.ViewModels;

namespace MatrixMemory.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _view;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGameButton(object? sender, RoutedEventArgs e)
        { 
            _view ??= DataContext as MainWindowViewModel ?? throw new InvalidOperationException();

            _view.StartMenu = false;
            _view.MainGame = true;
        }
    }
}