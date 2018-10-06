using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    // fields set in the Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMulti = 8f;
    

    // fields set dynamically
    [Header("Set Dynamically")]

    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody
        projectileRigidbody;

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint");                     // a
        launchPoint = launchPointTrans.gameObject;                                      // b
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
        
    }

     void OnMouseEnter() {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit() {

        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }
     void OnMouseDown() {
        // The player has pressed he mouse button while over slingshot
        aimingMode = true;
        // Instantiate a Projectile
        projectile = Instantiate(prefabProjectile) as GameObject;

        // Start it at the launchPoint
        projectile.transform.position = launchPos;
        // Set it to isKinematic for now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();

        projectile.GetComponent<Rigidbody>().isKinematic = true;

    }
     void Update() {
        // If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode)
            return;

        // Get the current mouse position in 2D screen coordinates
        Vector3 mousepos2D = Input.mousePosition;
        mousepos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousepos2D);

        // Find the detla from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0)) {

            // The mouse has been released
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMulti;
            FollowCam.POI = projectile;
            projectile = null;
        }
    }
}
