using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButtonBehavior : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void OnResetButtonClicked()
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        }
        Invoke(nameof(LoadScene), 0.15f);   // give the click time to play
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Start");
    }
}