using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
  public static GameMaster gm;

  [SerializeField]
  private int maxLives = 3;

  private static int _remainingLives;
  public static int RemainingLives
  {
    get { return _remainingLives; }
  }

  [SerializeField]
  private int startingMoney;
  public static int money;

  void Awake()
  {
    if (gm == null)
    {
      gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
  }

  public Transform player;
  public Transform spawnPoint;
  public float spawnDelay = 2;
  public Transform spawnParticles;
  public string respawnCountdownSoundName = "RespawnCountdown";
  public string spawnSoundName = "Spawn";

  public string gameOverSoundName = "GameOver";

  public CameraShake cameraShake;

  [SerializeField]
  private GameObject gameOverUI;

  [SerializeField]
  private GameObject upgradeMenu;

  [SerializeField]
  private WaveSpawner waveSpawner;

  public delegate void UpgradeMenuCallback(bool active);
  public UpgradeMenuCallback onToggleUpgradeMenu;

  private AudioManager audioManager;

  void Start()
  {
    if (cameraShake == null)
    {
      Debug.LogError("No 'cameraShake' reference in Game Master.");
    }

    _remainingLives = maxLives;
    money = startingMoney;

    // caching
    audioManager = AudioManager.instance;
    if (audioManager == null)
    {
      Debug.LogError("No 'AudioManager' found in the scene.");
    }
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.U))
    {
      ToggleUpgradeMenu();
    }
  }

  private void ToggleUpgradeMenu()
  {
    upgradeMenu.SetActive(!upgradeMenu.activeSelf);
    waveSpawner.enabled = !upgradeMenu.activeSelf;
    onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
  }

  public void EndGame()
  {
    audioManager.PlaySound(gameOverSoundName);
    Debug.Log("GAME OVER");
    gameOverUI.SetActive(true);
  }

  public IEnumerator _RespawnPlayer()
  {
    audioManager.PlaySound(respawnCountdownSoundName);
    yield return new WaitForSeconds(spawnDelay);

    audioManager.PlaySound(spawnSoundName);

    Instantiate(player, spawnPoint.position, spawnPoint.rotation);
    Transform clone = Instantiate(spawnParticles, spawnPoint.position, spawnPoint.rotation) as Transform;
    Destroy(clone.gameObject, 3f);
  }

  public static void KillPlayer(Player player)
  {
    Destroy(player.gameObject);
    _remainingLives--;
    if (_remainingLives <= 0)
    {
      gm.EndGame();
    }
    else
    {
      gm.StartCoroutine(gm._RespawnPlayer());
    }
  }

  public static void KillEnemy(Enemy enemy)
  {
    gm._KillEnemy(enemy);
  }

  public void _KillEnemy(Enemy _enemy)
  {
    // sound
    audioManager.PlaySound(_enemy.deathSoundName);

    // gain some money
    money += _enemy.moneyDrop;
    audioManager.PlaySound("Money");

    // add particles
    Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);

    // camera shake
    cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
    Destroy(_enemy.gameObject);
  }
}
