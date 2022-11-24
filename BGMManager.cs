using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * BGMManager
 * 캐릭터가 특정 scene에 입장하거나 특정 NPC를 만나는 등 다양한 이벤트에서 object를 만들어 BGMManger를 사용할 수 있다. 
 */

public class BGMManager : MonoBehaviour
{
    //싱글턴화(only one exists)를 위한 자기 자신을 객체로 가지는 static 변수를 생성한다.
    static public BGMManager instance; 

    //BGM 객체
    public AudioClip[] clips; //BGM 객체들을 위한 배열
    private AudioSource source; //BGM 객체 실행(재생)을 위한 플레이어

    //Coroutine
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    //싱글턴화 과정 (Scene 이동 시, BGMManger Destroy를 막는다.)
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
        source.volume = 1f; // BGM을 FadeOut했을 경우, volume이 0이 되어 재생해도 BGM이 들리지않기 때문에, 1f로 초기화하는 작업을 해준다.
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void Pause() //일시정지
    {
        source.Pause();
    }


    public void UnPause() //일시정지 해제
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
        // i: source의 volume값
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
        // i: source의 volume값
        for (float i = 0f; i <= 1.0f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
    // Update is called once per frame -> BGMManager에 필요하지 않으므로 삭제함.
}
