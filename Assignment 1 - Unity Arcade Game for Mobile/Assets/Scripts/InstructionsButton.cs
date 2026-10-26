using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsButtonBehavior : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void OnInstructionsButtonClicked()
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        }
        Invoke(nameof(LoadScene), 0.15f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Instructions");
    }
}