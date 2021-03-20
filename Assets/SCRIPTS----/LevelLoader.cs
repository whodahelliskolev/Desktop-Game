using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    public int sceneIndex;
    public Slider loadingProgress;
    public GameObject sliderGameObject;
    public ScreenFade screenFade;   
    public GameObject pressAnyButton;
    public GameObject tipsObject;
    
    public Text tips;
    public string[] tipsArray;
    private bool isLoading;
   
    
    public IEnumerator LoadLevel()
    {
        isLoading = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        screenFade.FadeToBlack();
        sliderGameObject.SetActive(true);
        pressAnyButton.SetActive(true);
        tipsObject.SetActive(true);
        tips.text = tipsArray[Random.Range(0, tipsArray.Length - 1)];
        while (!operation.isDone)
        {

            loadingProgress.value = operation.progress;


            if (operation.progress >= 0.9f)
            {

                

                if (Input.GetKeyDown(KeyCode.Space))

                {
                    operation.allowSceneActivation = true;
                    isLoading = false;
                }
            }

            yield return null;
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isLoading)
        {
            StartCoroutine(LoadLevel());

        }
    }


}
