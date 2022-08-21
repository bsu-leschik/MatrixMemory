using System;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace MatrixMemory.Models;

public class Matrix : Grid
{
    private int _amountOfRects;
    private readonly int _rectsAtStart;
    private readonly int _sizeOfRect;
    private IBrush[,]? _realColors;
    
    private Button? _previousButton;
    private Button? _currentButton;

    private bool _showInOperation;

    private int _totalAmountOfRects;

    private int _failures;

    private readonly ArrayList _colors = new(new[] {Brushes.Black, Brushes.Blue, Brushes.Brown, Brushes.Green,
        Brushes.Orange, Brushes.Purple, Brushes.Red, Brushes.Yellow, Brushes.Pink, Brushes.Navy, Brushes.Gold,
        Brushes.Magenta, Brushes.Aqua, Brushes.Tomato, Brushes.Wheat, Brushes.Cyan});

    public int Failures => _failures;

    public event EventHandler? Win;

    public Matrix(int amountOfRects, int sizeOfRect)
    {
        if (amountOfRects is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(amountOfRects), $"{amountOfRects} cannot be more then 5 or less then 1");
        }

        _rectsAtStart = amountOfRects;
        _amountOfRects = amountOfRects;
        _sizeOfRect = sizeOfRect;
        VerticalAlignment = VerticalAlignment.Center;
        HorizontalAlignment = HorizontalAlignment.Center;

        _totalAmountOfRects = amountOfRects * amountOfRects;

        SetDefinitions();
        InitializeColors();
        SetButtons();
    }

    private void SetDefinitions()
    {
        for (var i = 0; i < _amountOfRects; i++)
        {
            var standardColumnDef = new ColumnDefinition
            {
                Width = new GridLength(_amountOfRects * _sizeOfRect)
            };
            ColumnDefinitions.Add(standardColumnDef);
            
            var standardRowDef = new RowDefinition
            {
                Height = new GridLength(_amountOfRects * _sizeOfRect)
            };
            RowDefinitions.Add(standardRowDef);
        }
    }

    private void SetButtons()
    {
        for (var i = 0; i < _amountOfRects; i++)
        {
            for (var j = 0; j < _amountOfRects; j++)
            {
                var standardButton = new Button
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = Brushes.Gray,
                };

                var j1 = j;
                var i1 = i;
                standardButton.Click += delegate(object? sender, RoutedEventArgs _)
                {
                    if (_showInOperation)
                    {
                        return;
                    }
                    if (sender == null)
                    {
                        throw new ArgumentNullException($"{sender} cannot be null");
                    }
                    _currentButton = (Button)sender;

                    if (!Equals(_currentButton.Background, Brushes.Gray))
                    {
                        return;
                    }
                    
                    _currentButton.Background = _realColors![i1, j1];

                    if (Equals(_currentButton.Background, Brushes.White))
                    {
                        _totalAmountOfRects--;
                        CheckIfWin();
                        return;
                    }

                    if (_previousButton == null)
                    {
                        _previousButton = _currentButton;
                    }
                    else if (!Equals(_previousButton.Background, _currentButton.Background))
                    {
                        DispatcherTimer.RunOnce(CloseTiles, TimeSpan.FromMilliseconds(500));
                        _failures++;
                    }
                    else
                    {
                        _previousButton = null;
                        _totalAmountOfRects -= 2;
                        CheckIfWin();
                    }
                };
                
                SetColumn(standardButton, i);
                SetRow(standardButton, j);
                Children.Add(standardButton);
            }
        }
    }

    private void CloseTiles()
    {
        _previousButton!.Background = Brushes.Gray;
        _currentButton!.Background = Brushes.Gray;
        _previousButton = null;
    }

    private void InitializeColors()
    {
        var random = new Random(DateTime.Now.Millisecond);
        _realColors = new IBrush[_amountOfRects,_amountOfRects];

        var colors = new ArrayList();
        if ((_amountOfRects * _amountOfRects) / 2 == 16)
        {
            colors = _colors;
        }
        else
        {
            for (var i = 0; i < (_amountOfRects * _amountOfRects) / 2; i++)
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

        for (var i = 0; i < _amountOfRects; i++)
        {
            for (var j = 0; j < _amountOfRects; j++)
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

    private void OpenAllCards()
    {
        _showInOperation = true;
        int i = 0, j = 0;
        foreach (var control in Children)
        {
            (control as Button)!.Background = _realColors![i, j];
            if (j == _amountOfRects - 1)
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

    private void CloseAllCards()
    {
        foreach (var control in Children)
        {
            var button = control as Button;

            if (button != null)
            {
                button.Background = Brushes.Gray;
            }
        }

        _showInOperation = false;
    }
    
    public void ShowCards(long sec)
    {
        OpenAllCards();
        DispatcherTimer.RunOnce(CloseAllCards, TimeSpan.FromSeconds(sec));

    }

    private void DisableAllButtons(bool disable = true)
    {
        foreach (var control in Children)
        {
            (control as Button)!.IsEnabled = !disable;
        }
    }
    
    private void CheckIfWin()
    {
        if (_totalAmountOfRects == 0)
        {
            DisableAllButtons();
            Win!.Invoke(this, EventArgs.Empty);
        }
    }

    public void NextLevel(int secToShowCards)
    {
        Children.RemoveRange(0, _amountOfRects * _amountOfRects);
        RowDefinitions.RemoveRange(0 ,_amountOfRects);
        ColumnDefinitions.RemoveRange(0, _amountOfRects);

        if (_amountOfRects < 6)
        {
            _amountOfRects++;
        }
        else
        {
            EndGame();
            return;
        }

        _totalAmountOfRects = _amountOfRects * _amountOfRects;

        
        SetDefinitions();
        InitializeColors();
        SetButtons();
        DisableAllButtons(false);
        ShowCards(secToShowCards);
    }

    public void Restart(int secToShowCards)
    {
        Children.RemoveRange(0, _amountOfRects * _amountOfRects);
        RowDefinitions.RemoveRange(0 ,_amountOfRects);
        ColumnDefinitions.RemoveRange(0, _amountOfRects);

        _totalAmountOfRects = _amountOfRects * _amountOfRects;
        
        SetDefinitions();
        InitializeColors();
        SetButtons();
        DisableAllButtons(false);
        ShowCards(secToShowCards);
    }

    public void EndGame()
    {
        Children.RemoveRange(0, _amountOfRects * _amountOfRects);
        RowDefinitions.RemoveRange(0 ,_amountOfRects);
        ColumnDefinitions.RemoveRange(0, _amountOfRects);
        
        _amountOfRects = _rectsAtStart;
        _totalAmountOfRects = _amountOfRects * _amountOfRects;
        
        SetDefinitions();
        InitializeColors();
        SetButtons();
        DisableAllButtons(false);
    }
}