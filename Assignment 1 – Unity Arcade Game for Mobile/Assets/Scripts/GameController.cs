using UnityEngine;
using TMPro;

// Keeps the score and shows the score and lives on the HUD.
public class GameController : MonoBehaviour
{
    public static int Score { get; private set; }
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text livesLabel;
    [SerializeField] private PlayerBehavior player;

    void Start()
    {
        // Score is static so it resets every game. Scene changes don't effect it. 
        Score = 0;
    }

    void Update()
    {
        scoreLabel.text = "Score: " + Score;
        livesLabel.text = "Lives: " + player.Lives;
    }

    // Sharks and pickups call this to add points: GameController.AddPoints(10);
    public static void AddPoints(int points)
    {
        Score += points;
    }
}