using UnityEngine;
using TMPro;

// Keeps the score, gives a bonus life every 1000 points, and shows the score and lives on the HUD.
public class GameController : MonoBehaviour
{
    public static int Score { get; private set; }

    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text livesLabel;
    [SerializeField] private PlayerBehavior player;

    [Header("Bonus Life")]
    [SerializeField] private int extraLifeEvery = 1000;
    [SerializeField] private AudioClip extraLifeSound;

    private int nextExtraLife;

    void Start()
    {
        // Score is static, so it survives scene changes. Reset it at the start of every game.
        Score = 0;
        nextExtraLife = extraLifeEvery;
    }

    void Update()
    {
        // Give a life each time the score passes the next 1000-point milestone.
        while (Score >= nextExtraLife)
        {
            player.AddLife();
            nextExtraLife += extraLifeEvery;

            if (extraLifeSound != null)
            {
                AudioSource.PlayClipAtPoint(extraLifeSound, Camera.main.transform.position);
            }
            Debug.Log("Extra life! Lives now: " + player.Lives);
        }

        scoreLabel.text = "Score: " + Score;
        livesLabel.text = "Lives: " + player.Lives;
    }

    // Sharks and pickups call this to add points: GameController.AddPoints(10);
    public static void AddPoints(int points)
    {
        Score += points;
    }
}