using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using MatrixMemory.Models;

namespace MatrixMemory.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _loggedIn;
        private bool _endedRound;
        private Player? _currentUser;

        public bool LoggedIn
        {
            get => _loggedIn;
            set => this.RaiseAndSetIfChanged(ref _loggedIn, value);
        }

        public bool EndedRound
        {
            get => _endedRound;
            set => this.RaiseAndSetIfChanged(ref _endedRound, value);
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