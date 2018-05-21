using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    private bool gameStarted = false;
    private Vector3 startPosition;
    private Vector3 startScale;

    void OnTriggerEnter(Collider other) {
        // If either of the controllers touch this gameobject
        if (other.transform.name == "Controller (left)" || other.transform.name == "Controller (right)") {
            // Move this gameobject away from the enivronment
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(0, 0, 0);

            // Start the game
            animations anim = transform.parent.Find("SoldierFree").GetComponent<animations>();
	        anim.startGame(gameStarted); // initiate the game
	        gameStarted = true;

            // Delete this object
            Destroy(gameObject);
        }
    }

    public void resetPosition() {
        transform.localScale = startScale;
        transform.localPosition = startPosition;
    }

    // Use this for initialization
    void Start () {
        Vector3 startScale = transform.localScale;
        Vector3 startPosition = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
