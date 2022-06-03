using System.Linq;
using UnityEngine;

public class MouseDraw : SelfDraw
{
    public Game game;
    public Material terrainMaterial;

    private void Update() => Render(Flush);

    private void Flush()
    {
        currentLineRenderer.material = terrainMaterial;
        var path = 
            Enumerable
                .Range(0, currentLineRenderer.positionCount)
                .Select(currentLineRenderer.GetPosition)
                .ToList();
        var path2 = path.Select(v => new Vector2(v.x, v.y)).ToList();
        game.AddTerrains(path2.ToList(), currentLineRenderer);
        currentLineRenderer = null;
    }

}