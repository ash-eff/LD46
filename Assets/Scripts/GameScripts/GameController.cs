using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public StateMachine<GameController> stateMachine;
    public static GameController controller;
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

    public GameObject pauseMenu;
    public Slider sfxVolume;
    public Slider musicVolume;

    [Range(0,1)]
    public float gameSFXVolume;
    [Range(0,1)]
    public float gameMusicVolume;

    public float HalfMapWidth() { return  mapSizeX / 2; }
    public float HalfMapHeight() { return mapSizeY / 2; }

    private void Awake()
    {
        controller = this;
        stateMachine = new StateMachine<GameController>(controller);
        stateMachine.ChangeState(GCPlayState.Instance);
        StartCoroutine(RunDayPhase());
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void AdjustVolume()
    {
        gameSFXVolume = sfxVolume.value;
        gameMusicVolume = musicVolume.value;
    }

    IEnumerator RunDayPhase()
    {
        float lerpTime = DayLengthInSeconds;
        float currentLerpTime = 0;
        Quaternion startPos = theSun.transform.rotation;
        Quaternion targetPos = Quaternion.Euler(0, 0, -180);
        while(currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            theSun.transform.rotation = Quaternion.Slerp(startPos, targetPos, perc);
            globalLight.intensity = Mathf.Lerp(.5f, 1.5f, Mathf.PingPong(perc * 2, 1));
            sunLight.shadowIntensity = Mathf.Lerp(.05f, .5f, Mathf.PingPong(perc * 2, 1));
            yield return null;
        }

        Debug.Log("Day complete in: " + currentLerpTime + " seconds");
        //stateMachine.ChangeState(GCWaitState.Instance);
    }
}
