using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorClicked;
    private Vector2 hotSpot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    public static CursorManager Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorClicked, hotSpot, cursorMode);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
        }
    }
}