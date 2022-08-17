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
        private const int MaxFailures = 5;

        public MainWindow()
        {
            InitializeComponent();
            _gameMatrix = new Matrix(2, 20 );
            MatrixHolder.Child = _gameMatrix;
            _gameMatrix.Win += delegate(object? sender, EventArgs args) { _view.Won = true; };

        }

        private void StartGameButton(object? sender, RoutedEventArgs e)
        { 
            _view ??= DataContext as MainWindowViewModel ?? throw new InvalidOperationException();
            
            _view.StartMenu = false;
            _view.MainGame = true;
            
            _gameMatrix.ShowCards(1);
        }

        private void Restart(object? sender, RoutedEventArgs e)
        {
            _gameMatrix.Restart(1);
            _view.Won = false;
        }

        private void NextLevel(object? sender, RoutedEventArgs e)
        {
            _view.Won = false;
            _gameMatrix.NextLevel(1);
        }
        
        private void BackFromGame(object? sender, RoutedEventArgs e)
        {
            _view.Won = false;
            _view.MainGame = false;
            _view.StartMenu = true;
            _gameMatrix.EndGame();
        }

        private void Registration_OnClick(object? sender, RoutedEventArgs e)
        {
            _view ??= DataContext as MainWindowViewModel ?? throw new InvalidOperationException();

            _view.MainGame = false;
            _view.StartMenu = false;
            _view.Registration = true;
        }

        private void SignIn_OnClick(object? sender, RoutedEventArgs e)
        {
        }

        private void Register(object? sender, RoutedEventArgs e)
        {
            
        }

        private void BackFromReg(object? sender, RoutedEventArgs e)
        {
            
        }
    }
}