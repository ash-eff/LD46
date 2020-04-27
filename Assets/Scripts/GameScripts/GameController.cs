using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int numberOfEnemiesAlive = 0;
    public static int totalNumberOfBaddiesKilled;
    public static int waveNumberOfBaddiesKilled;
    public static bool completedTutorial = false;

    public StateMachine<GameController> stateMachine;
    public static GameController controller;

    public GameObject fanfare;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waterGUI;
    public TextMeshProUGUI plantGUI;
    public Button nextDayButton;

    [SerializeField]
    private float mapSizeX;
    [SerializeField]
    private float mapSizeY;

    public float DayLengthInSeconds;

    [SerializeField]
    private GameObject theSun;
    [SerializeField]
    private Light2D globalLight;
    [SerializeField]
    private Light2D sunLight;
    [SerializeField]
    private AudioClip statPunch, timePing1, timePing2;
    public AudioSource oneshotAudio;
    public AudioSource musicAudio;
    public AudioSource tutAudio;
    private WaveController waveController;
    public PlayerController player;
    private PlantController plant;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject waterUI;
    public GameObject plantUI;
    public GameObject tutWindow;
    public TextMeshProUGUI tutText;
    public GameObject aloePointer;
    public GameObject plantPointer;
    public GameObject plantButton;
    public GameObject waterPointer;
    public GameObject waterButton;
    public GameObject hydrantPointers;
    public GameObject hydrantButton;
    public GameObject enemyPointers;
    public GameObject enemyButton;
    public BaddieController tutEnemy;
    public GameObject moneyPointer;
    public GameObject moneyButton;
    public GameObject weaponButton;
    public GameObject endButton;
    public GameObject end2Button;

    [Range(0,1)]
    public float gameSFXVolume;
    [Range(0,1)]
    public float gameMusicVolume;

    public float HalfMapWidth() { return  mapSizeX / 2; }
    public float HalfMapHeight() { return mapSizeY / 2; }

    public int NumberOfEnemiesAlive { get { return numberOfEnemiesAlive; } set { numberOfEnemiesAlive = value; } }
    private float waveTimeToComplete;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        plant = FindObjectOfType<PlantController>();
        waveController = FindObjectOfType<WaveController>();
        controller = this;
        stateMachine = new StateMachine<GameController>(controller);
        musicAudio.loop = true;
        StartCoroutine(playEngineSound());
    }

    private void Start()
    {
        if (completedTutorial)
        {
            CloseTutorial();
        }
        else
        {
            stateMachine.ChangeState(GCWaitState.Instance);
            tutWindow.SetActive(true);
        }
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();


    public void UpdateGUI()
    {
        waterGUI.text = "WATER LEVEL " + Mathf.RoundToInt(player.waterAmount).ToString() + "/" + Mathf.RoundToInt(player.waterMaxAmount).ToString();
        plantGUI.text = "PLANT LIFE " + Mathf.RoundToInt(plant.life).ToString() + "/" + Mathf.RoundToInt(plant.maxLife).ToString();
    }

    public void TurnUpMusic(bool b)
    {
        if (b)
            musicAudio.volume = .75f;
        else
            musicAudio.volume = .3f;
    }

    public void UpdateKillCount()
    {
        totalNumberOfBaddiesKilled++;
        waveNumberOfBaddiesKilled++;
    }

    IEnumerator RunDayPhase()
    {
        waveController.stateMachine.ChangeState(WCSpawnState.Instance);

        float lerpTime = DayLengthInSeconds;
        float currentLerpTime = 0;
        Quaternion startPos = Quaternion.Euler(0, 0, 0);
        Quaternion targetPos = Quaternion.Euler(0, 0, -180);
        waveTimeToComplete = 0;
        waveNumberOfBaddiesKilled = 0;
        while (currentLerpTime < lerpTime)
        {
            if(numberOfEnemiesAlive <= 0 && stateMachine.currentState != GCWaitState.Instance)
            {
                stateMachine.ChangeState(GCWaitState.Instance);
                waveText.gameObject.SetActive(true);
                waveText.text = "Ending day in 3...";
                yield return new WaitForSeconds(1f);
                waveText.text = "Ending day in 2...";
                yield return new WaitForSeconds(1f);
                waveText.text = "Ending day in 1...";
                yield return new WaitForSeconds(1f);
                waveText.gameObject.SetActive(false);
                controller.player.stateMachine.ChangeState(PlayerWaitState.Instance);
                waveTimeToComplete = currentLerpTime;
                startPos = theSun.transform.rotation;
                targetPos = Quaternion.Euler(0, 0, -180);
                currentLerpTime = 0;
                lerpTime = 1f;
            }
            if(stateMachine.currentState == GCGameOverState.Instance)
            {
                break;
            }
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            theSun.transform.rotation = Quaternion.Slerp(startPos, targetPos, perc);
            globalLight.intensity = Mathf.Lerp(.5f, 1f, Mathf.PingPong(perc * 2, 1));
            sunLight.shadowIntensity = Mathf.Lerp(.05f, .5f, Mathf.PingPong(perc * 2, 1));
            yield return null;
        }

        if (stateMachine.currentState != GCGameOverState.Instance)
        {
            stateMachine.ChangeState(GCFanFareState.Instance);
            IGetDayTotals();
        }

    }

    void IGetDayTotals()
    {
        fanfare.SetActive(true);
    }

    public void StartNewDay()
    {
        StartCoroutine(NewDaySetUp());
    }

    IEnumerator NewDaySetUp()
    {
        plant.pointer.SetActive(true);
        stateMachine.ChangeState(GCWaitState.Instance);
        fanfare.SetActive(false);
        waveText.text = "Day " + (waveController.WaveNumber + 1).ToString();
        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        oneshotAudio.PlayOneShot(timePing1);
        waveText.text = "3";
        yield return new WaitForSeconds(1f);
        oneshotAudio.PlayOneShot(timePing1);
        waveText.text = "2";
        yield return new WaitForSeconds(1f);
        oneshotAudio.PlayOneShot(timePing1);
        waveText.text = "1";
        yield return new WaitForSeconds(1f);
        oneshotAudio.PlayOneShot(timePing2);
        waveText.text = "GO";
        yield return new WaitForSeconds(1f);
        waveText.gameObject.SetActive(false);
        stateMachine.ChangeState(GCPlayState.Instance);
        StartCoroutine(RunDayPhase());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        numberOfEnemiesAlive = 0;
        totalNumberOfBaddiesKilled = 0;
        waveNumberOfBaddiesKilled = 0;
        TransistionLoader loader = FindObjectOfType<TransistionLoader>();
        loader.LoadNextScene(1);
    }

    public void CloseTutorial()
    {
        tutAudio.loop = false;
        plant.pointer.SetActive(true);
        completedTutorial = true;
        tutWindow.SetActive(false);
        aloePointer.SetActive(false);
        plantPointer.SetActive(false);
        waterPointer.SetActive(false);
        hydrantPointers.SetActive(false);
        enemyPointers.SetActive(false);
        moneyPointer.SetActive(false);
        tutEnemy.gameObject.SetActive(false);
        tutText.text = "";
        StartNewDay();
    }

    public void PlantTut()
    {
        aloePointer.SetActive(false);
        plantButton.SetActive(false);
        waterButton.SetActive(true);
        plantPointer.SetActive(true);
        tutText.text = "Track your plant's life using this bar. The sun will deplete your plants life slowly. Enemies will kill it quickly.";
    }

    public void WaterTut()
    {
        waterButton.SetActive(false);
        hydrantButton.SetActive(true);
        plantPointer.SetActive(false);
        waterPointer.SetActive(true);
        tutText.text = "Track your water amount using this bar. Water your plant to regnerate it's life. Spray enmies to push them back before they reach the plant.";
    }

    public void HydrantTut()
    {
        hydrantButton.SetActive(false);
        enemyButton.SetActive(true);
        waterPointer.SetActive(false);
        hydrantPointers.SetActive(true);
        tutText.text = "You will need to refill your water eventually. You can use the purple distance markers to find a hydrant to replenish your water.";
    }

    public void EnemyTut()
    {
        enemyButton.SetActive(false);
        moneyButton.SetActive(true);
        hydrantPointers.SetActive(false);
        tutEnemy.gameObject.SetActive(true);
        tutEnemy.stateMachine.ChangeState(tutEnemy.frozenState);
        enemyPointers.SetActive(true);
        tutText.text = "Enemies also have distance markers that are red, so you can see where they are coming from and how close they are.";
    }

    public void MoneyTut()
    {
        moneyButton.SetActive(false);
        weaponButton.SetActive(true);
        enemyPointers.SetActive(false);
        tutEnemy.gameObject.SetActive(false);
        moneyPointer.SetActive(true);
        tutText.text = "As you kill enemies, they have a chance to drop coins. Use these to purchase upgrades at the end of each day.";
    }

    public void WeaponsTut()
    {
        enemyButton.SetActive(false);
        endButton.SetActive(true);
        aloePointer.SetActive(false);
        plantPointer.SetActive(false);
        waterPointer.SetActive(false);
        hydrantPointers.SetActive(false);
        enemyPointers.SetActive(false);
        tutEnemy.gameObject.SetActive(false);
        weaponButton.SetActive(false);
        moneyPointer.SetActive(false);
        tutText.text = "Holding the Right Mouse Button will spray your hose. Pressing your Left Mouse Button will attack with your shears.";
    }

    public void EndTut()
    {
        endButton.SetActive(false);
        end2Button.SetActive(true);
        tutText.text = "Good Luck!";
    }

    IEnumerator playEngineSound()
    {
        while (tutAudio.isPlaying)
        {
            yield return null;
        }

        tutAudio.Stop();
        musicAudio.Play();
    }
}
