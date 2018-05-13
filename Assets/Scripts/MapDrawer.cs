using Pacman3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour {

    public GameObject MapGeneratorObj;
    public GameObject prefabBean, prefabMonster, prefabWall;
    public GameObject BeansObj, MonstersObj, WallsObj;
    public GameObject mixedRealityCamera;

    private SuccesiveGameMap gameMap = null;
    private bool finish = false;
    private transform_coord trancord = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (finish)
            return;

        var mapGenerator = MapGeneratorObj.GetComponent<MapGenerator>();
        if (!mapGenerator.gameMapFinish)
            return;
        gameMap = mapGenerator.gameMap;
        trancord = mapGenerator.trancord;

        float dec = 4f;
        for (int i = 0; i < gameMap.n; ++i)
            for (int j = 0; j < gameMap.m; ++j)
                if (gameMap.t[i,j] == GameMap.Wall)
                {
                    Point3D worldPos = trancord.GameToWolrd(new GamePos(i,j));
                    GameObject obj = Instantiate(prefabWall);
                    obj.transform.parent = WallsObj.transform;
                    obj.transform.localPosition = new Vector3(worldPos.x, mixedRealityCamera.transform.localPosition.y - dec, worldPos.z);
                    obj.transform.localScale = new Vector3(transform_coord.rate, transform_coord.rate*2, transform_coord.rate);
                }
        for (int i = 0; i < gameMap.BeanNum; ++i)
        {
            Point3D worldPos = trancord.GameToWolrd(new Point3D(gameMap.Beans[i].c.x, gameMap.Beans[i].c.z, gameMap.Beans[i].c.y));
            GameObject obj = Instantiate(prefabBean);
            obj.transform.parent = WallsObj.transform;
            obj.transform.localPosition = new Vector3(worldPos.x, mixedRealityCamera.transform.localPosition.y - dec, worldPos.z);
            float d = gameMap.Beans[i].r * transform_coord.rate;
            obj.transform.localScale = new Vector3(d, d, d);
        }
        for (int i = 0; i < gameMap.Mons.Length; ++i)
        {
            Point3D worldPos = trancord.GameToWolrd(new Point3D(gameMap.Mons[i].cir.c.x, gameMap.Mons[i].cir.c.z, gameMap.Mons[i].cir.c.y));
            GameObject obj = Instantiate(prefabMonster);
            obj.transform.parent = WallsObj.transform;
            obj.transform.localPosition = new Vector3(worldPos.x, mixedRealityCamera.transform.localPosition.y - dec, worldPos.z);
            float d = gameMap.Mons[i].cir.r * transform_coord.rate;
            obj.transform.localScale = new Vector3(d, d, d);
        }

        finish = true;
    }
}
