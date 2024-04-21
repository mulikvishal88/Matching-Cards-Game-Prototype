using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class LevelSelectionToggle : MonoBehaviour
{
    public int RowCount
    {
        get
        {
            return _rowCount;
        }
    }
    public int ColumnCount
    {
        get
        {
            return _columnCount;
        }
    }
    [SerializeField]
    private int _rowCount, _columnCount;
}
