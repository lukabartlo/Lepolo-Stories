using UnityEngine;
using UnityEngine.UI;

public class LinkButton : MonoBehaviour {
    private string _url;
    
    [SerializeField] private Image image;
    
    public void SetupVariables(string _newUrl, Sprite _sprite) {
        _url = _newUrl;
        image.sprite = _sprite;
    }

    public void OpenUrl() {
        Application.OpenURL(_url);
    }
}
