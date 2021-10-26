using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour
{
    public Text lastTxt;
    public Text bestTxt;
    public Toggle blue;
    public Toggle yellow;
    public Toggle border;
    public Toggle noBorder;
    private void Awake()
    {
        //获取上一次设置的皮肤和边界模式
        if(PlayerPrefs.GetInt("border", 1) == 0)
        {
            noBorder.isOn = true;
        }
        else
        {
            border.isOn = true;
        }
        if(PlayerPrefs.GetString("sh", "sh02") == "sh02")
        {
            yellow.isOn = true;
        }
        else
        {
            blue.isOn = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //获取最佳成绩和上一次的成绩
        lastTxt.text = "上次：长度" + PlayerPrefs.GetInt("lastl", 0) + "，分数" + PlayerPrefs.GetInt("lasts", 0);
        bestTxt.text = "最佳：长度" + PlayerPrefs.GetInt("bestl", 0) + "，分数" + PlayerPrefs.GetInt("bests", 0);
        //设置初值的目的是：防止用户进入不选择皮肤和模式，那么就有可能发生游戏中的皮肤与选择的皮肤不一致（因为用户不选择就不会调用toggle的onvaluechange，进入游戏后显示的还是编辑器中的皮肤，和边界模式）
        if (blue.isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        } else if (yellow.isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
        if (border.isOn)
        {
            PlayerPrefs.SetInt("border", 1);
        }else if (noBorder.isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    //当toggle值发生改变是调用
    public void Blue(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
    }
    public void Yellow(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
    }
    public void Border(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 1);
        }
    }
    public void NoBorder(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }
}
