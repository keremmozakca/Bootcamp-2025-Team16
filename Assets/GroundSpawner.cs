using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject[] platformPrefabs; // 4 farkl� platform prefab'�
    public Transform player; // Drone/Player referans�
    public float platformLength = 19.39f; // Platform uzunlu�u (X scale'den)
    public float destroyDelay = 1f; // Platform ge�ildikten sonra yok olma s�resi

    [Header("Spawn Settings")]
    public Vector3 startPosition = new Vector3(0, -23.1f, 100f); // �lk platform pozisyonu
    public int initialPlatformCount = 5; // Ba�lang��ta olu�turulacak platform say�s�

    private Queue<GameObject> activePlatforms = new Queue<GameObject>();
    private float nextSpawnZ;
    private List<GameObject> platformsToDestroy = new List<GameObject>();

    void Start()
    {
        InitializePlatforms();
    }

    void Update()
    {
        CheckPlatformPassing();
        CleanupPlatforms();
    }

    // Ba�lang��ta 5 platform olu�tur
    void InitializePlatforms()
    {
        nextSpawnZ = startPosition.z;

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    // Yeni platform olu�tur
    void SpawnPlatform()
    {
        // Rastgele platform prefab se�
        int randomIndex = Random.Range(0, platformPrefabs.Length);
        GameObject selectedPrefab = platformPrefabs[randomIndex];

        // Platform pozisyonu hesapla
        Vector3 spawnPosition = new Vector3(startPosition.x, startPosition.y, nextSpawnZ);

        // Platform olu�tur
        GameObject newPlatform = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // Platform'a PlatformTrigger component'i ekle (drone i�in gerekli de�il)
        // Component eklemesi kald�r�ld� ��nk� drone zemini olmayacak

        // Queue'ye ekle
        activePlatforms.Enqueue(newPlatform);

        // Sonraki platform pozisyonunu g�ncelle
        nextSpawnZ += platformLength;
    }

    // Platform ge�ilip ge�ilmedi�ini kontrol et
    void CheckPlatformPassing()
    {
        if (activePlatforms.Count > 0 && player != null)
        {
            GameObject firstPlatform = activePlatforms.Peek();

            // Player platform'u ge�ti mi kontrol et
            if (player.position.z > firstPlatform.transform.position.z + platformLength / 2)
            {
                OnPlatformPassed(firstPlatform);
            }
        }
    }

    // Platform ge�ildi�inde �a�r�l�r
    public void OnPlatformPassed(GameObject passedPlatform)
    {
        if (activePlatforms.Count > 0 && activePlatforms.Peek() == passedPlatform)
        {
            // Queue'den ��kar
            activePlatforms.Dequeue();

            // Yok edilecekler listesine ekle
            StartCoroutine(DestroyPlatformAfterDelay(passedPlatform));

            // Yeni platform olu�tur
            SpawnPlatform();
        }
    }

    // Platform'u belirli s�re sonra yok et
    IEnumerator DestroyPlatformAfterDelay(GameObject platform)
    {
        yield return new WaitForSeconds(destroyDelay);

        if (platform != null)
        {
            Destroy(platform);
        }
    }

    // Yok edilmi� platform'lar� temizle
    void CleanupPlatforms()
    {
        platformsToDestroy.RemoveAll(platform => platform == null);
    }

    // Debug i�in
    void OnDrawGizmos()
    {
        // Spawn noktalar�n� g�ster
        Gizmos.color = Color.green;
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = new Vector3(startPosition.x, startPosition.y, startPosition.z + (i * platformLength));
            Gizmos.DrawWireCube(pos, new Vector3(1, 1, 1));
        }
    }
}
