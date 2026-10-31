using UnityEngine;

// The two kinds of bullet in the game.
public enum BulletType
{
    Player,
    Enemy
}

// Factory pattern: this is the one place that knows how to build a bullet.
// The bullet pools ask the factory for new bullets instead of calling Instantiate themselves,
// so if a bullet ever needs to be built differently, only this script changes.
public class BulletFactory : MonoBehaviour
{
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private GameObject enemyBulletPrefab;

    private int bulletsCreated;

    // Builds a new, hidden bullet of the requested type as a child of the given parent.
    public GameObject CreateBullet(BulletType type, Transform parent)
    {
        GameObject prefab = (type == BulletType.Player) ? playerBulletPrefab : enemyBulletPrefab;

        GameObject bullet = Instantiate(prefab, parent);
        bullet.name = type + "Bullet_" + bulletsCreated;
        bullet.SetActive(false);

        bulletsCreated++;
        return bullet;
    }
}