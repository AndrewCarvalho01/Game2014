using UnityEngine;
using UnityEngine.SceneManagement;

// Builds a grid of sharks, swims them in from the top, then sweeps them side to side,
// stepping down at each edge like Space Invaders. Also decides when a shark may fire.
public class SharkFormation : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GameObject sharkPrefab;
    [SerializeField] private int rows = 3;
    [SerializeField] private int columns = 4;         
    [SerializeField] private float spacingX = 1.0f;
    [SerializeField] private float spacingY = 1.1f;
    [SerializeField, Range(0.3f, 0.9f)] private float fillWidth = 0.75f;

    [Header("Movement")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float stepDown = 0.2f;
    [SerializeField] private float edgePadding = 0.5f;

    [Header("Entrance")]
    [SerializeField] private float entrySpeed = 2.0f;
    [SerializeField] private float topOffset = 0.8f;

    [Header("Shooting")]
    [SerializeField] private float minTimeBetweenShots = 0.6f;

    [SerializeField] private Transform thresholdLine;
    private float sharkHalfHeight;

    [SerializeField] private GameObject hunterPrefab;
    [SerializeField, Range(0.0f, 1.0f)] private float hunterChance = 0.1f;

    public bool IsEntering => entering;
    private Camera cam;
    private bool entering = true;
    private float targetY;
    private int direction = 1; // 1 = right, -1 = left
    private float lastShotTime = -100.0f;

    void Start()
    {
        cam = Camera.main;

        SpawnGrid();
        PrepareEntrance();
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            return;   // If every shark is gone
        }

        if (entering)
        {
            SwimIn();
        }
        else
        {
            Sweep();
        }
    }

    // Setup 
    private void SpawnGrid()
    {
        GetEdges(out float left, out float right);

        // Fit as many columns as cover fillWidth of the screen (+1 turns gaps into sharks).
        float usableWidth = right - left;
        columns = Mathf.Max(1, Mathf.FloorToInt(usableWidth * fillWidth / spacingX) + 1);

        // Centre the grid on the formation.
        float startX = -(columns - 1) * spacingX / 2.0f;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // Each shark is a child, so moving the formation moves every shark.
                GameObject prefab = (Random.value < hunterChance) ? hunterPrefab : sharkPrefab;
                GameObject shark = Instantiate(prefab, transform);
                shark.transform.localPosition = new Vector3(startX + c * spacingX, -r * spacingY, 0.0f);
                sharkHalfHeight = shark.GetComponent<SpriteRenderer>().bounds.extents.y;
            }
        }
    }

    private void PrepareEntrance()
    {
        float top = cam.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y;
        targetY = top - topOffset;

        // Start with the whole grid (including the bottom row) above the screen.
        float startY = top + (rows - 1) * spacingY + 1.0f;
        transform.position = new Vector3(transform.position.x, startY, 0.0f);
    }

    //  Movement 
    private void SwimIn()
    {
        float y = Mathf.MoveTowards(transform.position.y, targetY, entrySpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, y, 0.0f);

        if (Mathf.Approximately(y, targetY))
        {
            entering = false;
        }
    }
    private void Sweep()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        GetEdges(out float left, out float right);

        // Find the outer sharks that are still alive.
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;

        foreach (Transform shark in transform)
        {
            minX = Mathf.Min(minX, shark.position.x);
            maxX = Mathf.Max(maxX, shark.position.x);
            minY = Mathf.Min(minY, shark.position.y);
        }

        // Checking direction too means it turns only once per edge.
        if (direction == 1 && maxX > right)
        {
            direction = -1;
            transform.position += Vector3.down * stepDown;
        }
        else if (direction == -1 && minX < left)
        {
            direction = 1;
            transform.position += Vector3.down * stepDown;
        }

        // Game over if the lowest shark's nose reaches the line.
        if (thresholdLine != null && minY - sharkHalfHeight <= thresholdLine.position.y)
        {
            SceneManager.LoadScene("End");
        }
    }
    private void GetEdges(out float left, out float right)
    {
        left = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x + edgePadding;
        right = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)).x - edgePadding;
    }

    //  Shooting 
    // Sharks call this before firing. Only one shark may fire every minTimeBetweenShots.
    public bool RequestShot()
    {
        if (Time.time - lastShotTime < minTimeBetweenShots)
        {
            return false;
        }

        lastShotTime = Time.time;
        return true;
    }
}