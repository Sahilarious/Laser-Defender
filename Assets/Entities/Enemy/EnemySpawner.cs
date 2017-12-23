using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyShipPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 1f;
    public float spawnDelay = 1f;
    //public float oscDuration = 1f;

    private bool movingRight = true;
    private float xMax;
    private float xMin;

	// Use this for initialization
	void Start ()
    {
        SpawnUntilFull();

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
    void Update ()
    {
        changePosition();

        if (AllMembersAreDead())
        {
            SpawnUntilFull();
        }
    }

    void changePosition()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
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

    Transform NextFreePosition()
    {

        foreach (Transform childTransform in transform) {
            if (childTransform.childCount == 0)
            {
                return childTransform;
            }
        }

        return null;
    }

    bool AllMembersAreDead()
    {

        // The hierarchy is structured through an object's transform
        foreach(Transform childTransform in transform)
        {
            if (childTransform.childCount > 0)
            {
                return false;
            }
        }

        return true;
    }

    void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount == 0)
            {
                GameObject enemy = Instantiate(enemyShipPrefab, child.transform.position, Quaternion.identity) as GameObject;
                enemy.transform.parent = child;
            }
        }
    }

    void SpawnUntilFull()
    {
        Transform positionTransform = NextFreePosition();

        if (positionTransform != null)
        {
            GameObject enemy = Instantiate(enemyShipPrefab, positionTransform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = positionTransform;
            //enemy.GetComponent<Enemy>().resetTime();
        }

        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }
}
