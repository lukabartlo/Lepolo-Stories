using UnityEngine;

public abstract class AoeSpell : SpellBehavior
{
    protected float radius;

    private GameObject previewSphere;
    private LayerMask terrainLayer = 1 << 0;

    private void Start()
    {
        radius = data.aoeRadius;
        CreatePreview();
    }

    private void Update()
    {
        UpdatePreviewPosition();
    }

    private void OnDestroy()
    {
        DestroyPreview();
    }

    void CreatePreview()
    {
        Vector3 spawnPos = new Vector3(0, -1000, 0);

        if (data.aoePreviewPrefab != null)
            previewSphere = Instantiate(data.aoePreviewPrefab, spawnPos, Quaternion.identity);
        else
            previewSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        previewSphere.transform.localScale = Vector3.one * radius * 2f;

        Collider col = previewSphere.GetComponent<Collider>();
        if (col) col.enabled = false;
    }


    void UpdatePreviewPosition()
    {
        if (!previewSphere) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 2000f, terrainLayer))
        {
            previewSphere.transform.position = hit.point + Vector3.up * 0.02f;
        }
    }

    void DestroyPreview()
    {
        if (previewSphere)
            GameObject.Destroy(previewSphere);
    }

    public override void CastSpell(RaycastHit hit)
    {
        DestroyPreview();

        Collider[] hits = Physics.OverlapSphere(hit.point, radius);

        foreach (Collider c in hits)
        {
            IDamageable target = c.GetComponent<IDamageable>();
            if (target != null)
                UseSpell(target);
        }
    }
}
