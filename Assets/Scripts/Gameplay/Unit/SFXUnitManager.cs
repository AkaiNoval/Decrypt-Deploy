using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXUnitManager : MonoBehaviour
{
    [SerializeField] AudioSource _AudioSource;
    [SerializeField] SO_SFX soundFX;
    [Range(0,1f)]
    [SerializeField] float hurtChance;


    public void PlayHurtSFX()
    {
        if (Random.value <= hurtChance && soundFX.HurtSFXClip.Count > 0)
        {
            int randomIndex = Random.Range(0, soundFX.HurtSFXClip.Count);
            AudioClip clip = soundFX.HurtSFXClip[randomIndex];
            _AudioSource.PlayOneShot(clip);
        }
    }
    public void PlaySidearmSFX()
    {
        if (soundFX.PistolSFXClip.Count > 0)
        {
            int randomIndex = Random.Range(0, soundFX.PistolSFXClip.Count);
            AudioClip clip = soundFX.PistolSFXClip[randomIndex];
            _AudioSource.PlayOneShot(clip);
        }
    }
    public void PlayCarbineSFX()
    {
        if (soundFX.CarbineSFXClip.Count > 0)
        {
            int randomIndex = Random.Range(0, soundFX.CarbineSFXClip.Count);
            AudioClip clip = soundFX.CarbineSFXClip[randomIndex];
            _AudioSource.PlayOneShot(clip);
        }
    }
    public void PlayReloadingSFX()
    {
        if (soundFX.GunReloadingSFXClip.Count > 0)
        {
            int randomIndex = Random.Range(0, soundFX.GunReloadingSFXClip.Count);
            AudioClip clip = soundFX.GunReloadingSFXClip[randomIndex];
            _AudioSource.PlayOneShot(clip);
        }
    }
}
