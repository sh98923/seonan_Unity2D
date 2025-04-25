using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gridCellPrefab; // 그리드 셀 프리팹
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // 그리드 크기
    [SerializeField] private float _cellSize = 2f; // 셀 크기
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private ChracterMove _characterMove;

    private Vector3[,] _spawnPositions;
    private int _curSpawnIndex = 0;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        _spawnPositions = new Vector3[(int)_gridSize.x, (int)_gridSize.y];
        Vector2 gridOrigin = new Vector2(-7, -2.5f); // 그리드 시작점

        // 그리드 생성
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridOrigin + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // 셀 위치 계산
                _spawnPositions[x,y] = spawnPosition;

                // 셀 생성
                Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    public void SpawnCharacter()
    {
        // 현재 스폰 인덱스 계산
        int x = _curSpawnIndex % (int)_gridSize.x;
        int y = _curSpawnIndex / (int)_gridSize.x;

        // 범위를 초과하지 않도록 체크
        if (y >= _gridSize.y)
        {
            Debug.LogWarning("모든 그리드가 가득 찼습니다!");
            return;
        }

        // 캐릭터 생성
        Vector3 spawnPosition = _spawnPositions[x, y];
        GameObject spawnedCharacter = Instantiate(_characterPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
        _characterMove.AddCharacter(spawnedCharacter); // 참조로 AddCharacter 호출

        // SpriteRenderer의 sortingOrder 설정
        SortingGroup sortingGroup = spawnedCharacter.GetComponentInChildren<SortingGroup>();
        if (sortingGroup != null)
        {
            sortingGroup.sortingOrder = (int)(_gridSize.y - y);
            Debug.Log($"캐릭터 위치 ({x}, {y})  {sortingGroup.sortingOrder}");
        }
        else
        {
            Debug.LogError("SpriteRenderer가 캐릭터 프리팹에 없습니다.");
        }

        // 인덱스 증가
        _curSpawnIndex++;
    }
}