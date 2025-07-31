using UnityEngine;

public class BuildingScroller : MonoBehaviour
{
    public GameObject[] buildings; // Prefabs or GameObjects already in the scene
    public float scrollSpeed = 10f;
    public float buildingLength = 20f; // Length of one building segment
    public Transform player; // Optional, if you want to match the player's position

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        foreach (GameObject building in buildings)
        {
            building.transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime);

            // Recycle if off-screen
            if (building.transform.position.z < player.position.z - buildingLength)
            {
                float farthestZ = GetFarthestZ();
                building.transform.position = new Vector3(
                    building.transform.position.x,
                    building.transform.position.y,
                    farthestZ + buildingLength
                );
            }
        }
    }

    float GetFarthestZ()
    {
        float farthestZ = float.MinValue;
        foreach (GameObject b in buildings)
        {
            if (b.transform.position.z > farthestZ)
                farthestZ = b.transform.position.z;
        }
        return farthestZ;
    }
}
