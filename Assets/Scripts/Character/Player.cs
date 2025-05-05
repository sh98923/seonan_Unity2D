using UnityEngine;

public class Player : Character
{
    private PlayerData _playerData;

    protected override int _targetLayerMask => LayerMask.GetMask("Enemy");
    protected override float _characterRange => _playerData.Range;

    protected override void Awake()
    {
        int totalPlayers = DataManager.Instance.GetTotalPlayerCount();
        
        for (int i = 1; i < totalPlayers; i++)
        {
            _playerData = DataManager.Instance.GetPlayerData(i);
        }

        _currentHp = _playerData.Hp;
        base.Awake();
    }

    public void Initialize(PlayerData data)
    {
        _playerData = data;
        _playerData.Hp = _currentHp;

        Debug.Log($"ĳ���� ����: ID={data.Name}, ü��={data.Hp}, ���ݷ�={data.Atk}");
    }
}
