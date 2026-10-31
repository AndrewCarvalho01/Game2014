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
        // Score is static, so it survives scene changes. Reset it at the start of every game.
        Score = 0;
    }

    void Update()
    {
        scoreLabel.text = "Score: " + Score;
        livesLabel.text = "Lives: " + player.Lives;
    }

    // Player bullets call this when they destroy an enemy: GameController.AddPoints(10);
    public static void AddPoints(int points)
    {
        Score += points;
    }
}