using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private Transform _drone; // Drone objesi
    [SerializeField] private List<GameObject> _groundPrefabs; // 4 prefab
    [SerializeField] private float _spawnDistance = 40f; // Yeni spawn için mesafe eþiði
    [SerializeField] private int _minGroundCount = 4; // Her zaman sahnede olacak minimum zemin sayýsý

    private float _lastZ;
    private float _prefabLength;
    private Queue<GameObject> _spawnedGrounds = new Queue<GameObject>();

    private void Start()
    {
        if (_groundPrefabs.Count == 0)
        {
            Debug.LogError("Ground prefab listesi boþ!");
            return;
        }

        _prefabLength = _groundPrefabs[0].transform.localScale.z;

        // Baþlangýçta 4 platform yerleþtir
        for (int i = 0; i < _minGroundCount; i++)
        {
            SpawnNextPiece();
        }
    }

    private void Update()
    {
        // Drone ilerledikçe yeni platform ekle
        while (_drone.position.z + (_minGroundCount * _prefabLength) > _lastZ)
        {
            SpawnNextPiece();
        }

        // Fazla parçalarý sýradan sil
        while (_spawnedGrounds.Count > _minGroundCount)
        {
            GameObject oldGround = _spawnedGrounds.Dequeue();
            Destroy(oldGround,1f);
        }
    }

    private void SpawnNextPiece()
    {
        GameObject prefab = _groundPrefabs[Random.Range(0, _groundPrefabs.Count)];
        Vector3 spawnPos = new Vector3(0f, -23.1f, _lastZ);

        GameObject newGround = Instantiate(prefab, spawnPos, Quaternion.identity);
        _spawnedGrounds.Enqueue(newGround);

        _lastZ += _prefabLength;
    }
}
