using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButtonBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnResetButtonClicked()
    {
        Debug.Log("Reset button clicked");
        SceneManager.LoadScene("Start");
    }
}