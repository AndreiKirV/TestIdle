using UnityEngine;

public class BevelZoneController : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2 _position;
    [SerializeField] private GameObject _gardenBed;
    [SerializeField] private bool _startStatusGrass;

    private GardenBed [,] _gardenBeds;
    private Timer _timer;

    private void Awake() 
    {
        _gardenBeds = new GardenBed[_size.x,_size.y];
        _timer = transform.parent.GetComponentInChildren<Timer>();
    }

    private void Start()
    {
        CreateGardenBeds();
    }

    private void Update()
    {
        CheckMaturityOfWheat();
    }

    private void CreateGardenBeds()
    {
        for (int x = 0; x < _gardenBeds.GetLength(0); x++)
        {
            for (int z = 0; z < _gardenBeds.GetLength(1); z++)
            {
                _gardenBeds[x,z] = CreateGardenBed(CorrectionPpositionToStarting(x,z));
                _gardenBeds[x,z].SetStatusGrassHasGrown(_startStatusGrass);
                _gardenBeds[x,z].SetStartMaturationTime(_timer.CurrentTime);
                _gardenBeds[x,z].SetTimer(_timer);
            }
        }
    }

    private GardenBed CreateGardenBed(Vector3 position)
    {
        GameObject tempObject = Instantiate(_gardenBed, position, Quaternion.identity);
        return tempObject.GetComponent<GardenBed>();
    }

    private Vector3 CorrectionPpositionToStarting(int x, int z)
    {
        Vector3 targetPosition = new Vector3(x + _position.x, 0, z + _position.y);
        return targetPosition;
    }

    private void CheckMaturityOfWheat()
    {
        for (int x = 0; x < _gardenBeds.GetLength(0); x++)
        {
            for (int z = 0; z < _gardenBeds.GetLength(1); z++)
            {
                if ((int)_timer.CurrentTime !=  _gardenBeds[x,z].StartMaturationTime && ((int)_timer.CurrentTime - _gardenBeds[x,z].StartMaturationTime) % _gardenBeds[x,z].MaturationTime == 0)
                {
                    _gardenBeds[x,z].SetStatusGrassHasGrown(true);
                }   
            }
        }
    }

    public void GiveTime(GardenBed bed)
    {
        for (int x = 0; x < _gardenBeds.GetLength(0); x++)
        {
            for (int z = 0; z < _gardenBeds.GetLength(1); z++)
            {
                if (_gardenBeds[x,z] == bed)
                {
                    _gardenBeds[x,z].SetStartMaturationTime(_timer.CurrentTime);
                }
            }
        }
    }
}