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

    [Header("Minimap")]
    public Renderer MinimapMain;
    public Renderer MinimapEyes;
    public Color MinimapSafe;
    public Color MinimapBeginDanger;
    public Color MinimapDanger;
    public Color MinimapDead;
    private Color minimapBaseEyes;
    private Color minimapBaseMain;

    private float AttackTime=0;
    private bool MonsterAttacking = false;
    private UIManager UIM;
    private ScoreManager SM;

    private bool monsterAttackSuccessful = false;

    void Start()
    {
        UIM = FindObjectOfType<UIManager>();
        SM = FindObjectOfType<ScoreManager>();

        //Randomizing order of hands
        ShuffleHands();

        //StartMonsterAttack(); //testing

        // Minimap init
        MinimapMain.material = Instantiate(MinimapMain.material);
        MinimapEyes.material = Instantiate(MinimapEyes.material);
        minimapBaseEyes = MinimapEyes.material.color;
        minimapBaseMain = MinimapSafe;
    }

    void Update()
    {
        if (!UIM.IsAnyWindowOpen())  //If no window is open
        {
            if (MonsterAttackSuccessful())
            {
                // Minimap
                MinimapMain.material.color = MinimapDead;
                return;
            }
            if (MonsterAttacking)  //While monster is attacking
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

                // Minimap
                float percent = MonsterAttackProgress() / MonsterAttackDuration;
                minimapBaseMain = new Color(Mathf.Lerp(MinimapBeginDanger.r, MinimapDanger.r, percent), Mathf.Lerp(MinimapBeginDanger.g, MinimapDanger.g, percent), Mathf.Lerp(MinimapBeginDanger.b, MinimapDanger.b, percent));
                MinimapMain.material.color = minimapBaseMain;
                minimapBaseEyes.a = percent;
                MinimapEyes.material.color = minimapBaseEyes;
            }
            else //eyes can appear with small probablility even if no monster is attacking
            {
                if (!MonsterBlinking())
                {
                    if (Random.value < BlinkChanceSafe)
                        Blink();
                }

                // Minimap
                MinimapMain.material.color = MinimapSafe;
                minimapBaseEyes.a = 0;
                MinimapEyes.material.color = minimapBaseEyes;
            }

            if (MonsterAttackSuccessful())    //in case attack timer reaches the max value
            {
                MonsterWin();
                StartMonsterAttack();

                // Minimap
                MinimapMain.material.color = MinimapDead;
            }
                
        }
    }

    private void MonsterWin() //Things that happen once monster's attack succedes
    {
        monsterAttackSuccessful = true;
        SM.AddPoints(-20);  //ADD CHILD DEATH ANIMATION HERE
        ShuffleHands();
        StopMonsterAttack(true);

        SoundController.Play3DSound(MonsterWinSFX[Random.Range(0, MonsterWinSFX.Length)], gameObject, 0.5f);
        SM.LoseLife();

    }
    public bool IsMonsterAttacking()
    {
        return MonsterAttacking;
    }

    public bool MonsterAttackSuccessful()
    {
        return monsterAttackSuccessful || AttackTime >= MonsterAttackDuration-0.01f;
    }
    public float MonsterAttackProgress() //time since monster started attacking
    {
        return AttackTime;
    }

    public void StartMonsterAttack()
    {
        MonsterAttacking = true;

        SoundController.Play3DSound(MonsterSpawnSFX[Random.Range(0, MonsterSpawnSFX.Length)], gameObject, 0.3f);

        AttackTime = 0;
    }

    public void StopMonsterAttack(bool forceStop = false)
    {
        if (!forceStop && MonsterAttackSuccessful())
        {
            // Can't stop
            return;
        }
        if (MonsterAttacking)
        {
            MonsterAttacking = false;

            SoundController.Play3DSound(MonsterDeadSFX[Random.Range(0, MonsterDeadSFX.Length)], gameObject);

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
