using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GridSpawner : MonoBehaviour
{
    public static GridSpawner Instance { get; private set; } // �̱��� �ν��Ͻ�

    private PlayerData _playerData;
    [SerializeField] private GameObject _gridCellPrefab; // �׸��� �� ������
    [SerializeField] private Vector2 _gridSize = new Vector2(3, 3); // �׸��� ũ��
    [SerializeField] private float _cellSize = 2f; // �� ũ��
    [SerializeField] private GameObject[] _playerPrefabs;

    private Vector2[,] _spawnGrid;
    private HashSet<Vector2Int> _filledPosition; // ��� ���� ��ġ
    private Dictionary<int, GameObject> _spawnedPlayers = new Dictionary<int, GameObject>(); // ������ ĳ���͵�

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

        Vector2 gridStartPos = new Vector2(-7, -2.5f); // �׸��� ������

        // �׸��� ����
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2 spawnPosition = gridStartPos + new Vector2(x * _cellSize * 1.2f, y * _cellSize * 0.6f); // �� ��ġ ���
                _spawnGrid[x,y] = spawnPosition;
                //_isGridFilled[x, y] = false;

                // �� ����
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
            Debug.LogWarning($"���� �ڸ��� �����ϴ�!");
            return;
        }

        Vector3 spawnPosition = _spawnGrid[gridPosition.x, gridPosition.y];
        GameObject playerPrefab = _playerPrefabs[playerId % _playerPrefabs.Length];
        GameObject spawnedPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        // ĳ���� �ʱ�ȭ
        Player playerScript = spawnedPlayer.GetComponentInChildren<Player>();
        playerScript.Initialize(_playerData);

        // ��ġ ���� ó��
        _filledPosition.Add(gridPosition);
        _spawnedPlayers[playerId] = spawnedPlayer;

        // SpriteRenderer�� sortingOrder ����
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