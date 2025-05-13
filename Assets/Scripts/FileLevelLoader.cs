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

        var line = File.ReadAllLines(path);
        var meta = line[0].Split(" ");

        Debug.Log("Level Meta: " + meta[0] + " " + meta[1] + " " + meta[2]);

        Rows = int.Parse(meta[1]);
        Columns = int.Parse(meta[2]);

        MapLines = new List<string>();
        
        for (int i = 1; i < line.Length; i++)
        {
            if (line[i] == "Meta data:" || (i != 2 && line[i] == line[2])) {
                break;
            }
            MapLines.Add(line[i]);

            Debug.Log($"Level Data Line {i}: {line[i]}");
        }
    }
}
