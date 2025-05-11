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

    private CircleCollider2D _circleCollider;
    private LayerMask _layerTargetMask;

    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        //if (_target == null)
        //{
        //    //PoolingManager.Instance.Release(gameObject.name, gameObject);
        //    return;
        //}
        
        transform.Translate(_dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleCollider.radius, _layerTargetMask);

            Character oppositeScript = colliders[0].gameObject.GetComponentInChildren<Character>();
            oppositeScript.TakeDamage(damage);

            this.gameObject.SetActive(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleCollider.radius, _layerTargetMask);

            Character oppositeScript = colliders[0].gameObject.GetComponentInChildren<Character>();
            oppositeScript.TakeDamage(damage);

            this.gameObject.SetActive(false);
        }
    }
    
    public void SetTargetLayer(LayerMask layerTargetMask)
    {
        _layerTargetMask = layerTargetMask;
    }

    public void SetDirection(Vector2 dir)
    {
        _dir = dir;
    }
}
