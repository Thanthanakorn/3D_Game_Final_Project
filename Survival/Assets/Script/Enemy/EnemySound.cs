using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Settings")]
    public bool soundsEnabled = true;

    [Header("Sounds")]
    public AudioClip hurtSound;
    public AudioClip deadSound;
    public AudioClip parriedSound;
    public AudioClip attackSound;

    [Header("Delay")]
    public float delayBetweenSounds = 0.2f;
    private bool canPlaySound = true;
    
    
    
    
    public void EnemyDeadSound()
    {
        if (soundsEnabled && deadSound!= null && canPlaySound)
        {
            audioSource.clip = deadSound;
            audioSource.Play();
        }
    }
    
    public void EnemyHurtSound()
    {
        if (soundsEnabled && hurtSound!= null && canPlaySound)
        {
            audioSource.clip = hurtSound;
            audioSource.Play();
        }
    }

    public void EnemyParriedSound()
    {
        if (soundsEnabled && parriedSound!= null && canPlaySound)
        {
            audioSource.clip = parriedSound;
            audioSource.Play();
        }
    }

    public void EnemyAttackSound()
    {
        if (soundsEnabled && attackSound!= null && canPlaySound)
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
