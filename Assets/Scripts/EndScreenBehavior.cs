using UnityEngine;
using TMPro;

public class EndScreenBehavior : MonoBehaviour
{
    public TMP_Text finalScoreLabel;

    void Start()
    {
        finalScoreLabel.text = "Score: " + GameController.score;
    }
}