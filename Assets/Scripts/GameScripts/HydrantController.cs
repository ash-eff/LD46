using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HydrantController : MonoBehaviour
{
    public float waterAmount = 100f;
    public float maxWaterAmount = 100f;
    public float refillAmount = 2f;
    public Image waterPressureFill;
    public GameObject pumpText;
    public GameObject indicator;
    public TextMeshProUGUI hydrantDistText;
    private PlayerController player;
    private bool canRefill;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(RefillPump());
    }

    private void Update()
    {
        CheckScreenPos();
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

    void CheckScreenPos()
    {
        Vector2 screenPos = cam.WorldToViewportPoint(transform.position); //get viewport positions

        if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
        {
            indicator.SetActive(false);
            return;
        }

        indicator.SetActive(true);
        Vector2 centerPos = cam.transform.position;
        Vector2 directionToMe = MyUtils.Direction2D(centerPos, transform.position);
        hydrantDistText.text = directionToMe.magnitude.ToString("00") + "m";
        float clampDirX = Mathf.Clamp(directionToMe.x, -23.75f, 23.75f);
        float clampDirY = Mathf.Clamp(directionToMe.y, -13f, 13f);
        indicator.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y) + new Vector2(clampDirX, clampDirY);
    }
}
