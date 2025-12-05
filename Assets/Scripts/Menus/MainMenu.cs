using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    

    private void Start()
    {
        
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
