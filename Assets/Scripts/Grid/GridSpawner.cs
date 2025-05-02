using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gridCellPrefab; // 그리드 셀 프리팹
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // 그리드 크기
    [SerializeField] private float _cellSize = 2f; // 셀 크기
    [SerializeField] private GameObject[] _characterPrefabs;

    private Vector3[,] _spawnPositions;
    private HashSet<Vector2Int> _filledPosition; // 사용 중인 위치

    private void Awake()
    {
        DataManager.Instance.LoadData();
        _filledPosition = new HashSet<Vector2Int>();
    }

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        _spawnPositions = new Vector3[(int)_gridSize.x, (int)_gridSize.y];
        //_isGridFilled = new bool[(int)_gridSize.x, (int)_gridSize.y];

        Vector2 gridOrigin = new Vector2(-7, -2.5f); // 그리드 시작점

        // 그리드 생성
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridOrigin + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // 셀 위치 계산
                _spawnPositions[x,y] = spawnPosition;
                //_isGridFilled[x, y] = false;

                // 셀 생성
                Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    public void SpawnCharacter(int characterId)
    {
        // 캐릭터 데이터 가져오기
        if (!DataManager.Instance.TryGetCharacterData(characterId, out var characterData))
        {
            Debug.LogError($"CharacterData ID {characterId}를 찾을 수 없습니다!");
            return;
        }

        int x = characterData.PositionX;
        Vector2Int gridPosition = new Vector2Int(x, _filledPosition.Count % (int)_gridSize.y);

        if (_filledPosition.Contains(gridPosition))
        {
            Debug.LogWarning($"그리드 위치 ({gridPosition.x}, {gridPosition.y})가 이미 점유되었습니다!");
            return;
        }

        Vector3 spawnPosition = _spawnPositions[gridPosition.x, gridPosition.y];
        GameObject characterPrefab = _characterPrefabs[characterId % _characterPrefabs.Length];
        GameObject spawnedCharacter = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);

        // 캐릭터 초기화
        Character characterScript = spawnedCharacter.GetComponent<Character>();
        if (characterScript != null)
        {
            characterScript.Initialize(characterData);
        }

        // 위치 점유 처리
        _filledPosition.Add(gridPosition);

        // SpriteRenderer의 sortingOrder 설정
        SortingGroup sortingGroup = spawnedCharacter.GetComponentInChildren<SortingGroup>();
        if (sortingGroup != null)
        {
            sortingGroup.sortingOrder = (int)(_gridSize.y - gridPosition.y);
        }
    }
    //public void SpawnCharacter()
    //{
    //    // 현재 스폰 인덱스 계산
    //    int x = (int)_characterData.PositionX;
    //    int y = _curSpawnIndex / (int)_gridSize.x;
    //
    //    while (y < _gridSize.y && _isGridFilled[x, y])
    //    {
    //        y++; // 다음 줄로 이동
    //    }
    //
    //    // 범위를 초과하지 않도록 체크
    //    if (y >= _gridSize.y)
    //    {
    //        Debug.LogWarning($"그리드의 열 {x}에 빈 자리가 없습니다!");
    //        return;
    //    }
    //
    //    if (_isGridFilled[x,y])
    //    {
    //        Debug.LogWarning($"그리드 위치 ({x}, {y})에 캐릭터가 이미 스폰되었습니다!");
    //        return;
    //    }
    //
    //    // 캐릭터 생성
    //    Vector3 spawnPosition = _spawnPositions[x, y];
    //    GameObject spawnedCharacter = _characterPrefabs[_characterData.Key];
    //    Instantiate(spawnedCharacter, spawnPosition, Quaternion.identity);
    //
    //    // SpriteRenderer의 sortingOrder 설정
    //    SortingGroup sortingGroup = spawnedCharacter.GetComponentInChildren<SortingGroup>();
    //    if (sortingGroup != null)
    //    {
    //        sortingGroup.sortingOrder = (int)(_gridSize.y - y);
    //        Debug.Log($"캐릭터 위치 ({x}, {y})  {sortingGroup.sortingOrder}");
    //    }
    //    else
    //    {
    //        Debug.LogError("SpriteRenderer가 캐릭터 프리팹에 없습니다.");
    //    }
    //
    //    _isGridFilled[x, y] = true;
    //
    //}
}