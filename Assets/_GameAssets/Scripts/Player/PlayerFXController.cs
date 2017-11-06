using UnityEngine;
using System.Collections;

public class PlayerFXController : MonoBehaviour {
    
    [Header("Hit Effects")]
    public Renderer playerRenderer;
    public Color hitColour;
    public AnimationCurve hitMatLerpCurve;
    public float hitFXTime = 1f;

    public void PlayPlayerHitFX()
    {
        StartCoroutine(HitFX());
    }

    IEnumerator HitFX()
    {
        float timer = 0f;

        while(timer < hitFXTime)
        {
            timer += Time.deltaTime;
            float curveEval = hitMatLerpCurve.Evaluate(timer / hitFXTime);
            playerRenderer.material.color = Color.Lerp(Color.white, hitColour, curveEval);
            yield return null;
        }

        playerRenderer.material.color = Color.white;
    }
}
