using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerUpGenerator : MonoBehaviour {
    public float probability = 0.1f;
    public GameObject shieldPowerUp;

	// Update is called once per frame
	void Update ()
    {
        if (Random.value * 100 < probability)
        {
            Instantiate(shieldPowerUp, new Vector3(Random.Range(0,16), 16, 0), Quaternion.identity);
        }
	}
}
