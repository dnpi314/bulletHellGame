using System.Collections.Generic;
using UnityEngine;

public class DebugTurret : MonoBehaviour 
{
  public static bool debugStatic;
  public static string turretNameStatic;

  public bool debug;
  public string turretName;

  private void Start()
  {
    debugStatic = debug;
    turretNameStatic = turretName;
  }

  public static void registerTurretList(ref List<Turret> activeTurrets, TurretData[] data)
  {
    for (int i = 0; i < data.Length; i++)
    {
      if (data[i].name.Equals(turretNameStatic))
      {
        var debugTurret = new Turret(data[i], Turret.Location.Top);
        activeTurrets.Add(debugTurret);
        return;
      }
    }
  }
}
