using UnityEngine;
using Unity.Mathematics;

[CreateAssetMenu(fileName = "LevelData", menuName = "Match3/Level Data")]
public class LevelData : ScriptableObject
{
    public int2 Size;
    [Range(4, 8)]
    public int TileTypeCount;
    public int TargetScore;
}
