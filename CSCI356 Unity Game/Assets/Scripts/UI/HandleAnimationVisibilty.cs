using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutsceneVisibility : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector director;
    [Tooltip("Parent object that holds all cutscene UI images/animations")]
    public GameObject cutsceneRoot;

    void Awake()
    {
        ResetCutscene();

        if (director != null)
        {
       
            director.played -= OnCutsceneStart;
            director.stopped -= OnCutsceneEnd;

            director.played += OnCutsceneStart;
            director.stopped += OnCutsceneEnd;
        }
    }

    void OnDestroy()
    {
        if (director != null)
        {
            director.played -= OnCutsceneStart;
            director.stopped -= OnCutsceneEnd;
        }
    }


    void OnCutsceneStart(PlayableDirector d)
    {
        Time.timeScale = 0f; 
        director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;

        if (cutsceneRoot != null)
        {
            cutsceneRoot.SetActive(true);

   
            foreach (var anim in cutsceneRoot.GetComponentsInChildren<Animator>(true))
            {
                anim.enabled = true;
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                anim.Rebind();
                anim.Update(0f);
            }
        }
    }


    void OnCutsceneEnd(PlayableDirector d)
    {
        StartCoroutine(CleanupAfterTimeline());
    }

    private IEnumerator CleanupAfterTimeline()
    {
  
        yield return null;

        Time.timeScale = 1f; 

        if (director != null)
        {
            director.Stop();
            director.time = 0;
        }

        if (cutsceneRoot != null)
        {
            foreach (var anim in cutsceneRoot.GetComponentsInChildren<Animator>(true))
            {
                anim.Rebind();
                anim.Update(0f);
                anim.enabled = false;
            }

            cutsceneRoot.SetActive(false);
        }

  
        var transition = FindFirstObjectByType<SceneTransitionManager>();
        if (transition != null)
            transition.StartMinigame();
    }

    private void ResetCutscene()
    {
        if (cutsceneRoot == null) return;

    
        foreach (var anim in cutsceneRoot.GetComponentsInChildren<Animator>(true))
        {
            anim.enabled = false;
            anim.Rebind();
            anim.Update(0f);
        }

        cutsceneRoot.SetActive(false);
    }
}
