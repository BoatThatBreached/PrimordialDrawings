﻿using System;
using System.Collections.Generic;

public static class PlayerInfo
{
    public static List<string> Learned;
    private static int _currentIndex;
    public static Dictionary<string, string> Gradation;
    public static string CurrentType => Learned.Count == 0 ? "" : Learned[_currentIndex%Learned.Count];
    public static bool CanSpawnCurrentType => Learned.Count != 0;
    public static bool Changed;

    public static string WantedToLearn;

    public static Dictionary<string, float> Paints;
    public static Action Pass = () => { };
    public static void Reset()
    {
        Changed = false;
        Learned = new List<string>();
        _currentIndex = 0;
        WantedToLearn = "";
        Gradation = new Dictionary<string, string> {["sprout"] = "tree"};
        Paints = new Dictionary<string, float>
        {
            ["earth"] = 1,
            ["wood"] = 1,
            ["blood"] = 1
        };
    }

    public static void UpdateIndex(int i)
    {
        if (Learned.Count == 0)
            _currentIndex = 0;
        else
            _currentIndex = (_currentIndex + Math.Sign(i)) % Learned.Count;
        if (_currentIndex < 0)
            _currentIndex += Learned.Count;
    }
}