using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasketballGameDefinition;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] private AudioClip successfullBallClip;
    [SerializeField] private AudioClip shootBallBallClip;
    [SerializeField] private AudioClip hitBallClip;

    [SerializeField] private AudioClip successLevel;
    [SerializeField] private AudioClip failedLevel;

    //[SerializeField] private AudioClip crowdIdleClip;
    [SerializeField] private AudioClip crowdHappyClip;
    //[SerializeField] private AudioClip crowdUnhappyClip;
        
    private AudioSource _auSource;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        //auSource = this.GetComponent<AudioSource>();
    }

    public void PlayOneShotSoundClip(AudioClip _clip)
    {
        if (_clip == null)
            return;

        if (_auSource == null)
            return;

        _auSource.PlayOneShot(_clip);
    }

    public void PlaySound(SoundEffect _soundEffect, AudioSource _au)
    {
        _auSource = _au;
        switch (_soundEffect)
        {
            case SoundEffect.SuccessfullBall:
                PlayOneShotSoundClip(successfullBallClip);
                break;
            case SoundEffect.ShootBall:
                PlayOneShotSoundClip(shootBallBallClip);
                break;
            case SoundEffect.HitBall:
                PlayOneShotSoundClip(hitBallClip);
                break;
            case SoundEffect.SuccessLevel:
                PlayOneShotSoundClip(successLevel);
                break;
            case SoundEffect.FailedLevel:
                PlayOneShotSoundClip(failedLevel);
                break;
            case SoundEffect.CrowdHappy:
                PlayOneShotSoundClip(crowdHappyClip);
                break;
            default:
                Debug.Log("boş geldi");
                break;
        }

    }

}
