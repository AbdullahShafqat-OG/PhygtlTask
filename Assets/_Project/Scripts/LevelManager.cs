using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelData[] _levels;
    [SerializeField]
    private LevelData _currentLevel;

    private void Awake()
    {
        Messenger<LevelData>.AddListener(GameEvent.SET_CURRENT_LEVEL, SetCurrentLevel);
    }

    private void OnDestroy()
    {
        Messenger<LevelData>.RemoveListener(GameEvent.SET_CURRENT_LEVEL, SetCurrentLevel);
    }

    private void Start()
    {
        Messenger<LevelData[]>.Broadcast(GameEvent.LOAD_LEVELS, _levels);
    }

    private void SetCurrentLevel(LevelData data)
    {
        _currentLevel.Size = data.Size;
        _currentLevel.TileTypeCount = data.TileTypeCount;
        _currentLevel.TargetScore = data.TargetScore;
    }
}
