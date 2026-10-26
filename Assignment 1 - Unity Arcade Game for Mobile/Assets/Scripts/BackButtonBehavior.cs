using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonBehavior : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void OnBackButtonClicked()
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        }
        Invoke(nameof(LoadScene), 0.15f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Start");
    }
}