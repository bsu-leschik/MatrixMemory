using System.Text.Json;
using System.Text.Json.Serialization;

namespace MatrixMemory.Models;

public class GameSave
{
    [JsonInclude]
    public int[] OpenedTiles { get; set; }

    [JsonInclude] 
    public int[][] RealColors { get; set; }

    [JsonInclude]
    public int PreviousTile{ get; set; }
    
    [JsonInclude]
    public int CurrentTile{ get; set; }

    [JsonInclude]
    public int Failures{ get; set; }
    
    [JsonInclude]
    public int Score{ get; set; }

    public GameSave(int[] opened, int[][] realColors, int previousTile, int currentTile, int failures, int score)
    {
        RealColors = realColors;
        OpenedTiles = opened;
        PreviousTile = previousTile;
        CurrentTile = currentTile;
        Failures = failures;
        Score = score;
    }
}