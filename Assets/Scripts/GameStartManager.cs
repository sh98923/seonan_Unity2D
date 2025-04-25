using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject _gridContainer; // �׸��� �θ� ������Ʈ
    [SerializeField] private GameObject _buttonContainer; // ��ư �θ� ������Ʈ
    public bool IsButtonClicked = false;

    //��������Ʈ ����ؼ� ���⿡ �� ĳ���͵��� �̵� �Լ� ����ϱ�
    public delegate void MoveEvent();
    public static MoveEvent CharacterMoveEvnet;

    public void OnStartButtonClicked()
    {
        if (_gridContainer != null) _gridContainer.SetActive(false);
        if (_buttonContainer != null) _buttonContainer.SetActive(false);

        IsButtonClicked = true;

        CharacterMoveEvnet();
        //StartCoroutine(MoveCharactersForward(6)); 
    }
}