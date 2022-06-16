using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class UnityExtensions
    {
        public static string ToStrList(this IEnumerable<Vector3> source)
        {
            return string.Join("|", source.Select(ToCompactString));
        }

        public static List<Vector3> ToVectList(this string s)
        {
            return s.Split('|').Select(ToVector).ToList();
        }

        private static string ToCompactString(this Vector3 v)
        {
            return $"{v.x}_{v.y}_{v.z}";
        }

        private static Vector3 ToVector(this string s)
        {
            var split = s.Split('_');
            return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
        }

        public static List<Vector3> KeyPoints(this List<Vector3> line)
        {
            var keyPoints = new List<Vector3> {line[0]};
            var prev = line[0];
            for (var i = 1; i < line.Count - 1; i++)
            {
                var point = line[i];
                var next = line[i + 1];
                if (Vector3.Distance(prev, point) >= 0.2f &&
                    Math.Cos(Vector3.Angle(point - prev, next - point)) >= -Math.Sqrt(2) / 2)
                    keyPoints.Add(point);
            }

            keyPoints.Add(line[line.Count - 1]);
            return keyPoints;
        }

        public static Vector3 Center(this List<Vector3> source)
            => 1f / source.Count * source.Aggregate((v1, v2) => v1 + v2);

        public static List<Vector3> DividedPoints(this IEnumerable<Vector3> lines, float radius)
        {
            var allPoints = lines.ToList();
            if (allPoints.Count == 0)
                return allPoints;
            var result = new List<Vector3> {allPoints[0]};
            for (var i = 1; i < allPoints.Count; i++)
                if (Vector3.Distance(allPoints[i], result[result.Count - 1]) >= radius)
                    result.Add(allPoints[i]);
            result.Add(allPoints[allPoints.Count - 1]);
            return result;
        }

        public static Vector3 Rotate(this Vector3 source, Vector3 anchor, float angle)
        {
            var cosA = Mathf.Cos(angle);
            var sinA = Mathf.Sin(angle);
            var shift = source - anchor;
            var rotate = new Vector3(shift.x * cosA + shift.y * sinA, -shift.x * sinA + shift.y * cosA);
            var unshift = rotate + anchor;
            return unshift;
        }

        public static Vector3 Extend(this Vector3 source, Vector3 scale)
            => new Vector3(source.x * scale.x, source.y * scale.y, source.z * scale.z);

        public static Vector3 WithX(this Vector3 source, float x)
            => new Vector3(x, source.y, source.z);

        public static Vector3 WithY(this Vector3 source, float y)
            => new Vector3(source.x, y, source.z);

        public static Vector3 WithZ(this Vector3 source, float z)
            => new Vector3(source.x, source.y, z);

        public static bool KeyComboPressed(KeyCode code1, KeyCode code2)
            => Input.GetKey(code1) && Input.GetKeyDown(code2) || Input.GetKeyDown(code1) && Input.GetKey(code2);
    }
}