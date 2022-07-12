using System;
using System.Text;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace MatrixMemory.Models;

public class Matrix : Grid
{
    private readonly int _amountOfRects;
    private readonly int _sizeOfRect;

    public Matrix(int amountOfRects, int sizeOfRect)
    {
        _amountOfRects = amountOfRects;
        _sizeOfRect = sizeOfRect;
        VerticalAlignment = VerticalAlignment.Center;
        HorizontalAlignment = HorizontalAlignment.Center;
        ShowGridLines = true;
        SetDefinitions();

        for (var i = 0; i < _amountOfRects; i++)
        {
            for (var j = 0; j < _amountOfRects; j++)
            {
                var standardButton = new Button
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = Brushes.Indigo
                };
                SetColumn(standardButton, i);
                SetRow(standardButton, j);
                Children.Add(standardButton);
            }
        }
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
}