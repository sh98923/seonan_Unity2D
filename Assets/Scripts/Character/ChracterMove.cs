using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterMove : MonoBehaviour
{
    [SerializeField]
    private GameStartManager _gameStartManager;
    [SerializeField] 
    private float _moveSpeed = 2f;

    private List<GameObject> _spawnedCharacters = new List<GameObject>();

    private void Start()
    {
        if(_gameStartManager != null && _gameStartManager.IsButtonClicked)
        {
            StartCoroutine(MoveCharactersForward(6));
        }
    }
    public void AddCharacter(GameObject character)
    {
        _spawnedCharacters.Add(character);
    }
    private IEnumerator MoveCharactersForward(float targetX)
    {
        foreach (var character in _spawnedCharacters)
        {
            if (character != null)
            {
                // ���� Y ��ǥ�� �����ϰ� X ��ǥ�� ����
                Vector3 targetPosition = new Vector3(targetX, character.transform.position.y,
                                         character.transform.position.z); // Z�� ���� (2D���� ���� ��� �� ��)

                StartCoroutine(MoveCharacter(character, targetPosition));
            }
        }

        yield return null; // ��� ĳ���� �̵� ���� <- '���� �����ӿ��� �ٽ� ����'�� �ǹ�
    }

    private IEnumerator MoveCharacter(GameObject character, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(character.transform.position, targetPosition);

        while (distance > 0.1f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, targetPosition,
                                           _moveSpeed * Time.deltaTime);

            distance = Vector3.Distance(character.transform.position, targetPosition);
            yield return null;
        }

        // ��Ȯ�� ��ǥ ��ġ�� ����
        character.transform.position = targetPosition;
    }
}
