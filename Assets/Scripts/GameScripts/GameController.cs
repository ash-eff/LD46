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

    public StateMachine<GameController> stateMachine;
    public static GameController controller;

    public GameObject fanfare;
    public TextMeshProUGUI killedText;
    public TextMeshProUGUI totalKilledText;
    public TextMeshProUGUI plantLevelText;
    public TextMeshProUGUI waterLevelText;
    public TextMeshProUGUI waveTimeText;
    public TextMeshProUGUI waveText;
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
    private WaveController waveController;
    public PlayerController player;
    private PlantController plant;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject waterUI;
    public GameObject plantUI;

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
        oneshotAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartNewDay();
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

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
            globalLight.intensity = Mathf.Lerp(.5f, 1.5f, Mathf.PingPong(perc * 2, 1));
            sunLight.shadowIntensity = Mathf.Lerp(.05f, .5f, Mathf.PingPong(perc * 2, 1));
            yield return null;
        }

        if (stateMachine.currentState != GCGameOverState.Instance)
        {
            stateMachine.ChangeState(GCFanFareState.Instance);
            IGetDayTotals();
            Debug.Log("Day complete in: " + currentLerpTime + " seconds");
        }

    }

    void IGetDayTotals()
    {
        fanfare.SetActive(true);
        killedText.text = "NUMBER OF ENEMIES KILLED:  " + waveNumberOfBaddiesKilled.ToString();
        totalKilledText.text = "TOTAL NUMBER OF ENEMIES KILLED:  " + totalNumberOfBaddiesKilled.ToString();
        plantLevelText.text = "PLANT HEALTH:  " + Mathf.RoundToInt(plant.life).ToString();
        waterLevelText.text = "WATER LEVEL:  " + Mathf.RoundToInt(player.waterAmount).ToString();
        waveTimeText.text = "TIME:  " + waveTimeToComplete.ToString() + "  SECONDS";
    }

    public void StartNewDay()
    {
        StartCoroutine(NewDaySetUp());
    }

    IEnumerator NewDaySetUp()
    {
        if (waveController.waveNumber == 0)
            yield return new WaitForSeconds(1.5f);
        stateMachine.ChangeState(GCWaitState.Instance);
        fanfare.SetActive(false);
        waveText.text = "Wave " + (waveController.waveNumber + 1).ToString();
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
}
