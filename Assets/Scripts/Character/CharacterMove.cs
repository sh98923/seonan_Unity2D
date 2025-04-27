using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private Animator _animator;
    private Vector3 _lastPosition;
    private float _speed;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        _lastPosition = transform.position;
    }

    void Update()
    {
        _speed = (transform.position - _lastPosition).magnitude / Time.deltaTime;
        _animator.SetFloat("Speed", _speed);

        // ���� ��ġ�� ���� �����ӿ� ����� ���� ��ġ�� ����
        _lastPosition = transform.position;
    }
}
