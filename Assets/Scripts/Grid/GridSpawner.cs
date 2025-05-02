using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gridCellPrefab; // �׸��� �� ������
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // �׸��� ũ��
    [SerializeField] private float _cellSize = 2f; // �� ũ��
    [SerializeField] private GameObject[] _characterPrefabs;

    private Vector3[,] _spawnPositions;
    private HashSet<Vector2Int> _filledPosition; // ��� ���� ��ġ

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

        Vector2 gridOrigin = new Vector2(-7, -2.5f); // �׸��� ������

        // �׸��� ����
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridOrigin + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // �� ��ġ ���
                _spawnPositions[x,y] = spawnPosition;
                //_isGridFilled[x, y] = false;

                // �� ����
                Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    public void SpawnCharacter(int characterId)
    {
        // ĳ���� ������ ��������
        if (!DataManager.Instance.TryGetCharacterData(characterId, out var characterData))
        {
            Debug.LogError($"CharacterData ID {characterId}�� ã�� �� �����ϴ�!");
            return;
        }

        int x = characterData.PositionX;
        Vector2Int gridPosition = new Vector2Int(x, _filledPosition.Count % (int)_gridSize.y);

        if (_filledPosition.Contains(gridPosition))
        {
            Debug.LogWarning($"�׸��� ��ġ ({gridPosition.x}, {gridPosition.y})�� �̹� �����Ǿ����ϴ�!");
            return;
        }

        Vector3 spawnPosition = _spawnPositions[gridPosition.x, gridPosition.y];
        GameObject characterPrefab = _characterPrefabs[characterId % _characterPrefabs.Length];
        GameObject spawnedCharacter = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);

        // ĳ���� �ʱ�ȭ
        Character characterScript = spawnedCharacter.GetComponent<Character>();
        if (characterScript != null)
        {
            characterScript.Initialize(characterData);
        }

        // ��ġ ���� ó��
        _filledPosition.Add(gridPosition);

        // SpriteRenderer�� sortingOrder ����
        SortingGroup sortingGroup = spawnedCharacter.GetComponentInChildren<SortingGroup>();
        if (sortingGroup != null)
        {
            sortingGroup.sortingOrder = (int)(_gridSize.y - gridPosition.y);
        }
    }
    //public void SpawnCharacter()
    //{
    //    // ���� ���� �ε��� ���
    //    int x = (int)_characterData.PositionX;
    //    int y = _curSpawnIndex / (int)_gridSize.x;
    //
    //    while (y < _gridSize.y && _isGridFilled[x, y])
    //    {
    //        y++; // ���� �ٷ� �̵�
    //    }
    //
    //    // ������ �ʰ����� �ʵ��� üũ
    //    if (y >= _gridSize.y)
    //    {
    //        Debug.LogWarning($"�׸����� �� {x}�� �� �ڸ��� �����ϴ�!");
    //        return;
    //    }
    //
    //    if (_isGridFilled[x,y])
    //    {
    //        Debug.LogWarning($"�׸��� ��ġ ({x}, {y})�� ĳ���Ͱ� �̹� �����Ǿ����ϴ�!");
    //        return;
    //    }
    //
    //    // ĳ���� ����
    //    Vector3 spawnPosition = _spawnPositions[x, y];
    //    GameObject spawnedCharacter = _characterPrefabs[_characterData.Key];
    //    Instantiate(spawnedCharacter, spawnPosition, Quaternion.identity);
    //
    //    // SpriteRenderer�� sortingOrder ����
    //    SortingGroup sortingGroup = spawnedCharacter.GetComponentInChildren<SortingGroup>();
    //    if (sortingGroup != null)
    //    {
    //        sortingGroup.sortingOrder = (int)(_gridSize.y - y);
    //        Debug.Log($"ĳ���� ��ġ ({x}, {y})  {sortingGroup.sortingOrder}");
    //    }
    //    else
    //    {
    //        Debug.LogError("SpriteRenderer�� ĳ���� �����տ� �����ϴ�.");
    //    }
    //
    //    _isGridFilled[x, y] = true;
    //
    //}
}