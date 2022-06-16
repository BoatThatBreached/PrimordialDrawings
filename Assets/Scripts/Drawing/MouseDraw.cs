using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Drawing
{
    public class MouseDraw : SelfDraw
    {
        public Game game;
        public Material terrainMaterial;

        private void Update()
        {
            // PlayerInfo.Paints["earth"] = (maxPoints - totalPoints) / maxPoints;
            // if (totalPoints > maxPoints)
            //     totalPoints = maxPoints;
            Render(Flush);
        }

        private void Flush()
        {
            currentLineRenderer.material = terrainMaterial;
            var path = 
                Enumerable
                    .Range(0, currentLineRenderer.positionCount)
                    .Select(currentLineRenderer.GetPosition)
                    .DividedPoints(0.3f)
                    .ToList();
            if (!PlayerInfo.Platforms.ContainsKey(PlayerInfo.CurrentLevel))
                PlayerInfo.Platforms[PlayerInfo.CurrentLevel] = new List<List<Vector3>>();
            PlayerInfo.Platforms[PlayerInfo.CurrentLevel].Add(path);
            var path2 = path.Select(v => new Vector2(v.x, v.y)).ToList();
            game.AddTerrains(path2.ToList(), currentLineRenderer);
            currentLineRenderer = null;
        }

    }
}