using UnityEngine;

public class PowerupController : MonoBehaviour 
{
  public float powerupFrequency;
  public GameObject[] powerups;

  private float timer;
  private Rect spawnArea;
  private float margin = 0.25f;

  private void Start()
  {
    spawnArea = PanelSize.playArea;
    spawnArea.xMin += margin;
    spawnArea.xMax -= margin;
    spawnArea.yMin += margin;
    spawnArea.yMax -= margin;
    timer = Random.Range(5, 10) * powerupFrequency;
  }

  private void Update()
  {
    timer -= Time.deltaTime;
    
    if(timer <= 0)
    {
      SpawnPowerup();
    }
  }

  private void SpawnPowerup()
  {
    var spawnPoint = new Vector3();
    spawnPoint.x = Random.Range(spawnArea.xMin, spawnArea.xMax);
    spawnPoint.y = Random.Range(spawnArea.yMin, spawnArea.yMax);
    int powerType = Random.Range(0, powerups.Length);
    Instantiate(powerups[powerType], spawnPoint, Quaternion.identity);
    int value = powerups[powerType].GetComponent<Powerup>().value;
    timer = Random.Range(value, 2 * value) * powerupFrequency;
  }
}
