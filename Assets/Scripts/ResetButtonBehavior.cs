using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButtonBehavior : MonoBehaviour
{
    public void OnResetButtonClicked()
    {
        Debug.Log("Reset button clicked");
        SceneManager.LoadScene("Start");
    }
}