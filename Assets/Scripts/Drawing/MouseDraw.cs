using System.Linq;
using UnityEngine;

public class MouseDraw : SelfDraw
{
    public Game game;
    public Material terrainMaterial;

    private void Update()
    {
        PlayerInfo.Paints["earth"] = (maxPoints - totalPoints) / maxPoints;
        if (totalPoints > maxPoints)
            totalPoints = maxPoints;
        Render(Flush);
    }

    private void Flush()
    {
        currentLineRenderer.material = terrainMaterial;
        var path = 
            Enumerable
                .Range(0, currentLineRenderer.positionCount)
                .Select(currentLineRenderer.GetPosition)
                .ToList();
        var path2 = path.Select(v => new Vector2(v.x, v.y)).ToList();
        totalPoints += path2.Count;
        game.AddTerrains(path2.ToList(), currentLineRenderer);
        currentLineRenderer = null;
    }

}