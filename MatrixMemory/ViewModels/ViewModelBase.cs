using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using MatrixMemory.Models;

namespace MatrixMemory.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _startMenu = true;
        private bool _registration;
        private bool _mainGame;
        private bool _registered;
        private bool _won;
        private Player? _currentUser;

        public bool StartMenu 
        { 
            get => _startMenu;
            set => this.RaiseAndSetIfChanged(ref _startMenu, value);
        }

        public bool Registration
        {
            get => _registration;
            set => this.RaiseAndSetIfChanged(ref _registration, value);
        }

        public bool MainGame
        {
            get => _mainGame;
            set => this.RaiseAndSetIfChanged(ref _mainGame, value);
        }
        public bool Registered
        {
            get => _registered;
            set => this.RaiseAndSetIfChanged(ref _registered, value);
        }

        public bool Won
        {
            get => _won;
            set => this.RaiseAndSetIfChanged(ref _won, value);
        }

        public Player? CurrentPlayer
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                _registered = value != null;
            }
        }
    }
}