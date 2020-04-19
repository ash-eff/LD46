using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> stateMachine;
    public static PlayerController player;

    [SerializeField]
    private float weaponDamage;
    public float cursorMaxRadius;
    public LayerMask waterHitLayers;
    public float rateOfSpray;
    private float lastSpray;
    public float waterCollected;
    public float waterRegenAmount;
    public Image waterAmountFillBar;
    public GameObject weaponHolder;
    public SpriteRenderer cutters;
    public SpriteRenderer hose;

    public bool touchingHydrant;
    public HydrantController currentHydrant;

    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private SpriteRenderer cursorSprite;

    private Vector2 movement;
    private Rigidbody2D rb2d;
    private Weapon weapon;
    private GameController gc;
    private Camera cam;

    public float WeaponDamage { get { return weaponDamage; } }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        player = this;
        stateMachine = new StateMachine<PlayerController>(player);
        stateMachine.ChangeState(PlayerBaseState.Instance);
        hose.gameObject.SetActive(true);
        weapon = cutters.GetComponent<Weapon>();
        gc = FindObjectOfType<GameController>();
        cam = Camera.main;
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void PlayerInput()
    {
        if(gc.stateMachine.currentState != GCPlayState.Instance)
        {
            return;
        }

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        CursorPosition();
        RotateWeapons();
        if (touchingHydrant && currentHydrant != null && currentHydrant.waterAmount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(waterCollected + 5 >= 100)
                {
                    waterCollected = 100;
                }
                else
                {
                    currentHydrant.waterAmount -= 5f;
                    waterCollected += 5f;
                }
            }
        }

        if(waterCollected > 0)
        {
            if (Input.GetMouseButton(1))
            {
                hose.gameObject.SetActive(true);
                SprayHose();
            }
        }

        if (Input.GetMouseButton(0))
        {
            hose.gameObject.SetActive(false);
            SwingWeapon();
        }

        // MOVE THIS
        waterAmountFillBar.fillAmount = waterCollected / 100f;
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

    public void ClampPlayerToMap()
    {
        Vector2 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(transform.position.x, -gc.HalfMapWidth(), gc.HalfMapWidth());
        clampedPos.y = Mathf.Clamp(transform.position.y, -gc.HalfMapHeight(), gc.HalfMapHeight());
        transform.position = clampedPos;
    }

    private void SprayHose()
    {
        if(Time.time > rateOfSpray + lastSpray)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, MyUtils.Direction2D(transform.position, cursorSprite.transform.position), MyUtils.DistanceBetweenObjects(transform.position, cursorSprite.transform.position), waterHitLayers);
            Debug.DrawLine(transform.position, cursorSprite.transform.position, Color.blue, rateOfSpray);
            waterCollected -= waterRegenAmount;
            lastSpray = Time.time;
            if (hit)
            {
                HitTargetWithWater(hit.collider);
            }
        }
    }

    private void SwingWeapon()
    {
        weapon.Swing();
    }

    private void RotateWeapons()
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
            plant.Regen(waterRegenAmount);
        }

        if(target.tag == "Baddie")
        {
            BaddieController baddie = target.GetComponent<BaddieController>();
            baddie.GetPushedBack();
        }
    }

    private void CursorPosition()
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
}
