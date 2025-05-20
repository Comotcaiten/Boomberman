using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileLevelLoader
{
    public static int Rows { get; private set; }
    public static int Columns { get; private set; }
    public static List<string> MapLines { get; private set; }

    public static void Load(string path)
    {
        // if (!File.Exists(path))
        // {
        //     Debug.LogError($"Level file not found: {path}");
        //     return;
        // }
        var line = File.ReadAllLines(path);
        var meta = line[0].Split(" ");

        Debug.Log("Level Meta: " + meta[0] + " " + meta[1] + " " + meta[2]);

        Rows = int.Parse(meta[1]);
        Columns = int.Parse(meta[2]);

        MapLines = new List<string>();
        
        for (int i = 1; i < line.Length; i++)
        {
            if (line[i] == "Meta data:") {
                break;
            }
            MapLines.Add(line[i]);

            Debug.Log($"Level Data Line {i}: {line[i]}");

            if (i != 1 && line[i] == line[1])
            {
             break;   
            }
        }
    }

    public static void LoadFromText(string textData)
    {
        var lines = textData.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        if (lines.Length == 0)
        {
            Debug.LogError("Level data is empty.");
            return;
        }

        var meta = lines[0].Split(' ');

        if (meta.Length < 3)
        {
            Debug.LogError("Invalid level metadata format.");
            return;
        }

        Debug.Log($"Level Meta: {meta[0]} {meta[1]} {meta[2]}");

        Rows = int.Parse(meta[1]);
        Columns = int.Parse(meta[2]);

        MapLines = new List<string>();

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i] == "Meta data:")
            {
                break;
            }

            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                MapLines.Add(lines[i]);
                Debug.Log($"Level Data Line {i}: {lines[i]}");
            }
        }
    }
}