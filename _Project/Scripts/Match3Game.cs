using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class Match3Game : MonoBehaviour
{
    [SerializeField]
    private int2 _size = 8;
    [SerializeField, Range(4, 8)]
    private int _tileTypeCount = 5;

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

    public bool TryMove(Move move)
    {
        _grid.Swap(move.From, move.To);
        return true;
    }

    private void FillGrid()
    {
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                TileState a = TileState.None, b = TileState.None;
                int potentialMatchCount = 0;
                if (x > 1)
                {
                    a = _grid[x - 1, y];
                    if (a == _grid[x - 2, y])
                    {
                        potentialMatchCount = 1;
                    }
                }
                if (y > 1)
                {
                    b = _grid[x, y - 1];
                    if (b == _grid[x, y - 2])
                    {
                        potentialMatchCount += 1;
                        if (potentialMatchCount == 1)
                        {
                            a = b;
                        }
                        else if (b < a)
                        {
                            (a, b) = (b, a);
                        }
                    }
                }
                TileState t = (TileState)Random.Range(1, _tileTypeCount + 1 - potentialMatchCount);
                if (potentialMatchCount > 0 && t >= a)
                {
                    t += 1;
                }
                if (potentialMatchCount == 2 && t >= b)
                {
                    t += 1;
                }
                _grid[x, y] = t;
            }
        }
    }
}