using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameStartManager : MonoBehaviour
{
    public static GameStartManager Instance { get; private set; }

    [SerializeField] private GameObject _gridContainer; // 그리드 부모 오브젝트
    [SerializeField] private GameObject _buttonContainer; // 버튼 부모 오브젝트
    private Character _character;
    public bool IsButtonClicked = false;

    //델리게이트 사용해서 여기에 각 캐릭터들의 이동 함수 등록하기
    //public delegate void MoveEvent();
    //public static MoveEvent CharacterMoveEvnet;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnStartButtonClicked()
    {
        if (_gridContainer != null) _gridContainer.SetActive(false);
        if (_buttonContainer != null) _buttonContainer.SetActive(false);

        IsButtonClicked = true;
        
        //StartCoroutine(MoveCharactersForward(6)); 
    }
}