using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour 
{
  private Text score;

  private void Start()
  {
    score = GetComponent<Text>();
  }

  private void Update()
  {
    score.text = (int)Player.score+ "";
  }
}
