using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace MatrixMemory.Models;

public class Matrix : Grid
{
    private int _amountOfTiles;
    private readonly int _tilesAtStart;
    private readonly int _sizeOfElement;
    private readonly int _tileMargin;
    private IBrush[,]? _realColors;
    
    private Rectangle? _previousTile;
    private Rectangle? _currentTile;

    private bool _showInOperation;

    private int _totalAmountOfTiles;

    private int _failures;
    private readonly int _failureLimit;

    private bool _disabled;

    private int _score;

    private readonly ArrayList _colors = new(new[] {Brushes.Black, Brushes.Blue, Brushes.Brown, Brushes.Green,
        Brushes.Orange, Brushes.Purple, Brushes.Red, Brushes.Yellow, Brushes.Pink, Brushes.Navy, Brushes.Gold,
        Brushes.Magenta, Brushes.Aqua, Brushes.Tomato, Brushes.Wheat, Brushes.Cyan});
    
    public event EventHandler? Win;
    public event EventHandler? OverFailuresLimit;

    public event EventHandler? GameEnded;

    public Matrix(int amountOfTiles, int sizeOfElement, int failures = Int32.MaxValue, int tileMargin = 1)
    {
        _failureLimit = failures;
        if (amountOfTiles is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(amountOfTiles), $"{amountOfTiles} cannot be more then 5 or less then 1");
        }

        _tilesAtStart = amountOfTiles;
        _amountOfTiles = amountOfTiles;
        _sizeOfElement = sizeOfElement;
        _tileMargin = tileMargin;
        VerticalAlignment = VerticalAlignment.Stretch;
        HorizontalAlignment = HorizontalAlignment.Stretch;

        _totalAmountOfTiles = amountOfTiles * amountOfTiles;

        SetDefinitions();
        InitializeColors();
        SetTiles();
    }

    public int Failures => _failures;

    public int Score => _score;

    private void SetDefinitions()
    {
        for (var i = 0; i < _amountOfTiles; i++)
        {
            var standardColumnDef = new ColumnDefinition
            {
                Width = new GridLength(_sizeOfElement)
            };
            ColumnDefinitions.Add(standardColumnDef);
            
            var standardRowDef = new RowDefinition
            {
                Height = new GridLength(_sizeOfElement)
            };
            RowDefinitions.Add(standardRowDef);
        }
    }

    private void SetTiles()
    {
        for (var i = 0; i < _amountOfTiles; i++)
        {
            for (var j = 0; j < _amountOfTiles; j++)
            {
                var standardRectangle = new Rectangle()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Fill = Brushes.Gray,
                    Margin = Thickness.Parse(_tileMargin.ToString()),
                    Name = string.Join("", i, j)
                };
                
                var j1 = j;
                var i1 = i;
                standardRectangle.Tapped += delegate(object? sender, RoutedEventArgs _)
                {
                    if (_showInOperation || _disabled)
                    {
                        return;
                    }

                    if (sender == null)
                    {
                        throw new ArgumentNullException($"{sender} cannot be null");
                    }
                    _currentTile = (Rectangle)sender;

                    if (!Equals(_currentTile.Fill, Brushes.Gray))
                    {
                        return;
                    }
                    
                    _currentTile.Fill = _realColors![i1, j1];

                    if (Equals(_currentTile.Fill, Brushes.White))
                    {
                        _totalAmountOfTiles--;
                        _score++;
                        CheckIfWin();
                        return;
                    }

                    if (_previousTile == null)
                    {
                        _previousTile = _currentTile;
                    }
                    else if (!Equals(_previousTile.Fill, _currentTile.Fill))
                    {
                        _showInOperation = true;
                        DispatcherTimer.RunOnce(CloseTiles, TimeSpan.FromMilliseconds(500));
                        _failures++;
                        _score--;
                        if (_failures > _failureLimit)
                        {
                            OverFailuresLimit?.Invoke(this, EventArgs.Empty);
                            DisableAllTiles();
                        }
                    }
                    else
                    {
                        _previousTile = null;
                        _totalAmountOfTiles -= 2;
                        _score++;
                        CheckIfWin();
                    }
                };
                
                SetColumn(standardRectangle, i);
                SetRow(standardRectangle, j);
                Children.Add(standardRectangle);
            }
        }
    }

    private void CloseTiles()
    {
        _previousTile!.Fill = Brushes.Gray;
        _currentTile!.Fill = Brushes.Gray;
        _previousTile = null;
        _showInOperation = false;
    }

    private void InitializeColors()
    {
        var random = new Random(DateTime.Now.Millisecond);
        _realColors = new IBrush[_amountOfTiles,_amountOfTiles];

        var colors = new ArrayList();
        if ((_amountOfTiles * _amountOfTiles) / 2 == 16)
        {
            colors = _colors;
        }
        else
        {
            for (var i = 0; i < (_amountOfTiles * _amountOfTiles) / 2; i++)
            {
                var color = _colors[random.Next(0, 16)];
                if (!colors.Contains(color))
                {
                    colors.Add(color);
                }
                else
                {
                    i--;
                }
            }
        }

        colors.AddRange(colors);

        for (var i = 0; i < _amountOfTiles; i++)
        {
            for (var j = 0; j < _amountOfTiles; j++)
            {
                if (colors.Count != 0)
                {
                    var k = random.Next(0, colors.Count);
                    _realColors[i, j] = (IBrush)colors[k]! ?? throw new InvalidOperationException();
                    colors.RemoveAt(k);
                }
                else
                {
                    _realColors[i, j] = Brushes.White;
                }
            }
        }
    }

    private void OpenAllTiles()
    {
        _showInOperation = true;
        int i = 0, j = 0;
        foreach (var control in Children)
        {
            (control as Rectangle)!.Fill = _realColors![i, j];
            if (j == _amountOfTiles - 1)
            {
                i++;
                j = 0;
            }
            else
            {
                j++;
            }
        }
    }

    private void CloseAllTiles()
    {
        foreach (var control in Children)
        {
            if (control is Rectangle button)
            {
                button.Fill = Brushes.Gray;
            }
        }

        _showInOperation = false;
    }
    
    public void ShowTiles(long sec)
    {
        OpenAllTiles();
        DispatcherTimer.RunOnce(CloseAllTiles, TimeSpan.FromSeconds(sec));

    }

    private void DisableAllTiles(bool disable = true)
    {
        _disabled = disable;
    }
    
    private void CheckIfWin()
    {
        if (_totalAmountOfTiles == 0)
        {
            DisableAllTiles();
            Win?.Invoke(this, EventArgs.Empty);
            if (_amountOfTiles == 5)
            {
                GameEnded?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void NextLevel(int secToShowCards)
    {
        if (_failures > _failureLimit)
        {
            Children.RemoveRange(0, _amountOfTiles * _amountOfTiles);
            RowDefinitions.RemoveRange(0 ,_amountOfTiles);
            ColumnDefinitions.RemoveRange(0, _amountOfTiles);
            _amountOfTiles--;
        }
        else if (_amountOfTiles < 5)
        {
            Children.RemoveRange(0, _amountOfTiles * _amountOfTiles);
            RowDefinitions.RemoveRange(0 ,_amountOfTiles);
            ColumnDefinitions.RemoveRange(0, _amountOfTiles);
            _amountOfTiles++;
        }
        else
        {
            RestartGame(secToShowCards);
            return;
        }

        
        
        _failures = 0;

        _totalAmountOfTiles = _amountOfTiles * _amountOfTiles;

        
        SetDefinitions();
        InitializeColors();
        SetTiles();
        DisableAllTiles(false);
        ShowTiles(secToShowCards);
    }

    //score
    public void Restart(int secToShowCards)
    {
        Children.RemoveRange(0, _amountOfTiles * _amountOfTiles);
        RowDefinitions.RemoveRange(0 ,_amountOfTiles);
        ColumnDefinitions.RemoveRange(0, _amountOfTiles);

        _totalAmountOfTiles = _amountOfTiles * _amountOfTiles;
        
        SetDefinitions();
        InitializeColors();
        SetTiles();
        DisableAllTiles(false);
        ShowTiles(secToShowCards);
    }

    public void EndGame()
    {
        Children.RemoveRange(0, _amountOfTiles * _amountOfTiles);
        RowDefinitions.RemoveRange(0 ,_amountOfTiles);
        ColumnDefinitions.RemoveRange(0, _amountOfTiles);

        _amountOfTiles = _tilesAtStart;
        _totalAmountOfTiles = _amountOfTiles * _amountOfTiles;
        _failures = 0;
        _score = 0;

        SetDefinitions();
        InitializeColors();
        SetTiles();
        DisableAllTiles(false);
    }

    private void RestartGame(int secToShowCards)
    {
        Children.RemoveRange(0, _amountOfTiles * _amountOfTiles);
        RowDefinitions.RemoveRange(0 ,_amountOfTiles);
        ColumnDefinitions.RemoveRange(0, _amountOfTiles);

        _amountOfTiles = _tilesAtStart;
        _totalAmountOfTiles = _amountOfTiles * _amountOfTiles;
        _failures = 0;
        _score = 0;
        
        SetDefinitions();
        InitializeColors();
        SetTiles();
        DisableAllTiles(false);
        ShowTiles(secToShowCards);
    }
    
    public GameSave SaveGame()
    {
        var opened = new List<int>();
        var i = 0;
        foreach (var control in Children)
        {
            if (!Equals((control as Rectangle)?.Fill, Brushes.Gray))
            {
                opened.Add(i);
            }

            i++;
        }
        return new GameSave(opened.ToArray(), GetColorsAsInts(), Children.IndexOf(_previousTile), Children.IndexOf(_currentTile), _failures, _score);
    }

    private int[][] GetColorsAsInts()
    {
        var array = new int[_realColors!.GetLength(0)][];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new int[_realColors.GetLength(0)];
        }

        for (var i = 0; i < _realColors.GetLength(0); i++)
        {
            for (var j = 0; j < _realColors.GetLength(0); j++)
            {
                array[i][j] = _colors.IndexOf(_realColors[i, j]);
            }
        }

        return array;
    }

    public void LoadGame(GameSave save)
    {
        if (save.RealColors.Length is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(save.RealColors.Length), $"{save.RealColors.Length} cannot be more then 5 or less then 1");
        }
        
        Children.RemoveRange(0, _amountOfTiles * _amountOfTiles);
        RowDefinitions.RemoveRange(0 ,_amountOfTiles);
        ColumnDefinitions.RemoveRange(0, _amountOfTiles);
        
        _amountOfTiles = save.RealColors.GetLength(0);

        var amountOfOpened = save.OpenedTiles.Length;
        if (amountOfOpened % 2 != 0)
        {
            amountOfOpened--;
        }
        
        _totalAmountOfTiles = save.RealColors.Length * save.RealColors.Length - amountOfOpened;

        SetDefinitions();
        InitWithSavedColors(save.RealColors);
        SetTiles();

        _score = save.Score;
        
        if (save.PreviousTile > 0)
        {
            _previousTile = Children[save.PreviousTile] as Rectangle;
        }
        
        if (save.CurrentTile > 0)
        {
            _currentTile = Children[save.CurrentTile] as Rectangle;
        }
        
        InitClickedTiles(save.OpenedTiles);

    }

    private void InitWithSavedColors(int[][] realColors)
    {
        _realColors = new IBrush[realColors.GetLength(0), realColors.GetLength(0)];
        for (var i = 0; i < realColors.GetLength(0); i++)
        {
            for (var j = 0; j < realColors.GetLength(0); j++)
            {
                if (realColors[i][j] >= 0)
                {
                    _realColors[i, j] = (_colors[realColors[i][j]] as IBrush)!;
                }
                else
                {
                    _realColors[i, j] = Brushes.White;
                }
            }
        }
    }

    private void InitClickedTiles(int[] cords)
    {
        foreach (var cord in cords)
        {
            if (Children[cord] is not Rectangle child || child.Name == null)
            {
                throw new NullReferenceException("Internal problem occured when trying to load clicked tiles");
            }

            var i = int.Parse(child.Name[0].ToString());
            var j = int.Parse(child.Name[1].ToString());
            child.Fill = _realColors![i, j];
        }
    }
}