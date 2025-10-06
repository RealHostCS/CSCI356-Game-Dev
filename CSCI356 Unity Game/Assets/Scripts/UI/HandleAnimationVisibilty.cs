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
            // prevent double subscriptions
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

    // Called when Timeline starts playing
    void OnCutsceneStart(PlayableDirector d)
    {
        Time.timeScale = 0f; // freeze gameplay
        director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;

        if (cutsceneRoot != null)
        {
            cutsceneRoot.SetActive(true);

            // enable all animators and set to unscaled time
            foreach (var anim in cutsceneRoot.GetComponentsInChildren<Animator>(true))
            {
                anim.enabled = true;
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                anim.Rebind();
                anim.Update(0f);
            }
        }
    }

    // Called when Timeline stops
    void OnCutsceneEnd(PlayableDirector d)
    {
        StartCoroutine(CleanupAfterTimeline());
    }

    private IEnumerator CleanupAfterTimeline()
    {
        // wait for one frame so timeline finishes fully
        yield return null;

        Time.timeScale = 1f; // unfreeze gameplay

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

        // Start the next stage (like your minigame)
        var transition = FindFirstObjectByType<SceneTransitionManager>();
        if (transition != null)
            transition.StartMinigame();
    }

    private void ResetCutscene()
    {
        if (cutsceneRoot == null) return;

        // ensure cutscene objects are off and animators reset
        foreach (var anim in cutsceneRoot.GetComponentsInChildren<Animator>(true))
        {
            anim.enabled = false;
            anim.Rebind();
            anim.Update(0f);
        }

        cutsceneRoot.SetActive(false);
    }
}
