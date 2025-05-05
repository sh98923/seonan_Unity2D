using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameStartManager : MonoBehaviour
{
    public static GameStartManager Instance { get; private set; }

    [SerializeField] private GameObject _gridContainer; // 그리드 부모 오브젝트
    [SerializeField] private GameObject _buttonContainer; // 버튼 부모 오브젝트
    [SerializeField] private EnemySpawn _enemySpawner;
    [SerializeField] private int _currentStage = 1;

    public bool IsButtonClicked = false;

    public event Action BattleStartEvent;

    private void Awake()
    {
        DataManager.Instance.Awake();

        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnStartButtonClicked()
    {
        if (_gridContainer != null) _gridContainer.SetActive(false);
        if (_buttonContainer != null) _buttonContainer.SetActive(false);

        IsButtonClicked = true;

        List<EnemyData> stageEnemies = DataManager.Instance.GetStageEnemies(_currentStage);

        _enemySpawner.SpawnEnemies(stageEnemies);

        BattleStartEvent?.Invoke();
    }

    public void ResetButtonClicked()
    {
        IsButtonClicked = false;
    }
}