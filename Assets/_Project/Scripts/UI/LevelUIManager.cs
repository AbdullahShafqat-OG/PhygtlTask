using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private GameObject _levelCompletePanel;

    [Header("Buttons")]
    [SerializeField]
    private Button[] _backBtns;
    [SerializeField]
    private Button _retryBtn;

    private void Awake()
    {
        _gameOverPanel?.SetActive(false);
        _levelCompletePanel?.SetActive(false);

        foreach (var btn in _backBtns)
            btn.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
        _retryBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));

        Messenger.AddListener(GameEvent.GAME_OVER, GameOver);
        Messenger<int>.AddListener(GameEvent.LEVEL_COMPLETE, LevelComplete);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_OVER, GameOver);
        Messenger<int>.RemoveListener(GameEvent.LEVEL_COMPLETE, LevelComplete);
    }

    private void GameOver()
    {
        _gameOverPanel?.SetActive(true);
    }

    private void LevelComplete(int score)
    {
        _levelCompletePanel?.SetActive(true);
    }
}
