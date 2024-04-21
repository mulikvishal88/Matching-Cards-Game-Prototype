using System.IO;
using UnityEngine;

public class GameProgressData
{
    public bool _hasSavedProgress;
    public int _rowCount, _columnCount;
    [SerializeField]
    public CellData[] _spriteArrayIndexArray;
    public int MatchCount, TurnCount;
    public void Save()
    {
        var FilePath = Application.persistentDataPath + "/GameProgressData.json";
        var json = JsonUtility.ToJson(this);
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath);
        }
        File.WriteAllText(FilePath, json);
    }
    public static GameProgressData Load()
    {
        var path = Application.persistentDataPath + "/GameProgressData.json";
        if(!File.Exists(path))
        {
            return null;
        }
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameProgressData>(json);
    }
}
