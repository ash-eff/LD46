using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantController : MonoBehaviour
{
    public float life;
    public float maxLife;
    public int decayAmount = 2;
    public bool isRegening = false;
    public Image lifeAmountFillBar;
    public GameObject indicator;
    public TextMeshProUGUI plantDistText;
    PlayerController player;
    public Fence[] fences;

    public int upgradeLevel = 0;
    public int upgradeCost;

    private float timer = 0;

    private GameController gc;
    private Camera cam;

    public bool fenceBroken;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        gc = FindObjectOfType<GameController>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckScreenPos();
        fenceBroken = life < (maxLife / 2) ? true : false;
        fences[0].IsFenceBroken(fenceBroken);
        fences[1].IsFenceBroken(fenceBroken);
        fences[2].IsFenceBroken(fenceBroken);
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
        gc.stateMachine.ChangeState(GCGameOverState.Instance);
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
        float clampDirX = Mathf.Clamp(directionToMe.x, -23.75f, 23.75f);
        float clampDirY = Mathf.Clamp(directionToMe.y, -13f, 13f);
        indicator.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y) + new Vector2(clampDirX, clampDirY);
    }

    public bool UpgradePlantLife()
    {
        if (upgradeLevel >= 3)
            return false;
        else
        {
            if(upgradeLevel > 0)
            {
                fences[upgradeLevel - 1].gameObject.SetActive(false);
            }
            fences[upgradeLevel].gameObject.SetActive(true);
            upgradeLevel++;
            
            player.moneyCollected -= upgradeCost;
            upgradeCost += (upgradeCost * upgradeLevel);
            if (upgradeLevel == 1 || upgradeLevel == 2)
            {
                maxLife += 25f;
            }
            else
            {
                maxLife += 50f;
            }
            life = maxLife;
            return true;
        }
    }
}
