using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public MonsterHand[] MonsterHands;
    public MonsterEyes[] MonsterEyes;
    public float MonsterAttackDuration;

    public AudioClip[] MonsterWinSFX, MonsterSpawnSFX, MonsterDeadSFX;

    public float Hand1Time, Hand2Time, Hand3Time, Hand4Time;
    public float BlinkChanceMonster, BlinkChanceSafe;

    private float AttackTime=0;
    private bool MonsterAttacking = false;
    private UIManager UIM;
    private ScoreManager SM;

    void Start()
    {
        UIM = FindObjectOfType<UIManager>();
        SM = FindObjectOfType<ScoreManager>();

        //Randomizing order of hands
        ShuffleHands();

        //StartMonsterAttack(); //testing
    }

    void Update()
    {
        if (!UIM.IsAnyWindowOpen())  //If no window is open
        {
            if(MonsterAttacking)  //While monster is attacking
            {
                AttackTime += Time.deltaTime;
                AttackTime = Mathf.Min(AttackTime, MonsterAttackDuration);   //increase the attack timer and shor a correct ammount of hands based on it

                if (AttackTime > Hand1Time * MonsterAttackDuration)
                    MonsterHands[0].Show();
                if (AttackTime > Hand2Time * MonsterAttackDuration)
                    MonsterHands[1].Show();
                if (AttackTime > Hand3Time * MonsterAttackDuration)
                    MonsterHands[2].Show();
                if (AttackTime > Hand4Time * MonsterAttackDuration)
                    MonsterHands[3].Show();

                if(!MonsterBlinking()) //randomly show monster eyes 
                {
                    if (Random.value < BlinkChanceMonster * (AttackTime / MonsterAttackDuration))
                        Blink();
                }
            }
            else //eyes can appear with small probablility even if no monster is attacking
            {
                if (!MonsterBlinking())
                {
                    if (Random.value < BlinkChanceSafe)
                        Blink();
                }
            }

            if (MonsterAttackSuccessful())    //in case attack timer reaches the max value
            {
                MonsterWin();
                StartMonsterAttack();
            }
                
        }
    }

    private void MonsterWin() //Things that happen once monster's attack succedes
    {
        SM.AddPoints(-20);  //ADD CHILD DEATH ANIMATION HERE
        ShuffleHands();
        StopMonsterAttack();

        SoundController.PlaySound(MonsterWinSFX[Random.Range(0, MonsterWinSFX.Length)]);
        SM.LoseLife();

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

        SoundController.PlaySound(MonsterSpawnSFX[Random.Range(0, MonsterSpawnSFX.Length)]);

        AttackTime = 0;
    }

    public void StopMonsterAttack()
    {
        if (MonsterAttacking)
        {
            MonsterAttacking = false;

            SoundController.PlaySound(MonsterDeadSFX[Random.Range(0, MonsterDeadSFX.Length)]);

            if (AttackTime < MonsterAttackDuration / 10)  //give points based on the time player took to stop the monster
                SM.AddPoints(5);
            else if (AttackTime < MonsterAttackDuration / 4)
                SM.AddPoints(4);
            else if (AttackTime < MonsterAttackDuration / 2)
                SM.AddPoints(3);
            else if (AttackTime < MonsterAttackDuration)
                SM.AddPoints(2);
        }
        AttackTime = 0;

        ShuffleHands();
        foreach (var x in MonsterHands)   //hide monster hands
            x.Hide();
    }

    private bool MonsterBlinking()   //simple check whether there are monster eyes visible
    {
        return MonsterEyes[0].IsBlinking() || MonsterEyes[1].IsBlinking() || MonsterEyes[2].IsBlinking()|| MonsterEyes[3].IsBlinking() || MonsterEyes[4].IsBlinking() || MonsterEyes[5].IsBlinking();
    }

    private void Blink()   //randomly choose monster eyes to blink
    {
        int x = Random.Range(0, 6);
        MonsterEyes[x].Blink();
    }

    private void ShuffleHands()  //randomly change hands order
    {
        MonsterHand[] NewOrder = new MonsterHand[4];
        float[] order = new float[4] { Random.value, Random.value, Random.value, Random.value };

        for (int i = 0; i < 4; i++)
        {
            int min = 0;
            for (int j = 0; j < 4; j++)
            {
                if (order[j] < order[min])
                    min = j;
            }
            order[min] = 1;

            NewOrder[i] = MonsterHands[min];
        }
        for (int i = 0; i < 4; i++)
            MonsterHands[i] = NewOrder[i];
    }
}
