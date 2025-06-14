using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Match3Skin _match3;
    [SerializeField]
    private LevelData _currentLevelData;

    private Vector3 _dragStart;
    private bool _isDragging;

    private void Start() => StartNewGame();

    private void Update()
    {
        if (_match3.IsPlaying)
        {
            if (!_match3.IsBusy)
            {
                HandleInput();
            }
            _match3.DoWork();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            StartNewGame();
        }
    }

    private void HandleInput() 
    {
        if(!_isDragging && Input.GetMouseButtonDown(0))
        {
            _dragStart = Input.mousePosition;
            _isDragging = true;
        }
        else if (_isDragging && Input.GetMouseButton(0))
        {
            _isDragging = _match3.EvaluateDrag(_dragStart, Input.mousePosition);
        }
        else
        {
            _isDragging = false;
        }
    }

    private void StartNewGame()
    {
        Messenger<string, int>.Broadcast(GameEvent.LEVEL_LOADED, _currentLevelData.LevelName, _currentLevelData.TargetScore);
        _match3.StartNewGame(_currentLevelData.Size, _currentLevelData.TileTypeCount, _currentLevelData.TargetScore);
    }
}
