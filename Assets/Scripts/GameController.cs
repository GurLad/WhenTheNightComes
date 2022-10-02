using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public float MonsterAttackInterval;
    [HideInInspector]
    public float MonsterAttackIntervalDecrement;
    [HideInInspector]
    public string LevelData;
    [HideInInspector]
    public List<List<MonsterController>> Monsters = new List<List<MonsterController>>();
    private float count;
    private int currentIndex;
    private float currentInterval
    {
        get
        {
            return MonsterAttackInterval - UIClock.Current.CurrentHour() * MonsterAttackIntervalDecrement;
        }
    }

    private void Update()
    {
        count += Time.deltaTime;
        if (count >= currentInterval)
        {
            count -= currentInterval;
            int index = int.Parse(LevelData[currentIndex].ToString());
            currentIndex = (currentIndex + 1) % LevelData.Length;
            if (index > 0)
            {
                index--;
                List<MonsterController> available = Monsters[index].FindAll(a => !a.IsMonsterAttacking());
                if (available.Count > 0)
                {
                    MonsterController selected = available[Random.Range(0, available.Count)];
                    selected.StartMonsterAttack();
                }
            }
        }
    }
}
