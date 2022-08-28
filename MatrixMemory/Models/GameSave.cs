using System.Text.Json.Serialization;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace MatrixMemory.Models;

public class GameSave
{
    [JsonInclude]
    public int[] OpenedTiles { get; set; }

    [JsonInclude] 
    public IBrush[][]? RealColors { get; set; }

    [JsonInclude]
    public int PreviousTile{ get; set; }
    
    [JsonInclude]
    public int CurrentTile{ get; set; }

    [JsonInclude]
    public int Failures{ get; set; }
    
    [JsonInclude]
    public int Score{ get; set; }

    public GameSave(IBrush[,] realColors, int[] opened, int previousTile, int currentTile, int failures, int score)
    {
        RealColors = new IBrush[realColors.GetLength(0)][];
        for (var i = 0; i < RealColors.Length; i++)
        {
            RealColors[i] = new IBrush[realColors.GetLength(0)];
        }
        
        for (var i = 0; i < realColors.GetLength(0); i++)
        {
            for (var j = 0; j < realColors.GetLength(0); j++)
            {
                RealColors[i][j] = realColors[i, j];
            }
        }

        OpenedTiles = opened;
        PreviousTile = previousTile;
        CurrentTile = currentTile;
        Failures = failures;
        Score = score;
    }
}