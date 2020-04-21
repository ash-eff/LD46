using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorLerp : MonoBehaviour
{
    public Color A = Color.magenta;
    public Color B = Color.blue;
    public float speed = 1.0f;

    public TextMeshProUGUI text;

    void Update()
    {
        text.color = Color.Lerp(A, B, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}
