using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject _gridContainer; // 그리드 부모 오브젝트
    [SerializeField] private GameObject _buttonContainer; // 버튼 부모 오브젝트
    [SerializeField] private float _moveSpeed = 2f; // 캐릭터 이동 속도

    private List<GameObject> spawnedCharacters = new List<GameObject>(); // 생성된 캐릭터들 리스트

    // 캐릭터 추가
    public void AddCharacter(GameObject character)
    {
        spawnedCharacters.Add(character);
    }

    // 스타트 버튼에 연결할 함수
    public void OnStartButtonClicked()
    {
        // 그리드와 버튼 비활성화
        if (_gridContainer != null) _gridContainer.SetActive(false);
        if (_buttonContainer != null) _buttonContainer.SetActive(false);

        // 캐릭터 이동
        StartCoroutine(MoveCharactersForward(6)); // X 좌표 0으로 이동
    }

    private IEnumerator MoveCharactersForward(float targetX)
    {
        foreach (var character in spawnedCharacters)
        {
            if (character != null)
            {
                // 현재 Y 좌표를 유지하고 X 좌표만 변경
                Vector3 targetPosition = new Vector3(
                    targetX,
                    character.transform.position.y,
                    character.transform.position.z // Z는 유지 (2D에서 보통 사용 안 함)
                );

                // 캐릭터 이동 시작
                StartCoroutine(MoveCharacter(character, targetPosition));
            }
        }

        yield return null; // 모든 캐릭터 이동 시작
    }

    private IEnumerator MoveCharacter(GameObject character, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(character.transform.position, targetPosition);

        while (distance > 0.1f) // 목표 위치에 가까워질 때까지 이동
        {
            character.transform.position = Vector3.MoveTowards(
                character.transform.position,
                targetPosition,
                _moveSpeed * Time.deltaTime
            );

            distance = Vector3.Distance(character.transform.position, targetPosition);
            yield return null;
        }

        // 정확히 목표 위치에 정렬
        character.transform.position = targetPosition;
    }
}