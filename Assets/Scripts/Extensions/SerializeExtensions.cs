using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class SerializeExtensions
    {
        public static string ToVectString(this Vector3 source)
            => $"{source.x}_{source.y}_{source.z}";

        public static Vector3 ToVector(this string source)
        {
            var sp = source.Split('_');
            return new Vector3(float.Parse(sp[0]), float.Parse(sp[1]), float.Parse(sp[2]));
        }

        public static string ToCustomList(this IEnumerable<string> source)
            => string.Join("!", source);

        public static List<string> FromCustomList(this string source)
            => source.Split('!').ToList();

        public static string ToStrList(this IEnumerable<Vector3> source)
        {
            return string.Join("|", source.Select(ToVectString));
        }

        public static List<Vector3> ToVectList(this string s)
        {
            return s == ""
                ? new List<Vector3>()
                : s.Split('|').Select(ToVector).ToList();
        }
    }
}