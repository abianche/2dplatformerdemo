using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
  // the offset so that we don't get weird errors
  public int offsetX = 2;

  // these are used for checking if need to instantiate stuff
  public bool hasRightBuddy = false;
  public bool hasLeftBuddy = false;

  // used if the objects are not tilable
  public bool reverseScale = false;

  // the width of our element
  private float spriteWidth = 0f;
  private Camera cam;
  private Transform myTransform;

  // Awake is called when the script instance is being loaded.
  void Awake()
  {
    cam = Camera.main;
    myTransform = transform;
  }

  // Use this for initialization
  void Start()
  {
    SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
    spriteWidth = sRenderer.sprite.bounds.size.x;
  }

  // Update is called once per frame
  void Update()
  {
    // does it still need buddies? if not do nothing
    if (hasLeftBuddy == false || hasRightBuddy == false)
    {
      // calculate the camera's extent, half the width of what the camera can see in world coordinates
      float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;

      // calculate the X position where the camera can see the edge of the sprite (element)
      float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
      float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtent;

      // checking if we can see the edge of the element and then calling makeNewBuddy if we can
      if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasRightBuddy == false)
      {
        makeNewBuddy(1);
        hasRightBuddy = true;
      }
      else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasLeftBuddy == false)
      {
        makeNewBuddy(-1);
        hasLeftBuddy = true;
      }
    }
  }

  // A function that creates a buddy on the required side
  void makeNewBuddy(int rightOrLeft)
  {
    // calculating the new position for our new buddy
    Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
    // instantiating our new buddy and storing it in a variable
    Transform newBuddy = (Transform)Instantiate(myTransform, newPosition, myTransform.rotation);

    // if not tilable lets reverse the X size of our object to get rid of ugly seams
    if (reverseScale == true)
    {
      newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
    }

    newBuddy.parent = myTransform.parent;

    if (rightOrLeft > 0)
    {
      newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
    }
    else
    {
      newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
    }

  }
}
