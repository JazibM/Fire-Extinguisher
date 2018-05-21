using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour {

    void OnParticleTrigger() {
        Debug.Log("particle triggered");
    }

    void OnParticleCollision(GameObject other) {
        Debug.Log("anything?");
        if (other.name == "water stream") {
            // reduce this particle system's size
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
