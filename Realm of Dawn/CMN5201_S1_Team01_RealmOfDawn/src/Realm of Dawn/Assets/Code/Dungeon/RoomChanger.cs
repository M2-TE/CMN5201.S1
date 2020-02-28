using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChanger : MonoBehaviour {



    public GameObject RoomToActivate;
    public GameObject Door;
    public GameObject RoomToDeaktivate;
    private GameObject Player;
    private bool Used = false;
    
    
    
    // Use this for initialization
	void Start () {

        Player = GameObject.FindGameObjectWithTag("Player");
		
	}
	
	// Update is called once per frame
	void Update () {
        
		
	}

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Used == false)
        {
            Debug.Log("Collided with Player");
            Player.transform.position = Door.transform.position;
            RoomToActivate.SetActive(true);
            RoomToDeaktivate.SetActive(false);
            Used = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            Used = false;
        }
        Debug.Log("PLayer left Door");
    }
}
