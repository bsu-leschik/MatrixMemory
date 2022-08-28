using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using ReactiveUI;
using MatrixMemory.Models;

namespace MatrixMemory.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _loggedIn;
        private bool _endedRound;
        private Player _currentUser = new(string.Empty, string.Empty);
        private List<KeyValuePair<int, int>>? _scoresList;

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

        public async Task TrySetPlayer(Player? player)
        {
            if (player == null)
            {
                _currentUser = new Player(string.Empty, string.Empty);
                LoggedIn = false;
                return;
            }
            var realPlayer = await PlayerData.ReturnPlayerIfValid(player);
            if (realPlayer != null)
            {
                CurrentPlayer = realPlayer;
                LoggedIn = true;
                GameScores = CalculateScoresList();
            }
            else
            {
                throw new AuthenticationException("Username or Password is wrong");
            }
        }

        public void TryAddPlayerScore(int score)
        {
            if (!_loggedIn)
            {
                return;
            }

            if (_currentUser.Statistics == null)
            {
                PlayerStats = new ArrayList();
            }
            PlayerStats.Add(score);
            PlayerData.SavePlayer(CurrentPlayer);
            GameScores = CalculateScoresList();
        }

        public ArrayList? PlayerStats
        {
            get => _currentUser.Statistics;
            set
            {
                _currentUser.Statistics = value;
                GameScores = CalculateScoresList();
            }
        }

        public Player CurrentPlayer
        {
            get => _currentUser;
            private set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }

        private List<KeyValuePair<int, int>> CalculateScoresList()
        {
            if (_currentUser.Statistics == null)
            {
                return new List<KeyValuePair<int, int>>();
            }
            
            var scores = new List<KeyValuePair<int, int>>();
            var i = 0;
            foreach (var statistic in _currentUser.Statistics!)
            {
                KeyValuePair<int, int> pair;
                if (statistic is JsonElement statElement)
                {
                    pair = new KeyValuePair<int, int>(i, statElement.GetInt32());
                }
                else if (statistic is int statInt)
                {
                    pair = new KeyValuePair<int, int>(i, statInt);
                }
                else
                {
                    throw new ArgumentException("Internal error of authorization process");
                }
                
                scores.Add(pair);
                i++;
            }

            return scores;
        }

        public List<KeyValuePair<int, int>>? GameScores
        {
            get
            {
                return _scoresList ?? new List<KeyValuePair<int, int>>(new []{new KeyValuePair<int, int>(0,0)});
            }
            set => this.RaiseAndSetIfChanged(ref _scoresList, value);
        }
    }
}