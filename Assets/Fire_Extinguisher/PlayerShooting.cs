using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour {
    public bool playerIsShooting;
    ParticleSystem particles;
    private Dictionary<string, AudioSource> sfxAudio = new Dictionary<string, AudioSource>();

        
    void Awake () {
        particles = GetComponentInChildren<ParticleSystem>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources) {
            sfxAudio.Add(source.clip.name, source);
        }
    }

    public void activatePlayerShooting() {
        if (!sfxAudio["sfx_extinguisher_middle"].isPlaying) {
            sfxAudio["sfx_extinguisher_start"].Play();
            sfxAudio["sfx_extinguisher_middle"].Play();
        }

        playerIsShooting = true;
    }

    public void deactivatePlayerShooting() {
        if (sfxAudio["sfx_extinguisher_middle"].isPlaying) {
            sfxAudio["sfx_extinguisher_start"].Stop();
            sfxAudio["sfx_extinguisher_middle"].Stop();
            sfxAudio["sfx_extinguisher_end"].Play();
        }

        playerIsShooting = false;
    }

    void Update () {
        // If the Fire1 button is being press
	    if(playerIsShooting) {
            Shoot();
        }
    }
        
	public void Shoot() {
        // Stop the particles from playing if they were, then start the particles.
        particles.Stop();
        particles.Play();
    }
}
