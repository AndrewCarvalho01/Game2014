using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private int rowCount = 3;
    [SerializeField] private float firstRowOffset = 1.0f; // distance from the top of the screen to the first row
    [SerializeField] private float rowSpacing = 1.3f;     // distance between rows
    [SerializeField] private float spawnDelay = 1.0f;     // pause between spawns

    private GameObject[] rows;
    private float timer;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        rows = new GameObject[rowCount];
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
        {
            return;
        }
        // Finds an empty row and spawns an enemy in it. If all rows are full, it does nothing.
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i] == null || !rows[i].activeInHierarchy)
            {
                SpawnInRow(i);
                timer = spawnDelay;
                return;
            }
        }
    }
    private void SpawnInRow(int row)
    {
        float top = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
        GameObject enemy = enemyPool.Get(new Vector3(0f, top + 1f, 0f));
        EnemyBehavior behavior = enemy.GetComponent<EnemyBehavior>();
        behavior.bulletPool = bulletPool;
        behavior.targetY = top - firstRowOffset - row * rowSpacing;

        rows[row] = enemy;
    }
}