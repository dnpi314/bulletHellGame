using UnityEngine;

public class Powerup : MonoBehaviour 
{
  private float timeLeft;
  private bool active = false;

  public float duration;
  public PowerupType type;
  public float lifespan;
  public int value;

  public enum PowerupType { ExtraLife, ScoreMultiplier }

  private void Update()
  {
    if (active)
    {
      duration -= Time.deltaTime;

      if(duration <= 0)
      {
        EndEffect();
      }
    }
    else
    {
      lifespan -= Time.deltaTime;

      if(lifespan <= 0)
      {
        Destroy(gameObject);
      }
    }
  }

  public void StartEffect()
  {
    switch (type)
    {
      case PowerupType.ExtraLife:
        Player.health++;
        Destroy(gameObject);
        break;
      case PowerupType.ScoreMultiplier:
        GameController.scoreMultiplier = 3;
        active = true;
        break;
      default:
        break;
    }
  }

  public void EndEffect()
  {
    switch (type)
    {
      case PowerupType.ScoreMultiplier:
        GameController.scoreMultiplier = 1;
        Destroy(gameObject);
        break;
      default:
        break;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    StartEffect();
    GetComponent<SpriteRenderer>().enabled = false;
    GetComponent<Collider2D>().enabled = false;
  }
}
