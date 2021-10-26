using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeadManager : MonoBehaviour
{
    //单例模式
    private static HeadManager _instance;
    public static HeadManager Instance { get { return _instance; } }

    //0.3秒刷新一次蛇的位置
    public float speed = 0.30f;
    private Vector3 headPos;
    private int step=25;
    private int x;
    private int y;
    //生成食物的物体
    public GameObject foodMaker;
    //画布
    public Transform canves;
    //蛇身预制体
    public GameObject bodyPrefab;
    //蛇身的sprite
    public Sprite[] bodySprites = new Sprite[2];
    //存放蛇身的位置信息
    private List<Transform> bodyList = new List<Transform>();
    public int score = 0;
    public int length = 0;
    //死亡特效
    public ParticleSystem dieEffect;
    //音效
    public AudioClip[] audioClips;
    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        //加载用户选择的皮肤，默认为小黄人皮肤
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
        //默认向右移动
        x = step; y = 0;
        InvokeRepeating("Move", 0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        //判断当前是否处于暂停状态
        if (Time.timeScale != 0)
        {
            //当用户按下空格，加速
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //停止之前的invoke
                CancelInvoke();
                //重新invokeRepeating，缩短调用的时间，达到加速的效果
                InvokeRepeating("Move", 0, speed - 0.2f);
            }
            //当用户抬起空格，回到原本的速度
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //停止之前的invoke
                CancelInvoke();
                //重新invokeRepeating，设置回原本的调用时间
                InvokeRepeating("Move", 0, speed);
            }
            //当用户按下w，向上移动，正在向下移动的时候不可以向上移动，原本就是向上移动的话也不需要向上移动
            if (Input.GetKeyDown(KeyCode.W) && y != -step && y != step)
            {
                x = 0;
                y = step;
                Move();//立即转向
                       //改变蛇头朝向
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            if (Input.GetKeyDown(KeyCode.S) && y != step && y != -step)
            {
                x = 0;
                y = -step;
                Move();//立即转向
                       //改变蛇头朝向
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.A) && x != step && x != -step)
            {
                y = 0;
                x = -step;
                Move();//立即转向
                       //改变蛇头朝向
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
            if (Input.GetKeyDown(KeyCode.D) && x != -step && x != step)
            {
                y = 0;
                x = step;
                Move();//立即转向
                       //改变蛇头朝向
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
        }
        
    }

    //移动方法
    private void Move()
    {
        //记录蛇头移动前的位置
        headPos = transform.localPosition;
        //改变蛇头的position
        transform.localPosition = new Vector3(headPos.x + x, headPos.y + y, headPos.z);

        //问题：每一次移动身体的颜色都不一样
        //if (bodyList.Count > 0)
        //{
        //    //最后一个身体移动到最前面的位置
        //    bodyList.Last().localPosition = headPos;
        //    //将最后一个元素插入到第一个位置
        //    bodyList.Insert(0, bodyList.Last());
        //    //移除最后一个元素的引用
        //    bodyList.RemoveAt(bodyList.Count - 1);
        //}

        //将身体每一部分移动到前一个身体的位置
        if (bodyList.Count > 0)
        {
            //从后往前移动
            for (int i = bodyList.Count-1; i > 0; i--)
            {
                bodyList[i].localPosition = bodyList[i - 1].localPosition;
            }
            //第一个身体移动到蛇头移动前的位置
            bodyList[0].localPosition = headPos;
        }

    }
    //添加身体
    private void Grow()
    {
        //播放吃的音效
        AudioSource.PlayClipAtPoint(audioClips[0], Vector3.zero);
        //长度+1
        length++;
        //用来确定蛇身的颜色
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;
        //实例化蛇身
        GameObject body = Instantiate(bodyPrefab, new Vector3(2000, 2000, 0), Quaternion.identity);
        //设置蛇身的父物体
        body.transform.SetParent(canves,false);
        //将蛇身的位置信息保存到bodyList中，用来操作蛇身的位置
        bodyList.Add(body.transform);
        //设置蛇身的sprite
        body.GetComponent<Image>().sprite = bodySprites[index];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当碰撞到的触发器为食物
        if (collision.tag == "Food")
        {
            //摧毁碰撞到的食物
            Destroy(collision.gameObject);
            //生成新的食物
            foodMaker.SendMessage("MakeFood");
            //当吃到一个食物就实例化一个蛇身
            Grow();
            //吃到加分
            score += 5;
        }
        else if (collision.tag == "SnakeBody")
        {
            Die();
        }
        else if (collision.tag=="Reward")
        {
            //随机加分50-140
            score += Random.Range(5, 15) * 10;
            Destroy(collision.gameObject);
            Grow();
        }
        else
        {
            //如果当前是否为边界模式
            if (PlayerPrefs.GetInt("border", 1) == 1)
            {
                //边界模式，那么就死亡
                Die();
            }
            else
            {
                //不是边界模式，那么就穿过边界
                switch (collision.name)
                {
                    case "Up":
                        //Y值取反，并且向上走一步
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 25, transform.localPosition.z);
                        break;
                    case "Down":
                        //y值取反，并且向下走一步
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 25, transform.localPosition.z);
                        break;
                    case "Right":
                        //x值取反，并且向右走7步
                        transform.localPosition = new Vector3(-transform.localPosition.x + 180, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Left":
                        //x值取反，并且向右走6步
                        transform.localPosition = new Vector3(-transform.localPosition.x + 145, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
    }
    void Die()
    {
        //取消invoke
        CancelInvoke();
        //播放死亡音效
        AudioSource.PlayClipAtPoint(audioClips[1], Vector3.zero);
        //设置key为lasts的值为score,含义：上一次的分数
        PlayerPrefs.SetInt("lasts", score);
        //设置key为lastl的值为score,含义：上一次的长度
        PlayerPrefs.SetInt("lastl", length);
        //获得key为bests的值，如果不存在则为0，含义：最高分数
        if (PlayerPrefs.GetInt("bests", 0) < score)
        {
            //如果最高分小于本次分数，那么就将本次的分数和长度作为最高分，修改key为bests和bestl的值
            PlayerPrefs.SetInt("bests", score);
            PlayerPrefs.SetInt("bestl", length);
        }
        //实例化死亡特效
        Instantiate(dieEffect, Vector3.zero, Quaternion.identity) ;
        //销毁当前物体
        Destroy(gameObject);
    }
}
