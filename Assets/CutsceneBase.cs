using UnityEngine;
using UnityEngine.Playables;

public class CutsceneBase : MonoBehaviour
{
    protected bool playedOnce = false;
    public PlayableDirector cutSceneTimeLine;

    protected virtual void Start()
    {
        // cutSceneTimeLine.enabled = false;
        // cutSceneTimeLine.gameObject.SetActive(false);

        // GameManager.Instance.StateMachine.OnStateChange += CutsceneHandler;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkipCutscene();
        }
    }

    public virtual void CutSceneComplete()
    {
        GameManager.Instance.ChangeState(StateEnum.PlayState);
        cutSceneTimeLine.enabled = false;
        cutSceneTimeLine.gameObject.SetActive(false);
        // GetComponent<Collider>().enabled = false;
    }

    protected virtual void CutSceneEnter()
    {
        if (!playedOnce)
        {
            playedOnce = true;
        }
        else
            return;

        GameManager.Instance.ChangeState(StateEnum.PauseState);
        cutSceneTimeLine.gameObject.SetActive(true);
        cutSceneTimeLine.enabled = true;
        cutSceneTimeLine.Play();
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         CutSceneEnter(other.GetComponent<PlayerController>());
    //     }
    // }


    // private void CutsceneHandler(object sender, State targetState)
    // {
    //     switch (targetState)
    //     {
    //         case StartState _:
    //             {
    //                 break;
    //             }
    //         case PauseState _:
    //             {
    //                 break;
    //             }
    //         case PlayState _:
    //             {


    //                 break;
    //             }
    //         case FinishState _:
    //             {
    //                 break;
    //             }
    //     }
    // }


    public void SkipCutscene()
    {
        CutSceneComplete();
        this.enabled = false;
        gameObject.SetActive(false);
    }
}
