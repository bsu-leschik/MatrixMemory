using System.Text.Json.Serialization;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace MatrixMemory.Models;

public class GameSave
{
    [JsonInclude] public IBrush[][] RealColors { get; set; }

    [JsonInclude]
    public Rectangle? PreviousTile{ get; set; }
    
    [JsonInclude]
    public Rectangle? CurrentTile{ get; set; }
    
    [JsonInclude]
    public int TotalAmountOfTiles{ get; set; }
    
    [JsonInclude]
    public int Failures{ get; set; }
    
    [JsonInclude]
    public int Score{ get; set; }

    public GameSave(IBrush[,] realColors, Rectangle? previousTile, Rectangle? currentTile, int totalAmountOfTiles, int failures, int score)
    {
        RealColors = new IBrush[realColors.Length / realColors.Rank][];
        for (var i = 0; i < realColors.Length / realColors.Rank; i++)
        {
            RealColors[i] = new IBrush[realColors.Length / realColors.Rank];
        }
        
        for (var i = 0; i < realColors.Length / realColors.Rank; i++)
        {
            for (var j = 0; j < realColors.Length / realColors.Rank; j++)
            {
                RealColors[i][j] = realColors[i, j];
            }
        }

        PreviousTile = previousTile;
        CurrentTile = currentTile;
        TotalAmountOfTiles = totalAmountOfTiles;
        Failures = failures;
        Score = score;
    }
}