using UnityEngine;
using UnityEngine.Rendering;

public class CharacterSortingUpdater : MonoBehaviour
{
    private SortingGroup _sortingGroup;

    private void Awake()
    {
        _sortingGroup = GetComponentInChildren<SortingGroup>();
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
