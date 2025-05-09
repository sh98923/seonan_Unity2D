using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private int _defaultPoolSize = 10;

    private void Awake()
    {
        //리소스 폴더에서 프리펩 로드하고
        GameObject[] elfEffects = Resources.LoadAll<GameObject>("Prefabs/Effect/ElfEffect");
        GameObject[] skeletonEffects = Resources.LoadAll<GameObject>("Prefabs/Effect/SkeltonEffect");

        // 모든 프리팹 통합 처리
        foreach (GameObject effect in elfEffects)
        {
            PoolingManager.Instance.CreatePool(effect.name, effect, _defaultPoolSize); 
        }

        foreach (GameObject effect in skeletonEffects)
        {
            PoolingManager.Instance.CreatePool(effect.name, effect, _defaultPoolSize);
        }
    }
}