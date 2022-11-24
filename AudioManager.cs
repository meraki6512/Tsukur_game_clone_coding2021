using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sound & AudioManager
 * - 사운드 객체: AudioManger에서 사용할 객체.
 * - AudioManager: 게임 내 사운드를 모두 다루는 class로 AudioManager뿐만 아니라 BGMManager, ChoiceManager 등 다양한 class에서 활용할 수 있다.
 */

// 사운드 객체
[System.Serializable] // custom 객체는 자동으로 Inspector창에 뜨지 않기 때문에, 강제적인 명령이 필요하다.
public class Sound {

    public string name; //사운드명
    private AudioSource source; //사운드 플레이어
    public AudioClip clip; //사운드 파일
    public bool loop;
    public float Volume;

    // AudioSource Setter
    public void SetSource(AudioSource _source) {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volume; 
    }

    // AudioSource Functions : Play, Stop, SetLoop, SetLoopCancel, SetVolume
    public void Play() {
        source.Play();
    }

    public void Stop() {
        source.Stop();
    }

    public void SetLoop() {
        source.loop = true;
    }

    public void SetLoopCancel() {
        source.loop = false;
    }

    public void SetVolume()
    {
        source.volume = Volume;
    }

}

public class AudioManager : MonoBehaviour
{

    //싱글턴화를 위한 자기 자신을 객체로 가지는 static 변수를 생성한다.
    static public AudioManager instance; 

    [SerializeField] // custom 객체는 자동으로 Inspector창에 뜨지 않기 때문에, 강제적인 명령이 필요하다. 
    public Sound[] sounds; // sound 객체들을 담을 배열.

    //AudioManager를 싱글턴으로 만드는, 즉 단 한 개만 존재하도록 하는 과정이다. (Scene 이동 시, AudioManger Destroy를 막는다.)
    private void Awake() {
        if (instance != null) {
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
        for (int i = 0; i < sounds.Length; i++) {
            GameObject soundObject = new GameObject("사운드 파일 이름"+i+" = "+sounds[i].name); // sound 객체를 만들고, 이름을 지정해준다.
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>()); // sound 객체에 audiosource를 연결해준다.
            soundObject.transform.SetParent(this.transform);
        }
    }

    //sound 객체에서 주어진 이름과 동일한 객체를 찾아 play한다.
    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    //sound 객체에서 주어진 이름과 동일한 객체를 찾아 stop한다.
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    //sound 객체에서 주어진 이름과 동일한 객체를 찾아 setLoop한다.
    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    //sound 객체에서 주어진 이름과 동일한 객체를 찾아 setLoopCancel한다.
    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    //sound 객체에서 주어진 이름과 동일한 객체를 찾아 setLoopCancel한다.
    public void SetVolume(string _name, float _Volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Volume = _Volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }

    // Update is called once per frame -> AudioManager에 필요하지 않으므로 삭제함.
}
