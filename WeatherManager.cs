using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * WeatherManager
 * 캐릭터가 특정 scene에 입장하거나 특정 NPC를 만나는 등 다양한 이벤트에서 object를 만들어 WeatherManger를 사용할 수 있다. 
 */

public class WeatherManager : MonoBehaviour
{
    //싱글턴화(only one exists)를 위한 자기 자신을 객체로 가지는 static 변수를 생성한다.
    static public WeatherManager instance;
    
    //싱글턴화 과정 (Scene 이동 시, WeatherManger Destroy를 막는다.)
    private void Awake()
    {
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

    public ParticleSystem rain; //Particle System control을 위한 rain 변수 선언.
    private AudioManager theAudio; //빗소리 재생을 위한 오디오 매니저 객체 선언.
    public string rain_sound; //빗소리 재생 시 play할 오디오.


    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    // Particle Control Functions: play, stop, emit
    public void Rain()
    {
        theAudio.Play(rain_sound);
        rain.Play();
    }

    public void RainStop()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }

    public void RainDrop()
    {
        rain.Emit(1); //빗방울의 개수를 조절할 수 있다.
    }
}
