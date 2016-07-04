using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float smoothing = 5;
    public float restTime = 0.5f;
    public AudioClip chop1Audio;
    public AudioClip chop2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;
    public AudioClip soda1Audio;
    public AudioClip soda2Audio;
    public AudioClip food1Audio;
    public AudioClip food2Audio;

    private float restTimer = 0;
    [HideInInspector]public Vector2 targePos = new Vector2(1, 1);//[HideInInspector]表示不需要再组件里显示
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider; //自身的collider
    private Animator animator;

    void Awake()
    {
        rigidbody =GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //角色移动
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targePos, smoothing * Time.deltaTime));

        if (GameManager.Instance.food <= 0||GameManager.Instance.isEnd==true) return;

        restTimer += Time.deltaTime;
        if (restTimer < restTime) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h > 0)
        {
            v = 0;
        }

        if (h != 0 || v != 0)
        {
            GameManager.Instance.ReduceFood(1);
            //检测
            collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targePos,targePos+new Vector2(h,v)); //向目标方向发射一条射线,返回值类型RaycastHit2D
            collider.enabled = true;
            if (hit.transform == null)  //如果没有碰撞到东西
            {
                targePos += new Vector2(h, v);
                MusicManager.Instance.RandomPlay(step1Audio, step2Audio);
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "OutWall":
                        break;
                    case "Wall":
                        animator.SetTrigger("Attack");
                        MusicManager.Instance.RandomPlay(chop1Audio,chop2Audio);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        GameManager.Instance.AddFood(10);
                        targePos += new Vector2(h, v);
                        MusicManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);
                        MusicManager.Instance.RandomPlay(food1Audio, food2Audio);
                        break;
                    case "Soda":
                        GameManager.Instance.AddFood(20);
                        targePos += new Vector2(h, v);
                        MusicManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);
                        MusicManager.Instance.RandomPlay(soda1Audio,soda2Audio);
                        break;
                    case "Enemy":
                        break;
                }
            }
            GameManager.Instance.OnPlayerMove();

            restTimer = 0;
        }
    }
    public void TakeDamage(int loseFood)
    {
        GameManager.Instance.ReduceFood(loseFood);
        animator.SetTrigger("Damage");
    }
}
