using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MatrixMemory.Models;
using MatrixMemory.ViewModels;

namespace MatrixMemory.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _view;
        private readonly Matrix _gameMatrix;
        private StackPanel _lastPanel;
        private StackPanel _currentPanel;
        private const int MaxFailures = 20;

        public MainWindow()
        {
            InitializeComponent();
            _gameMatrix = new Matrix(2, 200 , MaxFailures);
            MatrixHolder.Child = _gameMatrix;
            _gameMatrix.Win += delegate
            {
                _view!.EndedRound = true;
                LevelEndText.Text = "You have won!";
            };
            _gameMatrix.OverFailuresLimit += delegate
            {
                _view!.EndedRound = true;
                LevelEndText.Text = "Sorry, but you have lost:(";
            };
            
            _gameMatrix.GameEnded += delegate(object? sender, EventArgs args)
            {
                _view!.TryAddPlayerScore(_gameMatrix.Score);
                LevelEndText.Text = $"Your score is {_gameMatrix.Score}";
            };
            _lastPanel = StartMenu;
            _currentPanel = StartMenu;
        }
        
        private void OnActivated(object? sender, EventArgs e)
        {
            _view = DataContext as MainWindowViewModel ?? throw new InvalidOperationException();
        }

        private void StartGameButton(object? sender, RoutedEventArgs e)
        {
            StartMenu.IsVisible = false;
            MainGame.IsVisible = true;

            _lastPanel = StartMenu;
            _currentPanel = MainGame;
    
            _gameMatrix.ShowTiles(1);
        }

        private void Restart(object? sender, RoutedEventArgs e)
        {
            _gameMatrix.Restart(1);
            _view!.EndedRound = false;
        }

        private void NextLevel(object? sender, RoutedEventArgs e)
        {
            _view!.EndedRound = false;
            _gameMatrix.NextLevel(1);
        }
        
        private void FromGameToMenu(object? sender, RoutedEventArgs e)
        {
            _view!.EndedRound = false;

            MainGame.IsVisible = false;
            StartMenu.IsVisible = true;

            _lastPanel = MainGame;
            _currentPanel = StartMenu;
            
            _gameMatrix.EndGame();
        }

        private void Registration_OnClick(object? sender, RoutedEventArgs e)
        {
            _lastPanel = _currentPanel;
            _currentPanel.IsVisible = false;

            _currentPanel = Registration;
            _currentPanel.IsVisible = true;
        }

        private void LogIn_OnClick(object? sender, RoutedEventArgs e)
        {
            _lastPanel = _currentPanel;
            _currentPanel.IsVisible = false;

            _currentPanel = Identification;
            _currentPanel.IsVisible = true;
        }

        private async void Register(object? sender, RoutedEventArgs e)
        {
            if (UserName.Text == "" || Password.Text == "")
            {
                RegErrorText.Text = "Invalid username or password";
                return;
            }
            var player = new Player(UserName.Text, PlayerData.EncryptPassword(Password.Text));

            try
            {
                await PlayerData.AddPlayer(player);
            }
            catch (ArgumentException)
            {
                RegErrorText.Text = "Such user already exists";
                return;
            }

            await _view!.TrySetPlayer(player);
            Password.Text = null;
            UserName.Text = null;
            Back(sender, e);
        }

        private void Back(object? sender, RoutedEventArgs e)
        {
            if (Equals(_lastPanel, MainGame))
            {
                _currentPanel.IsVisible = false;
                MainGame.IsVisible = true;
                _currentPanel = MainGame;
                _lastPanel = StartMenu;
                return;
            }

            if ((Equals(_lastPanel, Identification) && Equals(_currentPanel, Registration)) || 
                (Equals(_currentPanel, Identification) && Equals(_lastPanel, Registration)) ||
                Equals(_currentPanel, Identification) || Equals(_currentPanel, Registration))
            {
                _currentPanel.IsVisible = false;
                StartMenu.IsVisible = true;
                _currentPanel = StartMenu;
                _lastPanel = StartMenu;
            }
            else
            {
                _currentPanel.IsVisible = false;
                _lastPanel.IsVisible = true;
                (_lastPanel, _currentPanel) = (_currentPanel, _lastPanel);
            }
            
        }

        private void ShowPassword(object? sender, RoutedEventArgs e)
        {
            if ((sender as Button)!.Name == "PasswordBtnLogIn")
            {
                PasswordLogIn.PasswordChar = char.MinValue;
            }
            else if ((sender as Button)!.Name == "PasswordBtnReg")
            {
                Password.PasswordChar = char.MinValue;
            }
        }
        
        private void HidePassword(object? sender, RoutedEventArgs e)
        {
            if ((sender as Button)!.Name == "PasswordBtnLogIn")
            {
                PasswordLogIn.PasswordChar = '*';
            }
            else if ((sender as Button)!.Name == "PasswordBtnReg")
            {
                Password.PasswordChar = '*';
            }
        }

        private async void LogIn(object? sender, RoutedEventArgs e)
        {
            if (UserNameLogIn.Text == "" || PasswordLogIn.Text == null)
            {
                SignInErrorText.Text = "Invalid username or password";
                DispatcherTimer.RunOnce(() => SignInErrorText.Text = string.Empty, TimeSpan.FromSeconds(3));
                return;
            }

            try
            {
                await _view!.TrySetPlayer(new Player(UserNameLogIn.Text, PlayerData.EncryptPassword(PasswordLogIn.Text)));
            }
            catch (Exception exception)
            {
                SignInErrorText.Text = exception.Message;
                Console.WriteLine(exception);
                return;
            }

            Back(sender, e);
            PasswordLogIn.Text = null;
            UserNameLogIn.Text = null;
        }

        private async void LogOut_OnClick(object? sender, RoutedEventArgs e)
        {
            _currentPanel.IsVisible = false;
            StartMenu.IsVisible = true;
            _lastPanel = StartMenu;
            _currentPanel = StartMenu;
            await _view!.TrySetPlayer(null);
        }
        
        private void ShowResults(object? sender, RoutedEventArgs e)
        {
            if (!_view!.LoggedIn)
            {
                ResultsProblem.Text = "Log in firstly";
                DispatcherTimer.RunOnce(() => ResultsProblem.Text = string.Empty, TimeSpan.FromSeconds(3));
                return;
            }

            if (_view.PlayerStats == null || _view.PlayerStats.Count == 0)
            {
                ResultsProblem.Text = "You haven`t played any games yet";
                DispatcherTimer.RunOnce(() => ResultsProblem.Text = string.Empty, TimeSpan.FromSeconds(3));
                return;
            }
            _lastPanel = StartMenu;
            _currentPanel = Statistics;

            _lastPanel.IsVisible = false;
            _currentPanel.IsVisible = true;
        }

        private void SaveGame(object? sender, RoutedEventArgs e)
        {
            if (_view!.CurrentPlayer == null)
            {
                return;
            }

            //_view!.CurrentPlayer.LastGame = _gameMatrix.SaveGame();
            
            
        }

        
    }
}