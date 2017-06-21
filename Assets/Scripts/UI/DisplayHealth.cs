using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour 
{
  private Text health;

  private void Start()
  {
    health = GetComponent<Text>();
  }


  private void Update()
  {
    health.text = Player.health + "";
  }
}
