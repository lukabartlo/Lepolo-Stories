using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    private SpellData activeSpell;
    private Spell Spell;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ActivateSpell(SpellData newSpell)
    {
        if (activeSpell != null && activeSpell.spellID == newSpell.spellID)
        {
            CancelSpell();
            return;
        }

        activeSpell = newSpell;
        Cursor.SetCursor(activeSpell.cursorTexture, Vector2.zero, CursorMode.Auto);

        switch (activeSpell.type)
        {
            case SpellEnum.clawSlashes:
                Spell = gameObject.AddComponent<ClawSlashes>();
                break;
            case SpellEnum.catNip:
                Spell = gameObject.AddComponent<Catnip>();
                break;
            case SpellEnum.changeRole:
                Spell = gameObject.AddComponent<ChangeRole>();
                break;
            case SpellEnum.spawnRessources:
                Spell = gameObject.AddComponent<SpawnRessources>();
                break;
        }

        Spell.data = activeSpell;
    }

    public void CancelSpell()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        activeSpell = null;
        if (Spell) Destroy(Spell);
    }

    void Update()
    {
        if (activeSpell == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawLine(ray.origin, ray.GetPoint(100f), Color.green, 20f);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Spell.CastSpell(hit);
                CancelSpell();
            }
        }
    }
}