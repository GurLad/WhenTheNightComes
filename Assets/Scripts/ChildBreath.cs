using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBreath : MonoBehaviour
{
    public GameObject Blanket;
    public Vector2 SizeRange;
    public Vector2 SpeedRange;
    public float DeadSize;
    private float count;
    private float size;
    private float speed;
    private Vector3 baseScale;
    private bool dead = false;

    private void Start()
    {
        size = Random.Range(SizeRange.x, SizeRange.y);
        speed = Random.Range(SpeedRange.x, SpeedRange.y);
        count = Random.Range(0, 2 * Mathf.PI / speed);
        baseScale = Blanket.transform.localScale;
    }

    private void Update()
    {
        if (!dead)
        {
            count += Time.deltaTime;
            Blanket.transform.localScale = baseScale + new Vector3(0, 1, 0) * Mathf.Sin(count * speed) * size;
        }
        else if (count < 1)
        {
            count += Time.deltaTime;
            count = Mathf.Min(count, 1);
            Vector3 temp = new Vector3(baseScale.x, baseScale.y, baseScale.z);
            temp.y = Mathf.Lerp(DeadSize * baseScale.y, baseScale.y, 1 - count);
            Blanket.transform.localScale = temp;
        }
    }

    public void Kill()
    {
        dead = true;
        count = 0;
    }
}
