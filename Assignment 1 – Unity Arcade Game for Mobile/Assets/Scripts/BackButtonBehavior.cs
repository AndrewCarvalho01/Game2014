using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBackButtonClicked()
    {
        Debug.Log("Back button clicked");
        SceneManager.LoadScene("Start");
    }
}