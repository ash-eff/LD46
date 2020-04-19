using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrantController : MonoBehaviour
{
    public float waterAmount = 100f;
    public float maxWaterAmount = 100f;
    public float refillAmount = 2f;
    public Image waterPressureFill;
    public GameObject pumpText;
    private PlayerController player;
    private bool canRefill;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(RefillPump());
    }

    private void Update()
    {
        waterPressureFill.fillAmount = waterAmount / maxWaterAmount;
        if (waterAmount == maxWaterAmount)
            canRefill = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.touchingHydrant = true;
            player.currentHydrant = this;
            pumpText.SetActive(true);
            canRefill = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.touchingHydrant = true;
            player.currentHydrant = null;
            pumpText.SetActive(false);
            canRefill = true;
        }
    }

    IEnumerator RefillPump()
    {
        while (true)
        {
            if (!canRefill)
            {
                // wait here
            }
            else
            {
                waterAmount += Time.deltaTime * refillAmount;
            }
            yield return null;
        }
    }
}
