using UnityEngine;
using System.Collections;

public class objectCollision : MonoBehaviour {

    public GameObject extinguisher;
    public GameObject pin;
    public GameObject hose;

    void OnTriggerEnter(Collider other) {
        if (other.name == "Extinguisher") {
            extinguisher = other.gameObject;
        }
        else if (other.name == "Pin") {
            pin = other.gameObject;
        }
        else if (other.name == "hose_end") {
            hose = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject == extinguisher) {
            extinguisher = null;
        }
        else if (other.gameObject == pin) {
            pin = null;
        }
        else if (other.gameObject == hose) {
            hose = null;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
