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

public class DataManager : Singleton<DataManager>
{
    private Dictionary<int, CharacterData> characterDatas = new Dictionary<int, CharacterData>();

    public CharacterData GetCharacterData(int key)
    {
        return characterDatas[key];
    }

    public bool TryGetCharacterData(int key, out CharacterData characterData)
    {
        return characterDatas.TryGetValue(key, out characterData);
    }

    public void LoadData()
    {
        LoadCharacterTable();
    }

    public int GetTotalCharacterCount()
    {
        return characterDatas.Count;
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
}
