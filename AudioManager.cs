using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sound & AudioManager
 * - ���� ��ü: AudioManger���� ����� ��ü.
 * - AudioManager: ���� �� ���带 ��� �ٷ�� class�� AudioManager�Ӹ� �ƴ϶� BGMManager, ChoiceManager �� �پ��� class���� Ȱ���� �� �ִ�.
 */

// ���� ��ü
[System.Serializable] // custom ��ü�� �ڵ����� Inspectorâ�� ���� �ʱ� ������, �������� ����� �ʿ��ϴ�.
public class Sound {

    public string name; //�����
    private AudioSource source; //���� �÷��̾�
    public AudioClip clip; //���� ����
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

    //�̱���ȭ�� ���� �ڱ� �ڽ��� ��ü�� ������ static ������ �����Ѵ�.
    static public AudioManager instance; 

    [SerializeField] // custom ��ü�� �ڵ����� Inspectorâ�� ���� �ʱ� ������, �������� ����� �ʿ��ϴ�. 
    public Sound[] sounds; // sound ��ü���� ���� �迭.

    //AudioManager�� �̱������� �����, �� �� �� ���� �����ϵ��� �ϴ� �����̴�. (Scene �̵� ��, AudioManger Destroy�� ���´�.)
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
            GameObject soundObject = new GameObject("���� ���� �̸�"+i+" = "+sounds[i].name); // sound ��ü�� �����, �̸��� �������ش�.
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>()); // sound ��ü�� audiosource�� �������ش�.
            soundObject.transform.SetParent(this.transform);
        }
    }

    //sound ��ü���� �־��� �̸��� ������ ��ü�� ã�� play�Ѵ�.
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

    //sound ��ü���� �־��� �̸��� ������ ��ü�� ã�� stop�Ѵ�.
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

    //sound ��ü���� �־��� �̸��� ������ ��ü�� ã�� setLoop�Ѵ�.
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

    //sound ��ü���� �־��� �̸��� ������ ��ü�� ã�� setLoopCancel�Ѵ�.
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

    //sound ��ü���� �־��� �̸��� ������ ��ü�� ã�� setLoopCancel�Ѵ�.
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

    // Update is called once per frame -> AudioManager�� �ʿ����� �����Ƿ� ������.
}
