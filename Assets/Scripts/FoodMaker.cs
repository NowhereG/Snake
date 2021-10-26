using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour
{
    private int xRightLimit = 14;
    private int xLeftLimit = -9;
    private int yLimit = 7;
    //食物预制体
    public GameObject foodPrefab;
    //存放sprite的数组
    public Sprite[] foodSprites;
    //特殊道具
    public GameObject rewardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        MakeFood();
    }
    private void MakeFood()
    {
        //实例化食物
        GameObject food = GameObject.Instantiate(foodPrefab);
        //随机食物的sprite
        int index = Random.Range(0, foodSprites.Length);
        //设置sprite
        food.GetComponent<Image>().sprite = foodSprites[index];
        //设置食物的父物体
        food.transform.SetParent(GameObject.Find("bg").transform, false);
        //随机食物的x坐标
        int x = Random.Range(xLeftLimit, xRightLimit + 1);
        //随机食物的y坐标
        int y = Random.Range(-yLimit, yLimit + 1);
        //设置食物的坐标
        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        //随机数为0-19就生成奖励道具
        if (Random.Range(0, 100) < 20 ? true : false)
        {
            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.SetParent(GameObject.Find("bg").transform, false);
            //随机奖励的x坐标
            x = Random.Range(xLeftLimit, xRightLimit + 1);
            //随机奖励的y坐标
            y = Random.Range(-yLimit, yLimit + 1);
            //设置奖励的坐标
            reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }


    }
}
