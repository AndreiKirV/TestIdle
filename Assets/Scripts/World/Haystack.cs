using UnityEngine;

public class Haystack : MonoBehaviour
{
    [SerializeField] private float _throwingForce;
    [SerializeField] private float _speedFly;
    [SerializeField] private float _warningLength;
    [SerializeField] private int _cost;
    private Rigidbody _rigidBody;
    private GameObject _barnZone = null;
    private BoxCollider _collider; 
    private bool _isBarnReached = false;
    private Vector3 _startPosition = new Vector3();
    public delegate void StateReached();
    public StateReached BarnZoneReached;
    public StateReached WarningLengthReached;

    public bool IsBarnReached => _isBarnReached;
    public int Cost => _cost;

    private void Awake() 
    {
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        BarnZoneReached += SetIsBarnReached;
    }

    private void Start() 
    {
        _rigidBody.AddForce(new Vector3(0, _throwingForce, 0), ForceMode.Impulse);
    }

    private void Update() 
    {
        TryFlyToIntoBarn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _barnZone)
        {
            BarnZoneReached.Invoke();
            BarnZoneReached = null;
            Destroy(gameObject);
        }
    }

    private void TryFlyToIntoBarn()
    {
        if (_barnZone != null)
        {
            transform.position = Vector3.Lerp(transform.position, _barnZone.transform.position, Time.deltaTime * _speedFly);

            if (WarningLengthReached != null && Vector3.Distance(transform.position, _startPosition) > _warningLength)
            {
                WarningLengthReached.Invoke();
                WarningLengthReached = null;
            }
        }

    }

    private void SetStartPosition()
    {
        _startPosition = transform.position;
    }

    private void SetIsBarnReached()
    {
        _isBarnReached = true;
    }

    public void SetBarn(GameObject barnZone)
    {
        _barnZone = barnZone;
        SetStartPosition();
    }

    public bool GiveInfoToBarn()
    {
        if (_barnZone == null)
            return false;
        else 
            return true;
    }
}