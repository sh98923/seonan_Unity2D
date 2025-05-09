using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private int _defaultPoolSize = 10;

    private void Awake()
    {
        //���ҽ� �������� ������ �ε��ϰ�
        GameObject[] elfEffects = Resources.LoadAll<GameObject>("Prefabs/Effect/ElfEffect");
        GameObject[] skeletonEffects = Resources.LoadAll<GameObject>("Prefabs/Effect/SkeltonEffect");

        // ��� ������ ���� ó��
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