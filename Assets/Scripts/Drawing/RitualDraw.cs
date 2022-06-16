using System.Linq;
using Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Drawing
{
    public class RitualDraw : SelfDraw
    {
        public SavedEntry container;
        public SavedEntry original;
        public string material;
        public string title;
        public bool isTough;
        public bool isSync;

        private void Start()
        {
            
            maxPoints = original.totalPoints * 1.5f;
            container.Init();
            material = PlayerInfo.WantedToLearn switch
            {
                "spear" => "1",
                "sprout" => "1",
                "boat"=>"1",
                _ => "2"
            };
        }

        public void Save()
        {
            // if (!DividedCheck())
            // {
            //     Debug.Log("Try again!");
            //     Clear();
            //     return;
            // }

            container.envType = PlayerInfo.WantedToLearn;
            PlayerInfo.Learned.Add(container.envType);
            PlayerInfo.Save();
            
            PrefabUtility.SaveAsPrefabAsset(container.gameObject, $"Assets/Resources/Prefabs/your_{title}.prefab");
            Clear();

            SceneManager.LoadScene(PlayerInfo.PrevScene);
        }

        public void Clear()
        {
            container.transform.SetParent(transform.parent);
            Game.Clear(transform);
            Game.Clear(container.transform);
            container.Init();
            container.transform.SetParent(transform);
        }

        public bool Check()
        {
            var newLines = container.lines.Select(line => line.ToVectList()).ToList();
            foreach (var line in original.lines)
            {
                var realLine = line.ToVectList();
                var keyPoints = realLine.KeyPoints();
                foreach (var keyPoint in keyPoints)
                {
                    var check = false;
                    foreach (var newLine in newLines)
                    {
                        if (newLine.Any(newPoint => !(Vector3.Distance(newPoint, keyPoint) > 0.1f)))
                        {
                            check = true;
                        }

                        if (check) break;
                    }
                }
            }

            return true;
        }

        public bool DividedCheck()
        {
            var originalPoints = original.lines
                .Select(line => line.ToVectList())
                .SelectMany(line => line)
                .ToList();
            var oMx = originalPoints.Max(v => v.x);
            var oMy = originalPoints.Max(v => v.y);
            var omx = originalPoints.Min(v => v.x);
            var omy = originalPoints.Min(v => v.y);
            var points = container.lines
                .Select(line => line.ToVectList())
                .SelectMany(line => line)
                .ToList();
            var cMx = points.Max(v => v.x);
            var cMy = points.Max(v => v.y);
            var cmx = points.Min(v => v.x);
            var cmy = points.Min(v => v.y);
            var border = 0.3f;
            return Mathf.Abs(oMx - cMx) <= border
                   && Mathf.Abs(oMy - cMy) <= border
                   && Mathf.Abs(omx - cmx) <= border
                   && Mathf.Abs(omy - cmy) <= border;
        }

        private void Update() => Render(Confirm);

        private void Confirm()
        {
            if (currentLineRenderer.positionCount < 2 || totalPoints > maxPoints)
            {
                Destroy(currentLineRenderer.gameObject);
                return;
            }

            currentLineRenderer.transform.SetParent(transform);

            var path =
                Enumerable
                    .Range(0, currentLineRenderer.positionCount)
                    .Select(currentLineRenderer.GetPosition)
                    .ToList();
            var path2 = path.Select(v => new Vector3(v.x, v.y)).ToList();
            container.Add(path2.ToStrList(), material, isTough, isSync);
            print(path2.ToStrList());
            currentLineRenderer = null;
        }
    }
}