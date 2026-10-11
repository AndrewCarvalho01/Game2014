using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonBehavior : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked");
        SceneManager.LoadScene("Play");
    }
}