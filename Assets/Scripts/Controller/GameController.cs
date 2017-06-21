using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
  private static GameController _Instance;

  private bool gameActive;
  private GameObject player;
  private GameObject gameOverScreen;
  private GameObject nameInput;

  private void Start()
  {
    Player.Reset();
    _Instance = this;
    gameActive = true;
    player = GameObject.Find("Player");
    gameOverScreen = GameObject.Find("GameOverScreen");
    nameInput = GameObject.Find("NameInput");
    gameOverScreen.SetActive(false);
    nameInput.SetActive(false);
  }

  private void Update()
  {
    if(gameActive)
      Player.score += TurretController.Difficulty * Time.deltaTime;
  }

  private void GameOver()
  {
    gameActive = false;
    GetComponent<TurretController>().enabled = false;
    player.SetActive(false);

    if (ApplicationManager.NewHighScore((int)Player.score))
    {
      nameInput.SetActive(true);
    }
    else
    {
      gameOverScreen.SetActive(true);
    }
  }

  public static void ReturnToGameOver()
  {
    _Instance.gameOverScreen.SetActive(true);
    _Instance.nameInput.SetActive(false);
  }

  public static void TakeDamage()
  {
    Player.health--;
    if(Player.health <= 0)
    {
      _Instance.GameOver();
    }
  }
}
