using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    public Image blackScreen;
    public float fadeSpeed = 5f;
    Camera cam;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            Color c = blackScreen.color;
            c.a = 0;
            blackScreen.color = c;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main;
        Vector3 pos = cam.transform.position + new Vector3(0, 0, 10);
        pos.z = 0; // or whatever layer you need
        transform.position = pos;
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(1);
        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneTransitionData.entryType == SceneEntryType.FromGame)

            StartCoroutine(Fade(0, "Open"));
        else
            StartCoroutine(Fade(0));
    }

    IEnumerator Fade(float target, string tag = "")
    {
        Color c = blackScreen.color;

        while (Mathf.Abs(c.a - target) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, target, Time.deltaTime * fadeSpeed);
            blackScreen.color = c;
            yield return null;
        }

        c.a = target;
        blackScreen.color = c;
        if (tag == "Open")
        {
            MainStall.instance.GameStart();
        }
    }
}