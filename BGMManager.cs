using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * BGMManager
 * ĳ���Ͱ� Ư�� scene�� �����ϰų� Ư�� NPC�� ������ �� �پ��� �̺�Ʈ���� object�� ����� BGMManger�� ����� �� �ִ�. 
 */

public class BGMManager : MonoBehaviour
{
    //�̱���ȭ(only one exists)�� ���� �ڱ� �ڽ��� ��ü�� ������ static ������ �����Ѵ�.
    static public BGMManager instance; 

    //BGM ��ü
    public AudioClip[] clips; //BGM ��ü���� ���� �迭
    private AudioSource source; //BGM ��ü ����(���)�� ���� �÷��̾�

    //Coroutine
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    //�̱���ȭ ���� (Scene �̵� ��, BGMManger Destroy�� ���´�.)
    private void Awake() {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }

    public void Play(int _playMusicTrack) {
        source.volume = 1f; // BGM�� FadeOut���� ���, volume�� 0�� �Ǿ� ����ص� BGM�� �鸮���ʱ� ������, 1f�� �ʱ�ȭ�ϴ� �۾��� ���ش�.
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void Pause() //�Ͻ�����
    {
        source.Pause();
    }


    public void UnPause() //�Ͻ����� ����
    {
        source.UnPause();
    }

    public void Stop() {
        source.Stop();
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine() {
        // i: source�� volume��
        for (float i = 1.0f; i>=0f; i-=0.01f) {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        // i: source�� volume��
        for (float i = 0f; i <= 1.0f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
    // Update is called once per frame -> BGMManager�� �ʿ����� �����Ƿ� ������.
}
