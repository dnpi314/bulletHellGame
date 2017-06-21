using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour 
{
  public GameObject MainMenu;
  public GameObject highScoreList;
  public GameObject inputField;

	public void playGame()
  {
    ApplicationManager.LoadGame();
  }

  public void menu()
  {
    ApplicationManager.LoadMainMenu();
  }

  public void quit()
  {
    Application.Quit();
  }

  public void nameInput()
  {
    string name = inputField.GetComponent<InputField>().text;
    ApplicationManager.UpdateHighScoreList((int)Player.score, name);
    ApplicationManager.SaveHighScoreList();
    GameController.ReturnToGameOver();
  }

  public void showHighScores()
  {
    MainMenu.SetActive(false);
    highScoreList.SetActive(true);
    var txt = highScoreList.transform.GetChild(1).GetComponent<Text>();
    txt.text = ApplicationManager.GetHighScoreList();
    txt.lineSpacing = 1.25f;
  }
}
