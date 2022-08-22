using System.Collections;
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

        public ArrayList GameNumbers
        {
            get
            {
                var list = new ArrayList { "Last Game" };

                for (var i = 1; i < _currentUser!.Statistics!.Count; i++)
                {
                    list.Add(i);
                }

                return list;
            }
        }

        public ArrayList GameScores
        {
            get => _currentUser!.Statistics!;
        }
    }
}