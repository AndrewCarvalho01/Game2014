using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButtonBehavior : MonoBehaviour
{
   public void OnNextButtonClicked()
    {
        Debug.Log("Next button clicked");
        SceneManager.LoadScene("End");
    }
}