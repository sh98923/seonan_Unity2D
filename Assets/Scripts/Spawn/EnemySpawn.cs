using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnArea; // ������ ��ġ�� ���� ����
    [SerializeField] private GameObject[] enemyPrefabs; // �� ������ �迭

    private float columnSpacing = 2.0f; // �� ����
    private float rowSpacing = 1.5f;   // �� ����
    private int columns = 3;          // �� ��

    private List<Enemy> _alivedEnemies = new List<Enemy>(); // ����ִ� ���� ����Ʈ

    public void SpawnEnemies(List<EnemyData> enemies)
    {
        _alivedEnemies.Clear();
        
        foreach (var enemyData in enemies)
        {
            GameObject prefab = GetEnemyPrefabByName(enemyData.Name);
            // enemyData.Count ��ŭ �� ����
            for (int i = 0; i < enemyData.Count; i++)
            {
                Vector3 spawnPosition = CalculateSpawnPosition(i);
                GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity, spawnArea);

                // �� ������ �ʱ�ȭ
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
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
        int row = index / columns; // �� ���
        int column = index % columns; // �� ���

        Vector2 startPosition = (Vector2)spawnArea.position;
        Vector2 offset = new Vector2(column * columnSpacing, -row * rowSpacing); // ���� �� ���� �ݿ�

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