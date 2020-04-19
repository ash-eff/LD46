using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantController : MonoBehaviour
{
    public float life;
    public float maxLife;
    private int decayAmount = 1;
    public bool isRegening = false;
    public Image lifeAmountFillBar;
    public GameObject indicator;
    public TextMeshProUGUI plantDistText;

    private float timer = 0;

    private GameController gc;
    private Camera cam;

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckScreenPos();
        if (gc.stateMachine.currentState == GCPlayState.Instance && !isRegening)
        {
            Decay();
        }

        if(timer > 0)
        {
            isRegening = true;
            timer -= Time.deltaTime;

        }
        else
        {
            isRegening = false;
        }

        lifeAmountFillBar.fillAmount = life / maxLife;
    }

    public void TakeDamage(float amount)
    {
        life -= amount;
    }

    public void Regen(float amount)
    {
        timer = 2f;
        life += amount;
        CheckLife();
    }

    private void Decay()
    {
        if (life > 0)
        {
            life -= decayAmount * Time.deltaTime;
        }
        else
        {
            Die();
        }

        CheckLife();
    }

    private void Die()
    {
        Debug.Log("Plant is dead");
    }

    private void CheckLife()
    {
        if(life >= maxLife)
        {
            life = maxLife;
        }
        else if(life <= 0)
        {
            life = 0;
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
        plantDistText.text = directionToMe.magnitude.ToString("00") + "m";
        float clampDirX = Mathf.Clamp(directionToMe.x, -24.75f, 24.75f);
        float clampDirY = Mathf.Clamp(directionToMe.y, -14f, 14f);
        indicator.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y) + new Vector2(clampDirX, clampDirY);
    }
}
