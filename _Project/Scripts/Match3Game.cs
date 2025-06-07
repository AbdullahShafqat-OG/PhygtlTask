using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class Match3Game : MonoBehaviour
{
    [SerializeField]
    private int2 _size = 8;

    private Grid2D<TileState> _grid;

    public TileState this[int x, int y] => _grid[x, y];

    public TileState this[int2 c] => _grid[c];

    public int2 Size => _size;

    public void StartNewGame()
    {
        if (_grid.IsUndefined)
        {
            _grid = new(_size);
        }
        FillGrid();
    }

    void FillGrid()
    {
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                _grid[x, y] = (TileState)Random.Range(1, 8);
            }
        }
    }
}