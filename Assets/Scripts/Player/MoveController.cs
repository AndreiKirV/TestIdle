using UnityEngine;

public class MoveController : MonoBehaviour
{    
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3Int _minMove;
    [SerializeField] private Vector3Int _maxMove;
    [SerializeField] private Camera _camera;
    private Vector3 _touch;
    private GameObject _scythe;
    private AnimatorController _animatorController;
    private Backpack _backpack;
    private ParticleSystem _smokeEffect;

    private delegate void State(); 
    private State _runningReached;
    private State _runningStopped;
    private void Awake()
    {
        transform.position = _startPosition;
        _scythe = transform.Find("Scythe").gameObject;
        _scythe.SetActive(false);
        _animatorController = GetComponent<AnimatorController>();
        _backpack = GetComponent<Backpack>();
        _animatorController.PunchReached += _animatorController.StopRunning;
        _animatorController.PunchReached += ActivateScythe;
        _animatorController.IdleReached += DeactivateScythe;
        _runningReached += _animatorController.PlayRunning;
        _runningStopped += _animatorController.StopRunning;
        _smokeEffect = transform.Find("Smoke").gameObject.GetComponent<ParticleSystem>();
    }
    
    private void Update()
    {
        TryTouchMove();
    }

    private void OnTriggerStay(Collider other) 
    {
        GardenBed tempObject = other.GetComponent<GardenBed>();

        if (tempObject!= null)
        {
           _animatorController.PlayAttackAnimation();
        }
    }

    private void TryTouchMove()
    {
        Vector3 targetPosition;
        Vector3 tempMousePosition;
        Vector3 direction;

        if (Input.GetMouseButtonDown(0))
        {
            _runningReached();
            _touch = _camera.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            tempMousePosition = _camera.ScreenToViewportPoint(Input.mousePosition);
            direction = new Vector3(Mathf.Clamp((tempMousePosition.x - _touch.x), -0.2f, 0.2f), 0, Mathf.Clamp((tempMousePosition.y - _touch.y), -0.2f, 0.2f));

            targetPosition = Vector3.Lerp(transform.position, transform.position + direction, _speed * Time.deltaTime);
                
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;

            if (CheckBoundaries(targetPosition) == true)
            {
                transform.position = targetPosition;
            }

            if (tempMousePosition != _touch && _animatorController.IsIdle)
            {
                _runningReached();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _runningStopped();
        }

        if (transform.position.y != _minMove.y)
        {
            transform.position = new Vector3(transform.position.x, _minMove.y, transform.position.z);
        }
    }

    private void ActivateScythe()
    {
        _scythe.SetActive(true);
    }

    private void DeactivateScythe()
    {
        _scythe.SetActive(false);
    }
    
    private bool CheckBoundaries(Vector3 target)
    {
        bool isLimitNotReached = true;

        if (target.x < _minMove.x || target.x > _maxMove.x || target.z < _minMove.z || target.z > _maxMove.z)
        {
            isLimitNotReached = false;
        }

        return isLimitNotReached;
    }

    public void SmokeStart()
    {
        _smokeEffect.Play();
    }

    public void SmokeStop()
    {
        _smokeEffect.Stop();
    }
}