using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using UnityEngine;

public static class PlayerInfo
{
    public static List<string> Learned;
    public static int CurrentLevel;
    private static int _currentIndex;
    public static Vector3 LastPlayerPos;
    public static string CurrentType => Learned.Count == 0 ? "" : Learned[_currentIndex % Learned.Count];
    public static bool CanSpawnCurrentType => Learned.Count != 0;

    public static string Path => Application.persistentDataPath;
    public static string PrevScene { get; set; }
    public static float LineWidth => 0.3f;
    public static Vector3 WantedScale;

    public static string WantedToLearn;

    public static Dictionary<string, float> Paints;
    public static readonly Action Pass = () => { };
    public static Dictionary<int, List<Vector3>> Palms; //Level - position mapping
    public static Dictionary<int, List<Vector3>> Corpses; //Level - position mapping
    public static Dictionary<int, List<List<Vector3>>> Platforms; //Level - terrain additions mapping
    public static Dictionary<int, Dictionary<string, List<Vector3>>> Spawned;//Level - spawned objects (types and positions) mapping


    public static void UpdateIndex(int i)
    {
        if (Learned.Count == 0)
            _currentIndex = 0;
        else
            _currentIndex = (_currentIndex + Math.Sign(i)) % Learned.Count;
        if (_currentIndex < 0)
            _currentIndex += Learned.Count;
    }

    public static void Load()
    {
        Paints = ReadString($"paints_{CurrentLevel}").FromDict();
        Learned = ReadString("learned").FromCustomList();
        Palms = ReadString($"palms_{CurrentLevel}").FromVectorDict();
        Corpses = ReadString($"corpses_{CurrentLevel}").FromVectorDict();
        LastPlayerPos = ReadString($"lastPos_{CurrentLevel}").ToVector();
        Platforms = ReadString($"platforms_{CurrentLevel}").FromComplexDict();
        Spawned = ReadString($"spawned_{CurrentLevel}").FromVeryComplexDict();
        
    }

    public static void Save()
    {
        WriteString($"paints_{CurrentLevel}", Paints.ToDict());
        WriteString("learned", Learned.ToCustomList());
        WriteString($"palms_{CurrentLevel}", Palms.ToVectorDict());
        WriteString($"corpses_{CurrentLevel}", Corpses.ToVectorDict());
        WriteString($"lastPos_{CurrentLevel}", LastPlayerPos.ToVectString());
        WriteString($"platforms_{CurrentLevel}", Platforms.ToComplexDict());
        WriteString($"spawned_{CurrentLevel}", Spawned.ToVeryComplexDict());
    }

    public static string ReadString(string filename)
    {
        using var fs = new FileStream($"{Path}/{filename}.prim", FileMode.OpenOrCreate, FileAccess.Read);
        using var sw = new StreamReader(fs);
        return sw.ReadToEnd();
    }

    public static void WriteString(string filename, string s)
    {
        using var fs = new FileStream($"{Path}/{filename}.prim", FileMode.OpenOrCreate, FileAccess.Write);
        using var sw = new StreamWriter(fs);
        sw.Write(s);
    }

    public static void InitLevelInfo()
    {
        Paints = new Dictionary<string, float>();
        Learned = new List<string>();
        Palms = new Dictionary<int, List<Vector3>>
        {
            //[_currentLevel] = new List<Vector3>()
        };
        Corpses = new Dictionary<int, List<Vector3>>
        {
            //[_currentLevel] = new List<Vector3>()
        };
        Platforms = new Dictionary<int, List<List<Vector3>>>
        {
            //[_currentLevel] = new List<List<Vector3>>()
        };
        Spawned = new Dictionary<int, Dictionary<string, List<Vector3>>>();
        // WriteString($"paints_{_currentLevel}", Paints.ToDict());
        // WriteString("learned", Learned.ToCustomList());
        // WriteString($"palms_{_currentLevel}", Palms.ToVectorDict());
        // WriteString($"corpses_{_currentLevel}", Corpses.ToVectorDict());
        // WriteString($"platforms_{_currentLevel}", Platforms.ToComplexDict());
        Save();
    }
}