using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Extensions;
using UnityEngine;

public static class PlayerInfo
{
    public static List<string> Learned;
    public static int CurrentLevel;
    private static int _currentIndex;
    public static string CurrentType => Learned.Count == 0 ? "" : Learned[_currentIndex % Learned.Count];
    public static bool CanSpawnCurrentType => Learned.Count != 0;

    public static string Path => Application.persistentDataPath;
    public static string PrevScene { get; set; }
    public static float LineWidth => 0.3f;
    public static Vector3 WantedScale;

    public static string WantedToLearn;

    public static readonly Action Pass = () => { };
    public static readonly Func<bool> False = () => false;

    public static Dictionary<int, float> MaxEarth => new Dictionary<int, float>
    {
        [0] = 100f,
        [1] = 100f,
        [2] = 200f,
        [3] = 200f,
    };

    private static Dictionary<int, int> MaxWood => new Dictionary<int, int>
    {
        [0] = 1,
        [1] = 1,
        [2] = 2,
        [3] = 2
    };

    public static float EarthLeft;
    public static int WoodenPiecesLeft;
    public static int FireIndex;


    public static List<Vector3> Skulls;
    public static List<List<Vector3>> Platforms;
    public static List<Vector3> SpawnedSprouts;

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
        Learned = ReadString("learned").FromCustomList();
        EarthLeft = float.Parse(ReadStringCurrent("earth"));
        WoodenPiecesLeft = int.Parse(ReadStringCurrent("wood"));
        FireIndex = int.Parse(ReadStringCurrent("fire"));
        Skulls = ReadStringCurrent("skulls").ToVectList();
        SpawnedSprouts = ReadStringCurrent("sprouts").ToVectList();
        Platforms = ReadStringCurrent("platforms").FromCustomList().Select(s => s.ToVectList()).ToList();
    }

    [SuppressMessage("ReSharper", "SpecifyACultureInStringConversionExplicitly")]
    public static void Save()
    {
        WriteString("learned", Learned.ToCustomList());
        WriteStringCurrent("earth", EarthLeft.ToString());
        WriteStringCurrent("wood", WoodenPiecesLeft.ToString());
        WriteStringCurrent("fire", FireIndex.ToString());
        WriteStringCurrent("skulls", Skulls.ToStrList());
        WriteStringCurrent("sprouts", SpawnedSprouts.ToStrList());
        WriteStringCurrent("platforms", Platforms.Select(list => list.ToStrList()).ToCustomList());
    }

    public static string ReadString(string filename)
    {
        using var fs = new FileStream($"{Path}/{filename}.prim", FileMode.OpenOrCreate, FileAccess.Read);
        using var sw = new StreamReader(fs);
        return sw.ReadToEnd();
    }
    
    public static string ReadStringCurrent(string filename)
    {
        using var fs = new FileStream($"{Path}/{filename}_{CurrentLevel}.prim", FileMode.OpenOrCreate, FileAccess.Read);
        using var sw = new StreamReader(fs);
        return sw.ReadToEnd();
    }

    public static void WriteString(string filename, string s)
    {
        using var fs = new FileStream($"{Path}/{filename}.prim", FileMode.OpenOrCreate, FileAccess.Write);
        using var sw = new StreamWriter(fs);
        sw.Write(s);
    }

    public static void WriteStringCurrent(string filename, string s)
    {
        using var fs = new FileStream($"{Path}/{filename}_{CurrentLevel}.prim", FileMode.OpenOrCreate,
            FileAccess.Write);
        using var sw = new StreamWriter(fs);
        sw.Write(s);
    }

    public static void InitLevelInfo(int level)
    {
        Learned = new List<string>();
        EarthLeft = MaxEarth[level];
        WoodenPiecesLeft = MaxWood[level];
        FireIndex = 0;
        Skulls = new List<Vector3>();
        SpawnedSprouts = new List<Vector3>();
        Platforms = new List<List<Vector3>>();
        Save();
    }
}