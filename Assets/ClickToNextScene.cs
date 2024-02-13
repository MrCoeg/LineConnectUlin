using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToNextScene : MonoBehaviour
{
    public int sceneIndex = 2; // Name of the next scene

    void Update()
    {
        // Check for user click
        if (Input.GetMouseButtonDown(0))
        {
            // Load the next scene
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
