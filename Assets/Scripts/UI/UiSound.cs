using UnityEngine;

public class UiSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public void PlaySoundCoins()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
