using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> stateMachine;
    public static PlayerController player;

    public float weaponDamage;
    public float cursorMaxRadius;
    public LayerMask waterHitLayers;
    public float rateOfSpray;
    public float rateOfSwing;
    private float lastSpray;
    private float lastSwing;
    public float waterAmount;
    public float waterMaxAmount;
    public float waterRegenAmount;
    public Image waterAmountFillBar;
    public GameObject weaponHolder;
    public GameObject backScissors;
    public SpriteRenderer cutters;
    public SpriteRenderer hose;
    public Hose hoseSprayer;
    public ParticleSystem confettiOne;
    public ParticleSystem confettiTwo;
    public GameObject foot;

    public int moneyCollected;

    public int weaponUpgradeLevel;
    public int tankUpgradeLevel;
    public bool freezingWater;

    public int weaponUpgradeCost;
    public int waterUpgradeCost;
    public int tankUpgradeCost;

    public bool touchingHydrant;
    public HydrantController currentHydrant;

    [SerializeField]
    private SpriteRenderer playerSprite;
    
    public SpriteRenderer cursorSprite;

    private Vector2 movement;
    private Rigidbody2D rb2d;
    private Weapon weapon;
    public GameController gc;
    private Camera cam;
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip coinPickup;
    public TextMeshProUGUI moneyText;
    public ObjectPooler pool;

    private void Awake()
    {
        pool = FindObjectOfType<ObjectPooler>();
        hoseSprayer = hose.GetComponent<Hose>();
        backScissors.SetActive(true);
        hose.gameObject.SetActive(true);
        cutters.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        player = this;
        stateMachine = new StateMachine<PlayerController>(player);
        stateMachine.ChangeState(PlayerWaitState.Instance);
        weapon = cutters.GetComponent<Weapon>();
        gc = FindObjectOfType<GameController>();
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void PlayerInput()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (touchingHydrant && currentHydrant != null && currentHydrant.waterAmount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(waterAmount + 5 >= waterMaxAmount)
                {
                    waterAmount = waterMaxAmount;
                }
                else
                {
                    currentHydrant.waterAmount -= 5f;
                    waterAmount += 5f;
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            SprayHose();
            backScissors.SetActive(true);
            hose.gameObject.SetActive(true);
            cutters.gameObject.SetActive(false);
        }
        else
        {
            hoseSprayer.SetWaterSplashActive(false);
            hoseSprayer.ResetHosePos();
        }

        if (Input.GetMouseButtonDown(0))
        {
            SwingWeapon();
            backScissors.SetActive(false);
            hose.gameObject.SetActive(false);
            cutters.gameObject.SetActive(true);
        }
        //else
        //{
        //    backScissors.SetActive(true);
        //    hose.gameObject.SetActive(true);
        //    cutters.gameObject.SetActive(false);
        //}

        waterAmountFillBar.fillAmount = waterAmount / waterMaxAmount;
    }

    public void SetPlayerVelocity(float _atSpeed, bool allowMovement)
    {
        rb2d.velocity = allowMovement ? new Vector2(movement.x, movement.y) * _atSpeed : Vector2.zero;
    }

    public void SetSpriteDirection()
    {
        if (movement.x != 0)
        {
            playerSprite.transform.localScale = new Vector2(movement.x, 1f);
        }
    }

    public void PlaceFootprint()
    {
        pool.SpawnFromPool("Foot", foot.transform.position, Quaternion.identity);
    }

    public void ClampPlayerToMap()
    {
        Vector2 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(transform.position.x, -gc.HalfMapWidth(), gc.HalfMapWidth());
        clampedPos.y = Mathf.Clamp(transform.position.y, -gc.HalfMapHeight(), gc.HalfMapHeight());
        transform.position = clampedPos;
    }

    public void UpdateMoney()
    {
        moneyText.text = moneyCollected.ToString();
    }

    public void CheckAnimation()
    {
        if(movement.x > 0 || movement.x < 0 || movement.y > 0 || movement.y < 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            SetPlayerIdle();
        }
    }

    public void SetPlayerIdle()
    {
        anim.SetBool("IsRunning", false);
    }

    private void SprayHose()
    {
        if (waterAmount > 0 && stateMachine.currentState == PlayerBaseState.Instance)
        {
            hoseSprayer.SetWaterSplashActive(true);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, MyUtils.Direction2D(transform.position, cursorSprite.transform.position), MyUtils.DistanceBetweenObjects(transform.position, cursorSprite.transform.position), waterHitLayers);
            Debug.DrawLine(transform.position, cursorSprite.transform.position, Color.blue, rateOfSpray);
            if (hit)
            {
                hoseSprayer.SetHoseEndPos(hit.point);
                HitTargetWithWater(hit.collider);
            }
            else
            {
                hoseSprayer.SetHoseEndPos(cursorSprite.transform.position);
            }
        }
        else
        {
            hoseSprayer.ResetHosePos();
            hoseSprayer.SetWaterSplashActive(false);
        }
    }

    private void SwingWeapon()
    {
        if (Time.time > rateOfSwing + lastSwing)
        {
            lastSwing = Time.time;
        }
    }

    public void RotateWeapons()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 weaponPos = cam.WorldToScreenPoint(weaponHolder.transform.position);
        mousePos.x = mousePos.x - weaponPos.x;
        mousePos.y = mousePos.y - weaponPos.y;
        float angle = MyUtils.GetAngleTo(mousePos);
        weaponHolder.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void HitTargetWithWater(Collider2D target)
    {
        if(target.tag == "Plant")
        {
            PlantController plant = target.GetComponent<PlantController>();

            if (Time.time > rateOfSpray + lastSpray)
            {
                waterAmount -= waterRegenAmount;
                lastSpray = Time.time;
                plant.Regen(waterRegenAmount);
            }
        }

        if(target.tag == "Baddie")
        {
            BaddieController baddie = target.GetComponent<BaddieController>();
            if (Time.time > rateOfSpray + lastSpray)
            {
                waterAmount -= waterRegenAmount;
                lastSpray = Time.time;
            }

            if (freezingWater)
            {
                if (!baddie.isFrozen)
                {
                    baddie.Freeze();
                }
            }
            else
            {
                baddie.GetPushedBack();

            }
        }
    }

    public void SetCursorActive(bool b)
    {
        cursorSprite.gameObject.SetActive(b);
    }

    public void CursorPosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 originPos = transform.position;
        float distance = MyUtils.DistanceBetweenObjects(originPos, mousePos);
        if(distance > cursorMaxRadius)
        {
            Vector2 clampedToRad = MyUtils.Direction2D(originPos, mousePos);
            clampedToRad *= cursorMaxRadius / distance;
            cursorSprite.transform.position = transform.position + MyUtils.Vec2DTo3D(clampedToRad);
        }
        else
        {
            cursorSprite.transform.position = mousePos;
        }
    }

    public void ShootConfetti()
    {
        confettiOne.Play();
        confettiTwo.Play();
    }

    public bool UpgradeWeapon()
    {
        if (weaponUpgradeLevel >= 3)
            return false;
        else
        {
            weaponUpgradeLevel++;
            moneyCollected -= weaponUpgradeCost;
            weaponUpgradeCost += (weaponUpgradeCost * weaponUpgradeLevel);
            if (weaponUpgradeLevel == 1 || weaponUpgradeLevel == 2)
            {
                weaponDamage += 25f;
            }
            else
            {
                weaponDamage += 50f;
            }

            return true;
        }
    }

    public bool UpgradeWaterTank()
    {
        if (tankUpgradeLevel >= 3)
            return false;
        else
        {
            tankUpgradeLevel++;
            waterRegenAmount = tankUpgradeLevel * (2 + tankUpgradeLevel);
            moneyCollected -= tankUpgradeCost;
            tankUpgradeCost += (tankUpgradeCost * tankUpgradeLevel);
            if (tankUpgradeLevel == 1 || tankUpgradeLevel == 2)
            {
                waterMaxAmount += 25f;
            }
            else
            {
                waterMaxAmount += 50f;
            }

            waterAmount = waterMaxAmount;
            return true;
        }
    }

    public void UpgradeWaterToFreeze()
    {
        moneyCollected -= waterUpgradeCost;
        freezingWater = true;
    }

    public void CollectMoney(int val)
    {
        audioSource.PlayOneShot(coinPickup);
        moneyCollected += val;
    }
}
