using UnityEngine;

public class SpawnRessources : InteractionMapSpell
{
    private BuildingSystem buildingSystem;

    private void Start()
    {
        buildingSystem = TestBuildManager.Instance.buildingSystem;
    }
    protected override void UseSpell(Vector2Int coords)
    {
        if (gm.currentMana < data.spellCost)
            return;
        
        if (buildingSystem.TryBuild(data.obj, coords))
        {
            ConsumeMana(gm);

            buildingSystem.TryBuild(data.obj, coords);
        }
    }
}
