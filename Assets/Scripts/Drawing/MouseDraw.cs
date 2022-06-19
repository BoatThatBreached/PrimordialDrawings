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
            Render(() =>
                {
                    PlayerInfo.EarthLeft -= 0.1f;

                    if (PlayerInfo.EarthLeft < 0)
                    {
                        PlayerInfo.EarthLeft = 0;
                        return true;
                    }

                    return false;
                },
                Flush);
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

            PlayerInfo.Platforms.Add(path);
            var path2 = path.Select(v => new Vector2(v.x, v.y)).ToList();
            game.AddTerrains(path2.ToList(), currentLineRenderer);
            currentLineRenderer = null;
        }
    }
}