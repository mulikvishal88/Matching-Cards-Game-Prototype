using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    [SerializeField]
    private AudioClip _matchAudio, _mismatchAudio;
    private AudioSource _audioSource;
    private SoundManager() { }
    void Awake()
    {
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMatchAudio()
    {
        _audioSource.PlayOneShot(_matchAudio);
    }
    public void PlayMissmatchAudio()
    {
        _audioSource.PlayOneShot(_mismatchAudio);
    }
}
