using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * WeatherManager
 * ĳ���Ͱ� Ư�� scene�� �����ϰų� Ư�� NPC�� ������ �� �پ��� �̺�Ʈ���� object�� ����� WeatherManger�� ����� �� �ִ�. 
 */

public class WeatherManager : MonoBehaviour
{
    //�̱���ȭ(only one exists)�� ���� �ڱ� �ڽ��� ��ü�� ������ static ������ �����Ѵ�.
    static public WeatherManager instance;
    
    //�̱���ȭ ���� (Scene �̵� ��, WeatherManger Destroy�� ���´�.)
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

    public ParticleSystem rain; //Particle System control�� ���� rain ���� ����.
    private AudioManager theAudio; //���Ҹ� ����� ���� ����� �Ŵ��� ��ü ����.
    public string rain_sound; //���Ҹ� ��� �� play�� �����.


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
        rain.Emit(1); //������� ������ ������ �� �ִ�.
    }
}
