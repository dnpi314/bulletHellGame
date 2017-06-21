using UnityEngine;
using UnityEngine.UI;

public class PanelSize : MonoBehaviour 
{
  public static Vector2 centerPoint;
  public static Rect playArea;

  public RectTransform sidebar;
  public RectTransform left;
  public RectTransform right;
  public RectTransform gameOver;
  public RectTransform highScore;

  private void Start()
  {
    float size = Screen.width - Screen.height;
    float letterboxSize = (size - sidebar.sizeDelta.x) / 2;
    sidebar.anchoredPosition = new Vector2(-letterboxSize, 0);
    left.sizeDelta = new Vector2(letterboxSize, 1);
    right.sizeDelta = new Vector2(letterboxSize, 1);
    var localCenter = new Vector3((Screen.width - sidebar.sizeDelta.x) / 2, Screen.height / 2, 0);
    float anchorRatio = localCenter.x / Screen.width;
    gameOver.anchorMin = new Vector2(anchorRatio, 0.5f);
    gameOver.anchorMax = new Vector2(anchorRatio, 0.5f);
    gameOver.anchoredPosition = new Vector2(0, 0);
    highScore.anchorMin = new Vector2(anchorRatio, 0.5f);
    highScore.anchorMax = new Vector2(anchorRatio, 0.5f);
    highScore.anchoredPosition = new Vector2(0, 0);
    centerPoint = Camera.main.ScreenToWorldPoint(localCenter);
    var area = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - letterboxSize, Screen.height, 0));
    playArea = new Rect(-area.x,-area.y, 2 * (area.x + centerPoint.x), 2 * area.y);
  }
}
