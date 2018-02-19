using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleBag
{
  private static System.Random random = new System.Random();
  private List<Transform> data;

  private Transform currentItem;
  private int currentPosition = -1;

  private int Capacity { get { return data.Capacity; } }
  public int Size { get { return data.Count; } }

  public ShuffleBag(int initCapacity)
  {
    data = new List<Transform>(initCapacity);
  }

  public void Add(Transform item, int amount)
  {
    for (int i = 0; i < amount; i++)
      data.Add(item);

    currentPosition = Size - 1;
  }

  public Transform Next()
  {
    if (currentPosition < 1)
    {
      currentPosition = Size - 1;
      currentItem = data[0];

      return currentItem;
    }

    var pos = random.Next(currentPosition);

    currentItem = data[pos];
    data[pos] = data[currentPosition];
    data[currentPosition] = currentItem;
    currentPosition--;

    return currentItem;
  }
}

  public class PlatformSpawner : MonoBehaviour {

  [SerializeField]
  Transform[] platforms;
  [SerializeField]
  Transform[] spawnPoints;

  private ShuffleBag spawnShuffleBag;

  private Transform lastPlatform;
  //private int lastSpawnpointIndex;

  public float spawnRate = 1f;

	// Use this for initialization
	void Start () {

    spawnShuffleBag = new ShuffleBag(spawnPoints.Length);
    foreach (Transform spawnPoint in spawnPoints)
    {
      spawnShuffleBag.Add(spawnPoint, 1);
    }

    StartCoroutine(SpawnPlatform());
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  IEnumerator SpawnPlatform()
  {
    if(platforms.Length == 0)
    {
      Debug.LogError("No 'platforms' for Platform spawner!");
      yield return false;
    }

    if (spawnPoints.Length == 0)
    {
      Debug.LogError("No 'spawnPoints' for Platform spawner!");
      yield return false;
    }

    if(lastPlatform != null)
    {
      Destroy(lastPlatform.gameObject);
    }

    // spawn a random platform in a random spawn
    Transform _platform = platforms[Random.Range(0, platforms.Length)];
    lastPlatform = Instantiate(_platform, spawnShuffleBag.Next());

    yield return new WaitForSeconds(1f / spawnRate);
    StartCoroutine(SpawnPlatform());
  }
}
