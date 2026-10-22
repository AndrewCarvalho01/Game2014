using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonBehavior : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        Debug.Log("Back button clicked");
        SceneManager.LoadScene("Start");
    }
}