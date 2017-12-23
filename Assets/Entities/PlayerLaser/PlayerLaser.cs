using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour {
    public float laserSpeed = 1f;
    public float damage = 100f;

   

    private float upperBound;
	// Use this for initialization
	void Start () {

        float distance = transform.position.z - Camera.main.transform.position.z;
        upperBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;

    }
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.up* laserSpeed * Time.deltaTime;

        if (transform.position.y >= upperBound) {
            Destroy(gameObject);
        }
    }

    public float GetDamage() {
        return damage;
    }

}
