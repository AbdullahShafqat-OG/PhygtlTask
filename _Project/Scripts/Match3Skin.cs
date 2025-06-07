using Unity.Mathematics;
using UnityEngine;

public class Match3Skin : MonoBehaviour
{
    public bool IsPlaying => true;

    public bool IsBusy => false;

    [SerializeField]
    private Tile[] _tilePrefabs;
    [SerializeField]
    private Match3Game _game;

    private Grid2D<Tile> _tiles;
    private float2 _tileOffset;

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

    Tile SpawnTile(TileState t, float x, float y) =>
        _tilePrefabs[(int)t - 1].Spawn(new Vector3(x + _tileOffset.x, y + _tileOffset.y));

    public void DoWork() { }

    public bool EvaluateDrag(Vector3 start, Vector3 end)
    {
        return false;
    }
}