using Managers.Audio;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///     For add an event call : EventManager.Instance.Event.AddListener(Function);
///     The function should have the shape : UnityEvent
///     <params>
///         take Function(params)
///         For call an event just add this line : EventManager.Instance.Event.Invoke(params);
///         this will call all function you have add to your event
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }

    // Audio
    public UnityEvent<SoundsName> onPlayMusic { get; private set; } = new();
    public UnityEvent onPauseMusic { get; private set; } = new();
    public UnityEvent onStopMusic { get; private set; } = new();

    public UnityEvent<SoundsName> onPlaySfx { get; private set; } = new();
    public UnityEvent onPauseSfx { get; private set; } = new();
    public UnityEvent onStopSfx { get; private set; } = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }
}
