using UnityEngine;
using System.Collections;

public class Walls : MonoBehaviour
{
    public float hp = 2;
    public Sprite damageSprite;

    //受伤
    public void TakeDamage()
    {
        hp -= 1;
        GetComponent<SpriteRenderer>().sprite = damageSprite;   //把受伤后的图片调用在Sprite Renderer里
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
