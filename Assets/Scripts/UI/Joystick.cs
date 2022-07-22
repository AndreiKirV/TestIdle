using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField] private float _maxOffset;
    private GameObject _outLineCircle;
    private GameObject _filledCircle;
    private Vector3 _touch;

    private void Awake()
    {
        _outLineCircle = transform.Find("OutLineCircle").gameObject;
        _filledCircle = transform.Find("FilledCircle").gameObject;
        NotShow(_outLineCircle);
        NotShow(_filledCircle);
    }

    private void Update()
    {
        ChangePosition();
    }

    private void NotShow(GameObject targetObject)
    {
        targetObject.SetActive(false);
    }

    private void Show(GameObject targetObject)
    {
        targetObject.SetActive(true);
    }

    private void ChangePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touch = Input.mousePosition;
            transform.position = _touch;
            Show(_outLineCircle);
            Show(_filledCircle);
        }

        if (Input.GetMouseButton(0))
        {
            _filledCircle.transform.position = Input.mousePosition;

            if(_filledCircle.transform.localPosition.magnitude > _maxOffset) 
                _filledCircle.transform.localPosition = _filledCircle.transform.localPosition.normalized * _maxOffset;

            
        }

        if (Input.GetMouseButtonUp(0))
        {
            NotShow(_outLineCircle);
            NotShow(_filledCircle);
        }
    }
}