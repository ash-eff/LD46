using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransistionLoader : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;
    public GameObject logo;
    public GameObject canvas;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        canvas.SetActive(true);
    }

    public void LoadNextScene(int sceneIndex)
    {
        StartCoroutine(ILoadNextScene(sceneIndex));
    }

    IEnumerator ILoadNextScene(int sceneToLoad)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(1.5f);

        SceneManager.LoadScene(sceneToLoad);
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake(.25f, 2));
        audioSource.Play();
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = logo.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            logo.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        logo.transform.localPosition = originalPos;
    }
}


