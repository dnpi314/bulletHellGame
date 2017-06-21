public class Player
{
  public static int health;
  public static float score;

  public static void Reset(int h = 3)
  {
    health = h;
    score = 0;
  }
}
