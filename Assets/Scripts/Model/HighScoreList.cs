using System;

public class HighScoreList
{
  public HighScore[] highScores;

  public HighScoreList(HighScore[] highScores)
  {
    this.highScores = highScores;
  }
}

[Serializable]
public class HighScore
{
  public string name;
  public int score;

  public HighScore(string name, int score)
  {
    this.name = name;
    this.score = score;
  }
}
