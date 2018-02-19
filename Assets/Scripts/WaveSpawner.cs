using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

  public enum SpawnState
  {
    SPAWNING, WAITING, COUNTING
  };

  [System.Serializable]
  public class Wave
  {
    public string name;
    public Transform enemy;
    public int count;
    public float rate;
  }

  public Wave[] waves;
  public Transform[] spawnPoints;
  private int nextWave = 0;

  public int NextWave
  {
    get { return nextWave + 1; }
  }

  public float timeBetweenWaves = 5f;
  public float waveCountdown;
  public float WaveCountdown
  {
    get { return waveCountdown; }
  }

  // limit searches once per second
  private float searchCountdown = 1f;

  private SpawnState state = SpawnState.COUNTING;

  public SpawnState State
  {
    get { return state; }
  }

  void Start()
  {
    if (spawnPoints.Length == 0)
    {
      Debug.LogError("No 'spawnPoints' referenced.");
    }

    waveCountdown = timeBetweenWaves;
  }

  void Update()
  {
    if (state == SpawnState.WAITING)
    {
      if (!EnemyIsAlive())
      {
        WaveCompleted();
        return;
      }
      else
      {
        return;
      }
    }

    if (waveCountdown <= 0)
    {
      if (state != SpawnState.SPAWNING)
      {
        StartCoroutine(SpawnWave(waves[nextWave]));
      }
    }
    else
    {
      waveCountdown -= Time.deltaTime;
    }
  }

  void WaveCompleted()
  {
    Debug.Log("Wave Completed!");

    state = SpawnState.COUNTING;
    waveCountdown = timeBetweenWaves;

    if (nextWave + 1 > waves.Length - 1)
    {
      nextWave = 0;
      Debug.Log("ALL WAVES COMPLETE! Looping...");
      // TODO: increase difficulty or complete game
    }
    else
    {
      nextWave++;
    }
  }

  bool EnemyIsAlive()
  {
    searchCountdown -= Time.deltaTime;

    if (searchCountdown <= 0)
    {
      searchCountdown = 1f;
      if (GameObject.FindGameObjectWithTag("Enemy") == null)
      {
        return false;
      }
    }
    return true;
  }

  IEnumerator SpawnWave(Wave _wave)
  {
    Debug.Log("Spawning wave: " + _wave.name);
    state = SpawnState.SPAWNING;

    for (int i = 0; i < _wave.count; i++)
    {
      SpawnEnemy(_wave.enemy);
      yield return new WaitForSeconds(1 / _wave.rate);
    }

    state = SpawnState.WAITING;

    yield break; // whenver using IEnumerator, end with yield break
  }

  void SpawnEnemy(Transform _enemy)
  {
    Debug.Log("Spawning enemy: " + _enemy.name);

    Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
    Instantiate(_enemy, _sp.position, _sp.rotation);
  }
}
