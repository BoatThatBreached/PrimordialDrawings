using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class SerializeExtensions
    {
        public static string ToVectString(this Vector3 source)
            => $"{source.x} {source.y} {source.z}";

        public static Vector3 ToVector(this string source)
        {
            Debug.Log(source);
            var sp = source.Split(' ');
            return new Vector3(float.Parse(sp[0]), float.Parse(sp[1]), float.Parse(sp[2]));
        }

        public static string ToDict(this Dictionary<string, float> source)
            => string
                .Join(",", source.Keys.Select(k => $"{k}:{source[k]}"));

        public static Dictionary<string, float> FromDict(this string source)
        {
            if (source == "")
                return new Dictionary<string, float>();
            return source
                .Split(',')
                .ToDictionary(s => s.Split(':')[0], s => float.Parse(s.Split(':')[1]));
        }

        public static string ToVectorDict(this Dictionary<int, List<Vector3>> source)
            => string.Join("^",
                source.Keys.Select(k => $"{k}:{string.Join("|", source[k].Select(v => v.ToVectString()))}"));

        public static Dictionary<int, List<Vector3>> FromVectorDict(this string source)
        {
            if (source == "")
                return new Dictionary<int, List<Vector3>>();
            if (!source.Contains('^'))
                return new Dictionary<int, List<Vector3>>
                {
                    [int.Parse(source.Split(':')[0])] =
                        source.Split(':')[1].Split('|').Select(v => v.ToVector()).ToList()
                };
            return source.Split(',')
                .ToDictionary(
                    s => int.Parse(s.Split(':')[0]),
                    s => s.Split(':')[1].Split('|').Select(v => v.ToVector()).ToList());
        }

        public static string ToCustomList(this IEnumerable<string> source)
            => string.Join("!", source);

        public static string ToComplexDict(this Dictionary<int, List<List<Vector3>>> source)
            => string.Join("^",
                source.Keys.Select(k => $"{k}:{source[k].Select(list => list.ToStrList()).ToCustomList()}"));

        public static List<string> FromCustomList(this string source)
            => source.Split('!').ToList();

        public static Dictionary<int, List<List<Vector3>>> FromComplexDict(this string source)
        {
            if (source == "")
                return new Dictionary<int, List<List<Vector3>>>();
            if (!source.Contains('^'))
                return new Dictionary<int, List<List<Vector3>>>
                {
                    [int.Parse(source.Split(':')[0])] =
                        source.Split(':')[1].FromCustomList().Select(vl => vl.ToVectList()).ToList()
                };
            return source.Split('^')
                .ToDictionary(
                    s => int.Parse(s.Split(':')[0]),
                    s => s.Split(':')[1].FromCustomList().Select(vl => vl.ToVectList()).ToList());
        }

        public static string ToVeryComplexDict(this Dictionary<int, Dictionary<string, List<Vector3>>> source)
        {
            return string.Join(
                "^", 
                source.Keys
                    .Select(k => 
                        $"{k};{string.Join("#", source[k].Keys.Select(k2=>$"{k2}:{source[k][k2].ToStrList()}"))}"));
        }

        public static Dictionary<int, Dictionary<string, List<Vector3>>> FromVeryComplexDict(this string source)
        {
            if (source == "")
                return new Dictionary<int, Dictionary<string, List<Vector3>>>();
            if (!source.Contains('^'))
                return new Dictionary<int, Dictionary<string, List<Vector3>>>
                {
                    [int.Parse(source.Split(';')[0])] = 
                        source
                            .Split(';')[1]
                            .Split('#')
                            .ToDictionary(s=>s.Split(':')[0], s=>s.Split(':')[1].ToVectList())
                };
            return source
                .Split('^')
                .ToDictionary(
                    s => int.Parse(s.Split(';')[0]), 
                    s => s.Split(';')[1]
                        .Split('#')
                        .ToDictionary(
                            s2=>s2.Split(':')[0],
                            s2=>s2.Split(':')[1].ToVectList()));
        }
    }
}