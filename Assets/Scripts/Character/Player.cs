using UnityEngine;

public class Player : Character
{
    private PlayerData _playerData;

    protected override int _targetLayerMask => LayerMask.GetMask("Enemy");
    protected override int _characterAtk => _playerData.Atk;
    protected override float _characterRange => _playerData.Range;

    protected override void Awake()
    {
        int totalPlayers = DataManager.Instance.GetTotalPlayerCount();
        
        for (int i = 1; i < totalPlayers; i++)
        {
            _playerData = DataManager.Instance.GetPlayerData(i);
        }

        base.Awake();
    }

    protected override void Update()
    {
        if (GameStartManager.Instance.IsUIReturned())
        {
            _currentHp = _playerData.Hp;
            _isDead = false;
            SetIdle();
            _animator.SetBool("isDead", false);
        }

        base.Update();
    }

    public void Initialize(PlayerData data)
    {
        _playerData = data;
        _currentHp = _playerData.Hp;

        Debug.Log($"ĳ���� ����: ID={data.Name}, ü��={data.Hp}, ���ݷ�={data.Atk}");
    }
}
