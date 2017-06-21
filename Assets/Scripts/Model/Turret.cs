using System;
using System.Collections.Generic;

public class Turret
{
  private float time;
  private TurretData data;

  public Aim AimType { get; private set; }
  public Location LocatedAt { get; private set; }
  public string BulletName { get; private set; }

  public enum Aim { Direct, Lead, Random}
  public enum Location { Top, Right, Bottom, Left }
  
  public bool GetTime(float time)
  {
    this.time += time;

    if(this.time >= data.cooldown)
    {
      return true;
    }

    return false;
  }

  public bool Fire()
  {
    data.clipSize--;
    time = 0f;

    if(data.clipSize <= 0)
    {
      return true;
    }

    return false;
  }

  public int GetDifficulty()
  {
    return data.difficulty;
  }

  public TurretData.Shotgun GetShotgun()
  {
    return data.shotgun;
  }

  public Turret(TurretData data, Location location)
  {
    this.data = data;
    AimType = (Aim)data.aimType;
    LocatedAt = location;
    BulletName = data.bulletName;
    time = 0f;
  }
}

[Serializable]
public struct TurretData
{
  public string name;
  public string bulletName;
  public float cooldown;
  public int aimType;
  public int clipSize;
  public int difficulty;
  public Shotgun shotgun;

  [Serializable]
  public struct Shotgun
  {
    public int amount;
    public float spread;
  }
}