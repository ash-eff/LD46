using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fanfare : MonoBehaviour
{
    public TextMeshProUGUI plantInfo;
    public Button plantButton;
    public Image plantImage;
    public Sprite[] plantSprites;
    public TextMeshProUGUI plantPrice;
    public TextMeshProUGUI weaponInfo;
    public Button weaponButton;
    public Image weaponImage;
    public Sprite[] weaponSprites;
    public TextMeshProUGUI weaponPrice;
    public TextMeshProUGUI tankInfo;
    public Button tankButton;
    public Image tankImage;
    public Sprite[] tankSprites;
    public TextMeshProUGUI tankPrice;
    public TextMeshProUGUI waterInfo;
    public Button waterButton;
    public Image waterImage;
    public Sprite[] waterSprites;
    public TextMeshProUGUI waterPrice;
    PlayerController player;
    PlantController plant;
    AudioSource source;
    public int amountToSpend;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        plant = FindObjectOfType<PlantController>();
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        CheckBankAccount();
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

        if (amountToSpend >= player.waterUpgradeCost)
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
            if(plant.upgradeLevel < 3)
            {
                plantInfo.text = "Fence\n lvl " + (plant.upgradeLevel + 1).ToString();
            }
            else
            {
                plantInfo.text = "Fence\n MAX";
            }

            plantImage.sprite = plantSprites[plant.upgradeLevel];
        }

        CheckBankAccount();
        if (plant.upgradeLevel >= 3)
        {
            plantButton.interactable = false;
        }
    }

    public void UpgradeWeapon()
    {
        if (player.UpgradeWeapon())
        {
            if (player.weaponUpgradeLevel < 3)
            {
                weaponInfo.text = "Sharpen\n lvl " + (player.weaponUpgradeLevel + 1).ToString();
            }
            else
            {
                weaponInfo.text = "Sharpen\n MAX";
            }

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
            if (player.tankUpgradeLevel < 3)
            {
                tankInfo.text = "H20 Tank\n lvl " + (player.tankUpgradeLevel + 1).ToString();
            }
            else
            {
                tankInfo.text = "H20 Tank\n MAX";
            }

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
        waterInfo.text = "Freeze\n aquired";
        waterButton.interactable = false;
        CheckBankAccount();
    }
}
