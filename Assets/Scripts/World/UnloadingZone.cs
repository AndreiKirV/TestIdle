using UnityEngine;

public class UnloadingZone : MonoBehaviour
{
    private GameObject _barn;

    private void Awake() 
    {
        _barn = transform.parent.gameObject;
        _barn.AddComponent<BoxCollider>().isTrigger = true;
    }

    public GameObject GiveBarn()
    {
        return _barn;
    }
}