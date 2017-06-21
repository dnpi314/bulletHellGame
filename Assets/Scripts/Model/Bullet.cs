using System;
using System.Collections.Generic;

public class Bullet
{
  private BulletData data;
  private float time;
  private bool moveActive;
  private Direction direction;

  public Move MoveType { get; private set; }
  public float Speed { get; private set; }

  public bool MoveActive
  {
    get { return moveActive; }
    private set { moveActive = value; }
  }

  public struct Direction
  {
    public float x;
    public float y;
  }

  public enum Move { Straight, Curve, Home }

  private void Normalize()
  {
    float magnitude = (float)Math.Sqrt((direction.x * direction.x) + (direction.y * direction.y));
    direction.x /= magnitude;
    direction.y /= magnitude;
  }

  public bool IsAlive(float time)
  {
    this.time += time;
    if(this.time >= data.delay)
    {
      moveActive = true;
    }
    if(this.time >= data.lifeSpan)
    {
      return false;
    }
    return true;
  }

  public float GetLifespan()
  {
    return data.lifeSpan;
  }

  public BulletData.Explosive GetExplosive()
  {
    return data.warhead;
  }

  public Direction UpdateDirection() //default
  {
    Normalize();
    return direction;
  }
  public Direction UpdateDirection(float time) //moveType == Curve
  {
    float x = (float)((direction.x * Math.Cos(data.strength * time)) - (direction.y * Math.Sin(data.strength * time)));
    float y = (float)((direction.x * Math.Sin(data.strength * time)) + (direction.y * Math.Cos(data.strength * time)));
    direction.x = x;
    direction.y = y;
    Normalize();
    return direction;
  }
  public Direction UpdateDirection(float time, Direction target) //moveType == Home
  {
    direction.x += target.x * time * data.strength;
    direction.y += target.y * time * data.strength;
    Normalize();
    return direction;
  }

  public Bullet(BulletData data, Direction direction)
  {
    this.data = data;
    this.direction = direction;
    Speed = data.speed;
    MoveType = (Move)data.moveType;
    moveActive = false;
    time = 0f;
  }
}

[Serializable]
public struct BulletData
{
  public string name;
  public float lifeSpan;
  public float speed;
  public int moveType;
  public float strength;
  public float delay;
  public Explosive warhead;

  [Serializable]
  public struct Explosive
  {
    public string bulletName;
    public int amount;
  }
}
