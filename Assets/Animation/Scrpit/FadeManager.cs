using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour {

    //검정색, 흰색 배경 통제
    public SpriteRenderer white; 
    public SpriteRenderer black;

    private Color color;//이미지의 투명도를 조절하기 위한 변수
    
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);//반복마다 해야하기 때문에 변수로 만들어 프로그램의 부담 감소

    public void FadeOut(float _speed = 0.02f)
    {
        StartCoroutine(FadeOutCoroutine(_speed));
    }

    IEnumerator FadeOutCoroutine(float _speed)
    {
        color = black.color;

        while(color.a < 1f) //투명도가 1이상이면 = 검정색이면 반복 중지
        {
            color.a += _speed; //_speed만큼 색이 진해짐
            black.color = color;
            yield return waitTime;
        }
    }

    public void FadeIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }

    IEnumerator FadeInCoroutine(float _speed)
    {
        color = black.color;

        while (color.a > 0f) //투명도가 0dlgk이면 = 투명이면 반복 중지
        {
            color.a -= _speed; //_speed만큼 색이 연해짐
            black.color = color;
            yield return waitTime;
        }
    }

    public void Flash(float _speed = 0.01f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine(_speed));
    }
    IEnumerator FlashCoroutine(float _speed)
    {
        color = white.color;

        while (color.a < 1f) //투명도가 1이상이면 = 검정색이면 반복 중지
        {
            color.a += _speed; //_speed만큼 색이 진해짐
            white.color = color;
            yield return waitTime;
        }

        while (color.a > 0f) //투명도가 1이상이면 = 검정색이면 반복 중지
        {
            color.a -= _speed; //_speed만큼 색이 진해짐
            white.color = color;
            yield return waitTime;
        }
    }

    public void FlashOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashOutCoroutine(_speed));
    }

    IEnumerator FlashOutCoroutine(float _speed)
    {
        color = white.color;

        while (color.a < 1f) //투명도가 1이상이면 = 검정색이면 반복 중지
        {
            color.a += _speed; //_speed만큼 색이 진해짐
            white.color = color;
            yield return waitTime;
        }
    }

    public void FlashIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }

    IEnumerator FlashInCoroutine(float _speed)
    {
        color = white.color;

        while (color.a > 0f) //투명도가 1이상이면 = 검정색이면 반복 중지
        {
            color.a -= _speed; //_speed만큼 색이 진해짐
            white.color = color;
            yield return waitTime;
        }
    }

}
