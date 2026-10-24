using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsButton : MonoBehaviour
{
    public void OnInstructionsButtonClicked()
    {
        SceneManager.LoadScene("Instructions");
    }
}