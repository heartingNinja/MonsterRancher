using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster")]
public class MakeMonsterSO : ScriptableObject
{
    public new string name;

    public int life; // hp
    public int power; //increase power attack
    public int intelligence; // increase magic attack
    public int skill; // hit chance of tec
    public int speed; // dodge chance
    public int defense; // ability to take a hit

    public Sprite attackClose;
    public bool attackCLosePowerBool;
    public int attackDamageClose;
    public int willCostClose;
    public int hitPercentClose; //make it between 1 -999, make miss attack from random range. S-D rating s= 100%, D = 60% each letter 10% less

    public Sprite attackCloseMid;
    public bool attackCloseMidPowerBool;
    public int attackDamageCloseMid;
    public int willCostCloseMid;
    public int hitPercentCloseMid;

    public Sprite attackFarMid;
    public bool attackFarMidPowerBool;
    public int attackDamageFarMid;
    public int willCostFarMid;
    public int hitPercentFarMid;

    public Sprite attackFar;
    public bool attackFarPowerBool;
    public int attackDamageFar;
    public int willCostFar;
    public int hitPercentFar;

}
