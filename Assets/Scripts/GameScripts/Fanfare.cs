using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fanfare : MonoBehaviour
{
    //public TextMeshProUGUI plantInfo;
    public Button plantButton;
    public Image plantImage;
    public Sprite[] plantSprites;
    public TextMeshProUGUI plantPrice;
    //public TextMeshProUGUI weaponInfo;
    public Button weaponButton;
    public Image weaponImage;
    public Sprite[] weaponSprites;
    public TextMeshProUGUI weaponPrice;
    //public TextMeshProUGUI tankInfo;
    public Button tankButton;
    public Image tankImage;
    public Sprite[] tankSprites;
    public TextMeshProUGUI tankPrice;
    //public TextMeshProUGUI waterInfo;
    public Button waterButton;
    public Image waterImage;
    public Sprite[] waterSprites;
    public TextMeshProUGUI waterPrice;
    PlayerController player;
    PlantController plant;
    AudioSource source;
    public int amountToSpend;
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        plant = FindObjectOfType<PlantController>();
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        CheckBankAccount();
        BankInfo();
    }

    void CheckBankAccount()
    {
        plantPrice.text = plant.upgradeCost.ToString();
        weaponPrice.text = player.weaponUpgradeCost.ToString();
        tankPrice.text = player.tankUpgradeCost.ToString();
        waterPrice.text = player.waterUpgradeCost.ToString();
        amountToSpend = player.moneyCollected;

        if (amountToSpend >= plant.upgradeCost)
            plantButton.interactable = true;
        else
            plantButton.interactable = false;

        if (amountToSpend >= player.weaponUpgradeCost)
            weaponButton.interactable = true;
        else
            weaponButton.interactable = false;

        if (amountToSpend >= player.tankUpgradeCost)
            tankButton.interactable = true;
        else
            tankButton.interactable = false;

        if (amountToSpend >= player.waterUpgradeCost && !player.freezingWater)
            waterButton.interactable = true;
        else
            waterButton.interactable = false;
    }

    public void ShootConfetti()
    {
        player.ShootConfetti();
        source.Play();
    }

    public void UpgradePlant()
    {
        if (plant.UpgradePlantLife())
        {
            CheckBankAccount();
            plantImage.sprite = plantSprites[plant.upgradeLevel];
        }

        if (plant.upgradeLevel >= 3)
        {
            plantButton.interactable = false;
        }
    }

    public void UpgradeWeapon()
    {
        if (player.UpgradeWeapon())
        {
            CheckBankAccount();
            weaponImage.sprite = weaponSprites[player.weaponUpgradeLevel];
        }

        if (player.weaponUpgradeLevel >= 3)
        {
            weaponButton.interactable = false;
        }
    }

    public void UpgradeTank()
    {
        if (player.UpgradeWaterTank())
        {
            CheckBankAccount();
            tankImage.sprite = tankSprites[player.tankUpgradeLevel];
        }

        if (player.tankUpgradeLevel >= 3)
        {
            tankButton.interactable = false;
        }
    }

    public void UpgradeWater()
    {
        player.UpgradeWaterToFreeze();
        waterImage.sprite = waterSprites[1];
        waterButton.interactable = false;
        CheckBankAccount();
    }

    public void BankInfo()
    {
        infoText.text = "You have " + amountToSpend.ToString() + " to spend in the shop.";
    }

    public void FenceInfo()
    {
        infoText.text = "Build a fence around your plant friend. Gives the plant extra sustainability. Increases plant life to full on each upgrade.";
    }

    public void FreezingInfo()
    {
        infoText.text = "instead of pushing your enemies back, freeze them in place for a few seconds.";
    }

    public void WeaponInfo()
    {
        infoText.text = "Sharpen those shears so they do more damage!";
    }

    public void H20Info()
    {
        infoText.text = "Increases the amount of damage your water hose can heal. Increases water amount to full on each upgrade.";
    }
}
