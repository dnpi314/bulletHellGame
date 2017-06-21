using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour 
{
  private static HighScore[] highScoreList;

  public int highScoreListSize = 5;
  public static int scoreListSize;

  private void Awake()
  {
    DontDestroyOnLoad(gameObject);
    scoreListSize = highScoreListSize;
    LoadHighScoreList();
    LoadMainMenu();
  }

  private void LoadHighScoreList()
  {
    string path = Path.Combine(Application.streamingAssetsPath, "HighScores.json");
    string json = File.ReadAllText(path);
    if (json.Equals(""))
    {
      highScoreList = new HighScore[0];
      return;
    }
    highScoreList = JsonUtility.FromJson<HighScoreList>(json).highScores;
    Array.Sort(highScoreList, delegate (HighScore score1, HighScore score2) { return score1.score.CompareTo(score2.score); });
  }

  public static void UpdateHighScoreList(int score, string name)
  {
    if(highScoreList.Length < scoreListSize)
    {
      var tempArray = new HighScore[highScoreList.Length + 1];

      for (int i = 0; i < highScoreList.Length; i++)
      {
        tempArray[i] = highScoreList[i];
      }

      tempArray[tempArray.Length - 1] = new HighScore(name, score);
      highScoreList = tempArray;
      Array.Sort(highScoreList, delegate (HighScore score1, HighScore score2) { return score1.score.CompareTo(score2.score); });
      return;
    }

    for (int i = 0; i < highScoreList.Length; i++)
    {
      if(score > highScoreList[i].score)
      {
        for (int j = 0; j < i; j++)
        {
          highScoreList[j] = highScoreList[j + 1];
        }

        highScoreList[i] = new HighScore(name, score);
        Array.Sort(highScoreList, delegate (HighScore score1, HighScore score2) { return score1.score.CompareTo(score2.score); });
        return;
      }
    }
  }

  public static bool NewHighScore(int score)
  {
    if (highScoreList.Length < scoreListSize)
    {
      return true;
    }

    for (int i = 0; i < highScoreList.Length; i++)
    {
      if (score > highScoreList[i].score)
      {
        return true;
      }
    }

    return false;
  }

  public static string GetHighScoreList()
  {
    string formattedList = "";

    for (int i = highScoreList.Length - 1; i >= 0; i--)
    {
      formattedList += string.Format("{0}: {1}\n", highScoreList[i].name, highScoreList[i].score);
    }

    return formattedList;
  }

  public static void SaveHighScoreList()
  {
    string path = Path.Combine(Application.streamingAssetsPath, "HighScores.json");
    var container = new HighScoreList(highScoreList);
    string json = JsonUtility.ToJson(container);
    File.WriteAllText(path, json);
  }

  public static void LoadMainMenu()
  {
    SceneManager.LoadScene("main");
  }

  public static void LoadGame()
  {
    SceneManager.LoadScene("game");
  }
}
