using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeftStats : MonoBehaviour
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
        life.text = "Life: " + moster.life.ToString();
        power.text = "Pow: " + moster.power.ToString();
        intelligence.text = "Int: " + moster.intelligence.ToString();
        skill.text ="Skill: " + moster.skill.ToString();
        speed.text = "Spd: " + moster.speed.ToString();
        defense.text ="Def: "+ moster.defense.ToString();
    }
   
}
