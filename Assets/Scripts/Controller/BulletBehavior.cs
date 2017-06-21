using UnityEngine;

public class BulletBehavior : MonoBehaviour 
{
  private Vector2 moveDirection;
  private Bullet bulletModel;
  private Bullet.Move moveType;
  private Transform player;
  private Rigidbody2D rigidBody;
  private Animator animator;

  private void Start()
  {
    try
    {
      player = GameObject.Find("Player").transform;
    }
    catch (System.NullReferenceException)
    {
      Destroy(gameObject);
    }

    if(bulletModel.GetExplosive().amount != 0)
    {
      var sprite = GetComponent<SpriteRenderer>();
      var sprites = Resources.LoadAll<Sprite>("Sprites/Placeholders");
      sprite.sprite = sprites[1];
      animator = gameObject.AddComponent<Animator>();
      animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/Explosive");
      animator.SetFloat("TimeLeft", bulletModel.GetLifespan());
    }
    
    rigidBody = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    if (!bulletModel.IsAlive(Time.deltaTime)) //bullet expired
    {
      if (bulletModel.GetExplosive().amount != 0) //is explosive
      {
        var warhead = bulletModel.GetExplosive();
        var bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

        for (int i = 0; i < warhead.amount; i++)
        {
          float angle = i * 2 * Mathf.PI / warhead.amount;
          Bullet.Direction direction;
          direction.x = Mathf.Cos(angle);
          direction.y = Mathf.Sin(angle);
          Bullet bulletModel = new Bullet(TurretController.BulletReference[warhead.bulletName], direction);
          GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
          BulletBehavior bulletBehavior = bulletGO.GetComponent<BulletBehavior>();
          bulletBehavior.SetModel(bulletModel);
        }
      }

      Destroy(gameObject);
    }

    if(bulletModel.GetExplosive().amount != 0)
    {
      animator.SetFloat("TimeLeft", animator.GetFloat("TimeLeft") - Time.deltaTime);
    }

    if (!bulletModel.MoveActive)
    {
      moveType = Bullet.Move.Straight;
    }
    else
    {
      moveType = bulletModel.MoveType;
    }

    switch (moveType)
    {
      case Bullet.Move.Curve:
        bulletModel.UpdateDirection(Time.deltaTime);
        break;
      case Bullet.Move.Home:
        Vector3 relativePosition = player.position - transform.position;
        relativePosition.Normalize();
        bulletModel.UpdateDirection(Time.deltaTime,ConvertVector(relativePosition));
        break;
      default:
        break;
    }
    moveDirection = ConvertVector(bulletModel.UpdateDirection());
    rigidBody.velocity = moveDirection * bulletModel.Speed;
  }

  private Bullet.Direction ConvertVector(Vector3 vector)
  {
    Bullet.Direction direction;
    direction.x = vector.x;
    direction.y = vector.y;
    return direction;
  }
  private Vector2 ConvertVector(Bullet.Direction direction)
  {
    Vector2 vector;
    vector.x = direction.x;
    vector.y = direction.y;
    return vector;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Destroy(gameObject);
  }

  public void SetModel(Bullet bullet)
  {
    bulletModel = bullet;
    moveType = bullet.MoveType;
  }
}
