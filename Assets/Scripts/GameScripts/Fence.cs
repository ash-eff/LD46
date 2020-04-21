using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    public SpriteRenderer spr;
    public Sprite brokenFront;
    public Sprite repairedFront;

    public void IsFenceBroken(bool b)
    {
        if (b)
            spr.sprite = brokenFront;
        else
            spr.sprite = repairedFront;
    }
}
