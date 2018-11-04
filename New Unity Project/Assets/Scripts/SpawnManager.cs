using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public bool enableSpawn = false;
    public GameObject Enemy;
    public GameObject Item_03;
    public GameObject Item_28;
    public GameObject Item_25;

    void SpawnItem_03() //바나나 스폰
    {
        float randomX = Random.Range(-24.0f, 24.0f);
        float randomZ = Random.Range(-31.0f, 31.0f);
        Quaternion qRotation = Quaternion.Euler(0f, 129f, 0f);  //회전 변수
        if (!enableSpawn)
        {
            GameObject items = Instantiate(Item_03, new Vector3(randomX, 1f, randomZ), qRotation);
            Destroy(items, 6);//파괴될 시간설정
        }
    }
    void SpawnItem_28() //포션 스폰
    {
        float randomX = Random.Range(-24.0f, 24.0f);
        float randomZ = Random.Range(-31.0f, 31.0f);
        if (!enableSpawn)
        {
            GameObject items = Instantiate(Item_28, new Vector3(randomX, 2.52f, randomZ), Item_28.transform.rotation);
            Destroy(items, 6);//파괴될 시간설정
        }
    }
    void SpawnItem_25() //썩은 고기 스폰
    {
        float randomX = Random.Range(-24.0f, 24.0f);
        float randomZ = Random.Range(-31.0f, 31.0f);
        Quaternion qRotation = Quaternion.Euler(0f, 46.079f, 0f);
        if (!enableSpawn)
        {
            GameObject items = Instantiate(Item_25, new Vector3(randomX, 0.93f, randomZ), qRotation);
            Destroy(items, 6);//파괴될 시간설정
        }
    }


    void SpawnEnemy_L()
    {
        float randomX = Random.Range(-24.0f, -20.0f);
        float randomZ = Random.Range(-31.0f, 31.0f);
        if (!enableSpawn)
        {
            GameObject enemy = Instantiate(Enemy, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
        }
    }
    void SpawnEnemy_R()
    {
        float randomX = Random.Range(20.0f, 24.0f);
        float randomZ = Random.Range(-31.0f, 31.0f);
        if (!enableSpawn)
        {
            GameObject enemy = Instantiate(Enemy, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
        }
    }
    void SpawnEnemy_T()
    {
        float randomX = Random.Range(-24.0f, 24.0f);
        float randomZ = Random.Range(28.0f, 31.0f);
        if (!enableSpawn)
        {
            GameObject enemy = Instantiate(Enemy, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
        }
    }
    void SpawnEnemy_B()
    {
        float randomX = Random.Range(-24.0f, 24.0f);
        float randomZ = Random.Range(-31.0f, -28.0f);
        if (!enableSpawn)
        {
            GameObject enemy = Instantiate(Enemy, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
        }
    }
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnEnemy_L", 1, 5);
        InvokeRepeating("SpawnEnemy_R", 2, 5);
        InvokeRepeating("SpawnEnemy_T", 3, 5);
        InvokeRepeating("SpawnEnemy_B", 4, 5);

        InvokeRepeating("SpawnItem_03", 5, 17); //사과 생성
        InvokeRepeating("SpawnItem_28", 20, 30); //포션 생성
        InvokeRepeating("SpawnItem_25", 10, 13); //썩은 고기 생성

        InvokeRepeating("SpawnEnemy_L", 30, 10);
        InvokeRepeating("SpawnEnemy_R", 31, 10);
        InvokeRepeating("SpawnEnemy_T", 32, 10);
        InvokeRepeating("SpawnEnemy_B", 33, 10);

        InvokeRepeating("SpawnEnemy_L", 60, 20);
        InvokeRepeating("SpawnEnemy_R", 61, 20);
        InvokeRepeating("SpawnEnemy_T", 62, 20);
        InvokeRepeating("SpawnEnemy_B", 63, 20);

        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
