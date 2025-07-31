using UnityEngine;

public class Gun : MonoBehaviour
{
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
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = _firePoint.forward * _bulletSpeed;
        }
    }
}
