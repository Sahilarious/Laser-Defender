using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour {
    public float laserSpeed = 1f;

    private float lowerBound;

	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        lowerBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.down * laserSpeed * Time.deltaTime;
        if (transform.position.y <= 0) {
            Destroy(gameObject);
        }
	}
}
