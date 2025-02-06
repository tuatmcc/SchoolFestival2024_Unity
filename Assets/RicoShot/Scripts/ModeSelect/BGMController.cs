using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    [SerializeField] AudioClip BGM;
    [SerializeField] AudioClip BGM_Play;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneLoaded;
    }

    // Update is called once per frame
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "Play")
        {
            source.clip = BGM_Play;
            source.Play();
        }
        else
        {
            source.clip = BGM;
            source.Play();
        }
    }

}
