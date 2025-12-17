using UnityEngine;
using UnityEngine.Playables;
public class PlayTimeLine : MonoBehaviour
{
    public  PlayableDirector playableDirector;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playableDirector.Play();
        }
    }
}
