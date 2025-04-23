using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gridCellPrefab; // 그리드 셀 프리팹
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // 그리드 크기
    [SerializeField] private float _cellSize = 2f; // 셀 크기

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        Vector2 gridOrigin = new Vector2(-7, -2.5f); // 그리드 시작점

        // 그리드 생성
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridOrigin + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // 셀 위치 계산

                // 셀 생성
                Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}