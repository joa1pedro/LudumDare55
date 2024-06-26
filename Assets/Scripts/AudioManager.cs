using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] List<AudioClip> executeComboAudios = new List<AudioClip>();
    [SerializeField] List<AudioClip> failComboAudios = new List<AudioClip>();
    [SerializeField] List<AudioClip> destroyEnemyAudios = new List<AudioClip>();
    [SerializeField] List<AudioClip> summonAudios = new List<AudioClip>();

    [SerializeField] AudioSource audioSource;


    private System.Random random = new System.Random();

    public void PlaySoundSummon()
    {
        if (summonAudios != null && summonAudios.Count > 0)
        {
            int randomIndex = random.Next(0, summonAudios.Count);
            audioSource.PlayOneShot(summonAudios[randomIndex]);
        }
    }

    public void PlaySoundFailCombo()
    {
        if (failComboAudios != null && failComboAudios.Count > 0)
        {
            int randomIndex = random.Next(0, failComboAudios.Count);
            audioSource.PlayOneShot(failComboAudios[randomIndex]);
        }
    }

    public void PlaySoundEnemyDied()
    {
        if (destroyEnemyAudios != null && destroyEnemyAudios.Count > 0)
        {
            int randomIndex = random.Next(0, destroyEnemyAudios.Count);
            audioSource.PlayOneShot(destroyEnemyAudios[randomIndex]);
        }
    }

    public void PlaySoundExecuteCombo()
    {
        if (executeComboAudios != null && executeComboAudios.Count > 0)
        {
            int randomIndex = random.Next(0, executeComboAudios.Count);
            audioSource.PlayOneShot(executeComboAudios[randomIndex]);
        }

        if (summonAudios != null && summonAudios.Count > 0)
        {
            int randomIndex = random.Next(0, summonAudios.Count);
            audioSource.PlayOneShot(summonAudios[randomIndex]);
        }
    }
}
