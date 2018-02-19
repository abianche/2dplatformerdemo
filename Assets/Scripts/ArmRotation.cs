using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
  public int rotationOffeset = 90;
  private bool flipped = false;

  void Update()
  {
    // substracting the position of the player from the mouse position
    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    // normalizing the vector meaning that the sum of the vector will be equal to 1
    difference.Normalize();

    // find the angle in degress
    float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    float totalRotZ = rotZ + rotationOffeset;
    transform.rotation = Quaternion.Euler(0f, 0f, totalRotZ);

    if (((totalRotZ > 90f || totalRotZ < -90f) && !flipped) || ((totalRotZ <= 90f && totalRotZ >= -90f) && flipped))
    {
      Vector3 scale = transform.localScale;
      scale.y *= -1;
      transform.localScale = scale;
      flipped = !flipped;
    }
  }
}
