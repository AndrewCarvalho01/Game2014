using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonBehavior : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void OnStartButtonClicked()
    {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        Invoke(nameof(LoadScene), 0.15f);   // give the click time to play
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Play");
    }
}