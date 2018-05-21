using UnityEngine;
using System.Collections;
using System;

public class ButtonController : MonoBehaviour {

    private SteamVR_Controller.Device controller;
    private SteamVR_TrackedObject trackedObj;
    private ulong gripButton = SteamVR_Controller.ButtonMask.Grip;
    private ulong trigButton = SteamVR_Controller.ButtonMask.Trigger;

    private bool debug = false;
    
    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    
    // Update is called once per frame
    void Update () {
        if (trackedObj != null) {
            controller = SteamVR_Controller.Input((int)trackedObj.index);
        }/*
        if (controller == null) {
            Debug.Log("controller not initialised.");
            return;
        }*/
        
        if (Input.GetKeyDown(KeyCode.F)) {        
        //if (controller.GetPressDown(gripButton)) {
            if (debug){
                Debug.Log("gripped pressed");
            }

            GameObject extinguisher = GetComponent<objectCollision>().extinguisher;
            GameObject hose = GetComponent<objectCollision>().hose;
            // Only pick up the extinguisher if it is nearby and not in the other hand
            if (extinguisher != null && extinguisher.transform.parent == null) {
                // Set the object's mode relative to the player's hand
                extinguisher.transform.SetParent(transform);

                // Move the extinguisher to the right hand
                //extinguisher.transform.localPosition = new Vector3(2.2f, -4f, 3.8f);
                extinguisher.transform.localPosition = new Vector3(0.1f, -1.4f, 0f);
                extinguisher.transform.localEulerAngles = new Vector3(0, 80, 0);

                // Change the body type to not follow gravity anymore
                Rigidbody extinguisherBody = extinguisher.GetComponent<Rigidbody>();
                extinguisherBody.useGravity = false;
                extinguisherBody.isKinematic = true;
            }
            // Only pick up the hose if it is nearby and the extinguisher is not in this hand
            else if (hose != null && extinguisher.transform.parent != transform) {
                // Move the hose end part of hose position

                Transform hoseEnd = hose.transform.parent; // grab b11
                hoseEnd.localPosition = new Vector3(-1.5f, 0f, 1.0f);
                hoseEnd.localEulerAngles = new Vector3(0, 65, 0);

                hoseEnd = hoseEnd.parent; // grab b10
                hoseEnd.localPosition = new Vector3(-1.3f, 0f, -0.3f);
                hoseEnd.localEulerAngles = new Vector3(0, 20, 0);

                hoseEnd = hoseEnd.parent; // grab b9
                hoseEnd.localPosition = new Vector3(-0.4f, 1.2f, -5.0f);
                hoseEnd.localEulerAngles = new Vector3(-80, 0, -90);
                
                // Grab the hose and change it's mode relative to the player's hand
                // Move the extinguisher part of the hose position
                Transform hoseMiddle = hoseEnd.parent.parent; // grab b7
                hoseMiddle.localPosition = new Vector3(-0.5f, 0f, -0.2f);
                hoseMiddle.localEulerAngles = new Vector3(0, 0, 20);

                // Make the mode type set relative to the player's hand
                hoseMiddle = hoseEnd.parent; // grab b8
                hoseMiddle.SetParent(transform);
                hoseMiddle.localPosition = new Vector3(0.1f, -0.1f, -0.12f);
                hoseMiddle.localEulerAngles = new Vector3(0, 90, 90);
            }
            // add extinguisher to pick up or 
            // hold the pipe
        }
        else if (Input.GetKeyUp(KeyCode.F)) {
        //else if (controller.GetPressUp(gripButton)) {
            if (debug){
                Debug.Log("gripped released");
            }
            
            // Release the extinguisher and pipe from this controller
            Transform extinguisher = transform.Find("Extinguisher");
            Transform hose = transform.Find("b8");
            if (extinguisher != null) {
                // Grab the hose in the other hand/controller
                int siblingIndex = (transform.GetSiblingIndex() == 1) ? 0 : 1;
                Transform otherController = transform.parent.GetChild(siblingIndex);
                hose = otherController.Find("b8");

                if (hose != null) { // Remove the hose if it is in the other hand
                    deactivateHose(extinguisher, hose);
                }
                
                // Remove the extinguisher from this hand
                extinguisher.SetParent(null);
                Rigidbody extinguisherBody = extinguisher.GetComponent<Rigidbody>();
                extinguisherBody.useGravity = true;
                extinguisherBody.isKinematic = false;
            }
            else if (hose != null) {
                deactivateHose(extinguisher, hose);
            }
            // release the extinguisher or 
            // pipe from the hand
        }
        
        if (Input.GetKey(KeyCode.G)) {
        //if (controller.GetPressDown(trigButton)) {
            // Start shooting the extinguisher
            if (debug){
                Debug.Log("trigger pressed");
            }

            GameObject pin = gameObject.GetComponent<objectCollision>().pin;
            int siblingIndex = (transform.GetSiblingIndex() == 1) ? 0 : 1;
            Transform otherController = transform.parent.GetChild(siblingIndex);
            Transform waterStream = otherController.transform.Find("b8/b9/b10/b11/hose_end/water stream");
            if (pin != null && pin.transform.parent != null) {
                // Move the x position of the pin to following the hand
                Vector3 newPinPosition = pin.transform.localPosition;
                newPinPosition.x = pin.transform.parent.transform.InverseTransformPoint(transform.position).x;
                if (newPinPosition.x > 0) { // only move in the positive direction
                    pin.transform.localPosition = newPinPosition;
                }
            }
            else if (waterStream != null) {
                Transform pinCheck = transform.Find("Extinguisher/fire extinguisher/Pin-parent/Pin");
                if (pinCheck == null) { // If the extinguisher in the other hand does not have a pin
                    // Start the extinguisher to be shooting
                    waterStream.GetComponent<PlayerShooting>().activatePlayerShooting();
                }
            }
            // add particle system to be active on extinguisher or 
            // pull pin out of the extinguisher
        }
        else if (Input.GetKeyUp(KeyCode.G)) {
        //else if (controller.GetPressUp(trigButton)) {
            // Stop the shooting from the extinguisher
            if (debug){
                Debug.Log("trigger released");
            }
            
            int siblingIndex = (transform.GetSiblingIndex() == 1) ? 0 : 1;
            Transform otherController = transform.parent.GetChild(siblingIndex);
            Transform waterStream = otherController.transform.Find("b8/b9/b10/b11/hose_end/water stream");
            GameObject pin = gameObject.GetComponent<objectCollision>().pin;
            if (pin != null && pin.transform.parent != null) {
                // If the pin has been released by a distance of 1.5 then remove the pin
                //if (pin.transform.localPosition.x > 10) {
                // Set gravity to the pin
                    Rigidbody pinBody = pin.GetComponent<Rigidbody>();
                    pinBody.useGravity = true;
                    pinBody.isKinematic = false;

                    // Remove it as an object of the extinguisher
                    pin.transform.SetParent(null);
                //}
            }
            else if (waterStream != null) {
                // Stop the extinguisher from shooting
                waterStream.GetComponent<PlayerShooting>().deactivatePlayerShooting();
            }
            // stop the particle system from be used or 
            // stop moving the pin and check if it is fully out
        }
    }

    private void deactivateHose(Transform extinguisher, Transform hose) {
        Transform waterStream = hose.transform.Find("b9/b10/b11/hose_end/water stream");
        waterStream.GetComponent<PlayerShooting>().deactivatePlayerShooting();

        // Remove the hose from the other hand and reassign back to the extinguisher
        if (extinguisher == null) {
            int siblingIndex = (transform.GetSiblingIndex() == 1) ? 0 : 1;
            Transform otherController = transform.parent.GetChild(siblingIndex);
            extinguisher = otherController.Find("Extinguisher");
        }
        hose.SetParent(extinguisher.Find("fire extinguisher/Hose/b1/b2/b3/b4/b5/b6/b7"));

        // Reassign the position of the hose back to the extinguisher
        // Move the extinguisher part of the hose position
        hose = waterStream.parent; // grab parent of b11
        Transform b11 = hose.parent;

        for (int i = 0; i < 5; i++)
        {
            hose = hose.parent; // grab b11, b10, b9, b8, b7
            hose.localPosition = new Vector3(-5, 2, 0);
            hose.localEulerAngles = new Vector3(0, 0, 0);
        }

        b11.localEulerAngles = new Vector3(0, 0, -20); // one-off transform
    }
}
