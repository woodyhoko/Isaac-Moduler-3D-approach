using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour

//Attach this script to your GameObject. This GameObject doesn’t need to have a Collider component
//Set the Layer Mask field in the Inspector to the layer you would like to see collisions in (set to Everything if you are unsure).
//Create a second Gameobject for testing collisions. Make sure your GameObject has a Collider component (if it doesn’t, click on the Add Component button in the GameObject’s Inspector, and go to Physics>Box Collider).
//Place it so it is overlapping your other GameObject.
//Press Play to see the console output the name of your second GameObject
{
    bool m_Started;
    public LayerMask m_LayerMask;

    private bool gotBlocks;
    HashSet<Vector3> takenSpots;

    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
        takenSpots = new HashSet<Vector3>();
        //this.transform.localScale = 
    }

    void OnCollisionEnter(Collision other)
    {
        if (!gotBlocks)
        {
            gotBlocks = true;
            if (other.contactCount > 0)
            {
                //Debug.Log("contactCOunt: " + other.contactCount);
                Debug.DrawRay(other.contacts[0].point, Vector3.up, Color.red, 10.0f);
                //Debug.Log("scale" + transform.localScale);
                for (int i = 0; i < other.contactCount; i++)
                {
                    takenSpots.Add(other.contacts[i].point);
                }
            }
            //Debug.Log("this is how many taken spots there are: " + takenSpots.Count);
        }
    }

    //void FixedUpdate()
    //{
    //    MyCollisions();
    //}

    //void MyCollisions()
    //{
    //    //Use the OverlapBox to detect if there are any other colliders within this box area.
    //    //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
    //    Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);

    //    foreach(Collider c in hitColliders)
    //    {
    //        foreach(ContactPoint cp in c.)
    //        takenSpots.Add(c.transform.position);
    //    }
    //    //int i = 0;
    //    ////Check when there is a new collider coming into contact with the box
    //    //while (i < hitColliders.Length)
    //    //{
    //    //    //Output all of the collider names
    //    //    Debug.Log("Hit : " + hitColliders[i].name + i);
    //    //    //Increase the number of Colliders in the array
    //    //    i++;
    //    //}
    //}

    public HashSet<Vector3> getTakenPositions()
    {
        return takenSpots;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
