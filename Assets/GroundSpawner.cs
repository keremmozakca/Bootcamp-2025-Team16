using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject[] platformPrefabs; // 4 farklý platform prefab'ý
    public Transform player; // Drone/Player referansý
    public float platformLength = 19.39f; // Platform uzunluðu (X scale'den)
    public float destroyDelay = 1f; // Platform geçildikten sonra yok olma süresi

    [Header("Spawn Settings")]
    public Vector3 startPosition = new Vector3(0, -23.1f, 33.2f); // Ýlk platform pozisyonu
    public int initialPlatformCount = 5; // Baþlangýçta oluþturulacak platform sayýsý

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

    // Baþlangýçta 5 platform oluþtur
    void InitializePlatforms()
    {
        nextSpawnZ = startPosition.z;

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    // Yeni platform oluþtur
    void SpawnPlatform()
    {
        // Rastgele platform prefab seç
        int randomIndex = Random.Range(0, platformPrefabs.Length);
        GameObject selectedPrefab = platformPrefabs[randomIndex];

        // Platform pozisyonu hesapla
        Vector3 spawnPosition = new Vector3(startPosition.x, startPosition.y, nextSpawnZ);

        // Platform oluþtur
        GameObject newPlatform = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // Platform'a PlatformTrigger component'i ekle (drone için gerekli deðil)
        // Component eklemesi kaldýrýldý çünkü drone zemini olmayacak

        // Queue'ye ekle
        activePlatforms.Enqueue(newPlatform);

        // Sonraki platform pozisyonunu güncelle
        nextSpawnZ += platformLength;
    }

    // Platform geçilip geçilmediðini kontrol et
    void CheckPlatformPassing()
    {
        if (activePlatforms.Count > 0 && player != null)
        {
            GameObject firstPlatform = activePlatforms.Peek();

            // Player platform'u geçti mi kontrol et
            if (player.position.z > firstPlatform.transform.position.z + platformLength / 2)
            {
                OnPlatformPassed(firstPlatform);
            }
        }
    }

    // Platform geçildiðinde çaðrýlýr
    public void OnPlatformPassed(GameObject passedPlatform)
    {
        if (activePlatforms.Count > 0 && activePlatforms.Peek() == passedPlatform)
        {
            // Queue'den çýkar
            activePlatforms.Dequeue();

            // Yok edilecekler listesine ekle
            StartCoroutine(DestroyPlatformAfterDelay(passedPlatform));

            // Yeni platform oluþtur
            SpawnPlatform();
        }
    }

    // Platform'u belirli süre sonra yok et
    IEnumerator DestroyPlatformAfterDelay(GameObject platform)
    {
        yield return new WaitForSeconds(destroyDelay);

        if (platform != null)
        {
            Destroy(platform);
        }
    }

    // Yok edilmiþ platform'larý temizle
    void CleanupPlatforms()
    {
        platformsToDestroy.RemoveAll(platform => platform == null);
    }

    // Debug için
    void OnDrawGizmos()
    {
        // Spawn noktalarýný göster
        Gizmos.color = Color.green;
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = new Vector3(startPosition.x, startPosition.y, startPosition.z + (i * platformLength));
            Gizmos.DrawWireCube(pos, new Vector3(1, 1, 1));
        }
    }
}
