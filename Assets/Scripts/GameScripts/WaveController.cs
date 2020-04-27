using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public StateMachine<WaveController> stateMachine;
    public static WaveController waveController;

    private int waveNumber = 0;

    private ObjectPooler pool;
    private GameController gc;
    private PlantController plant;

    private void Awake()
    {
        plant = FindObjectOfType<PlantController>();
        waveController = this;
        stateMachine = new StateMachine<WaveController>(waveController);
        stateMachine.ChangeState(WCWaitState.Instance);
        pool = FindObjectOfType<ObjectPooler>();
        gc = FindObjectOfType<GameController>();
        waveNumber = 0;
    }

    public int WaveNumber { get { return waveNumber; } }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void StartSpawning()
    {
        StartCoroutine(IStartSpawning());
    }

    IEnumerator IStartSpawning()
    {
        Debug.Log("StartSpawning");
        string[] enemies = { "Baddie", "Baddie2", "Baddie3", "Baddie4" };
        waveNumber++;
        //plant.AdjustDecayAmount();
        int numberOfBaddies = 0;
        bool spawnMiniboss = false;

        spawnMiniboss = waveNumber % 5 == 0 ? true : false;
        numberOfBaddies = spawnMiniboss ? (waveNumber - 1) * 2 : waveNumber * 2;
        float spawnTimer = spawnMiniboss ? ((waveNumber - 1) * 2) / (gc.DayLengthInSeconds / 2) : waveNumber * 2 / (gc.DayLengthInSeconds / 2);

        if (spawnMiniboss)
            gc.NumberOfEnemiesAlive = numberOfBaddies + 1;
        else
            gc.NumberOfEnemiesAlive = numberOfBaddies;

        while (numberOfBaddies > 0)
        {
            Vector2 legalPos = GetLegalSpawnPoint();
            GameObject obj = pool.SpawnFromPool(enemies[Random.Range(0, enemies.Length)], legalPos, Quaternion.identity);
            obj.GetComponent<BaddieController>().CheckHealth();
            numberOfBaddies--;
            yield return new WaitForSeconds(spawnTimer);
        }

        if (spawnMiniboss)
        {
            GameObject obj = pool.SpawnFromPool("Miniboss", GetLegalSpawnPoint(), Quaternion.identity);
            obj.GetComponent<BaddieController>().CheckMiniBossHealth();
        }

        stateMachine.ChangeState(WCWaitState.Instance);
    }

    Vector2 GetLegalSpawnPoint()
    {
        float halfMapSafeWidth = gc.HalfMapWidth();
        float halfMapSafeHeight = gc.HalfMapHeight();

        Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        int chosenDirection = Random.Range(0, directions.Length);
        Vector2 directionToSpawn = Vector2.zero;
        switch (chosenDirection)
        {
            case 0:
                directionToSpawn = new Vector2(Random.Range(-halfMapSafeWidth, halfMapSafeWidth), halfMapSafeHeight + 2f);
                break;
            case 1:
                directionToSpawn = new Vector2(halfMapSafeWidth + 2f, Random.Range(-halfMapSafeHeight, halfMapSafeHeight));
                break;
            case 2:
                directionToSpawn = new Vector2(Random.Range(-halfMapSafeWidth, halfMapSafeWidth), -halfMapSafeHeight + 2f);
                break;
            case 3:
                directionToSpawn = new Vector2(-halfMapSafeWidth + 2f, Random.Range(-halfMapSafeHeight, halfMapSafeHeight));
                break;
            default:
                Debug.LogWarning("DIRECTION ERROR");
                break;
        }

        return directionToSpawn;
    }
}
