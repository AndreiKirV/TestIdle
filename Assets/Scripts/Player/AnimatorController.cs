using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator _animator;
    private bool _isIdle = true;
    public delegate void State();
    public State IdleReached;
    public State PunchReached;

    public bool IsIdle => _isIdle;

    private void Awake() 
    {
        _animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        TrySetIdle();
    }

    public void PlayAttackAnimation()
    {
        _isIdle = false;
        PunchReached();

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            _animator.ResetTrigger("GardenBed");
            _animator.SetTrigger("GardenBed");
        }
    }

    private void TrySetIdle()
    {
        if(_isIdle == false && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            IdleReached();
            _isIdle = true;
        }
    }

    public void PlayRunning()
    {
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            _animator.SetBool("Running", true);
        }
    }

    public void StopRunning()
    {
        _animator.SetBool("Running", false);
    }
}