using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;
    public enum SpawnState {SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public Transform boss;
        public int bossCount;
        public int enemyCount;
        public float rate;
    }
    public GameObject pushToStart;
    public float delay;
    public Wave [] waves;
    private int nextWave = 0;
    public bool complete = false;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 30f;
    public float waveCountdown;

    private float searchCowntdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if(spawnPoints.Length == 0) 
        {
            Debug.LogError("No spawn points referenced.");
        }
        waveCountdown = timeBetweenWaves;
    }
    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if(!EnemyIsAlive())
            {
                pushToStart.SetActive(true);
                WaveCompleted();
                
            }
            else
            {
                return;
            }
        }
        if(waveCountdown <= 0 )
        { 
            if(state != SpawnState.SPAWNING) 
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            waveCountdown = 0;
            pushToStart.SetActive(false);
        }
        else waveCountdown -= Time.deltaTime;
    }
    void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0; 
            Debug.Log("ALL WAYES COMPLETE!");
            complete = true;
        }
        else nextWave++;
    }

    bool EnemyIsAlive()
    {
        searchCowntdown -= Time.deltaTime;
       
        if (searchCowntdown <= 0 )
        { 
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
            searchCowntdown = 1f;
            
        }
        return true;
    }
    IEnumerator SpawnWave (Wave _wave)
    {
        Debug.Log("Spawning Wave:" +_wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i< _wave.enemyCount; i++)
        { 
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f/delay);
        }

        for (int i = 0; i < _wave.bossCount; i++)
        {
            SpawnEnemy(_wave.boss);
            yield return new WaitForSeconds(1f /delay);
        }
        
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy (Transform _enemy)
        
    {
        Debug.Log("Spawning Enemy:" + _enemy.name);
     
        Transform _sp =spawnPoints[Random.Range(0,spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation); 
    }
}
