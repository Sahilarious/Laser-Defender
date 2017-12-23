using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSilverPowerUp : ShieldPowerUpBase
{
    public float speed = 0.5f;

	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.position += Vector3.down  * speed * Time.deltaTime;
	}

}
