using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MatrixMemory.Models;

public class GameSaveJsonConverter : JsonConverter<GameSave>
{
    public override GameSave? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        //OpenedTiles
        reader.Read();

        var openedTiles = new List<int>();
        //[
        reader.Read();
        //1
        reader.Read();

        while (Encoding.UTF8.GetString(reader.ValueSpan) != "]")
        {
            openedTiles.Add(reader.GetInt32());
            reader.Read();
        }
        //RealColors
        reader.Read();
        //[
        reader.Read();
        //[
        reader.Read();

        var realColors = new List<int[]>();
        while (Encoding.UTF8.GetString(reader.ValueSpan) != "]")
        {

            var temp = new List<int>();
            reader.Read();
            while (Encoding.UTF8.GetString(reader.ValueSpan) != "]")
            {
                temp.Add(reader.GetInt32());
                reader.Read();
            }
            realColors.Add(temp.ToArray());
            reader.Read();
        }
        
        reader.Read();
        reader.Read();
        var previousTile = reader.GetInt32();
        reader.Read();
        reader.Read();
        var currentTile = reader.GetInt32();
        reader.Read();
        reader.Read();
        var failures = reader.GetInt32();
        reader.Read();
        reader.Read();
        var score = reader.GetInt32();
        reader.Read();
        return new GameSave(openedTiles.ToArray(), realColors.ToArray(), previousTile, currentTile, failures, score);
    }

    public override void Write(Utf8JsonWriter writer, GameSave value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value));
    }
}