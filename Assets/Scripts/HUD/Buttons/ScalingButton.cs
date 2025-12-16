using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScalingButton : MonoBehaviour
{
    [SerializeField] private float scaleFactor;
    [SerializeField] private float duration;

    private Button _button;

    private void Start() {
        _button = GetComponent<Button>();
    }

    public void ScaleButton(bool _doScale) {
        float _scaleFactor = _doScale ? scaleFactor : 1f;
        _button.transform.DOScale(_scaleFactor, duration);
    }
}