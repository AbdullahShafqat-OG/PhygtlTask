using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 BaseScale
    { get; private set; }
    
    [SerializeField, Range(0f, 1f)]
    private float _disappearDuration = 0.25f;

    private PrefabInstancePool<Tile> _pool;
    private float _disappearProgress;

    [System.Serializable]
    private struct FallingState
    {
        public float fromY, toY, duration, progress;
    }
    private FallingState _falling;

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

        if (_falling.progress >= 0f)
        {
            Vector3 position = transform.localPosition;
            _falling.progress += Time.deltaTime;
            if (_falling.progress >= _falling.duration)
            {
                _falling.progress = -1f;
                position.y = _falling.toY;
                this.enabled = _disappearProgress >= 0f;
            }
            else
            {
                position.y = Mathf.Lerp(
                    _falling.fromY, _falling.toY, _falling.progress / _falling.duration
                );
            }
            transform.localPosition = position;
        }
    }

    public Tile Spawn(Vector3 position)
    {
        Tile instance = _pool.GetInstance(this);
        instance._pool = _pool;
        instance.transform.localPosition = position;
        instance.transform.localScale = instance.BaseScale;
        instance._disappearProgress = -1f;
        instance._falling.progress = -1f;
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

    public float Fall(float toY, float speed)
    {
        _falling.fromY = transform.localPosition.y;
        _falling.toY = toY;
        _falling.duration = (_falling.fromY - toY) / speed;
        _falling.progress = 0f;
        enabled = true;
        return _falling.duration;
    }
}