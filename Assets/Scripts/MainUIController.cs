using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public Text modeTxt;
    public Text scoreTxt;
    public Text lengthTxt;
    private int score = 0;
    public Image bgImage;
    private Color tempColor;
    public Image home;
    public Image pause;
    public Sprite[] pauseSprites;
    private bool isPause = false;
    private void Awake()
    {
        //自由模式
        if (PlayerPrefs.GetInt("border", 1) == 0)
        {
            //foreach会遍历bgImage下的所有子物体的Transform
            foreach (Transform t in bgImage.transform)
            {
                //自由模式下不需要显示边界
                t.GetComponent<Image>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //刷新文本
        ChangeMode();
        ChangeTxt();
    }
    public void ChangeMode()
    {
        //获取分数
        score = HeadManager.Instance.score;
        //根据分数更改背景颜色和阶段
        switch (score / 100)
        {
            case 0:
            case 1:
            case 2: break;
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#CCEEFFF", out tempColor);
                bgImage.color = tempColor;
                modeTxt.text = "阶段" + 2;
                break;
            case 5:
            case 6:
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);
                bgImage.color = tempColor;
                modeTxt.text = "阶段" + 3; break;
            case 7:
            case 8:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                bgImage.color = tempColor;
                modeTxt.text = "阶段" + 4; 
                break;
            case 9:
            case 10:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                bgImage.color = tempColor;
                modeTxt.text = "阶段" + 5; break;
            case 11:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                bgImage.color = tempColor;
                modeTxt.text = "无尽阶段"; 
                break;
        }
    }
    public void ChangeTxt()
    {
        scoreTxt.text = "得分：\n" + HeadManager.Instance.score;
        lengthTxt.text = "长度：\n" + HeadManager.Instance.length;
    }
    //回到首页
    public void GoHome()
    {
        //如果暂停状态回到首页的话，timeScale还是0，如果再重新开始的话，游戏仍然处于暂停状态
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        //暂停标志位取反
        isPause = !isPause;
        if (isPause)
        {
            //暂停游戏
            Time.timeScale = 0;
            pause.sprite = pauseSprites[1];
        }
        else
        {
            //继续游戏
            Time.timeScale = 1;
            pause.sprite = pauseSprites[0];
        }

    }
}
