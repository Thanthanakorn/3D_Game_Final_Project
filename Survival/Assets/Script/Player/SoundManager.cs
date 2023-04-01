using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Settings")]
    public bool soundsEnabled = true;

    [Header("Sounds")]
    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip runSound;
    public AudioClip rollSound;
    public AudioClip jumpSound;
    public AudioClip hurtSound;
    public AudioClip deadSound;
    public AudioClip shieldSound;
    
    [Header("Delay")]
    public float delayBetweenSounds = 0.2f;
    private bool canPlaySound = true;

    private PlayerManager _playerManager;

    private void Awake()
    {
        _playerManager = GetComponentInParent<PlayerManager>();
    }

    public void PlayShieldSound()
    {
        if (soundsEnabled && shieldSound!= null && canPlaySound)
        {
            audioSource.clip = shieldSound;
            audioSource.Play();
        }
    }
    
    public void PlayDeadSound()
    {
        if (soundsEnabled && deadSound!= null && canPlaySound)
        {
            audioSource.clip = deadSound;
            audioSource.Play();
        }
    }
    
    public void PlayHurtSound()
    {
        if (soundsEnabled && hurtSound!= null && canPlaySound)
        {
            audioSource.clip = hurtSound;
            audioSource.Play();
        }
    }
    
    public void PlayJumpSound()
    {
        if (soundsEnabled && jumpSound!= null && canPlaySound)
        {
            audioSource.clip = jumpSound;
            audioSource.Play();
        }
    }
    
    public void PlayRollSound()
    {
        if (soundsEnabled && rollSound != null && canPlaySound)
        {
            audioSource.clip = rollSound;
            audioSource.Play();
        }
    }
    
    public void PlayRunSound()
    {
        if(_playerManager.isAttacking){return;}
        if (soundsEnabled && runSound != null && canPlaySound)
        {
            audioSource.clip = runSound;
            audioSource.Play();
        }
    }

    public void PlayWalkSound()
    {
        if(_playerManager.isAttacking){return;}
        if (soundsEnabled && walkSound != null && canPlaySound)
        {
            audioSource.clip = walkSound;
            audioSource.Play();
        }
    }

    public void PlayAttackSound()
    {
        if (soundsEnabled && attackSound != null && canPlaySound)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
        }
    }
    private IEnumerator DelayNextSound()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(delayBetweenSounds);
        canPlaySound = true;
    }

    public void ToggleSound()
    {
        soundsEnabled = !soundsEnabled;
    }
}