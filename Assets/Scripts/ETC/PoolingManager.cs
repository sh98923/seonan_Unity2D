using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    [SerializeField] private GameObject[] _baseAttacks;
    [SerializeField] private int _poolSize = 20;

    private List<GameObject> _pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        for (int i = 0; i < _baseAttacks.Length; i++)
        {
            for (int j = 0; i < _poolSize / _baseAttacks.Length; j++)
            {
                GameObject effect = Instantiate(_baseAttacks[j]);
                effect.SetActive(false);
                _pool.Add(effect);
            }
        }
    }

    public GameObject GetEffect(int index)
    {
        foreach(GameObject effect in _pool)
        {
            if(!effect.activeSelf)
            {
                effect.SetActive(true);
                return effect;
            }
        }

        GameObject newEffect = Instantiate(_baseAttacks[index]);
        newEffect.SetActive(false);
        _pool.Add(newEffect);
        return newEffect;
    }

    public void ReturnEffect(GameObject effect)
    {
        effect.SetActive(false);
    }
}
