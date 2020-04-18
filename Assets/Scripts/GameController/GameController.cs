using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public StateMachine<GameController> stateMachine;
    public static GameController controller;

    public GameObject pauseMenu;
    public Slider sfxVolume;
    public Slider musicVolume;

    [Range(0,1)]
    public float gameSFXVolume;
    [Range(0,1)]
    public float gameMusicVolume;

    private void Awake()
    {
        controller = this;
        stateMachine = new StateMachine<GameController>(controller);
        stateMachine.ChangeState(GCPlayState.Instance);
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void AdjustVolume()
    {
        gameSFXVolume = sfxVolume.value;
        gameMusicVolume = musicVolume.value;
    }
}
