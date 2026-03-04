using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	[Header("Havuz Ayarlarý")]
	public GameObject[] obstaclePrefabs;
	public int poolSizePerPrefab = 5;
	private List<List<GameObject>> poolList;

	[Header("Doðma Ayarlarý")]
	public Transform playerTransform;
	public float spawnDistanceZ = 60f;
	public float laneDistance = 3.5f;

	[Header("Zorluk Ayarlarý")]
	public float spawnDistanceInterval = 25f;

	private float lastSpawnZ;

	void Start()
	{
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

		lastSpawnZ = playerTransform.position.z;
	}

	void Update()
	{
		if (playerTransform.position.z - lastSpawnZ >= spawnDistanceInterval)
		{
			SpawnObstacle();
			lastSpawnZ = playerTransform.position.z;
		}
	}

	void SpawnObstacle()
	{
		int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Length);

		GameObject obstacle = GetPooledObject(randomObstacleIndex);

		if (obstacle != null)
		{
			int randomLane = Random.Range(0, 3);
			float xPos = (randomLane - 1) * laneDistance;

			// 1. YENÝ EKLENEN KISIM: Objenin Inspector'da ayarladýðýn orijinal Y deðerini al
			float originalY = obstacle.transform.position.y;

			// 2. DEÐÝÞEN KISIM: Sabit 0.5f yerine originalY deðerini kullan!
			obstacle.transform.position = new Vector3(xPos, originalY, playerTransform.position.z + spawnDistanceZ);

			obstacle.SetActive(true);
		}
	}

	// GÜNCELLENEN KISIM: DÝNAMÝK HAVUZ
	GameObject GetPooledObject(int index)
	{
		// 1. Önce havuzda boþta olan var mý diye bak
		for (int i = 0; i < poolList[index].Count; i++)
		{
			if (!poolList[index][i].activeInHierarchy)
			{
				return poolList[index][i];
			}
		}

		// 2. EÐER HAVUZDA BOÞTA ENGEL KALMADIYSA, YENÝDEN ÜRET VE HAVUZA EKLE!
		// Bu sayede oyun hiçbir zaman "engel bulamadým" demez, sonsuza kadar çalýþýr.
		GameObject newObj = Instantiate(obstaclePrefabs[index]);
		newObj.SetActive(false);
		poolList[index].Add(newObj);

		return newObj;
	}
}