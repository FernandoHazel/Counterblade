using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWaypoints : MonoBehaviour
{
    /* public static int waysQuantity = 0;
    [Tooltip("This is the ID for this way")]
    public int wayID; */
    public List<Transform> wpList = new List<Transform>();
    Transform thisObject;

    private void Awake() 
    {
        //generate ID for this way
        /* waysQuantity++;
        wayID = waysQuantity; */
        //Debug.Log($"This is the way {wayID} ");

        //add all the waypoints to the list automatically
        thisObject = GetComponent<Transform>();
        for (int i=0; i < thisObject.childCount; i++)
        {
            wpList.Add(thisObject.GetChild(i));
        }
    }
}
