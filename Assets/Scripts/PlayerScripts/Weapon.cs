using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerController player;
    public GameObject castPoint;
    public LayerMask enemyLayer;
    public Collider2D[] enemies;
    private ObjectPooler pool;
    private string[] bloodList = { "Blood1", "Blood2", "Blood3", "Blood4" };

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        pool = FindObjectOfType<ObjectPooler>();
    }

    public void AttackEnemies()
    {
        enemies = Physics2D.OverlapCircleAll(castPoint.transform.position, 2f, enemyLayer);
        if(enemies != null)
        {
            foreach(Collider2D enemy in enemies)
            {
                enemy.GetComponent<BaddieController>().TakeDamage(player.weaponDamage);
                pool.SpawnFromPool(bloodList[Random.Range(0, bloodList.Length)], castPoint.transform.position, transform.rotation);
            }
        }
    }
}
