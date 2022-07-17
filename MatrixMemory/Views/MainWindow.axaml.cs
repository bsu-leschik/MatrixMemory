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
        private readonly Matrix _gameMatrix;
        private const int MaxFailures = 5;

        public MainWindow()
        {
            InitializeComponent();
            _gameMatrix = new Matrix(2, 20 );
            MatrixHolder.Child = _gameMatrix;
            
        }

        private void StartGameButton(object? sender, RoutedEventArgs e)
        { 
            _view ??= DataContext as MainWindowViewModel ?? throw new InvalidOperationException();
            
            _gameMatrix.Win += delegate(object? sender, EventArgs args) { _view.Won = true; };
            
            _view.StartMenu = false;
            _view.MainGame = true;
            
            _gameMatrix.ShowCards(1);
        }

        private void Restart(object? sender, RoutedEventArgs e)
        {
            _gameMatrix.Restart(1, _view.Won);
            _view.Won = false;
        }

        private void NextLevel(object? sender, RoutedEventArgs e)
        {
            _view.Won = false;
            _gameMatrix.Restart(1, _gameMatrix.Failures < MaxFailures);
        }
    }
}