using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _currentTime = 0;

    public float CurrentTime =>  _currentTime;

    void Update()
    {
        _currentTime += Time.deltaTime;
    }
}
