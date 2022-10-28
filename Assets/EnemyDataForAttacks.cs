using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDataForAttacks : MonoBehaviour
{
    [SerializeField] MakeMonsterSO thisMonster;

    [SerializeField] TextMeshProUGUI willCostCloseText;
    [SerializeField] Image closeAttackSprite;

    [SerializeField] TextMeshProUGUI willCostMidCloseText;
    [SerializeField] Image midCloseAttackSprite;

    [SerializeField] TextMeshProUGUI willCostMidFarText;
    [SerializeField] Image midFarAttackSprite;

    [SerializeField] TextMeshProUGUI willCostFarText;
    [SerializeField] Image farAttackSprite;

    private void Awake() // in future these will need to be updated when attacks box changes
    {
        willCostCloseText.text = thisMonster.willCostClose.ToString();
        closeAttackSprite.sprite = thisMonster.attackClose;

        willCostMidCloseText.text = thisMonster.willCostCloseMid.ToString();
        midCloseAttackSprite.sprite = thisMonster.attackCloseMid;

        willCostMidFarText.text = thisMonster.willCostFarMid.ToString();
        midFarAttackSprite.sprite = thisMonster.attackFarMid;

        willCostFarText.text = thisMonster.willCostFar.ToString();
        farAttackSprite.sprite = thisMonster.attackFar;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
