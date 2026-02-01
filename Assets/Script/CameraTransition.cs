using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class CameraTransition : MonoBehaviour
{
    public Camera cam;
    public Vector3 sceneBCameraPos;
    public float zoomInSize = 2.5f;  // how close we zoom
    public float duration = 0.6f;

    float defaultSize;

    void Awake()
    {
        defaultSize = 5.4f; // your current size
    }

    public void ZoomAndLoad(string sceneName)
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(cam.DOOrthoSize(zoomInSize, duration).SetEase(Ease.InQuad));
        seq.Join(cam.transform.DOMove(sceneBCameraPos, duration).SetEase(Ease.InQuad));

        seq.OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}
