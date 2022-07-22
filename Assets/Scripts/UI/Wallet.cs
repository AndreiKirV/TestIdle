using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour
{
    [SerializeField] private Backpack _backpack;
    [SerializeField] private GameObject _canvas;
    [SerializeField]private GameObject _flyCoin;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _positionsBarn;
    [SerializeField] private float _speedFlying;
    [SerializeField] private Timer _timer;
    [SerializeField] private float _delayFlying;
    [SerializeField] private float _distanceFlyingCoins; 
    private GameObject _coin;
    private TextMeshProUGUI _valueCoins;
    private TextMeshProUGUI _valueStacks;
    private int _startCoins = 0;
    private List<GameObject> _tempCoins = new List<GameObject>();
    private Animator _animator;
    private bool _isHoldCoins = true;
    private float _startFlyingCoins;
    private float _normalizedPixelScreen = 900f;

    private void Start() 
    {  
        InitUI();
        _backpack.ValueHaystacksChanged = SetValueStack;
        _backpack.EnterHaystack += SetCoin;
        _backpack.ValueCoinsChanged += SetValueCoin;
        _animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        SetMoveCoins();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        _animator.ResetTrigger("IsFaced");
        _animator.SetTrigger("IsFaced");

        if (_tempCoins.Contains(other.gameObject))
        {
            _tempCoins.Remove(other.gameObject);
            _backpack.ValueCoinsChanged.Invoke();
            Destroy(other.gameObject);
        }
    }

    private void InitUI () 
    {
        _coin = transform.Find("Coin").gameObject;
        _valueCoins = _coin.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        SetValue(_valueCoins, _startCoins.ToString());

        _valueStacks = transform.Find("Backpack").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        SetValue(_valueStacks, SetTextStack(_backpack.GiveValueStack(), _backpack.GiveMaxValueStack()));
    }

    private void SetValueStack()
    {
        SetValue(_valueStacks, SetTextStack(_backpack.GiveValueStack(), _backpack.GiveMaxValueStack()));
    }

    private void SetValue(TextMeshProUGUI targetValue, string targetText)
    {
        targetValue.text = targetText;
    }

    private string SetTextStack(int currentValue, int maxValue)
    {
        return $"{currentValue.ToString()} \n {maxValue.ToString()}";
    }

    private void CreateCoin()
    {
        GameObject tempCoin = Instantiate(_flyCoin, _canvas.transform);
        _tempCoins.Add(tempCoin);
        _startFlyingCoins = _timer.CurrentTime;
        tempCoin.SetActive(false);
    }

    private void SetCoin(Haystack hayStack)
    {
        hayStack.BarnZoneReached += CreateCoin;
        hayStack.BarnZoneReached += StartMove;
    }

    private void SetMoveCoins()
    {

        if (_isHoldCoins || _startFlyingCoins + _delayFlying >= _timer.CurrentTime)
        {
            SetStartPosition();
        }
        else if(_isHoldCoins == false && _startFlyingCoins + _delayFlying <= _timer.CurrentTime)
        {
            for (int i = 0; i < _tempCoins.Count; i++)
            {
                if (_tempCoins[i].activeSelf == false)
                {
                    _tempCoins[i].SetActive(true);
                }

                if (i == 0)
                {

                    _tempCoins[i].transform.position = Vector3.Lerp(_tempCoins[i].transform.position, _coin.transform.position, Time.deltaTime*_speedFlying);
                }
                else if(Vector3.Distance(_tempCoins[i-1].transform.position, _tempCoins[i].transform.position) > _distanceFlyingCoins)
                {
                    _tempCoins[i].transform.position = Vector3.Lerp(_tempCoins[i].transform.position, _tempCoins[i-1].transform.position, Time.deltaTime*_speedFlying);
                }
            }
        }
    }

    private void SetStartPosition()
    {
        for (int i = 0; i < _tempCoins.Count; i++)
            {
                if (_camera.WorldToScreenPoint(_positionsBarn.position).y < _normalizedPixelScreen)
                    _tempCoins[i].transform.position = _camera.WorldToScreenPoint(_positionsBarn.position);
            } 
    }

    private void StartMove()
    {
        _isHoldCoins = false;
    }

    private void SetValueCoin()
    {
        SetValue(_valueCoins, _backpack.Coins.ToString());
    }
}