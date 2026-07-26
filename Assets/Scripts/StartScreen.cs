using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public string sceneToLoad;

    public void Play()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}