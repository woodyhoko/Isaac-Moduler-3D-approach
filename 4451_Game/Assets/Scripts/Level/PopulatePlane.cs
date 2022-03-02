using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopulatePlane : MonoBehaviour {

    //gameobjects
    //public GameObject Door;
    //public GameObject shortWall;
    //public GameObject longWall;
    //public GameObject Ground;

    public GameObject[] planeItems = new GameObject[0];

    //statisticals
    private float shortWallPerc;
    private float longWallPerc;
    private float crabPerc;

    private float maxX;
    private float maxY;

    public static bool IsPrefabRef(UnityEngine.Object o)
    {
        #if UNITY_EDITOR
        return PrefabUtility.GetCorrespondingObjectFromSource(o) == null && PrefabUtility.GetPrefabObject(o) != null;
        #endif
        return true;
    }

    static GameObject CreatePrefab(UnityEngine.Object fab, Vector3 pos, Quaternion rot)
    {
        #if UNITY_EDITOR
        GameObject e = PrefabUtility.InstantiatePrefab(fab as GameObject) as GameObject;
        e.transform.position = pos;
        e.transform.rotation = rot;
        return e;
        #endif
        GameObject o = GameObject.Instantiate(fab as GameObject) as GameObject;
        o.transform.position = pos;
        o.transform.rotation = rot;
        return o;
    }

    private void Start()
    {
        GameObject Ground = GetPlaneItems(planeItems, "Plane");

        maxX = Ground.transform.localScale.x;
        maxY = Ground.transform.localScale.z;

        putStuffDown(maxX, maxY);
    }

    GameObject GetPlaneItems(GameObject[] g, string name)
    {
        for (int i = 0; i < g.Length; i++)
        {
            if (g[i].name == name)
                return g[i];
        }

        Debug.Log("No gameobject has the name '" + name + "'.");
        return null;
    }

    private void putStuffDown(float x, float y)
    {
        float totalArea = x * y;

        foreach(GameObject g in planeItems)
        {
            //based on the percentage of ground that you want the object to appear, instantiate it? 
            //
        }
    }
}
