using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject spatialProcessingObj;
    public GameObject initializeObj;

    private SurfaceMeshesToPlanes meshToPlanes;
    private Initialize initializer;


    private bool meshProcessed = false;

    // Use this for initialization
    void Start () {
        meshToPlanes = spatialProcessingObj.GetComponent<SurfaceMeshesToPlanes>();
        initializer = initializeObj.GetComponent<Initialize>();

        meshToPlanes.MakePlanesComplete += SurfaceMeshesToPlanes_MakePlanesComplete;
    }
	
	// Update is called once per frame
	void Update () {
        /* wait initialization */
        if (!initializer.Finished)
            return;

        if (!meshProcessed)
        {
            Debug.Log("plane making");
            meshToPlanes.MakePlanes();
            meshProcessed = true;
        }

    }

    // Handler for the SurfaceMeshesToPlanes MakePlanesComplete event.
    private void SurfaceMeshesToPlanes_MakePlanesComplete(object source, System.EventArgs args)
    {
        Debug.Log("plane complete");
        List<GameObject> floor, walls;
        floor = meshToPlanes.GetActivePlanes(PlaneTypes.Floor);
        walls = meshToPlanes.GetActivePlanes(PlaneTypes.Wall);

        foreach (GameObject plane in meshToPlanes.ActivePlanes)
        {
            SurfacePlane surfacePlane = plane.GetComponent<SurfacePlane>();
            if (surfacePlane != null)
            {
                if (((PlaneTypes.Wall & surfacePlane.PlaneType) == surfacePlane.PlaneType)
                    || ((PlaneTypes.Floor & surfacePlane.PlaneType) == surfacePlane.PlaneType))
                {
                    ;
                }
                else
                    surfacePlane.enabled = false;
            }
        }
    }
}
