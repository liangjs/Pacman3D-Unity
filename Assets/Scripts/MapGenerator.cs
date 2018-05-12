using HoloToolkit.Unity.SpatialMapping;
using Pacman3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject spatialProcessingObj;
    public GameObject initializeObj;
    public GameObject mixedRealityCamera;

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
        Vector3 flr_pos = floor.transform.parent.TransformPoint(floor.transform.localPosition);
        Vector3 flr_scl = floor.transform.parent.TransformPoint(floor.transform.localScale);
        //walls = meshToPlanes.GetActivePlanes(PlaneTypes.Wall);

        foreach (GameObject plane in meshToPlanes.ActivePlanes)
        {
            SurfacePlane surfacePlane = plane.GetComponent<SurfacePlane>();
            if (surfacePlane != null)
            {
                if (((surfacePlane.PlaneType & PlaneTypes.Floor) == surfacePlane.PlaneType))
                {
                    //OrientedBoundingBox flr = surfacePlane.Plane.Bounds;
                    //Debug.Log(flr);
                    /*GameObject obj = Instantiate(Resources.Load("Prefabs/Plane") as GameObject);
                    obj.transform.SetPositionAndRotation(flr.Center, flr.Rotation);*/
                    //obj.transform = surfacePlane.transform;
                    ;
                }
                else
                    surfacePlane.IsVisible = false;
            }
        }

        List<MeshFilter> meshFilters = SpatialMappingManager.Instance.GetMeshFilters();
        for (int i = 0; i < meshFilters.Count; i++)
        {
            Vector3[] vertices = meshFilters[i].mesh.vertices;
            int[] tris = meshFilters[i].mesh.triangles;
            for (int j = 0; j < tris.Length; ++j)
            {
                //Debug.Log(vertices);
                /*
                GameObject m_goTriangle = new GameObject();
                m_goTriangle.AddComponent<MeshFilter>();
                m_goTriangle.AddComponent<MeshRenderer>();
                Mesh m_meshTriangle = m_goTriangle.GetComponent<MeshFilter>().mesh;
                m_meshTriangle.Clear();
                m_meshTriangle.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 0.25f), new Vector2(0.25f, 0.25f) };
                m_meshTriangle.vertices = new Vector3[] { vertices[tris[ };
                m_meshTriangle.triangles = new int[] { 0, 1, 2 };
                */
            }
        }

        /*
        Transform floor_trans = floor.GetComponent<Transform>();
        Debug.Log(floor.transform.TransformPoint(new Vector3(0, 0, 0)));
        Debug.Log(floor.transform.TransformPoint(new Vector3(10, 0, 0)));
        Debug.Log(floor.transform.TransformPoint(new Vector3(10, 0, 10)));
        Debug.Log(floor.transform.TransformPoint(new Vector3(0, 0, 10)));
        */

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
