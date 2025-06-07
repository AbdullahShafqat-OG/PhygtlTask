using UnityEngine;

public class Tile : MonoBehaviour
{
    private PrefabInstancePool<Tile> _pool;

    public Tile Spawn(Vector3 position)
    {
        Tile instance = _pool.GetInstance(this);
        instance._pool = _pool;
        instance.transform.localPosition = position;
        return instance;
    }

    public void Despawn() => _pool.Recycle(this);
}