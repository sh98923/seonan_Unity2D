using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnArea; 
    [SerializeField] private GameObject[] enemyPrefabs; 

    private float columnSpacing = 2.0f; 
    private float rowSpacing = 1.5f;  
    private int columns = 3;         

    private List<Enemy> _alivedEnemies = new List<Enemy>(); 

    public void SpawnEnemies(List<EnemyData> enemies)
    {
        _alivedEnemies.Clear();
        
        foreach (var enemyData in enemies)
        {
            GameObject prefab = GetEnemyPrefabByName(enemyData.Name);

            for (int i = 0; i < enemyData.Count; i++)
            {
                Vector3 spawnPosition = CalculateSpawnPosition(i);
                GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity, spawnArea);

                Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();
                enemyComponent.Initialize(enemyData);

                enemyComponent.OnEnemyDefeated += CheckEnemyDefeat;

                _alivedEnemies.Add(enemyComponent);
            }
        }
    }
    private GameObject GetEnemyPrefabByName(string name)
    {
        foreach (var prefab in enemyPrefabs)
        {
            if (prefab.name == name)
                return prefab;
        }

        return null;
    }

    private Vector2 CalculateSpawnPosition(int index)
    {
        int row = index / columns; // 행 계산
        int column = index % columns; // 열 계산

        Vector2 startPosition = (Vector2)spawnArea.position;
        Vector2 offset = new Vector2(column * columnSpacing, -row * rowSpacing); // 열과 행 간격 반영

        float randomYOffset = Random.Range(-0.5f, 0.5f);
        offset.y += randomYOffset;

        return startPosition + offset;
    }

    private void CheckEnemyDefeat(Enemy defeatedEnemy)
    {
        _alivedEnemies.Remove(defeatedEnemy);

        if(isAllEnemiesDefeated())
        {
            Debug.Log("All defeated");
            GameStartManager.Instance.ReturnToPlace();
        }
    }

    public bool isAllEnemiesDefeated()
    {
        return _alivedEnemies.Count == 0;
    }
}