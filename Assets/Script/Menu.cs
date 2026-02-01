using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public enum SceneEntryType
{
    None,
    FromMenu,
    FromGame
}

public static class SceneTransitionData
{
    public static SceneEntryType entryType = SceneEntryType.None;
}
public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject settingGO, creditGO, settingBGO, creditBGO, exitBGO, exitGO;
    private Button settingB, exitB, creditB, playBt;
    public GameObject[] masks; // assign in Inspector
    public float interval = 3f;
    float timer;
    int lastIndex = -1;
    public Image panel;
    MaskType randomMaskType;
    bool gateOpen;
    public GameObject gate;
    Tween gateTweenOn, gateTweenOff, gateTweenQuickOff;
    public SpriteRenderer gateLight;
    public Sprite red, green;
    public GameObject playB;
    public Slider musicSli, SFXSli;
    public AudioSource music;
    void Start()
    {
        ActivateRandomMask();
        StartCoroutine(RandomGateRoutine());
        settingB = settingBGO.GetComponent<Button>();
        exitB = exitBGO.GetComponent<Button>();
        creditB = creditBGO.GetComponent<Button>();
        playBt = playB.GetComponent<Button>();
    }

    IEnumerator RandomGateRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(waitTime);

            ToggleGate(); // uses your existing logic
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            ActivateRandomMask();
        }
        music.volume = Audio.instance.musicVolume * 0.1f;
        Color col = Color.white;
        switch (randomMaskType)
        {
            case MaskType.Sad:
                ColorUtility.TryParseHtmlString("#26C5FF", out col);
                break;

            case MaskType.Anger:
                ColorUtility.TryParseHtmlString("#FF2626", out col);
                break;

            case MaskType.Confuse:
                ColorUtility.TryParseHtmlString("#939393", out col);
                break;
            case MaskType.Anxiety:
                ColorUtility.TryParseHtmlString("#DBC93A", out col);
                break;
        }
        Audio.instance.mainVolume = SFXSli.value;
        Audio.instance.musicVolume = musicSli.value;
        panel.color = col;
    }

    public void ToggleGate()
    {
        gateOpen = !gateOpen;
        if (gateOpen)
        {
            GateOn();
          
        }
        else
        {
            GateOff();
            
        }
    }
    public void ChangeMainVolume()
    {
      
    }
    public void ChangeSfxVolume()
    {
      
    }

    public void StartGame()
    {
        StopAllCoroutines();
        gateTweenOn.Kill();
        gateTweenOff.Kill();
        GateOff(0.4f);
        settingBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
        creditBGO.transform.DOMoveX(11f, 0.5f).SetEase(Ease.OutQuad);
        settingGO.transform.DOMoveX(-12.25f, 0.5f).SetEase(Ease.OutQuad);
        creditGO.transform.DOMoveX(12.25f, 0.5f).SetEase(Ease.OutQuad);
        exitBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
        exitGO.transform.DOMoveX(-12.25f, 0.5f).SetEase(Ease.OutQuad);
        playB.transform.DOMoveY(-11f, 0.5f).SetEase(Ease.OutQuad);
        settingB.interactable = false;
        exitB.interactable = false;
        creditB.interactable = false;
        playBt.interactable = false;
        SceneTransitionData.entryType = SceneEntryType.FromMenu;
        StartCoroutine(StartGameRoutine());
       
    }

    IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<CameraTransition>().ZoomAndLoad("InGame");
    }

    public void GateOn()
    {
        gateTweenOn = gate.transform.DOMoveY(8f, 0.5f).SetEase(Ease.OutQuad);
        Audio.instance.PlaySound("GOpen");
        gateOpen = true;
        gateLight.sprite = green;
        
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void GateOff(float f = 0.5f)
    {
        if (f < .5f)
        {
            gateTweenQuickOff = gate.transform.DOMoveY(-0.035f, f).SetEase(Ease.InQuad);
        }
        else
        {
            gateTweenOff = gate.transform.DOMoveY(-0.035f, f).SetEase(Ease.InQuad);
        }
        Audio.instance.PlaySound("GClose");
        gateOpen = false;
        gateLight.sprite = red;
    }

    void ActivateRandomMask()
    {
        foreach (var mask in masks)
            mask.SetActive(false);

        int index;
        do
        {
            index = Random.Range(0, masks.Length);
        } while (index == lastIndex && masks.Length > 1);

        lastIndex = index;
        randomMaskType = (MaskType)index;
        masks[index].SetActive(true);
    }

    public void SettingOn()
    {
        Audio.instance.PlaySound("Button");
        settingGO.transform.DOMoveX(-7.25f, 0.5f).SetEase(Ease.OutQuad);
        settingBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
        exitBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
        settingB.interactable = false;
        exitB.interactable = false;
    }

    public void SettingOff()
    {
        Audio.instance.PlaySound("Button");
        settingGO.transform.DOMoveX(-12.25f, 0.5f).SetEase(Ease.OutQuad);
        settingBGO.transform.DOMoveX(-9.2f, 0.5f).SetEase(Ease.OutQuad);
        exitBGO.transform.DOMoveX(-9.2f, 0.5f).SetEase(Ease.InQuad);
        settingB.interactable = true;
        exitB.interactable = true;
    }

    public void ExitOn()
    {
        Audio.instance.PlaySound("Button");
        exitGO.transform.DOMoveX(-7.25f, 0.5f).SetEase(Ease.OutQuad);
        exitBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
        settingBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
        settingB.interactable = false;
        exitB.interactable = false;
    }

    public void ExitOff()
    {
        Audio.instance.PlaySound("Button");
        exitGO.transform.DOMoveX(-12.25f, 0.5f).SetEase(Ease.OutQuad);
        exitBGO.transform.DOMoveX(-9.2f, 0.5f).SetEase(Ease.OutQuad);
        settingBGO.transform.DOMoveX(-9.2f, 0.5f).SetEase(Ease.InQuad);
        settingB.interactable = true;
        exitB.interactable = true;
    }

    public void CreditOn()
    {
        Audio.instance.PlaySound("Button");
        creditGO.transform.DOMoveX(7.25f, 0.5f).SetEase(Ease.OutQuad);
        creditBGO.transform.DOMoveX(11f, 0.5f).SetEase(Ease.OutQuad);
    }

    public void CreditOff()
    {
        Audio.instance.PlaySound("Button");
        creditGO.transform.DOMoveX(12.25f, 0.5f).SetEase(Ease.InQuad);
        creditBGO.transform.DOMoveX(9.2f, 0.5f).SetEase(Ease.InQuad);
    }
}