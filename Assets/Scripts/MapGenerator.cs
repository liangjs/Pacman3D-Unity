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
    public transform_coord trancord = null;
    public bool gameMapFinish = false;

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
                else
                {
                    //surfacePlane.IsVisible = false;
                    if ((surfacePlane.PlaneType & PlaneTypes.Ceiling) == surfacePlane.PlaneType)
                    {
                        Bounds bd = surfacePlane.GetComponent<Renderer>().bounds;
                        if (ceilSurface == null || bd.extents.x * bd.extents.z > ceilbd.extents.x * ceilbd.extents.z)
                        {
                            ceilSurface = surfacePlane;
                            ceilbd = ceilSurface.GetComponent<Renderer>().bounds;
                        }
                    }
                }
                surfacePlane.IsVisible = false;
            }
        }

        if (floorSurface == null)
        {
            Debug.LogError("floor not found");
            return;
        }

        trancord = new transform_coord();
        GamePos nm = trancord.tranformxyz(4, new Point3D[4] { new Point3D(floorbd.center.x - floorbd.extents.x / 2, floorbd.center.y, floorbd.center.z - floorbd.extents.z / 2),
                                                    new Point3D(floorbd.center.x + floorbd.extents.x / 2, floorbd.center.y, floorbd.center.z - floorbd.extents.z / 2),
                                                    new Point3D(floorbd.center.x + floorbd.extents.x / 2, floorbd.center.y, floorbd.center.z + floorbd.extents.z / 2),
                                                    new Point3D(floorbd.center.x - floorbd.extents.x / 2, floorbd.center.y, floorbd.center.z + floorbd.extents.z / 2)});

        gameMap = new SuccesiveGameMap(nm.x + 1, nm.y + 1);
        gameMap.setPlayer(new Point3D(mixedRealityCamera.transform.localPosition));

        /*List<Mesh> meshes = SpatialMappingManager.Instance.GetMeshes();
        Point3D pmnx, pmnz, pmxx, pmxz;
        pmnx = new Point3D(FloatCmp.INF, 0, 0);
        pmnz = new Point3D(0, 0, FloatCmp.INF);
        pmxx = new Point3D(-FloatCmp.INF, 0, 0);
        pmxz = new Point3D(0, 0, -FloatCmp.INF);
        float mny = FloatCmp.INF, mxy = -FloatCmp.INF;

        for (int i = 0; i < meshes.Count; i++)
        {
            Vector3[] vertices = meshes[i].vertices;
            int[] tris = meshes[i].triangles;
            for (int j = 0; j < tris.Length; j += 3)
            {
                Vector3 p1 = vertices[tris[j]];
                Vector3 p2 = vertices[tris[j + 1]];
                Vector3 p3 = vertices[tris[j + 2]];
                Vector3 center = (p1 + p2 + p3) / 3;
                if (floorbd.center.x - floorbd.extents.x / 2 <= center.x && center.x <= floorbd.center.x + floorbd.extents.x / 2
                    && floorbd.center.z - floorbd.extents.z / 2 <= center.z && center.z <= floorbd.center.z + floorbd.extents.z / 2
                    )//&& System.Math.Abs(floorbd.center.y - center.y) < 0.01)
                {
                    if (center.x < pmnx.x)
                        pmnx = new Point3D(center);
                    if (center.x > pmxx.x)
                        pmxx = new Point3D(center);
                    if (center.z < pmnz.z)
                        pmnz = new Point3D(center);
                    if (center.z > pmxz.z)
                        pmxz = new Point3D(center);
                }
                mny = System.Math.Min(mny, center.y);
                mxy = System.Math.Max(mxy, center.y);
            }
        }
        pmnx = trancord.WorldToGame(pmnx);
        pmxx = trancord.WorldToGame(pmxx);
        pmnz = trancord.WorldToGame(pmnz);
        pmxz = trancord.WorldToGame(pmxz);*/
        /*gameMap.addBorder(new Line[4] {
                new Line(pmnx, pmnz),
                new Line(pmxx, pmnz),
                new Line(pmnx, pmxz),
                new Line(pmxx, pmxz)
            });*/
        gameMap.addBorder(new Line[4] {
                new Line(new Point3D(0,0), new Point3D(nm.x,0)),
                new Line(new Point3D(nm.x,0), new Point3D(nm.x,nm.y)),
                new Line(new Point3D(nm.x,nm.y), new Point3D(0,nm.y)),
                new Line(new Point3D(0,nm.y), new Point3D(0,0))
            });

        /*for (int i = 0; i < meshes.Count; i++)
        {
            Vector3[] vertices = meshes[i].vertices;
            int[] tris = meshes[i].triangles;
            for (int j = 0; j < tris.Length; j += 3)
            {
                Vector3 p1 = vertices[tris[j]];
                Vector3 p2 = vertices[tris[j+1]];
                Vector3 p3 = vertices[tris[j+2]];
                Vector3 center = (p1 + p2 + p3) / 3;
                if (floorbd.center.x - floorbd.extents.x / 2 <= center.x && center.x <= floorbd.center.x + floorbd.extents.x / 2
                    && floorbd.center.z - floorbd.extents.z / 2 <= center.z && center.z <= floorbd.center.z + floorbd.extents.z / 2
                    )//&& FloatCmp.cmp(floorbd.center.y, center.y) == 0)
                    continue;
                if (ceilSurface != null && ceilbd.center.x - ceilbd.extents.x / 2 <= center.x && center.x <= ceilbd.center.x + ceilbd.extents.x / 2
                    && ceilbd.center.z - ceilbd.extents.z / 2 <= center.z && center.z <= ceilbd.center.z + ceilbd.extents.z / 2
                    )//&& FloatCmp.cmp(ceilbd.center.y, center.y) == 0)
                    continue;
                gameMap.addTriangle(new Triangle(trancord.WorldToGame(new Point3D(p1)), trancord.WorldToGame(new Point3D(p2)), trancord.WorldToGame(new Point3D(p3))));
            }
        }*/

        gameMap.generateMap();
        gameMapFinish = true;

        Debug.Log("Finish Generating Map");
    }
}
