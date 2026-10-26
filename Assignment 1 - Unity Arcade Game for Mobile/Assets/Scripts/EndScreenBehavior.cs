using UnityEngine;
using TMPro;

// Shows the final score from the game that just ended.
public class EndScreenBehavior : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreLabel;

    void Start()
    {
        finalScoreLabel.text = "Final Score: " + GameController.Score;
    }
}