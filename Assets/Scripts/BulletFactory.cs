using System.Collections;
using UnityEngine;
using static BulletType;

public class BulletFcatory : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyBullet;
    [SerializeField]
    private GameObject playerBullet;

    public GameObject CreateBullet(Bullet type = Bullet.Enemy)
    {
        GameObject tempBullet = null;

        switch (type)
        {
            case Bullet.Enemy:
                tempBullet = Instantiate(enemyBullet);
                break;
            case Bullet.Player:
                tempBullet = Instantiate(playerBullet);
                break;
        }
        tempBullet.transform.SetParent(transform);
        tempBullet.SetActive(false);
        return tempBullet;
    }
}
