using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Normal Roads")]
    public GameObject[] roadPrefabs;

    [Header("Tunnel Sequence")]
    public GameObject tunnelStartPrefab;
    public GameObject tunnelMiddlePrefab;
    public GameObject tunnelEndPrefab;

    [Range(0f, 100f)]
    public float tunnelChance = 15f; // Tünel çýkma ihtimali (%15)
    public int tunnelMiddleCount = 3; // Tünelin ortasýna kaç parça eklenecek

    [Header("General Settings")]
    public Transform playerTransform;
    public float spawnZ = 0.0f;
    public float roadLength = 30f;
    public int amountOfRoadsOnScreen = 5;

    private List<GameObject> activeRoads = new List<GameObject>();

    // Tünel sýrasýný takip edecek gizli deðiþkenler
    private bool isSpawningTunnel = false;
    private int spawnedMiddleCount = 0;

    void Start()
    {
        for (int i = 0; i < amountOfRoadsOnScreen; i++)
        {
            // Oyun baþladýðýnda arabanýn direkt tünel içinde doðmamasý için
            // ilk yollarý garanti olarak dizideki 0. normal yol yapýyoruz.
            GameObject go = Instantiate(roadPrefabs[0], transform.forward * spawnZ, transform.rotation);
            activeRoads.Add(go);
            spawnZ += roadLength;
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
        GameObject roadToSpawn;

        // 1. Tünel dizilimi devam ediyorsa
        if (isSpawningTunnel)
        {
            if (spawnedMiddleCount < tunnelMiddleCount)
            {
                roadToSpawn = tunnelMiddlePrefab; // Tünel içini ekle
                spawnedMiddleCount++;
            }
            else
            {
                roadToSpawn = tunnelEndPrefab; // Tünel çýkýþýný ekle ve tüneli bitir
                isSpawningTunnel = false;
            }
        }
        // 2. Normal yoldaysak ve rastgele seçim yapýlýyorsa
        else
        {
            if (Random.Range(0f, 100f) <= tunnelChance)
            {
                roadToSpawn = tunnelStartPrefab; // Tüneli baþlat
                isSpawningTunnel = true;
                spawnedMiddleCount = 0;
            }
            else
            {
                // Tünel denk gelmediyse normal yollardan rastgele seç
                int randomIndex = Random.Range(0, roadPrefabs.Length);
                roadToSpawn = roadPrefabs[randomIndex];
            }
        }

        // Seçilen yolu sahneye yerleþtirme iþlemi (Orijinal kodun)
        GameObject go = Instantiate(roadToSpawn, transform.forward * spawnZ, transform.rotation);
        activeRoads.Add(go);
        spawnZ += roadLength;
    }

    private void DeleteRoad()
    {
        Destroy(activeRoads[0]);
        activeRoads.RemoveAt(0);
    }
}