using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using Pacman3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject spatialProcessingObj;
    public GameObject initializeObj;
    public GameObject mixedRealityCamera;
    public GameObject resourcePlane;
    //public GameObject spatialUnderStandingObj;

    private SurfaceMeshesToPlanes meshToPlanes;
    private Initialize initializer;
    //private SpatialUnderstandingSourceMesh spatialUnderstandingSource;

    private bool meshProcessed = false;

    public SuccesiveGameMap gameMap = null;

    // Use this for initialization
    void Start () {
        meshToPlanes = spatialProcessingObj.GetComponent<SurfaceMeshesToPlanes>();
        initializer = initializeObj.GetComponent<Initialize>();

        meshToPlanes.MakePlanesComplete += SurfaceMeshesToPlanes_MakePlanesComplete;

        //spatialUnderstandingSource = spatialUnderStandingObj.GetComponent<SpatialUnderstandingSourceMesh>();
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
        SurfacePlane floorSurface = null, ceilSurface = null;

        Bounds floorbd = new Bounds();
        Bounds ceilbd = new Bounds();

        //walls = meshToPlanes.GetActivePlanes(PlaneTypes.Wall);

        foreach (GameObject plane in meshToPlanes.ActivePlanes)
        {
            SurfacePlane surfacePlane = plane.GetComponent<SurfacePlane>();
            if (surfacePlane != null)
            {
                if ((surfacePlane.PlaneType & PlaneTypes.Floor) == surfacePlane.PlaneType)
                {
                    //OrientedBoundingBox flr = surfacePlane.Plane.Bounds;
                    //Debug.Log(flr);
                    /*
                    GameObject obj = Instantiate(resourcePlane);
                    obj.transform.localPosition = surfacePlane.transform.localPosition;
                    obj.transform.localRotation = surfacePlane.transform.localRotation;
                    obj.transform.localScale = surfacePlane.transform.localScale;
                    */
                    Bounds bd = surfacePlane.GetComponent<Renderer>().bounds;
                    if (floorSurface == null || bd.extents.x * bd.extents.z > floorbd.extents.x * floorbd.extents.z)
                    {
                        floorSurface = surfacePlane;
                        floorbd = floorSurface.GetComponent<Renderer>().bounds;
                    }
                }
                else if ((surfacePlane.PlaneType & PlaneTypes.Ceiling) == surfacePlane.PlaneType)
                {
                    Bounds bd = surfacePlane.GetComponent<Renderer>().bounds;
                    if (ceilSurface == null || bd.extents.x * bd.extents.z > ceilbd.extents.x * ceilbd.extents.z)
                    {
                        ceilSurface = surfacePlane;
                        ceilbd = ceilSurface.GetComponent<Renderer>().bounds;
                    }
                }
                surfacePlane.IsVisible = false;
            }
        }

        transform_coord trancord = new transform_coord();
        GamePos nm = trancord.tranformxyz(4, new Point3D[4] { new Point3D(floorbd.center.x - floorbd.extents.x / 2, 0, floorbd.center.z - floorbd.extents.z / 2),
                                                    new Point3D(floorbd.center.x + floorbd.extents.x / 2, 0, floorbd.center.z - floorbd.extents.z / 2),
                                                    new Point3D(floorbd.center.x + floorbd.extents.x / 2, 0, floorbd.center.z + floorbd.extents.z / 2),
                                                    new Point3D(floorbd.center.x - floorbd.extents.x / 2, 0, floorbd.center.z + floorbd.extents.z / 2)});

        gameMap = new SuccesiveGameMap(nm.x + 1, nm.y + 1);
        gameMap.addBorder(new Line[4] {
                new Line(new Point3D(0,0), new Point3D(nm.x, 0)),
                new Line(new Point3D(nm.x, 0), new Point3D(nm.x, nm.y)),
                new Line(new Point3D(nm.x, nm.y),new Point3D(0, nm.y)),
                new Line(new Point3D(0, nm.y),new Point3D(0,0))
            });
        gameMap.setPlayer(new Point3D(mixedRealityCamera.transform.localPosition));

        List<MeshFilter> meshFilters = SpatialMappingManager.Instance.GetMeshFilters();
        for (int i = 0; i < meshFilters.Count; i++)
        {
            Vector3[] vertices = meshFilters[i].mesh.vertices;
            int[] tris = meshFilters[i].mesh.triangles;
            for (int j = 0; j < tris.Length; j += 3)
            {
                Vector3 p1 = vertices[tris[j]];
                Vector3 p2 = vertices[tris[j+1]];
                Vector3 p3 = vertices[tris[j+2]];
                Vector3 center = (p1 + p2 + p3) / 3;
                if (floorbd.Contains(center) || ceilbd.Contains(center))
                    continue;
                gameMap.addTriangle(new Triangle(new Point3D(p1), new Point3D(p2), new Point3D(p3)));
            }
        }

        gameMap.generateMap();
    }
}
