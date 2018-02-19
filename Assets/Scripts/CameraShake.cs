using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  public Camera cam;

  float shakeAmount = 0f;

  void Awake()
  {
    if (cam == null)
    {
      cam = Camera.main;
    }
  }

  public void Shake(float amount, float length)
  {
    shakeAmount = amount;
    InvokeRepeating("DoShake", 0, 0.01f);
    Invoke("StopShake", length);
  }

  void DoShake()
  {
    if (shakeAmount > 0)
    {
      Vector3 camPos = cam.transform.position;
      float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
      float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
      camPos.x += offsetX;
      camPos.y += offsetY;

      cam.transform.position = camPos;
      //cam.transform.localPosition = camPos;
    }
  }

  void StopShake()
  {
    CancelInvoke("DoShake");
    cam.transform.localPosition = Vector3.zero;
  }

}
