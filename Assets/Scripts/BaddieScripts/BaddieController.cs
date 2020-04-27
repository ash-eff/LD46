using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaddieController : MonoBehaviour
{
    public StateMachine<BaddieController> stateMachine;
    public static BaddieController baddie;

    private float coinDropChance = .35f;

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
    private ObjectPooler pool;
    private Vector2 target;
    private Vector2 position;
    private Vector2 screenPos;
    private BaddieWalkState walkState = new BaddieWalkState();
    private BaddieAttackState attackState = new BaddieAttackState();
    private BaddiePushbackState pushbackState = new BaddiePushbackState();
    public BaddieFrozenState frozenState = new BaddieFrozenState();
    private BaddieStopState gameOverState = new BaddieStopState();
    private Camera cam;
    private CameraController cc;
    private GameController gc;
    public Animator anim;
    private AudioSource audioSource;
    public AudioClip[] dmgSounds;
    public GameObject snowFlake;
    private WaveController wc;
    public GameObject foot;

    public bool isFrozen;

    public float Speed { get { return speed; } }
    public float RateOfAttack { get { return rateOfAttack; } }
    public float LastAttack { get { return lastAttack; } set { lastAttack = value; } }
    public float DamageAmount { get { return damageAmount; } }
    public Vector2 Target { get { return target; } }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim.SetBool("Running", true);
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

        cc = cam.GetComponentInParent<CameraController>();
        pool = FindObjectOfType<ObjectPooler>();
        wc = FindObjectOfType<WaveController>();
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    private void LateUpdate()
    {
        if(stateMachine.currentState == pushbackState)
        {
            stateMachine.ChangeState(walkState);
            speed = forwardSpeed;
        }
        target = plant.transform.position;
    }


    public void CheckHealth()
    {
        if(health < 350)
        {
            health += 5;
        }
    }

    public void CheckMiniBossHealth()
    {
        if (health < 500)
        {
            health += 100;
        }
    }

    public void GetBaddieDirection()
    {
        if(MyUtils.Direction2D(transform.position, plant.transform.position).x < 0)
        {
            baddieSprite.transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            baddieSprite.transform.localScale = new Vector2(1, 1);
        }
    }

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
        float clampDirX = Mathf.Clamp(directionToMe.x, -23.75f, 23.75f);
        float clampDirY = Mathf.Clamp(directionToMe.y, -13f, 13f);
        indicator.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y) + new Vector2(clampDirX, clampDirY);
    }

    public void GetPushedBack()
    {
        if(stateMachine.currentState != attackState)
        {
            stateMachine.ChangeState(pushbackState);
            target = player.transform.position;
            speed = -(forwardSpeed + 1);
        }

        //pushResetTimer = player.rateOfSpray + .1f;
        //if (!beingPushed)
        //{
        //    beingPushed = true;
        //    StartCoroutine(PushBackTimer());
        //}
    }

    public void Freeze()
    {
        isFrozen = true;
        StartCoroutine(IFreeze());
    }

    IEnumerator IFreeze()
    {
        stateMachine.ChangeState(frozenState);
        snowFlake.SetActive(true);
        yield return new WaitForSeconds(3f);
        stateMachine.ChangeState(walkState);
        isFrozen = false;
        snowFlake.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        audioSource.PlayOneShot(dmgSounds[Random.Range(0, dmgSounds.Length)]);
        GameObject obj = pool.SpawnFromPool("FloatingText", transform.position, Quaternion.identity);
        obj.GetComponent<FloatingText>().SetDisplay("- " + amount.ToString());
        health -= amount;
        CheckLife();
    }

    private IEnumerator PushBackTimer()
    {
        while (pushResetTimer > 0)
        {
            stateMachine.ChangeState(pushbackState);
            target = player.transform.position;
            speed = -(forwardSpeed + 1);
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
        else
        {
            if (!isFlashing)
            {
                isFlashing = true;
                StartCoroutine(FlashWhite());
            }
        }
    }

    private void Die()
    {
        float changeToDropCoin = Random.value;
        if (changeToDropCoin <= coinDropChance)
        {
            pool.SpawnFromPool("Coin", transform.position, Quaternion.identity);
        }

        gc.NumberOfEnemiesAlive--;
        gc.UpdateKillCount();
        cc.CameraShake();
        gameObject.SetActive(false);
    }

    public void IsGameOver()
    {
        bool gameover = gc.stateMachine.currentState == GCGameOverState.Instance ? true : false;
        if (gameover)
            stateMachine.ChangeState(gameOverState);
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
            stateMachine.ChangeState(walkState);
        }
    }
}
