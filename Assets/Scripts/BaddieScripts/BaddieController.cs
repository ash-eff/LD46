using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaddieController : MonoBehaviour
{
    public StateMachine<BaddieController> stateMachine;
    public static BaddieController baddie;

    private float speed;
    [SerializeField]
    private float rateOfAttack;
    [SerializeField]
    public float forwardSpeed;
    [SerializeField]
    private float pushResetTimer;
    private float lastAttack;
    [SerializeField]
    private float damageAmount;

    private bool beingPushed = false;
    public bool isFlashing;
    [SerializeField]
    private float health;

    public GameObject indicator;
    public SpriteRenderer baddieSprite;
    public TextMeshProUGUI baddDistText;
    public PlantController plant;
    private PlayerController player;
    private Vector2 target;
    private Vector2 position;
    private Vector2 screenPos;
    private BaddieWalkState walkState = new BaddieWalkState();
    private BaddieAttackState attackState = new BaddieAttackState();
    private BaddiePushbackState pushbackState = new BaddiePushbackState();
    private Camera cam;
    private GameController gc;

    public float Speed { get { return speed; } }
    public float RateOfAttack { get { return rateOfAttack; } }
    public float LastAttack { get { return lastAttack; } set { lastAttack = value; } }
    public float DamageAmount { get { return damageAmount; } }
    public Vector2 Target { get { return target; } }

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
        baddie = this;
        stateMachine = new StateMachine<BaddieController>(baddie); 
        stateMachine.ChangeState(walkState);
        plant = FindObjectOfType<PlantController>();
        player = FindObjectOfType<PlayerController>();
        target = plant.transform.position;
        position = transform.position;
        speed = forwardSpeed;
        cam = Camera.main;
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void CheckScreenPos()
    {
        screenPos = cam.WorldToViewportPoint(transform.position); //get viewport positions

        if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
        {
            indicator.SetActive(false);
            return;
        }

        indicator.SetActive(true);
        Vector2 centerPos = cam.transform.position;
        Vector2 directionToMe = MyUtils.Direction2D(centerPos, transform.position);
        baddDistText.text = directionToMe.magnitude.ToString("00") + "m";
        float clampDirX = Mathf.Clamp(directionToMe.x, -24.75f, 24.75f);
        float clampDirY = Mathf.Clamp(directionToMe.y, -14f, 14f);
        indicator.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y) + new Vector2(clampDirX, clampDirY);
    }

    public void GetPushedBack()
    {
        pushResetTimer = player.rateOfSpray + .1f;
        if (!beingPushed)
        {
            beingPushed = true;
            StartCoroutine(PushBackTimer());
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        CheckLife();
        if (!isFlashing)
        {
            isFlashing = true;
            StartCoroutine(FlashWhite());
        }
    }

    private IEnumerator PushBackTimer()
    {
        while (pushResetTimer > 0)
        {
            stateMachine.ChangeState(pushbackState);
            target = player.transform.position;
            speed = -forwardSpeed;
            pushResetTimer -= Time.deltaTime;
            yield return null;
        }

        stateMachine.ChangeState(walkState);
        target = plant.transform.position;
        speed = forwardSpeed;
        beingPushed = false;
    }

    private IEnumerator FlashWhite()
    {
        baddieSprite.enabled = false;
        yield return new WaitForSeconds(.1f);
        baddieSprite.enabled = true;

        isFlashing = false;
    }

    private void CheckLife()
    {
        if(health < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gc.NumberOfEnemiesAlive--;
        gc.UpdateKillCount();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Plant")
        {
            stateMachine.ChangeState(attackState);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Plant")
        {
            stateMachine.ChangeState(pushbackState);
        }
    }
}
