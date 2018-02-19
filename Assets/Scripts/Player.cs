using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityStandardAssets._2D.Platformer2DUserControl))]
public class Player : MonoBehaviour
{

  public int fallBoundary = -20;

  public string deathSoundName = "DeathVoice";
  public string damageSoundName = "Grunt";

  private AudioManager audioManager;

  public Transform arm;

  [SerializeField]
  private StatusIndicator statusIndicator;

  private PlayerStats stats;

  void Start()
  {
    stats = PlayerStats.instance;
    stats.curHealth = stats.maxHealth;

    if (arm == null)
    {
      Debug.LogError("PLAYER: no 'arm' found.");
    }

    if (statusIndicator == null)
    {
      Debug.LogError("STATUS INDICATOR: no 'statusIndicator' reference on Player.");
    }
    else
    {
      statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

    audioManager = AudioManager.instance;
    if (audioManager == null)
    {
      Debug.LogError("No 'audioManager' in scene.");
    }

    InvokeRepeating("RegenHealth", 1f / stats.healthRegenRate, 1f / stats.healthRegenRate);
  }

  void RegenHealth()
  {
    stats.curHealth += 1;
    statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
  }

  void Update()
  {
    if (transform.position.y <= fallBoundary)
    {
      DamagePlayer(9999999);
    }
  }

  void OnUpgradeMenuToggle(bool active)
  {
    // handle what happens when the upgrade menu is toggled
    GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().enabled = !active;
    Weapon _weapon = GetComponentInChildren<Weapon>();
    if (_weapon != null)
    {
      _weapon.enabled = !active;
    }
  }

  public void DamagePlayer(int damage)
  {
    stats.curHealth -= damage;
    if (stats.curHealth <= 0)
    {
      audioManager.PlaySound(deathSoundName);
      GameMaster.KillPlayer(this);
    }
    else
    {
      audioManager.PlaySound(damageSoundName);
    }

    statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
  }

  private void OnDestroy()
  {
    GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
  }


  private void OnTriggerEnter2D(Collider2D _other)
  {
    if (_other.tag.Equals("Weapon"))
    {
      Destroy(_other.GetComponent<BoxCollider2D>());

      _other.transform.SetParent(arm);
      _other.transform.rotation = arm.transform.rotation;
      _other.transform.localPosition = new Vector3(0.4f, 0f, 0f);

      MonoBehaviour[] scripts = _other.GetComponentsInChildren<MonoBehaviour>();
      foreach (MonoBehaviour script in scripts)
      {
        script.enabled = true;
      }
    }
  }


}
