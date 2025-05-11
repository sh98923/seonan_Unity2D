using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    private void Awake()
    {
        // ��ηκ��� ����Ʈ �������� �ε��ϰ� Ǯ�� �߰�
        LoadEffectsFromResources();
    }

    // Resources �������� ����Ʈ �����յ��� �ε�
    private void LoadEffectsFromResources()
    {
        // 'Effects' ���� �Ʒ� ��� ����Ʈ �������� ������
        GameObject[] effectPrefabs = Resources.LoadAll<GameObject>("Prefabs/Effect");

        foreach (GameObject prefab in effectPrefabs)
        {
            PoolingManager.Instance.Add(prefab.name, 5, prefab);
        }
    }
}