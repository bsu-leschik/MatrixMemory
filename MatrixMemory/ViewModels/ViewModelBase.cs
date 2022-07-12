using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;

namespace MatrixMemory.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _startMenu = true;
        private bool _registration;
        private bool _mainGame;
        private bool _registered;

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
    }
}