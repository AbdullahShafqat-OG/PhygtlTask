using UnityEngine;
using Unity.Mathematics;

[CreateAssetMenu(fileName = "LevelData", menuName = "Match3/Level Data")]
public class LevelData : ScriptableObject
{
    [HideInInspector]
    public string LevelName;

    public int2 Size;
    [Range(4, 8)]
    public int TileTypeCount;
    public int TargetScore;

    private void OnValidate()
    {
        LevelName = name;
    }
}
