using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections.Generic;

public struct CharacterData
{
    public int Key;
    public string Name;
    public int PositionX;
    public int Hp;
    public int Atk;
    public float CriRate;
    public float Range;
}

public struct EnemyData
{
    public int Key;
    public string Name;
    public int SpawnStage;
    public int Hp;
    public int Atk;
    public float CriRate;
    public float Range;
}

public class DataManager : Singleton<DataManager>
{
    private Dictionary<int, CharacterData> characterDatas = new Dictionary<int, CharacterData>();
    private Dictionary<int, EnemyData> enemyDatas = new Dictionary<int, EnemyData>();

    public CharacterData GetCharacterData(int key)
    {
        return characterDatas[key];
    }

    public EnemyData GetEnemyData(int key)
    {
        return enemyDatas[key];
    }

    public bool TryGetCharacterData(int key, out CharacterData characterData)
    {
        return characterDatas.TryGetValue(key, out characterData);
    }

    public bool TryGetEnemyData(int key, out EnemyData enemyData)
    {
        return enemyDatas.TryGetValue(key, out enemyData);
    }

    public void LoadCharacterData()
    {
        LoadCharacterTable();
    }

    public void LoadEnemyData()
    {
        LoadEnemyTable();
    }

    public int GetTotalCharacterCount()
    {
        return characterDatas.Count;
    }

    public int GetTotalEnemyCount()
    {
        return enemyDatas.Count;
    }

    private void LoadCharacterTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Tables/PlayableTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        for (int i = 1; i < str.Length; i++)
        {
            if (str[i].Length == 0)
                return;

            string[] data = str[i].Split(',');

            CharacterData characterData;
            characterData.Key = int.Parse(data[0]);
            characterData.Name = data[1];
            characterData.PositionX = int.Parse(data[2]);
            characterData.Hp = int.Parse(data[3]);
            characterData.Atk = int.Parse(data[4]);
            characterData.CriRate = float.Parse(data[5]);
            characterData.Range = float.Parse(data[6]);

            if (characterDatas.ContainsKey(characterData.Key))
                continue;

            characterDatas.Add(characterData.Key, characterData);
        }
    }

    private void LoadEnemyTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Tables/EnemyTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        for (int i = 1; i < str.Length; i++)
        {
            if (str[i].Length == 0)
                return;

            string[] data = str[i].Split(',');

            EnemyData enemyData;
            enemyData.Key = int.Parse(data[0]);
            enemyData.Name = data[1];
            enemyData.SpawnStage = int.Parse(data[2]);
            enemyData.Hp = int.Parse(data[3]);
            enemyData.Atk = int.Parse(data[4]);
            enemyData.CriRate = float.Parse(data[5]);
            enemyData.Range = float.Parse(data[6]);

            if (enemyDatas.ContainsKey(enemyData.Key))
                continue;

            enemyDatas.Add(enemyData.Key, enemyData);
        }
    }
}
