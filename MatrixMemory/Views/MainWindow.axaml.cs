using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MatrixMemory.Models;
using MatrixMemory.ViewModels;

namespace MatrixMemory.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _view;
        private Matrix _gameMatrix;

        public MainWindow()
        {
            InitializeComponent();
            _gameMatrix = new Matrix(3, 50 );
            MatrixHolder.Child = _gameMatrix;
        }

        private void StartGameButton(object? sender, RoutedEventArgs e)
        { 
            _view ??= DataContext as MainWindowViewModel ?? throw new InvalidOperationException();
            
            _view.StartMenu = false;
            _view.MainGame = true;
        }
    }
}