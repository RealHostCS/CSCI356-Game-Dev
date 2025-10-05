using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneVisibility : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject[] cutsceneObjects;

    void Awake()
    {
        // Start hidden and make sure their Animator won't auto-play when enabled
        foreach (var obj in cutsceneObjects)
        {
            if (obj == null) continue;
            obj.SetActive(false);
            var anim = obj.GetComponent<Animator>();
            if (anim != null) anim.enabled = false;
        }

        if (director != null)
        {
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
        director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;

        Time.timeScale = 0f; // freeze the game

        foreach (var obj in cutsceneObjects)
        {
            if (obj == null) continue;
            obj.SetActive(true);

            var anim = obj.GetComponent<Animator>();
            if (anim != null)
            {
                anim.enabled = true;
                anim.updateMode = AnimatorUpdateMode.UnscaledTime; // <- this fixes it
                anim.Rebind();
                anim.Update(0f);
            }
        }
    }

    void OnCutsceneEnd(PlayableDirector d)
    {
        // Unfreeze the game
        Time.timeScale = 1f;
        Debug.Log("[Cutscene] Ended - cleaning up");
        // Use coroutine to allow the director to finish its internal work,
        // then stop & reset animators before hiding objects.
        StartCoroutine(CleanupAfterTimeline());
        SceneManager.LoadScene("BaybladeBattle");
    }

    private IEnumerator CleanupAfterTimeline()
    {
        // Stop director so it won't keep evaluating clips
        if (director != null)
            director.Stop();

        // wait one frame so any pending evaluations finish
        yield return null;

        foreach (var obj in cutsceneObjects)
        {
            if (obj == null) continue;

            var anim = obj.GetComponent<Animator>();
            if (anim != null)
            {
                // reset animator to base/default pose and prevent it from auto-playing
                anim.Rebind();
                anim.Update(0f);
                anim.enabled = false;
            }

            // finally hide the object
            obj.SetActive(false);
        }
    }
}
