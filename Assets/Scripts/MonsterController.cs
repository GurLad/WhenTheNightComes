using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public MonsterHand[] MonsterHands;
    public MonsterEyes[] MonsterEyes;
    public float MonsterAttackDuration;

    public float Hand1Time, Hand2Time, Hand3Time, Hand4Time;
    public float BlinkChanceMonster, BlinkChanceSafe;

    private float AttackTime=0;
    private bool MonsterAttacking = false;
    private SettingsController SC;

    void Start()
    {
        SC = FindObjectOfType<SettingsController>();
        //Randomize monster hand, monster eyes arrays

        StartMonsterAttack(); //testing
    }

    void Update()
    {
        if (!SC.SettingsActive)
        {
            if(MonsterAttacking)
            {
                AttackTime += Time.deltaTime;
                AttackTime = Mathf.Min(AttackTime, MonsterAttackDuration);

                if (AttackTime > Hand1Time * MonsterAttackDuration)
                    MonsterHands[0].Show();
                if (AttackTime > Hand2Time * MonsterAttackDuration)
                    MonsterHands[1].Show();
                if (AttackTime > Hand3Time * MonsterAttackDuration)
                    MonsterHands[2].Show();
                if (AttackTime > Hand4Time * MonsterAttackDuration)
                    MonsterHands[3].Show();

                if(!MonsterBlinking())
                {
                    if (Random.value < BlinkChanceMonster * (AttackTime / MonsterAttackDuration))
                        Blink();
                }
            }
            else
            {
                if (!MonsterBlinking())
                {
                    if (Random.value < BlinkChanceSafe)
                        Blink();
                }
            }

            if(MonsterAttackSuccessful())
            {
                StopMonsterAttack();
                StartMonsterAttack();
            }
                
        }
    }

    public bool IsMonsterAttacking()
    {
        return MonsterAttacking;
    }

    public bool MonsterAttackSuccessful()
    {
        return AttackTime >= MonsterAttackDuration-0.01f;
    }
    public float MonsterAttackProgress() //time since monster started attacking
    {
        return AttackTime;
    }

    public void StartMonsterAttack()
    {
        MonsterAttacking = true;
        AttackTime = 0;
    }

    public void StopMonsterAttack()
    {
        MonsterAttacking = false;
        AttackTime = 0;

        foreach (var x in MonsterHands)
            x.Hide();
    }

    private bool MonsterBlinking()
    {
        return MonsterEyes[0].IsBlinking() || MonsterEyes[1].IsBlinking() || MonsterEyes[2].IsBlinking()|| MonsterEyes[3].IsBlinking() || MonsterEyes[4].IsBlinking() || MonsterEyes[5].IsBlinking();
    }

    private void Blink()
    {
        int x = Random.Range(0, 6);
        MonsterEyes[x].Blink();
    }
}
