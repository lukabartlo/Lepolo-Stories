using UnityEngine;

public abstract class InteractionMapSpell : Spell
{
    public override void CastSpell(RaycastHit hit)
    {
        Vector2Int coords = WorldToGrid(hit.point);
        UseSpell(coords);
    }

    private Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x);
        int y = Mathf.FloorToInt(worldPosition.z);

        return new Vector2Int(x, y);
    }
    protected abstract void UseSpell(Vector2Int coords);
}
