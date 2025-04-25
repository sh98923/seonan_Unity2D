using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gridCellPrefab; // �׸��� �� ������
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // �׸��� ũ��
    [SerializeField] private float _cellSize = 2f; // �� ũ��
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
        Vector2 gridOrigin = new Vector2(-7, -2.5f); // �׸��� ������

        // �׸��� ����
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridOrigin + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // �� ��ġ ���
                _spawnPositions[x,y] = spawnPosition;

                // �� ����
                Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    public void SpawnCharacter()
    {
        // ���� ���� �ε��� ���
        int x = _curSpawnIndex % (int)_gridSize.x;
        int y = _curSpawnIndex / (int)_gridSize.x;

        // ������ �ʰ����� �ʵ��� üũ
        if (y >= _gridSize.y)
        {
            Debug.LogWarning("��� �׸��尡 ���� á���ϴ�!");
            return;
        }

        // ĳ���� ����
        Vector3 spawnPosition = _spawnPositions[x, y];
        GameObject spawnedCharacter = Instantiate(_characterPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
        _characterMove.AddCharacter(spawnedCharacter); // ������ AddCharacter ȣ��

        // SpriteRenderer�� sortingOrder ����
        SortingGroup sortingGroup = spawnedCharacter.GetComponentInChildren<SortingGroup>();
        if (sortingGroup != null)
        {
            sortingGroup.sortingOrder = (int)(_gridSize.y - y);
            Debug.Log($"ĳ���� ��ġ ({x}, {y})  {sortingGroup.sortingOrder}");
        }
        else
        {
            Debug.LogError("SpriteRenderer�� ĳ���� �����տ� �����ϴ�.");
        }

        // �ε��� ����
        _curSpawnIndex++;
    }
}