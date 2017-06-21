using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
  public static Dictionary<string, BulletData> BulletReference { get; private set; }
  public static float Difficulty{ get; private set; }

  private TurretData[] turrets;
  private List<Turret> activeTurrets;
  private int currentDifficulty = 0;
  private int[] locationCounts = { 0, 0, 0, 0 };
  private Transform player;
  private Vector2 playerPosition;
  private GameObject bulletPrefab;
  private int difficultyCap;
  private float difficultyFloat;
  private float difficultyIncreaseRate;

  public float radius;

  private void Start()
  {
    string path = Path.Combine(Application.streamingAssetsPath, "Defines.json");
    string json = File.ReadAllText(path);
    var container = JsonUtility.FromJson<JsonContainer>(json);
    turrets = container.turrets;
    var bullets = container.bullets;
    BulletReference = new Dictionary<string, BulletData>();

    foreach (var bullet in bullets)
    {
      BulletReference.Add(bullet.name, bullet);
    }

    activeTurrets = new List<Turret>();
    player = GameObject.Find("Player").transform;
    bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
    difficultyCap = container.difficultyCapStart;
    difficultyFloat = difficultyCap;
    difficultyIncreaseRate = container.difficultyIncreaseRate;

    if (DebugTurret.debugStatic)
    {
      DebugTurret.registerTurretList(ref activeTurrets, turrets);
    }
  }

  private void Update()
  {
    playerPosition = player.position;

    if (!DebugTurret.debugStatic)
    {
      CheckDifficulty();
    }
    
    UpdateTurrets();
    difficultyFloat += (difficultyIncreaseRate * Time.deltaTime);
    difficultyCap = (int)difficultyFloat;
    Difficulty = currentDifficulty;
  }

  private void CheckDifficulty()
  {
    if(currentDifficulty <= 0.75f * difficultyCap)
    {
      float difficultyAllowance = difficultyCap - currentDifficulty;
      var availableTurrets = new List<TurretData>();

      foreach (var data in turrets)
      {
        if(data.difficulty <= difficultyAllowance)
        {
          availableTurrets.Add(data);
        }
      }

      if(availableTurrets.Count <= 0)
      {
        return;
      }

      int random = Random.Range(0, availableTurrets.Count);
      AddTurret(availableTurrets[random]);
      availableTurrets.Clear();
    }
  }

  private void AddTurret(TurretData data)
  {
    int nextLocation = 0;

    for (int i = 1; i < locationCounts.Length - 1; i++)
    {
      if(locationCounts[i] < locationCounts[nextLocation])
      {
        nextLocation = i;
      }
    }

    locationCounts[nextLocation]++;
    activeTurrets.Add(new Turret(data, (Turret.Location)nextLocation));
    currentDifficulty += data.difficulty;
  }

  private void RemoveTurret(Turret turret)
  {
    currentDifficulty -= turret.GetDifficulty();
    activeTurrets.Remove(turret);
  }

  private void UpdateTurrets()
  {
    var removeList = new List<Turret>();

    foreach (var turret in activeTurrets)
    {
      if (turret.GetTime(Time.deltaTime)) // Fire a bullet
      {
        bool empty = turret.Fire();
        Vector2 position = Position(turret);
        Vector2 aim = Aim(position, turret.AimType);
        if (turret.GetShotgun().amount == 0)
        {
          Fire(aim, position, turret.BulletName);
        }
        else
        {
          FireShotgun(aim, position, turret.BulletName, turret.GetShotgun());
        }
        
        if (empty)
        {
          removeList.Add(turret);
        }
      }
    }

    foreach (var turret in removeList)
    {
      RemoveTurret(turret);
    }
  }

  private Vector2 Position(Turret turret)
  {
    float angle;

    switch (turret.LocatedAt)
    {
      case Turret.Location.Top:
        angle = Random.Range(Mathf.PI / 4, 3 * Mathf.PI / 4);
        break;
      case Turret.Location.Right:
        angle = Random.Range(7 * Mathf.PI / 4, 9 * Mathf.PI / 4);
        if(angle >= Mathf.PI * 2)
        {
          angle -= Mathf.PI * 2;
        }
        break;
      case Turret.Location.Bottom:
        angle = Random.Range(5 * Mathf.PI / 4, 7 * Mathf.PI / 4);
        break;
      case Turret.Location.Left:
        angle = Random.Range(3 * Mathf.PI / 4, 5 * Mathf.PI / 4);
        break;
      default:
        angle = 0;
        break;
    }

    Vector2 position = new Vector2();
    position.x = radius * Mathf.Cos(angle);
    position.y = radius * Mathf.Sin(angle);
    position += PanelSize.centerPoint;
    return position;
  }

  private Vector2 Aim(Vector2 position, Turret.Aim aim)
  {
    Vector2 direction;

    switch (aim)
    {
      case Turret.Aim.Direct:
        direction = playerPosition - position;
        break;
      case Turret.Aim.Lead:
        Vector2 target = playerPosition + (Vector2)player.GetComponent<Rigidbody2D>().velocity;
        direction = target - position;
        break;
      case Turret.Aim.Random:
        Vector2 random = Random.insideUnitCircle * radius / 2;
        direction = random - position;
        break;
      default:
        direction = new Vector2();
        break;
    }

    return direction;
  }

  private void Fire(Vector2 vector, Vector2 position, string bullet)
  {
    Bullet.Direction direction;
    direction.x = vector.x;
    direction.y = vector.y;
    Bullet bulletModel = new Bullet(BulletReference[bullet], direction);
    GameObject bulletGO = Instantiate(bulletPrefab, position, Quaternion.identity);
    BulletBehavior bulletBehavior = bulletGO.GetComponent<BulletBehavior>();
    bulletBehavior.SetModel(bulletModel);
  }

  private void FireShotgun(Vector2 center, Vector2 position, string bullet, TurretData.Shotgun shotgunData)
  {
    float angle = shotgunData.spread / shotgunData.amount;
    float x = (center.x * Mathf.Cos(-shotgunData.spread / 2)) - (center.y * Mathf.Sin(-shotgunData.spread / 2));
    float y = (center.x * Mathf.Sin(-shotgunData.spread / 2)) + (center.y * Mathf.Cos(-shotgunData.spread / 2));
    center = new Vector2(x, y);

    for (int i = 0; i < shotgunData.amount; i++)
    {
      x = (center.x * Mathf.Cos(i * angle)) - (center.y * Mathf.Sin(i * angle));
      y = (center.x * Mathf.Sin(i * angle)) + (center.y * Mathf.Cos(i * angle));
      Fire(new Vector2(x, y), position, bullet);
    }
  }
}
