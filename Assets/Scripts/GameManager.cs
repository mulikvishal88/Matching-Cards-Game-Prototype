using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    private int _matchCount, turnCount;
    public int MatchCount
    {
        set
        {
            _matchCount = value;
            _matchesText.text = _matchCount.ToString();
            _gameProgressData.MatchCount = value;
            if(value >= _maximumMatchCount)
            {
                _gameProgressData._hasSavedProgress = false;
            }
        }
        get{ return _matchCount; }
    }
    public int TurnCount
    {
        set
        {
            turnCount = value;
            _turnsText.text = turnCount.ToString();
            _gameProgressData.TurnCount = value;
        }
        get { return turnCount; }
    }
    [SerializeField]
    private LevelSelectionToggleGroup _difficultyLevelManager;
    [SerializeField]
    private Button _playButton, _homeButton;
    [SerializeField]
    private GameObject _homeView, _gameView;
    [SerializeField]
    private GameObject _cellPrefab, _cellHorizontalGroupPrefab;
    [SerializeField]
    private Transform _horizontalGroupContainerTransform;
    [SerializeField]
    private Sprite[] _cellImageCollection;
    [SerializeField]
    private TMP_Text _matchesText, _turnsText;
    [HideInInspector]
    public GameProgressData _gameProgressData;
    private int _maximumMatchCount;
    private GameManager()
    {
        _instance = this;
    }
    private void Awake()
    {
        _gameProgressData = GameProgressData.Load();
    }
    private void Start()
    {
        _homeButton.onClick.AddListener(SetHomeView);
        _playButton.onClick.AddListener(OnPlay);
        if (_gameProgressData == null || !_gameProgressData._hasSavedProgress)
        {
            _gameProgressData = new GameProgressData();
            SetHomeView();
            return;
        }

        LaunchSavedGame();
    }
    private void OnPlay()
    {
        LaunchNewGame();
    }
    private void LaunchNewGame()
    {
        _gameView.SetActive(true);
        ResetGame();
        var rowCount = _difficultyLevelManager.SelectedLevel.RowCount;
        var columnCount = _difficultyLevelManager.SelectedLevel.ColumnCount;
        _maximumMatchCount = (rowCount * columnCount) / 2;
        var spriteArrayIndexArray = Enumerable.Range(0, _maximumMatchCount)
            .SelectMany(x => Enumerable.Repeat(x, 2))
            .OrderBy(x => Guid.NewGuid())
            .Select(x => new CellData(x, CellState.Wrapped))
            .ToArray();
        ConstructGameGrid(rowCount, columnCount, spriteArrayIndexArray);
        _homeView.SetActive(false);
    }
    private void LaunchSavedGame()
    {
        _gameView.SetActive(true);
        var rowCount = _gameProgressData._rowCount;
        var columnCount = _gameProgressData._columnCount;
        _maximumMatchCount = (rowCount * columnCount) / 2;
        var spriteArrayIndexArray = _gameProgressData._spriteArrayIndexArray;
        ConstructGameGrid(rowCount, columnCount, spriteArrayIndexArray);
        _homeView.SetActive(false);
        MatchCount = _gameProgressData.MatchCount;
        TurnCount = _gameProgressData.TurnCount;
    }
    private void ConstructGameGrid(int rowCount, int columnCount, CellData[] spriteArrayIndexArray)
    {
        var cellCounter = 0;
        for (int i = 0; i < rowCount; i++)
        {
            var cellHorizontalGroupInstance = Instantiate(_cellHorizontalGroupPrefab);
            cellHorizontalGroupInstance.transform.SetParent(_horizontalGroupContainerTransform, false);
            for (int j = 0; j < columnCount; j++)
            {
                var cellInstance = Instantiate(_cellPrefab);
                cellInstance.transform.SetParent(cellHorizontalGroupInstance.transform, false);
                var cell = cellInstance.GetComponent<Cell>();
                cell.ObjectId = spriteArrayIndexArray[cellCounter].ObjectId;
                cell._image.sprite = _cellImageCollection[cell.ObjectId];
                cell._state = spriteArrayIndexArray[cellCounter].CellState;
                cell.Id = cellCounter;
                cellCounter++;
            }
        }
        _gameProgressData._rowCount = rowCount;
        _gameProgressData._columnCount = columnCount;
        _gameProgressData._spriteArrayIndexArray = spriteArrayIndexArray;
        _gameProgressData._hasSavedProgress = true;

    }
    private void SetHomeView()
    {
        _gameProgressData._hasSavedProgress = false;
        _homeView.SetActive(true);
        _gameView.SetActive(false);
    }
    private void ResetGame()
    {
        MatchCount = TurnCount = 0;
        ResetGameView();
    }
    private void ResetGameView()
    {
        for (int i = 0; i < _horizontalGroupContainerTransform.childCount; i++)
        {
            Destroy(_horizontalGroupContainerTransform.GetChild(i).gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        _gameProgressData.Save();
    }
}
