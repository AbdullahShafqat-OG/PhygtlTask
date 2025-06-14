using System.Drawing;
using Unity.Mathematics;

[System.Serializable]
public struct Grid2D<T>
{
    private T[] _cells;
    public bool IsUndefined => _cells == null || _cells.Length == 0;

    private int2 _size;
    public int2 Size => _size;
    public int SizeX => _size.x;
    public int SizeY => _size.y;


    public Grid2D(int2 size)
    {
        this._size = size;
        _cells = new T[size.x * size.y];
    }

    public T this[int x, int y]
    {
        get => _cells[y * _size.x + x];
        set => _cells[y * _size.x + x] = value;
    }

    public T this[int2 c]
    {
        get => _cells[c.y * _size.x + c.x];
        set => _cells[c.y * _size.x + c.x] = value;
    }

    public bool AreValidCoordinates(int2 c) =>
        0 <= c.x && c.x < _size.x && 0 <= c.y && c.y < _size.y;

    public void Swap(int2 a, int2 b) => (this[a], this[b]) = (this[b], this[a]);
}