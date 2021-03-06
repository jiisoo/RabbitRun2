﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**************************************************
 * GameManager : 게임 시작 시 준비메시지 표시
 * 게임의 준비, 종료, 시작 처리 ( 게임의 전반적인 동작 관리)            
 * ************************************************/
public class GameManager : MonoBehaviour {
    
    public static GameManager manager;

    public GameObject resultPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;

    private int Timer = 0;
    public GameObject WaitOnePanel;
    public GameObject WaitTwoPanel;
    public GameObject WaitThreePanel;

    public Text resultScore;               
    public Text textScore;
    public Text textCoin;
    public Slider slider;

    //game ready : ready = true , end = false;
    //game play :  ready = false, end = false;
    //game end  :  ready = false, end = true;
    public bool ready = true; //게임 종료를 구분
    public bool end = false;

    private int score = 0;
    private int coin = 0;

    

    // Use this for initialization
    void Start () {
        manager = this;
        Timer = 0;
        WaitOnePanel.SetActive(false);
        WaitTwoPanel.SetActive(false);
        WaitThreePanel.SetActive(true);

        pausePanel.SetActive(false); //해당 Panel을 비활성화
        resultPanel.SetActive(false);
        this.GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
        slider.value = Character.character.health; //체력 지속적으로 업데이트
        textScore.text = score.ToString(); //먹이에 따라서 text UI 업데이트
        textCoin.text = coin.ToString();
        Wait();
		if(ready ==true)
        {
            ready = false;
        }
        
        if(Character.character.health == 0) //체력이 0일때 게임 중지 
        {
            GameOver();
        }
	}
    public void GameOver()
    {
        this.GetComponent<AudioSource>().Stop();
        end = true;
        resultScore.text = GetScore();
        
        resultPanel.SetActive(true);
        pausePanel.SetActive(false);//일시정지 패널에서 나갈경우 이 패널을 비활성화
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.0, "y", 0.0, "time", 0.0f)); //장애물에 충돌된 상태였을 경우 화면 흔들림 방지 
        Time.timeScale = 0; //화면정지
    }

    public void AddScore(int inputScore)
    {
        score += inputScore;
    }
    public string GetScore()
    {
        return score.ToString();
    }
    public void AddCoin(int inputCoin)
    {
        coin += inputCoin;
    }

    public void Pause()
    {
        Time.timeScale = 0; //화면정지
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ClickRetry()
    {
        Timer = 0;
        Wait();
    }
    public void Unpause()
    {
        Time.timeScale = 1; //화면 정지 해제
        Timer = 0; //일시정지를 해제 했으므로 timer을 0으로 재설정 
        pauseButton.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void Wait()
    {
        //게임 시작시 정지
        if (Timer == 0)
        {
            Time.timeScale = 0.0f;
        }

        //Timer 가 90보다 작거나 같을경우 Timer 계속증가
        if (Timer <= 90)
        {
            Timer++;
            // Timer가 30보다 작을경우 3번켜기
            if (Timer < 30)
            {
                WaitThreePanel.SetActive(true);
            }
            // Timer가 30보다 클경우 3번끄고 2번켜기
            if (Timer > 30)
            {
                WaitThreePanel.SetActive(false);
                WaitTwoPanel.SetActive(true);
            }
            // Timer가 60보다 클경우 2번끄고 1번켜기
            if (Timer > 60)
            {
                WaitTwoPanel.SetActive(false);
                WaitOnePanel.SetActive(true);
            }
            //Timer 가 90보다 크거나 같을경우 1번끄고 게임시작하기. LoadingEnd () 코루틴호출 
            if (Timer >= 90)
            {
                WaitOnePanel.SetActive(false);
                Time.timeScale = 1.0f; //게임시작
                   
            }
        }
    }
}
