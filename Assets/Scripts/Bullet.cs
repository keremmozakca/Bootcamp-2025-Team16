using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f; // Merminin ömrü

    private void Start()
    {
        // Merminin ömrü dolduðunda kendini yok et
        Destroy(gameObject, _lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Mermi bir nesneye çarptýðýnda kendini yok et
        // ilerde buraya kontroller ekleyeceðim
        Destroy(gameObject);
    }
}
