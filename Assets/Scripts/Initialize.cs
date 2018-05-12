using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine.XR.WSA.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour {

    public GameObject spatialUnderstandingObj;
    public GameObject spatialMappingObj;

    private SpatialUnderstanding spatialUnderstanding;
    private SpatialUnderstandingCustomMesh customMesh;
    private SpatialMappingManager mappingManager;

    private GestureRecognizer recognizer;

    private bool finished = false;

    public bool Finished
    {
        get
        {
            return finished;
        }
        private set { }
    }

    // Use this for initialization
    void Start () {
        spatialUnderstanding = spatialUnderstandingObj.GetComponent<SpatialUnderstanding>();
        customMesh = spatialUnderstanding.UnderstandingCustomMesh;
        mappingManager = spatialMappingObj.GetComponent<SpatialMappingManager>();

        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            OnTapped();
        };
        recognizer.StartCapturingGestures();
    }
	
	// Update is called once per frame
	void Update () {
        if (finished)
            return;

        if (spatialUnderstanding.ScanState == SpatialUnderstanding.ScanStates.None)
        {
            mappingManager.StartObserver();
            spatialUnderstanding.RequestBeginScanning();
        }
    }

    void OnTapped()
    {
        if (finished)
            return;

        Debug.Log("scan finish");
        finished = true;
        mappingManager.StopObserver();
        spatialUnderstanding.RequestFinishScan();
        recognizer.StopCapturingGestures();
        customMesh.DrawProcessedMesh = false;
    }
}
