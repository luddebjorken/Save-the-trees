using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundHandler : MonoBehaviour
{
    public AudioMixer Mixer;
    public static SoundHandler singleton;
    public AudioClip Music;
    public AudioClip[] FireClips;
    public AudioSource FireAudioSource;
    [Header("Fire settings")]
    public byte Tier1;
    public byte Tier2;
    public byte Tier3;
    [Header("Players")]
    public float FireSoundChance;
    public AudioSource CardSource;
    public float VolumeLerpSpeed;
    public AudioSource ChaosSource;
    public AudioSource CalmSource;
    private float CalmVolume, ChaosVolume;
    void Awake()
    {
        singleton = this;
    }
    void Start()
    {
        transform.position = Camera.main.transform.position;
    }
    void Update()
    {
        CalmVolume = Mathf.Lerp(CalmVolume, Mathf.Log10(Mathf.Clamp(1-World.singleton.FireCount*0.01f, 0.0001f, (float)World.singleton.TreesAmount/World.singleton.TreesToPlace)) * 20, VolumeLerpSpeed);
        ChaosVolume = Mathf.Lerp(ChaosVolume, Mathf.Log10(Mathf.Clamp(World.singleton.FireCount*0.001f, 1.0001f - (float)World.singleton.TreesAmount/World.singleton.TreesToPlace, 1)) * 20, VolumeLerpSpeed);
        ChaosSource.pitch = 1.5f-((float)World.singleton.TreesAmount/World.singleton.TreesToPlace)/2;
        Mixer.SetFloat("CalmVolume", CalmVolume);
        Mixer.SetFloat("ChaosVolume", ChaosVolume);
    }

    public static void SpawnFire(FireCounter source, int count)
    {
        if(Random.Range(0.0f,1.0f) < 10/World.singleton.FireCount)
        {
            singleton.SpawnFireInternal(source, count);
        }
    }
    
    private void SpawnFireInternal(FireCounter source, int count)
    {
        Instantiate(FireAudioSource.gameObject, source.transform.position, Quaternion.identity, transform).GetComponent<AudioSource>().PlayOneShot(FireClips[count < Tier1 ? 0: count < Tier2 ? 1 : count < Tier3 ? 2 : 3]);
    }
}
