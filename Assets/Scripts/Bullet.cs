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
        if(collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject); // Çarpan nesneyi yok et
        }
    }
}
