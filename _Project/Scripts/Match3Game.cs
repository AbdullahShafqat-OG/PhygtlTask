using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using Random = UnityEngine.Random;

public class Match3Game : MonoBehaviour
{
    public List<int2> ClearedTileCoordinates
    { get; private set; }
    public List<TileDrop> DroppedTiles
    { get; private set; }
    public List<SingleScore> Scores
    { get; private set; }
    public bool NeedsFilling
    { get; private set; }
    public int TotalScore
    { get; private set; }
    public Move PossibleMove
    { get; private set; }

    [SerializeField]
    private int2 _size = 8;
    public int2 Size => _size;
    [SerializeField, Range(4, 8)]
    private int _tileTypeCount = 5;

    private Grid2D<TileState> _grid;
    public TileState this[int x, int y] => _grid[x, y];
    public TileState this[int2 c] => _grid[c];

    private List<Match> _matches;
    public bool HasMatches => _matches.Count > 0;

    private int _scoreMultiplier;

    public void StartNewGame()
    {
        TotalScore = 0;
        if (_grid.IsUndefined)
        {
            _grid = new(_size);
            _matches = new();
            ClearedTileCoordinates = new();
            DroppedTiles = new();
            Scores = new();
        }
        FillGrid();
        PossibleMove = Move.FindMove(this);
    }

    public bool TryMove(Move move)
    {
        _scoreMultiplier = 1;
        _grid.Swap(move.From, move.To);
        if (FindMatches())
        {
            return true;
        }
        _grid.Swap(move.From, move.To);
        return false;
    }

    public void ProcessMatches()
    {
        ClearedTileCoordinates.Clear();
        Scores.Clear();

        for (int m = 0; m < _matches.Count; m++)
        {
            Match match = _matches[m];
            int2 step = match.isHorizontal ? int2(1, 0) : int2(0, 1);
            int2 c = match.coordinates;
            for (int i = 0; i < match.length; c += step, i++)
            {
                if (_grid[c] != TileState.None)
                {
                    _grid[c] = TileState.None;
                    ClearedTileCoordinates.Add(c);
                }
            }
            var score = new SingleScore
            {
                position = match.coordinates + (float2)step * (match.length - 1) * 0.5f,
                value = match.length * _scoreMultiplier++
            };
            Scores.Add(score);
            TotalScore += score.value;
            Messenger<int>.Broadcast(GameEvent.SCORE_UPDATED, TotalScore);
        }

        _matches.Clear();
        NeedsFilling = true;
    }

    public void DropTiles()
    {
        DroppedTiles.Clear();

        for (int x = 0; x < _size.x; x++)
        {
            int holeCount = 0;
            for (int y = 0; y < _size.y; y++)
            {
                if (_grid[x, y] == TileState.None)
                {
                    holeCount += 1;
                }
                else if (holeCount > 0)
                {
                    _grid[x, y - holeCount] = _grid[x, y];
                    DroppedTiles.Add(new TileDrop(x, y - holeCount, holeCount));
                }
            }

            for (int h = 1; h <= holeCount; h++)
            {
                _grid[x, _size.y - h] = (TileState)Random.Range(1, 8);
                DroppedTiles.Add(new TileDrop(x, _size.y - h, holeCount));
            }
        }

        NeedsFilling = false;
        if (!FindMatches())
        {
            PossibleMove = Move.FindMove(this);
        }
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

    private bool FindMatches()
    {
        // searching for horizontal _matches 
        for (int y = 0; y < _size.y; y++)
        {
            TileState start = _grid[0, y];
            int length = 1;
            for (int x = 1; x < _size.x; x++)
            {
                TileState t = _grid[x, y];
                if (t == start)
                {
                    length += 1;
                }
                else
                {
                    if (length >= 3)
                    {
                        _matches.Add(new Match(x - length, y, length, true));
                    }
                    start = t;
                    length = 1;
                }
            }
            if (length >= 3)
            {
                _matches.Add(new Match(_size.x - length, y, length, true));
            }
        }

        // searching for vertical _matches
        for (int x = 0; x < _size.x; x++)
        {
            TileState start = _grid[x, 0];
            int length = 1;
            for (int y = 1; y < _size.y; y++)
            {
                TileState t = _grid[x, y];
                if (t == start)
                {
                    length += 1;
                }
                else
                {
                    if (length >= 3)
                    {
                        _matches.Add(new Match(x, y - length, length, false));
                    }
                    start = t;
                    length = 1;
                }
            }
            if (length >= 3)
            {
                _matches.Add(new Match(x, _size.y - length, length, false));
            }
        }

        return HasMatches;
    }
}