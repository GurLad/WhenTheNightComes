using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static LevelGenerator;

public class GameController : MonoBehaviour
{
    // New way
    [HideInInspector]
    public List<WaveData> Waves;
    [HideInInspector]
    public List<MonsterController> Monsters = new List<MonsterController>();
    //// Old way
    //[HideInInspector]
    //public float MonsterAttackInterval;
    //[HideInInspector]
    //public float MonsterAttackIntervalDecrement;
    //[HideInInspector]
    //public string LevelData;
    //[HideInInspector]
    //public List<List<MonsterController>> Monsters = new List<List<MonsterController>>();
    private float count;
    private int currentWave = 0;
    //// Old way
    //private int currentIndex;
    //private float currentInterval
    //{
    //    get
    //    {
    //        return MonsterAttackInterval - UIClock.Current.CurrentHour() * MonsterAttackIntervalDecrement;
    //    }
    //}

    private void Update()
    {
        if (currentWave >= Waves.Count)
        {
            return;
        }
        count += Time.deltaTime;
        if (count >= Waves[currentWave].Length)
        {
            count -= Waves[currentWave++].Length;
            if (currentWave >= Waves.Count)
            {
                return;
            }
            if (Waves[currentWave].SpawnString != "")
            {
                string[] monsters = Waves[currentWave].SpawnString.Split(',');
                string[] deadAdditions = Waves[currentWave].DeadAdditions.Split(',');
                int deadCount = 0;
                foreach (string monster in monsters)
                {
                    int id = int.Parse(monster);
                    MonsterController selected = Monsters.Find(a => a.id == id);
                    if (!selected.MonsterAttackSuccessful())
                    {
                        selected.StartMonsterAttack();
                    }
                    else
                    {
                        deadCount++;
                    }
                }
                if (Waves[currentWave].DeadAdditions != "")
                { 
                    for (int i = 0; i < Mathf.Min(deadAdditions.Length, deadCount); i++)
                    {
                        int id = int.Parse(deadAdditions[i]);
                        MonsterController selected = Monsters.Find(a => a.id == id);
                        if (!selected.MonsterAttackSuccessful())
                        {
                            selected.StartMonsterAttack();
                        }
                        else
                        {
                            deadCount++;
                        }
                    }
                }
            }
        }
    }

    private void OldWay()
    {
        //count += Time.deltaTime;
        //if (count >= currentInterval)
        //{
        //    count -= currentInterval;
        //    int index = int.Parse(LevelData[currentIndex].ToString());
        //    currentIndex = (currentIndex + 1) % LevelData.Length;
        //    if (index > 0)
        //    {
        //        index--;
        //        List<MonsterController> available = Monsters[index].FindAll(a => !a.MonsterAttackSuccessful() && !a.IsMonsterAttacking());
        //        if (available.Count > 0)
        //        {
        //            MonsterController selected = available[Random.Range(0, available.Count)];
        //            selected.StartMonsterAttack();
        //        }
        //    }
        //}
    }
}
