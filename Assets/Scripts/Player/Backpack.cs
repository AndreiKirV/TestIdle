using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] private int _limit;
    [SerializeField] private float _offsetHaystack;
    private GameObject _backpack;
    private Stack<Haystack> _haystacks = new Stack<Haystack>();
    private GameObject [] _points;
    private Haystack _currentStack = null;
    private int _coins;
    private int _haystackCost = 0;
    public delegate void HaystackEnter(Haystack hayStack);
    public HaystackEnter EnterHaystack;
    public delegate void ChangeReached();
    public ChangeReached ValueHaystacksChanged; 
    public ChangeReached ValueCoinsChanged;

    public int Coins => _coins;

    private void Awake() 
    {
        _backpack = transform.Find("Backpack").gameObject;
        _points = new GameObject [_limit];
        SetHaystackObjectPosition();
        EnterHaystack = SetHaystackPosition;
        ValueCoinsChanged += IncreaseCoins;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Haystack>() && _haystacks.Count < _limit)
        {   
            Haystack tempObject = other.GetComponent<Haystack>();

            if (CheckFor(tempObject) == false && tempObject.GiveInfoToBarn() == false)
            {
                EnterHaystack.Invoke(tempObject);

                if (_haystackCost < tempObject.Cost)
                _haystackCost = tempObject.Cost;
            }
        }

        if (other.GetComponent<UnloadingZone>())
        {
            UnloadingZone tempZone = other.GetComponent<UnloadingZone>();
            TryAttemptingToSendToBarn(tempZone.GiveBarn());
        }
    }

    private void TryAttemptingToSendToBarn(GameObject barn)
    {
        if (_haystacks.Count > 0 && _currentStack == null)
        {
            _currentStack = _haystacks.Pop();
            _currentStack.WarningLengthReached += DeleteCurrentStack;
            _currentStack.transform.parent = null;

            if (_currentStack.GiveInfoToBarn() == false)
            {
                _currentStack.SetBarn(barn);
            }
        }
    }

    private void DeleteCurrentStack()
    {
        _currentStack = null;
        ValueHaystacksChanged.Invoke();
    }

    private void SetHaystackPosition(Haystack hayStack)
    {
        hayStack.transform.SetParent(_points[_haystacks.Count].transform);
        hayStack.GetComponent<Rigidbody>().isKinematic = true;
        hayStack.gameObject.transform.position = new Vector3(_points[_haystacks.Count].transform.position.x, _points[_haystacks.Count].transform.position.y, _points[_haystacks.Count].transform.position.z);
        hayStack.gameObject.transform.rotation = _points[_haystacks.Count].transform.rotation;
        _haystacks.Push(hayStack);
        ValueHaystacksChanged.Invoke();
    }

    private void SetHaystackObjectPosition()
    {
        for (int i = 0; i < _limit; i++)
        {
            GameObject _tempObject = new GameObject("point");
            _tempObject.transform.position = new Vector3(_backpack.transform.position.x, _backpack.transform.position.y + i * _offsetHaystack, _backpack.transform.position.z);
            _tempObject.transform.SetParent(_backpack.transform);
            _points[i] = _tempObject;
        }
    }

    private bool CheckFor(Haystack haystack)
    {
        foreach (var item in _haystacks)
        {
            if (item == haystack)
            {
                return true;
            }
        }

        return false;
    }

    private void IncreaseCoins()
    {
        _coins += _haystackCost;
    }

    public int GiveMaxValueStack()
    {
        return _points.Length;
    }

    public int GiveValueStack()
    {
        return _haystacks.Count;
    }
}