using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreTxt;

    private void Awake()
    {
        if (_scoreTxt == null)
            _scoreTxt = GetComponent<TMP_Text>();

        UpdateView(0);
        Messenger<int>.AddListener(GameEvent.SCORE_UPDATED, UpdateView);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.SCORE_UPDATED, UpdateView);
    }

    private void UpdateView(int score)
    {
        Debug.Log("Score broadcasted");
        _scoreTxt.text = score.ToString();
    }
}
