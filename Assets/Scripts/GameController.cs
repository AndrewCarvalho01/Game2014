using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text scoreLabel;
    public float secondsPerPoint = 10.0f;
    public float padding = 40.0f;

    // Static so it survives loading the End scene, which shows it.
    public static int score;

    private float timeAlive;
    private float scoreLabelHalfHeight;
    private float scoreLabelHalfWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        timeAlive = 0.0f;

        scoreLabelHalfHeight = scoreLabel.rectTransform.rect.height * 0.5f;
        scoreLabelHalfWidth = scoreLabel.rectTransform.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // 1 point for every 10 seconds alive.
        timeAlive += Time.deltaTime;
        score = (int)(timeAlive / secondsPerPoint);
        scoreLabel.text = "Score: " + score;

        // Keep the label in the top-right corner of the safe area, even after rotating.
        scoreLabel.rectTransform.position =
    new Vector2(
        Screen.safeArea.xMax - scoreLabelHalfWidth - padding,
        Screen.safeArea.yMax - scoreLabelHalfHeight - padding
    );
    }
}