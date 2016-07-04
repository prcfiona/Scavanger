﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float smoothing=3;
    public int loseFood = 10;
    public AudioClip attackAudio;

    private Vector2 targetPosition;
    private Transform player;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        GameManager.Instance.enemyList.Add(this);
    }

    void Update()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
    }

    public void Move()
    {
        float x = 0, y = 0;
        Vector2 offset = player.position - transform.position;
        if(offset.magnitude<1.1)    //player和enemy距离
        {
            //攻击
            anim.SetTrigger("Attack");
            MusicManager.Instance.RandomPlay(attackAudio);
            player.SendMessage("TakeDamage", loseFood);
        }
        else
        {
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                //按照y轴移动
                if (offset.y < 0)
                {
                    y = -1;
                }
                else
                {
                    y = 1;
                }
            }
            else
            {
                //按照x轴移动
                if (offset.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            //设置目标位置前先做检测
            collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y)); //向目标方向发射一条射线,返回值类型RaycastHit2D
            collider.enabled = true;
            if (hit.transform == null)
            {
                targetPosition += new Vector2(x, y);
            }
            else
            {
                if(hit.collider.tag=="Food"|| hit.collider.tag == "Soda")
                {
                    targetPosition += new Vector2(x, y);  
                }
            }
        }
    }
}
