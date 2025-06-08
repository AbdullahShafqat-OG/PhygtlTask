using TMPro;
using UnityEngine;

public class FloatingScore : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _displayTxt;
    [SerializeField, Range(0.1f, 1f)]
    private float _displayDuration = 0.5f;
    [SerializeField, Range(0f, 4f)]
    private float _riseSpeed = 2f;

    private float _age;

    PrefabInstancePool<FloatingScore> pool;

    private void Awake()
    {
        if (_displayTxt == null)
            _displayTxt = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        _age += Time.deltaTime;
        if (_age >= _displayDuration)
        {
            pool.Recycle(this);
        }
        else
        {
            Vector3 p = transform.localPosition;
            p.y += _riseSpeed * Time.deltaTime;
            transform.localPosition = p;
        }
    }

    public void Show(Vector3 position, int value)
    {
        FloatingScore instance = pool.GetInstance(this);
        instance.pool = pool;
        instance._displayTxt.SetText("{0}", value);
        instance.transform.localPosition = position;
        instance._age = 0f;
    }
}