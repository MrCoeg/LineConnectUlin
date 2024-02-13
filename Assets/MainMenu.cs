using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int nextSceneIndex = 3; // Name of the next scene

    // This function is called when the "Play" button is clicked
    public void Play()
    {
        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

    // You can add more functions for other menu options if needed
    // For example, a Quit function to exit the game

    public void Quit()
    {
        // This is just an example, you might want to handle quitting differently
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
