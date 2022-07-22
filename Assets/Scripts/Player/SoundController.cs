using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _scythe;
    [SerializeField] private AudioClip _steps;
    public void PlaySteps()
    {
        _audioSource.clip = _steps;
        _audioSource.volume = 0.05f;
        _audioSource.Play();
    }

    public void PlayScythe()
    {
        _audioSource.volume = 0.4f;
        _audioSource.clip = _scythe;
        _audioSource.Play();
    }

    public void StopSounds()
    {
        _audioSource.Stop();
    }
}
