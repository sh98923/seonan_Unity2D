using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject _gridContainer; // �׸��� �θ� ������Ʈ
    [SerializeField] private GameObject _buttonContainer; // ��ư �θ� ������Ʈ
    public bool IsButtonClicked = false;

    public void OnStartButtonClicked()
    {
        if (_gridContainer != null) _gridContainer.SetActive(false);
        if (_buttonContainer != null) _buttonContainer.SetActive(false);

        IsButtonClicked = true;

        //StartCoroutine(MoveCharactersForward(6)); 
    }
}