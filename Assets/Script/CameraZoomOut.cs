using UnityEngine;
using DG.Tweening;
public class CameraZoomOut : MonoBehaviour
{
    public static CameraZoomOut instance;
    public Transform focusTarget;   // drag the important object here
    public float startZoom = 2.5f;
    public float endZoom = 5.4f;
    public float duration = 0.6f;
    public float delay = 1f;
  
    void Start()
    {
        instance = this;
        if (SceneTransitionData.entryType != SceneEntryType.FromMenu)
        {
            // Skip cinematic
            if (SceneTransitionData.entryType != SceneEntryType.FromGame)
            {
                MainStall.instance.GameStart();
            }
            return;
        }

        // Reset so it doesn't repeat
        SceneTransitionData.entryType = SceneEntryType.None;
        Camera cam = GetComponent<Camera>();

        // Start zoomed in
        cam.orthographicSize = startZoom;

        // Keep current Z, but aim at target Y/X
        Vector3 targetPos = focusTarget.position;
        targetPos.z = cam.transform.position.z;

        DOVirtual.DelayedCall(delay, () =>
        {
            Sequence seq = DOTween.Sequence();

            seq.Join(cam.DOOrthoSize(endZoom, duration).SetEase(Ease.OutQuad));
            seq.Join(cam.transform.DOMove(targetPos, duration).SetEase(Ease.OutQuad));
            seq.OnComplete(OnZoomFinished);
        });
    }
    
    void OnZoomFinished()
    {
       MainStall.instance.GameStart();
    }
}
