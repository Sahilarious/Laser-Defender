using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyShipPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 1f;
    //public float oscDuration = 1f;

    private bool movingRight = true;
    private float xMax;
    private float xMin;

	// Use this for initialization
	void Start () {

        foreach (Transform child in transform) {
            GameObject enemy = Instantiate(enemyShipPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }

        float distance = transform.position.z - Camera.main.transform.position.z;

        Vector3 lowerBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 upperBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));

        xMin = lowerBound.x + width / 2;
        xMax = upperBound.x - width / 2;

    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(width, height,1f));
    }

    // Update is called once per frame
    void Update () {
        changePosition();
    }

    void changePosition() {
        //float lerp = Mathf.PingPong(Time.time, oscDuration) / oscDuration;
        //float xPos = Mathf.Lerp(xMin, xMax, lerp);
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (transform.position.x >= xMax)
        {
            movingRight = false;
        }
        else if (transform.position.x <= xMin)
        {
            movingRight = true;
        }
    }
}
