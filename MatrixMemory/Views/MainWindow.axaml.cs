using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MatrixMemory.Models;
using MatrixMemory.ViewModels;
using Timer = System.Timers.Timer;

namespace MatrixMemory.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _view;
        private Matrix _gameMatrix;

        public MainWindow()
        {
            InitializeComponent();
            _gameMatrix = new Matrix(3, 20 );
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

        private void Restart_OnClick(object? sender, RoutedEventArgs e)
        {
            _view.Won = false;
            _gameMatrix.Restart(1);
        }
    }
}