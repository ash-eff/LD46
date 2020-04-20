using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : MonoBehaviour
{
    public GameObject waterSplash;
    public GameObject hoseTip;
    LineRenderer lr;

    private void Awake()
    {
        lr = GetComponentInChildren<LineRenderer>();
    }

    public void SetHoseEndPos(Vector2 pos)
    {
        lr.SetPosition(0, hoseTip.transform.position);
        lr.SetPosition(1, pos);
        waterSplash.transform.position = pos;
    }

    public void ResetHosePos()
    {
        lr.SetPosition(0, hoseTip.transform.position);
        lr.SetPosition(1, hoseTip.transform.position);
    }

    public void SetWaterSplashActive(bool b)
    {
        waterSplash.gameObject.SetActive(b);
    }
}
