using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int minVal;
    public int maxVal;
    private int coinVal;
    ObjectPooler pool;
    PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        pool = FindObjectOfType<ObjectPooler>();
        coinVal = Random.Range(minVal, maxVal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Hello");
            GameObject obj = pool.SpawnFromPool("FloatingText", transform.position, Quaternion.identity);
            obj.GetComponent<FloatingText>().SetDisplay("+ " + coinVal.ToString());
            player.CollectMoney(coinVal);
            gameObject.SetActive(false);
        }
    }
}
