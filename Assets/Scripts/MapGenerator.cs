using HoloToolkit.Unity.SpatialMapping;
using Pacman3D;
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
        List<GameObject> floors;//, walls;
        floors = meshToPlanes.GetActivePlanes(PlaneTypes.Floor);
        GameObject floor = floors[0];
        //walls = meshToPlanes.GetActivePlanes(PlaneTypes.Wall);

        foreach (GameObject plane in meshToPlanes.ActivePlanes)
        {
            SurfacePlane surfacePlane = plane.GetComponent<SurfacePlane>();
            if (surfacePlane != null)
            {
                if (((surfacePlane.PlaneType & PlaneTypes.Floor) != surfacePlane.PlaneType)
                    && ((surfacePlane.PlaneType & PlaneTypes.Ceiling) != surfacePlane.PlaneType))
                    ;
                surfacePlane.IsVisible = false;
            }
        }
        

        //SuccesiveGameMap gameMap = new SuccesiveGameMap(100, 100);
        /*gameMap.addBorder(new Line[4] {
            new Line(new Point3D(0,0), new Point3D(99, 0)),
            new Line(new Point3D(99, 0), new Point3D(99, 99)),
            new Line(new Point3D(99, 99),new Point3D(0, 99)),
            new Line(new Point3D(0, 99),new Point3D(0,0))
        });
        */
        /*gameMap.addBorder(new Line[4] {
            new Line(new Point3D(10,10), new Point3D(99, 10)),
            new Line(new Point3D(99, 10), new Point3D(99, 99)),
            new Line(new Point3D(99, 99),new Point3D(10, 99)),
            new Line(new Point3D(10, 99),new Point3D(10, 10))
        });
        gameMap.setPlayer();
        gameMap.generateMap();*/
    }
}
