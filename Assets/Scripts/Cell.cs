using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    Wrapped,
    Unwarapped,
    Hidden
}
[Serializable]
public class CellData
{
    public int Id;
    public int ObjectId;
    public CellState CellState;
    public CellData(int objectId, CellState cellState)
    {
        ObjectId = objectId;
        CellState = cellState;
    }
}
public class Cell : MonoBehaviour
{
    public Image _image;
    public int ObjectId;
    public bool IsWarpped
    {
        get
        {
            return _scratchImage.enabled;
        }
    }
    [SerializeField]
    private Button _scratchButton;
    private Image _scratchImage;
    private static Cell _previousCell, _currentCell;
    public CellState _state;
    public Action<CellState> _onCellStateChange;
    public int Id;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _scratchImage = _scratchButton.GetComponent<Image>();
    }
    void Start()
    {
        _scratchButton.onClick.AddListener(OnCellClick);
        SetState();
        _onCellStateChange += (state) =>
        {
            GameManager._instance._gameProgressData._spriteArrayIndexArray[Id].CellState = state;
        };
    }
    void OnCellClick()
    {
        if(!IsWarpped)
        {
            return;
        }
        if (_currentCell != null)
        {
            _previousCell = _currentCell;
        }
        _currentCell = this;
        UnWarp();
        if(_previousCell != null)
        {
            if(_previousCell.ObjectId == _currentCell.ObjectId)
            {
                StartCoroutine(HideWithDelay(_previousCell, _currentCell, 0.2f));
            }
            else
            {
                StartCoroutine(WrapWithDelay(_previousCell, _currentCell,0.2f));
            }
            _previousCell = _currentCell = null;
        }
    }
    private void SetState()
    {
        switch (_state)
        {
            case CellState.Unwarapped:
                if(_previousCell == null)
                {
                    _previousCell = this;
                }
                UnWarp();
                break;
            case CellState.Hidden:
                Hide();
                break;
            case CellState.Wrapped:
            default:
                Wrap();
                break;
        }
    }
    void Hide()
    {
        _scratchImage.enabled = _image.enabled = false;
        _onCellStateChange?.Invoke(CellState.Hidden);
    }
    void Wrap()
    {
        _scratchImage.enabled = true;
        _image.enabled = false;
        _onCellStateChange?.Invoke(CellState.Wrapped);
    }
    void UnWarp()
    {
        _scratchImage.enabled = false;
        _image.enabled = true;
        _onCellStateChange?.Invoke(CellState.Unwarapped);
    }
    IEnumerator WrapWithDelay(Cell previousCell, Cell currentCell, float secends)
    {
        yield return new WaitForSeconds(secends);
        GameManager._instance.TurnCount++;
        previousCell.Wrap();
        currentCell.Wrap();
        SoundManager._instance.PlayMissmatchAudio();
    }
    IEnumerator HideWithDelay(Cell previousCell, Cell currentCell, float secends)
    {
        yield return new WaitForSeconds(secends);
        GameManager._instance.TurnCount++;
        GameManager._instance.MatchCount++;
        previousCell.Hide();
        currentCell.Hide();
        SoundManager._instance.PlayMatchAudio();
    }
}
