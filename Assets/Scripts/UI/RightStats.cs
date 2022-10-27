using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RightStats : MonoBehaviour
{
    public MakeMonsterSO moster;

    [SerializeField] TextMeshProUGUI life;
    [SerializeField] TextMeshProUGUI power;
    [SerializeField] TextMeshProUGUI intelligence;
    [SerializeField] TextMeshProUGUI skill;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI defense;

    // Start is called before the first frame update
    void Start()
    {
        life.text = moster.life.ToString() +" :Life";
        power.text = moster.power.ToString() + " :Pow" ;
        intelligence.text = moster.intelligence.ToString() + " :Int";
        skill.text = moster.skill.ToString() + " :Skill";
        speed.text = moster.speed.ToString() + " :Spd";
        defense.text = moster.defense.ToString() + " :Def";
    }
}
