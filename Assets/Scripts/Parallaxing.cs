using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
  // array of all the back- and foregrounds to be parallaxed
  public Transform[] backgrounds;
  // proportions of the camera movement to move the backgrounds by	 									
  private float[] parallaxScales;
  // how smooth the parallax is going to be - must be above 0										
  public float smoothing = 1f;

  // reference to the main camera transform
  private Transform cam;
  // the position of the camera in the previous frame
  private Vector3 previousCamPosition;

  // Is called before Start(). Great for references.
  void Awake()
  {
    // set up camera reference
    cam = Camera.main.transform;

  }

  // Use this for initialization
  void Start()
  {
    // the previous frame had the current frame camera position
    previousCamPosition = cam.position;

    // assigning corresponding parallax scales
    parallaxScales = new float[backgrounds.Length];
    for (int i = 0; i < backgrounds.Length; i++)
    {
      parallaxScales[i] = backgrounds[i].position.z * -1;
    }
  }

  // Update is called once per frame
  void Update()
  {
    // for each background
    for (int i = 0; i < backgrounds.Length; i++)
    {
			// the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
			float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];

			// set a target x position which is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			// create a target position which is the background's current position with its target x position
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			// fade between current position and the target position using Lerp
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
    }

		// set the previous camera pos to the camera's position at the end of the frame
		previousCamPosition = cam.position;
  }
}
