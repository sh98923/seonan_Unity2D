using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    private void Awake()
    {
        // 경로로부터 이펙트 프리팹을 로드하고 풀에 추가
        LoadEffectsFromResources();
    }

    // Resources 폴더에서 이펙트 프리팹들을 로드
    private void LoadEffectsFromResources()
    {
        // 'Effects' 폴더 아래 모든 이펙트 프리팹을 가져옴
        GameObject[] effectPrefabs = Resources.LoadAll<GameObject>("Prefabs/Effect");

        foreach (GameObject prefab in effectPrefabs)
        {
            PoolingManager.Instance.Add(prefab.name, 5, prefab);
        }
    }
}