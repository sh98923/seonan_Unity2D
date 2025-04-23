using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject _gridContainer; // �׸��� �θ� ������Ʈ
    [SerializeField] private GameObject _buttonContainer; // ��ư �θ� ������Ʈ
    [SerializeField] private float _moveSpeed = 2f; // ĳ���� �̵� �ӵ�

    private List<GameObject> spawnedCharacters = new List<GameObject>(); // ������ ĳ���͵� ����Ʈ

    // ĳ���� �߰�
    public void AddCharacter(GameObject character)
    {
        spawnedCharacters.Add(character);
    }

    // ��ŸƮ ��ư�� ������ �Լ�
    public void OnStartButtonClicked()
    {
        // �׸���� ��ư ��Ȱ��ȭ
        if (_gridContainer != null) _gridContainer.SetActive(false);
        if (_buttonContainer != null) _buttonContainer.SetActive(false);

        // ĳ���� �̵�
        StartCoroutine(MoveCharactersForward(6)); // X ��ǥ 0���� �̵�
    }

    private IEnumerator MoveCharactersForward(float targetX)
    {
        foreach (var character in spawnedCharacters)
        {
            if (character != null)
            {
                // ���� Y ��ǥ�� �����ϰ� X ��ǥ�� ����
                Vector3 targetPosition = new Vector3(
                    targetX,
                    character.transform.position.y,
                    character.transform.position.z // Z�� ���� (2D���� ���� ��� �� ��)
                );

                // ĳ���� �̵� ����
                StartCoroutine(MoveCharacter(character, targetPosition));
            }
        }

        yield return null; // ��� ĳ���� �̵� ����
    }

    private IEnumerator MoveCharacter(GameObject character, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(character.transform.position, targetPosition);

        while (distance > 0.1f) // ��ǥ ��ġ�� ������� ������ �̵�
        {
            character.transform.position = Vector3.MoveTowards(
                character.transform.position,
                targetPosition,
                _moveSpeed * Time.deltaTime
            );

            distance = Vector3.Distance(character.transform.position, targetPosition);
            yield return null;
        }

        // ��Ȯ�� ��ǥ ��ġ�� ����
        character.transform.position = targetPosition;
    }
}