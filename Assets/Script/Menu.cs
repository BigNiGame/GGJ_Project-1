using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject settingGO,creditGO,settingBGO,creditBGO;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void SettingOn()
    {
        settingGO.transform.DOMoveX(-7.25f, 0.5f).SetEase(Ease.OutQuad);
        settingBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
    }
    public void SettingOff()
    {
        settingGO.transform.DOMoveX(-12.25f, 0.5f).SetEase(Ease.InQuad);
        settingBGO.transform.DOMoveX(-9.2f, 0.5f).SetEase(Ease.InQuad);
    }
    public void CreditOn()
    {
        creditGO.transform.DOMoveX(7.25f, 0.5f).SetEase(Ease.OutQuad);
        creditBGO.transform.DOMoveX(11f, 0.5f).SetEase(Ease.OutQuad);
    }
    public void CreditOff()
    {
        creditGO.transform.DOMoveX(12.25f, 0.5f).SetEase(Ease.InQuad);
        creditBGO.transform.DOMoveX(9.2f, 0.5f).SetEase(Ease.InQuad);
    }
    public void Play()
    {
       
    }

}
