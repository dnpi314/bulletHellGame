using System;

[Serializable]
public class JsonContainer
{
  public BulletData[] bullets;
  public TurretData[] turrets;
  public int difficultyCapStart;
  public float difficultyIncreaseRate;
}
