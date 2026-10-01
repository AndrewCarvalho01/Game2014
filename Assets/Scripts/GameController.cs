using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text livesLabel;
    public TMP_Text scoreLabel;

    private float livesLabelHalfHeight;
    private float livesLabelHalfWidth;
    private float scoreLabelHalfHeight;
    private float scoreLabelHalfWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        livesLabelHalfHeight = livesLabel.rectTransform.rect.height * 0.5f;
        livesLabelHalfWidth = livesLabel.rectTransform.rect.width * 0.5f;
        scoreLabelHalfHeight = scoreLabel.rectTransform.rect.height * 0.5f;
        scoreLabelHalfWidth = scoreLabel.rectTransform.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        livesLabel.rectTransform.position =
            new Vector2(
                Screen.safeArea.xMin + livesLabelHalfWidth,
                Screen.safeArea.yMax - livesLabelHalfHeight
            );

        scoreLabel.rectTransform.position =
            new Vector2(
                Screen.safeArea.xMax - scoreLabelHalfWidth,
                Screen.safeArea.yMax - scoreLabelHalfHeight
            );
    }
}
