using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Transform> allMobSpawnPoints;
    private List<Transform> useableSpawnPoints;

    public List<GameObject> roomEnemyList;

    [SerializeField] private int maxEnemyCount = 10;
    [SerializeField] private int minEnemyCount = 5;
    [SerializeField] private int maxWaveCount = 5;
    [SerializeField] private int minWaveCount = 3;

    public int remainingEnemies;
    public int waveCount;
    public int currentWave = 0;

    private List<GameObject> enemyList;

    private bool isChangingWave = false;


    private void Awake()
    {
        useableSpawnPoints = allMobSpawnPoints;
        enemyList = new List<GameObject>();
        waveCount = Random.Range(minWaveCount, maxWaveCount);
        currentWave = 1;
    }

    private void Start()
    {
        CreateEnemyList();
        SpawnEnemies();
    }

    public void CreateEnemyList()
    {
        enemyList.Clear();
        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount);

        for(int i = 0;  i < enemyCount; i++)
        {
            enemyList.Add(roomEnemyList[Random.Range(0, roomEnemyList.Count)]);
        }
    }

    public void SpawnEnemies()
    {
        useableSpawnPoints = new List<Transform>(allMobSpawnPoints);
        foreach (GameObject enemy in enemyList)
        {
            int spawnPointNumber = Random.Range(0, useableSpawnPoints.Count);

            ParticleSystem spawnParticle = useableSpawnPoints[spawnPointNumber].Find("SpawnParticle").GetComponent<ParticleSystem>();
            spawnParticle.Play();
            StartCoroutine(SpawnRoutine(enemy, useableSpawnPoints[spawnPointNumber]));
            
            useableSpawnPoints.RemoveAt(spawnPointNumber);

            remainingEnemies++;
        }

    }

    public void OnEnemyDie()
    {
        remainingEnemies--;

        if(remainingEnemies <= 0 && !isChangingWave)
        {
            isChangingWave = true;
            remainingEnemies = 0;
            StartCoroutine(ChangeWaveRoutine());
        }
    }

    public IEnumerator ChangeWaveRoutine()
    {
        if(currentWave != waveCount)
        {
            yield return new WaitForSeconds(0.3f);
            currentWave++;
            CreateEnemyList();
            SpawnEnemies();
            isChangingWave=false;
            
        }
        else
        {
            Debug.Log("Oda Bitti");
        }
    }

    public IEnumerator SpawnRoutine(GameObject enemy, Transform spawnPoint)
    {
        yield return new WaitForSeconds(3f);
        GameObject newEnemy = Instantiate(enemy, spawnPoint.position, enemy.transform.rotation);
        newEnemy.GetComponent<Enemy>().OnDeath += OnEnemyDie;
    }

}
