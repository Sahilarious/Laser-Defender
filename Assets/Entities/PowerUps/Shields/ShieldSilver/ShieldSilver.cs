using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSilver : ShieldBase
{
    public float lifeTime = 5;
    public static GameObject shield = null;

	// Use this for initialization
	void Start ()
    {
        if (!shield)
        {
            shield = gameObject;
            Destroy(gameObject, lifeTime);
        }
        else {
            Destroy(shield);
            Destroy(gameObject, lifeTime);
            shield = gameObject;
        }
	}
}
