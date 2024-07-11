using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster_", menuName = "Encyclopedia/Data/Monster", order = 1)]
public class MonsterData : ScriptableObject
{
    public int monsterId;
    public string monsterName;
    public bool unlock = false;
}
