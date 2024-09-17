using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;

public class InstantianteObjectScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Variable that hold a prefab which will be swapend wht we click on a p;ain that has already been detected
    public GameObject objectToPlacePrefab;
    private List<ARRaycastHit> hitList = new List<ARRaycastHit>();
    //Plain manager refereange. Plane manager is component that is present on the same game object that has this script 
    //in the case of this project, Plane manager is present on the XR Origen. (Check your projects Haireachy)
    public ARPlaneManager planeManager;
    //When we  touch the spawned/deetected plane using out fingers, Unity casts a ray form the Screen Spawne Coordinates ro the Ar Scene Coordinates
    //Wehenever that raycast successfully hits somthing , it return and stores all the hit results in this list
    public ARRaycastManager raycastManager;

    private void Awake()
    {
        //THe followeng line assumes that the GameObject that has this script also has a PlaeneManager component
        //and by the above mentioned assumption,this script then gets a refference to the PlaeneManager component that is on that same game object as this script
        planeManager = GetComponent<ARPlaneManager>();
        //THe dfollowint line assuemd that the GameObject that has this script also has a ARRaycastManager Component
        //and ay the above mentiond assumeption , this scrip then gets a referance to the ARRaycastManager component that is on that same game object as this script
        raycastManager = GetComponent<ARRaycastManager>();
    }
    private void OnEnable()
    {
        Debug.Log("Enable");
        //Enable support for testing touch input through your computer
        //please note
        UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Enable();
        UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerTouchDetected;
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
        UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Disable();
        UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerTouchDetected;
    }
    // This finction contains the steps that need to take place every time a figuer tuches the screen 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FingerTouchDetected(UnityEngine.InputSystem.EnhancedTouch.Finger fingerTouch)
    {
        //Checking if a finger has indeed touch the screen.
        //Check the definition of index by right clicking on index and peeking the definition
        //Index basically is a number wehere 1 represents 1 finger touching the screen ,2 represents 2 finger touching the  screeen and so on unitl a certain limit

        //This idf conditon is to check if more then 1 finger are touching the screen
        //the first fingere touching the screen willl have a index 0
        //if the index is anything other then 0,we exit this funciton.
        if (fingerTouch.index != 0)
        {
            //this line gets executed if index is wqueal to zero. Which means on fingers are touching the screen
            //the return statement basically ends the execuation of a function

            //reruning the function because more then one fingers were
            return;
        }
        //Since the finction did nont end before insid that if statement, we know for sure that at least one finger has touched that screen
        //so we fire a arey cast form the position of the finger on you screen ,  ti the corresponding location in the AR scene
        //Thid raycast is instid a n if stament because the Raycast returns a bool. This bool is true if we hit somthing anit is a false if we did not hit somthing
        //IF we hit somthig , we store all sucessful hits in a variable called aRRaycastHuts(the second parameter inthe Raycast function)

        if (raycastManager.Raycast(fingerTouch.currentTouch.screenPosition, hitList, TrackableType.PlaneWithinPolygon))
        //Folleowint cosd happens if the raycast has iindeed hit somtheing
        //In the above line , we save all sucessful hits in the ARRaycastHits vaariable 
        //The foreach loop basically gose through each entry in that variable
        {
            //
            foreach (ARRaycastHit hit in hitList)
            //Each entry of the ARRaycastHits variable contains a property called Pose.
            //Pose id basically informaiton about the position anad the rotaion of an Object.
            //So we create a local variable called oriantion of type Pose which holds that value of the pose of the cuureent entry of the ARayCastHits variable
            {
                Pose orintation = hit.pose;
                //We now spawn an object that we want and the position and the rotation comes form that local variable orientation that we createc in the last line.
                GameObject spawnObject = Instantiate(objectToPlacePrefab, orintation.position, orintation.rotation);

            }
        }
    }
}
