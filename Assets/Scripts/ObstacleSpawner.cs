using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Havuz Ayarlarý (Engeller)")]
    public GameObject[] obstaclePrefabs;
    public int poolSizePerPrefab = 5;
    private List<List<GameObject>> poolList;

    [Header("Havuz Ayarlarý (Altýn)")]
    public GameObject coinPrefab; // Altýn prefabýmýzý buraya koyacaðýz
    public int coinPoolSize = 10;
    private List<GameObject> coinPool;
    [Range(0f, 100f)]
    public float coinSpawnChance = 60f; // %60 ihtimalle altýn çýksýn

    [Header("Doðma Ayarlarý")]
    public Transform playerTransform;
    public float spawnDistanceZ = 60f;
    public float laneDistance = 3.5f;

    [Header("Zorluk Ayarlarý")]
    public float spawnDistanceInterval = 25f;

    private float lastSpawnZ;

    void Start()
    {
        // 1. ENGEL HAVUZUNU OLUÞTUR
        poolList = new List<List<GameObject>>();
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            List<GameObject> objectPool = new List<GameObject>();
            for (int j = 0; j < poolSizePerPrefab; j++)
            {
                GameObject obj = Instantiate(obstaclePrefabs[i]);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            poolList.Add(objectPool);
        }

        // 2. ALTIN HAVUZUNU OLUÞTUR
        coinPool = new List<GameObject>();
        for (int i = 0; i < coinPoolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            coinPool.Add(coin);
        }

        lastSpawnZ = playerTransform.position.z;
    }

    void Update()
    {
        if (playerTransform.position.z - lastSpawnZ >= spawnDistanceInterval)
        {
            SpawnObstacleAndCoin();
            lastSpawnZ = playerTransform.position.z;
        }
    }

    void SpawnObstacleAndCoin()
    {
        // --- ENGEL ÇIKARMA ---
        int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstacle = GetPooledObstacle(randomObstacleIndex);
        int obstacleLane = Random.Range(0, 3); // Engelin çýkacaðý þerit

        if (obstacle != null)
        {
            float xPos = (obstacleLane - 1) * laneDistance;
            float originalY = obstacle.transform.position.y;
            obstacle.transform.position = new Vector3(xPos, originalY, playerTransform.position.z + spawnDistanceZ);
            obstacle.SetActive(true);
        }

        // --- ALTIN ÇIKARMA ---
        if (Random.Range(0f, 100f) <= coinSpawnChance)
        {
            GameObject coin = GetPooledCoin();
            if (coin != null)
            {
                // Altýn, engelin OLAMADIÐI rastgele bir þeritte çýksýn ki üst üste binmesinler
                int coinLane;
                do
                {
                    coinLane = Random.Range(0, 3);
                } while (coinLane == obstacleLane);

                float coinXPos = (coinLane - 1) * laneDistance;
                float coinY = coinPrefab.transform.position.y;
                coin.transform.position = new Vector3(coinXPos, coinY, playerTransform.position.z + spawnDistanceZ);
                coin.SetActive(true);
            }
        }
    }

    // ENGEL HAVUZU KONTROLÜ
    GameObject GetPooledObstacle(int index)
    {
        for (int i = 0; i < poolList[index].Count; i++)
        {
            if (!poolList[index][i].activeInHierarchy)
                return poolList[index][i];
        }
        GameObject newObj = Instantiate(obstaclePrefabs[index]);
        newObj.SetActive(false);
        poolList[index].Add(newObj);
        return newObj;
    }

    // ALTIN HAVUZU KONTROLÜ
    GameObject GetPooledCoin()
    {
        for (int i = 0; i < coinPool.Count; i++)
        {
            if (!coinPool[i].activeInHierarchy)
                return coinPool[i];
        }
        GameObject newCoin = Instantiate(coinPrefab);
        newCoin.SetActive(false);
        coinPool.Add(newCoin);
        return newCoin;
    }
}