using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using MatrixMemory.Models;

namespace MatrixMemory.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _loggedIn;
        private bool _won;
        private Player? _currentUser;
        private bool _identification;

        public bool LoggedIn
        {
            get => _loggedIn;
            set => this.RaiseAndSetIfChanged(ref _loggedIn, value);
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
                LoggedIn = value != null;
            }
        }
    }
}