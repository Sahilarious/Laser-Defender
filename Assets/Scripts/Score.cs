using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    private int score;

	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
   
    }

    public void changeScore(int points)
    {
        score += points;
        renderScore();
    }

    public void Reset()
    {
        score = 0;
        renderScore();
    }

    void renderScore() {
        gameObject.GetComponent<Text>().text = "Score: " + score;
    }
}
