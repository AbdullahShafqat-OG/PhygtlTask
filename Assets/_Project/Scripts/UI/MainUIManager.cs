using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _levelBtnPrefab;
    [SerializeField]
    private GameObject _levelBtnsParent;

    private void Awake()
    {
        Messenger<LevelData[]>.AddListener(GameEvent.LOAD_LEVELS, GenerateBtns);    
    }

    private void OnDestroy()
    {
        Messenger<LevelData[]>.RemoveListener(GameEvent.LOAD_LEVELS, GenerateBtns);
    }

    private void GenerateBtns(LevelData[] levels)
    {
        foreach (LevelData level in levels)
        {
            GameObject btn = Instantiate(_levelBtnPrefab, _levelBtnsParent.transform);
            btn.GetComponentInChildren<TMP_Text>().text = level.LevelName;

            Button button = btn.GetComponent<Button>();
            LevelData levelCopy = level;
            button.onClick.AddListener(() =>
            {
                Messenger<LevelData>.Broadcast(GameEvent.SET_CURRENT_LEVEL, levelCopy);
                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
            });
        }
    }
}
