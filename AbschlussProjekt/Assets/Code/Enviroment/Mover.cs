using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public GameObject ObjectToMove;
    public enum directions { Left, Right, Up, Down }

    public float distanceToTravel = 5;
    public float timeToTravel = 5;
    public directions direction;
    

    private string SelectedDirection;
    private Vector3 MoveDirection;
    private float Progress;
    private bool forward = true;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentTravelTime;
	// Use this for initialization
	void Start () {


        switch (direction)
        {

            case directions.Left:
                 MoveDirection = Vector3.left;
                 break;
            case directions.Right:
                MoveDirection = Vector3.right;
                break;
            case directions.Up:
                MoveDirection = Vector3.up;
                break;
            case directions.Down:
                MoveDirection = Vector3.down;
                break;



        }
           





       

       
        startPosition = ObjectToMove.transform.position;
        endPosition = ObjectToMove.transform.position + MoveDirection * distanceToTravel;
       
		
	}
	
	// Update is called once per frame
	void Update () {


        MoveObject();


    }

    private void MoveObject()
    {

        if (forward)
        {

            currentTravelTime += Time.deltaTime;

            Progress = currentTravelTime / timeToTravel;
            ObjectToMove.transform.position = Vector3.Lerp(startPosition, endPosition, Progress);
            if (ObjectToMove.transform.position == endPosition)
            {
                currentTravelTime = 0;
                Progress = 0;
                forward = false;
            }

        }
        if (forward == false)
        {
            currentTravelTime += Time.deltaTime;

            Progress = currentTravelTime / timeToTravel;
            ObjectToMove.transform.position = Vector3.Lerp(endPosition, startPosition, Progress);
            if (ObjectToMove.transform.position == startPosition)
            {
                currentTravelTime = 0;
                Progress = 0;
                forward = true;
            }



        }

    }

   
    
    

}
