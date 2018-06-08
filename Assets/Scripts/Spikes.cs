using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {


    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void onTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {

        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
