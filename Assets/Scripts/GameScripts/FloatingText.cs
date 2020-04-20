using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    public void SetDisplay(string s)
    {
        displayText.text = s;
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

}
