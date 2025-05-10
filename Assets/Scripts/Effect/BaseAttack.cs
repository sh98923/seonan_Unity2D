using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseAttack : MonoBehaviour
{
    public int damage = 10;
    public float speed = 5f;
    public float lifetime = 5f;

    private Transform _target;
    private Vector2 _dir;
    private float _timer;

    public void Initialize(Transform target, int attackdamage, float attackspeed)
    {
        _target = target;
        damage = attackdamage;
        speed = attackspeed;

        _timer = lifetime;
    }

    private void Update()
    {
        if (_target == null)
        {
            //PoolingManager.Instance.Release(gameObject.name, gameObject);
            return;
        }
        
        transform.Translate(_dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _target)
        {
            Character oppositeScript = _target.GetComponent<Character>();
            oppositeScript.TakeDamage(damage);

            //PoolingManager.Instance.Release(gameObject.name, gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        _dir = dir;
    }
}
