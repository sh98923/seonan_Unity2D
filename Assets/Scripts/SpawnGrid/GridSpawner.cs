using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gridCellPrefab; // �׸��� �� ������
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // �׸��� ũ��
    [SerializeField] private float _cellSize = 2f; // �� ũ��

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        Vector2 gridOrigin = new Vector2(-7, -2.5f); // �׸��� ������

        // �׸��� ����
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridOrigin + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // �� ��ġ ���

                // �� ����
                Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}