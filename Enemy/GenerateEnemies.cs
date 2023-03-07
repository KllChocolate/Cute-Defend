using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }
    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            xPos = Random.Range(-38, 40);
            zPos = Random.Range(44, 70);
            Instantiate(enemy, new Vector3(xPos, 0.07f, zPos), Quaternion.identity);
            yield return new WaitForSeconds (1);
            enemyCount += 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
