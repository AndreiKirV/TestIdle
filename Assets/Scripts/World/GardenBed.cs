using UnityEngine;

public class GardenBed : MonoBehaviour
{
    [SerializeField] private float _positionOfUnripe;
    [SerializeField] private int _maturationTime;
    [SerializeField] private GameObject _resource;
    [SerializeField] private Vector3 _offsetResource;
    
    private bool _isGrassHasGrown;
    private GameObject _grass;
    private int _startMaturationTime;
    private BoxCollider _collider;
    private Timer _timer;
    private ParticleSystem _destructionEffect;
    private ParticleSystem _createEffect;

    public int StartMaturationTime => _startMaturationTime;
    public int MaturationTime => _maturationTime;
    public bool IsGrassHasGrown => _isGrassHasGrown;

    private void Awake()
    {
        _grass = transform.Find("Grass").gameObject;
        _collider = GetComponent<BoxCollider>();
        _destructionEffect = transform.Find("DestroyGrass").gameObject.GetComponent<ParticleSystem>();
        _createEffect = transform.Find("CreateGrass").gameObject.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<GardeningTool>())
        {
            SetStatusGrassHasGrown(false);
            SetStartMaturationTime(_timer.CurrentTime);
            Instantiate(_resource, transform.position + _offsetResource, Quaternion.identity);
            _destructionEffect.Play();
            _createEffect.startLifetime = _maturationTime;
            _createEffect.Play();
        }
    }

    private Vector3 SetPositionOfUnripe(Vector3 currentPosition)
    {
        Vector3 targetPosition = new Vector3(currentPosition.x, 0 + _positionOfUnripe, currentPosition.z);
        return targetPosition;
    }

    private Vector3 SetPositionOfRipe(Vector3 currentPosition)
    {
        Vector3 targetPosition = new Vector3(currentPosition.x, 0, currentPosition.z);
        return targetPosition;
    }

    public void SetPosition()
    {
        if (_isGrassHasGrown)
            {
            _grass.transform.position = SetPositionOfRipe(_grass.transform.position);
            _collider.enabled = true;
            }
        else
            {
            _grass.transform.position = SetPositionOfUnripe(_grass.transform.position);
            _collider.enabled = false;
            }
    }

    public void SetStatusGrassHasGrown(bool status)
    {
        _isGrassHasGrown = status;
        SetPosition();
    }

    public void SetStartMaturationTime(float time)
    {
        _startMaturationTime = (int)time;
    }


    public void SetTimer(Timer timer)
    {
        _timer = timer;
    }
}