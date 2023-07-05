using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SFX Container")]
public class SO_SFX : ScriptableObject
{
    public List<AudioClip> HurtSFXClip = new List<AudioClip>();
    public List<AudioClip> PistolSFXClip = new List<AudioClip>();
    public List<AudioClip> CarbineSFXClip = new List<AudioClip>();
    public List<AudioClip> ObjectiveSFXClip = new List<AudioClip>();
    public List<AudioClip> DeathSFXClip = new List<AudioClip>();
    public List<AudioClip> GunReloadingSFXClip = new List<AudioClip>();
}
