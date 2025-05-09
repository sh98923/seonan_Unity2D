using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GridSpawner : MonoBehaviour
{
    public static GridSpawner Instance { get; private set; } // 싱글톤 인스턴스

    private PlayerData _playerData;
    [SerializeField] private GameObject _gridCellPrefab; // 그리드 셀 프리팹
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // 그리드 크기
    [SerializeField] private float _cellSize = 2f; // 셀 크기
    [SerializeField] private GameObject[] _playerPrefabs;

    private Vector2[,] _spawnGrid;
    private HashSet<Vector2Int> _filledPosition; // 사용 중인 위치
    private Dictionary<int, GameObject> _spawnedPlayers = new Dictionary<int, GameObject>(); // 생성된 캐릭터들

    private void Awake()
    {
        int totalPlayers = DataManager.Instance.GetTotalPlayerCount();

        for (int i = 1; i < totalPlayers; i++)
        {
            _playerData = DataManager.Instance.GetPlayerData(i);
        }

        _filledPosition = new HashSet<Vector2Int>();
    }

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        _spawnGrid = new Vector2[(int)_gridSize.x, (int)_gridSize.y];

        Vector2 gridStartPos = new Vector2(-7, -2.5f); // 그리드 시작점

        // 그리드 생성
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridStartPos + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // 셀 위치 계산
                _spawnGrid[x,y] = spawnPosition;
                //_isGridFilled[x, y] = false;

                // 셀 생성
                Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    public void SpawnPlayer(int playerId)
    {
        _playerData = DataManager.Instance.GetPlayerData(playerId);
        
        int x = _playerData.PositionX;
        Vector2Int gridPosition = new Vector2Int(x, _filledPosition.Count % (int)_gridSize.y);

        if (_filledPosition.Contains(gridPosition))
        {
            Debug.LogWarning($"남은 자리가 없습니다!");
            return;
        }

        Vector3 spawnPosition = _spawnGrid[gridPosition.x, gridPosition.y];
        GameObject playerPrefab = _playerPrefabs[playerId % _playerPrefabs.Length];
        GameObject spawnedPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        // 캐릭터 초기화
        Player playerScript = spawnedPlayer.GetComponentInChildren<Player>();
        playerScript.Initialize(_playerData);

        // 위치 점유 처리
        _filledPosition.Add(gridPosition);
        _spawnedPlayers[playerId] = spawnedPlayer;

        // SpriteRenderer의 sortingOrder 설정
        //SortingGroup sortingGroup = spawnedPlayer.GetComponentInChildren<SortingGroup>();
        //sortingGroup.sortingOrder = (int)(_gridSize.y - gridPosition.y);
    }
    public void ResetPlayerToInitPos()
    {
        _filledPosition.Clear();

        foreach(var playerInfo in _spawnedPlayers)
        {
            int playerKey = playerInfo.Key;
            GameObject playerObject = playerInfo.Value;

            PlayerData playerData = DataManager.Instance.GetPlayerData(playerKey);

            int x = playerData.PositionX;
            int y = _filledPosition.Count % (int)_gridSize.y;
            Vector2Int gridPosition = new Vector2Int(x, y);
            Vector2 initialPos = _spawnGrid[gridPosition.x, gridPosition.y];

            playerObject.transform.position = initialPos;

            _filledPosition.Add(gridPosition);
        }
    }
}