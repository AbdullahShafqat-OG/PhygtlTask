using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 BaseScale
    { get; private set; }
    
    [SerializeField, Range(0f, 1f)]
    private float _disappearDuration = 0.25f;

    private PrefabInstancePool<Tile> _pool;
    private float _disappearProgress;

    private void Awake()
    {
        BaseScale = transform.localScale;
    }

    private void Update()
    {
        if (_disappearProgress >= 0f)
        {
            _disappearProgress += Time.deltaTime;
            if (_disappearProgress >= _disappearDuration)
            {
                Despawn();
                return;
            }
            transform.localScale =
                BaseScale * (1f - _disappearProgress / _disappearDuration);
        }
    }

    public Tile Spawn(Vector3 position)
    {
        Tile instance = _pool.GetInstance(this);
        instance._pool = _pool;
        instance.transform.localPosition = position;
        instance.transform.localScale = instance.BaseScale;
        instance._disappearProgress = -1f;
        instance.enabled = false;
        return instance;
    }

    public void Despawn() => _pool.Recycle(this);

    public float Disappear()
    {
        _disappearProgress = 0f;
        enabled = true;
        return _disappearProgress;
    }
}