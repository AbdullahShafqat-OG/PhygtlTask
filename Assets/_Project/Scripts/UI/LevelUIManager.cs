using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("Labels")]
    [SerializeField]
    private TMP_Text _titleTxt;
    [SerializeField]
    private TMP_Text _targetTxt;

    private void Awake()
    {
        _gameOverPanel?.SetActive(false);
        _levelCompletePanel?.SetActive(false);

        foreach (var btn in _backBtns)
            btn.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
        _retryBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));

        Messenger<string, int>.AddListener(GameEvent.LEVEL_LOADED, LevelLoaded);
        Messenger.AddListener(GameEvent.GAME_OVER, GameOver);
        Messenger<int>.AddListener(GameEvent.LEVEL_COMPLETE, LevelComplete);
    }

    private void OnDestroy()
    {
        Messenger<string, int>.RemoveListener(GameEvent.LEVEL_LOADED, LevelLoaded);
        Messenger.RemoveListener(GameEvent.GAME_OVER, GameOver);
        Messenger<int>.RemoveListener(GameEvent.LEVEL_COMPLETE, LevelComplete);
    }

    private void LevelLoaded(string title, int target)
    {
        _titleTxt.SetText(title);
        _targetTxt.SetText("Score {0} to win", target);
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
