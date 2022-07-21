using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/New Config")]
public class Config : ScriptableObject
{
    public int maxPatrollPointsCount;
    public float minDistanceBetweenPoints;
    public float characterMoveSpeed;

    public static Config Instance;
}
