using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverPanel;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);

        Messenger.AddListener(GameEvent.GAME_OVER, GameOver);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_OVER, GameOver);
    }

    private void GameOver()
    {
        _gameOverPanel?.SetActive(true);
    }
}
