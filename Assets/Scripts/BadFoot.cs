using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadFoot : MonoBehaviour
{
    private ObjectPooler pool;
    public GameObject foot;

    private void Awake()
    {
        pool = FindObjectOfType<ObjectPooler>();
    }

    public void PlaceFootprint()
    {
        pool.SpawnFromPool("BadFoot", foot.transform.position, Quaternion.identity);
    }
}
