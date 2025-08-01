using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 0.1f; // saniyede 10 mermi
    [SerializeField] private float _bulletSpeed = 50f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _fireRate)
        {
            Fire();
            _timer = 0f;
        }
    }

    private void Fire()
    {
        if (_bulletPrefab == null || _firePoint == null)
        {
            Debug.LogWarning("Bullet prefab veya FirePoint atanmamış!");
            return;
        }

        // Mermi örneğini oluştur
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Mermide Rigidbody bulunamadı!");
            return;
        }

        // Bulleta fiziksel sapmaların etkisini kaldır
        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Ebeveyn objenin tag'ine göre yön belirle
        Vector3 shootDirection;
        Transform parent = transform.parent;
        if (parent != null && parent.CompareTag("Enemy"))
        {
            // Dünya Z+ yönünün tam tersi
            shootDirection = -Vector3.forward;
        }
        else
        {
            // Player veya diğer durumlarda ileri yön (Z+)
            shootDirection = Vector3.forward;
        }

        // Hızı uygula
        rb.linearVelocity = shootDirection.normalized * _bulletSpeed;
    }
}
