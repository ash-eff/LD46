using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public float shakeDuration = .25f;
    public float shakeMagnitude = .1f;
    private GameController gameController;
    private PlayerController player;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        player = FindObjectOfType<PlayerController>();
        gameController = FindObjectOfType<GameController>();
    }

    private void FixedUpdate()
    {
        FollowPlayerTarget(player.transform.position);
    }

    void FollowPlayerTarget(Vector2 _target)
    {
        Vector2 targetPos = new Vector3(_target.x, _target.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }

    public void CameraShake()
    {
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = mainCam.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            mainCam.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCam.transform.localPosition = originalPos;
    }
}
