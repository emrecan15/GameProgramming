using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
	public GameObject[] roadPrefabs;
	public Transform playerTransform;
	public float spawnZ = 0.0f;
	public float roadLength = 30f;
	public int amountOfRoadsOnScreen = 5;

	private List<GameObject> activeRoads = new List<GameObject>();

	void Start()
	{
		for (int i = 0; i < amountOfRoadsOnScreen; i++)
		{
			SpawnRoad();
		}
	}

	void Update()
	{
		if (playerTransform.position.z - roadLength > (spawnZ - amountOfRoadsOnScreen * roadLength))
		{
			SpawnRoad();
			DeleteRoad();
		}
	}

	private void SpawnRoad()
	{
		int randomIndex = Random.Range(0, roadPrefabs.Length);
		GameObject go = Instantiate(roadPrefabs[randomIndex], transform.forward * spawnZ, transform.rotation);
		activeRoads.Add(go);
		spawnZ += roadLength;
	}

	private void DeleteRoad()
	{
		Destroy(activeRoads[0]);
		activeRoads.RemoveAt(0);
	}
}