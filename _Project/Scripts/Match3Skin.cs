using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

public class Match3Skin : MonoBehaviour
{
    public bool IsPlaying => true;

    [SerializeField]
    private Tile[] _tilePrefabs;
    [SerializeField]
    private Match3Game _game;

    [Header("Animation Params")]
    [SerializeField]
    private TileSwapper _tileSwapper;
    [SerializeField, Range(0.1f, 20f)]
    private float _dropSpeed = 8f;
    [SerializeField, Range(0f, 10f)]
    private float _newDropOffset = 2f;

    [Header("Touch Params")]
    [SerializeField, Range(0.1f, 1f)]
    private float _dragThreshold = 0.5f;

    private Grid2D<Tile> _tiles;
    private float2 _tileOffset;
    private float _busyDuration;
    public bool IsBusy => _busyDuration > 0f;

    public void StartNewGame() 
    {
        _game.StartNewGame();
        _tileOffset = -0.5f * (float2)(_game.Size - 1);
        if (_tiles.IsUndefined)
        {
            _tiles = new(_game.Size);
        }
        else
        {
            for (int y = 0; y < _tiles.SizeY; y++)
            {
                for (int x = 0; x < _tiles.SizeX; x++)
                {
                    _tiles[x, y].Despawn();
                    _tiles[x, y] = null;
                }
            }
        }

        for (int y = 0; y < _tiles.SizeY; y++)
        {
            for (int x = 0; x < _tiles.SizeX; x++)
            {
                _tiles[x, y] = SpawnTile(_game[x, y], x, y);
            }
        }
    }


    public void DoWork() 
    {
        if (_busyDuration > 0f)
        {
            _tileSwapper.Update();
            _busyDuration -= Time.deltaTime;
            if (_busyDuration > 0f)
            {
                return;
            }
        }

        if (_game.HasMatches)
        {
            ProcessMatches();
        }
        else if (_game.NeedsFilling)
        {
            DropTiles();
        }
    }

    public bool EvaluateDrag(Vector3 start, Vector3 end)
    {
        float2 a = ScreenToTileSpace(start), b = ScreenToTileSpace(end);
        var move = new Move(
            (int2)floor(a), (b - a) switch
            {
                var d when d.x > _dragThreshold => MoveDirection.Right,
                var d when d.x < -_dragThreshold => MoveDirection.Left,
                var d when d.y > _dragThreshold => MoveDirection.Up,
                var d when d.y < -_dragThreshold => MoveDirection.Down,
                _ => MoveDirection.None
            }
        );
        if (
            move.IsValid &&
            _tiles.AreValidCoordinates(move.From) && _tiles.AreValidCoordinates(move.To)
            )
        {
            DoMove(move);
            return false;
        }
        return true;
    }

    private Tile SpawnTile(TileState t, float x, float y) =>
        _tilePrefabs[(int)t - 1].Spawn(new Vector3(x + _tileOffset.x, y + _tileOffset.y));

    private void DoMove(Move move)
    {
        bool success = _game.TryMove(move);
        Tile a = _tiles[move.From], b = _tiles[move.To];
        _busyDuration = _tileSwapper.Swap(a, b, !success);
        if (success)
        {
            _tiles[move.From] = b;
            _tiles[move.To] = a;
        }
    }

    private float2 ScreenToTileSpace(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Vector3 p = ray.origin - ray.direction * (ray.origin.z / ray.direction.z);
        return float2(p.x - _tileOffset.x + 0.5f, p.y - _tileOffset.y + 0.5f);
    }

    private void ProcessMatches()
    {
        _game.ProcessMatches();

        for (int i = 0; i < _game.ClearedTileCoordinates.Count; i++)
        {
            int2 c = _game.ClearedTileCoordinates[i];
            _busyDuration = Mathf.Max(_tiles[c].Disappear(), _busyDuration);
            _tiles[c] = null;
        }
    }

    private void DropTiles()
    {
        _game.DropTiles();

        for (int i = 0; i < _game.DroppedTiles.Count; i++)
        {
            TileDrop drop = _game.DroppedTiles[i];
            Tile tile;
            if (drop.fromY < _tiles.SizeY)
            {
                tile = _tiles[drop.coordinates.x, drop.fromY];
                //tile.transform.localPosition = new Vector3(
                //    drop.coordinates.x + _tileOffset.x, drop.coordinates.y + _tileOffset.y
                //);
            }
            else
            {
                tile = SpawnTile(
                    _game[drop.coordinates], drop.coordinates.x, drop.fromY + _newDropOffset
                );
            }
            _tiles[drop.coordinates] = tile;
            _busyDuration = Mathf.Max(
                tile.Fall(drop.coordinates.y + _tileOffset.y, _dropSpeed), _busyDuration
            );
        }
    }
}