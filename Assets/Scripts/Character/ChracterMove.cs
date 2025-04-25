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
                // 현재 Y 좌표를 유지하고 X 좌표만 변경
                Vector3 targetPosition = new Vector3(targetX, character.transform.position.y,
                                         character.transform.position.z); // Z는 유지 (2D에서 보통 사용 안 함)

                StartCoroutine(MoveCharacter(character, targetPosition));
            }
        }

        yield return null; // 모든 캐릭터 이동 시작 <- '다음 프레임에서 다시 실행'을 의미
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

        // 정확히 목표 위치에 정렬
        character.transform.position = targetPosition;
    }
}
