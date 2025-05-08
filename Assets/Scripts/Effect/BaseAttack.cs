using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseAttack : MonoBehaviour
{
    public int damage = 10;
    public float speed = 5f;
    public float lifetime = 5f;

    private Transform _target;

    public void Initialize(Transform target)
    {
        _target = target;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _target)
        {
            Character oppositeScript = _target.GetComponent<Character>();
            oppositeScript.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
