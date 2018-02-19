using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlamethrower : MonoBehaviour
{
  public float fireRate = 0;
  public int damage = 10;
  public LayerMask ToHit;
  public Transform HitParticle;
  public Transform FlameParticles;


  float timeToSpawnEffect = 0;
  public float effectSpawnRate = 10f;

  public string weaponShootSound = "FlameShot";

  float timeToFire = 0;
  Transform firePoint;

  AudioManager audioManager;

  void Awake()
  {
    firePoint = transform.Find("FirePoint");
    if (firePoint == null)
    {
      Debug.LogError("No 'firePoint'? WHAT !?");
    }
  }

  void Start()
  {
    audioManager = AudioManager.instance;
    if (audioManager == null)
    {
      Debug.LogError("No 'audioManager' found in scene.");
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (fireRate == 0)
    {
      if (Input.GetButtonDown("Fire1"))
      {
        Shoot();
      }
    }
    else
    {
      if (Input.GetButton("Fire1") && Time.time > timeToFire)
      {
        timeToFire = Time.time + 1 / fireRate;
        Shoot();
      }
    }
  }

  void Shoot()
  {
    Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

    Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

    Instantiate(FlameParticles, firePoint.position, firePoint.rotation);

  }

  void Effect(Vector3 hitPos, Vector3 hitNormal)
  {
    if (hitNormal != new Vector3(9999, 9999, 9999))
    {
      Instantiate(HitParticle, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
    }

    // Play shoot sound
    audioManager.PlaySound(weaponShootSound);
  }

}
