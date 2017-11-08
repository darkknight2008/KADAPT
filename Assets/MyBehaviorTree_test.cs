using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree_test : MonoBehaviour
{

    public GameObject ChrA, ChrB, ChrC;
    public bool stWave1;
    public bool stWave2;
    public Transform wander1;
    public Transform wander2;
    public Transform meetC;
    public Transform fetchBall;
   

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

 
    protected Node BuildTreeRoot()
    {
        Node roaming =new DecoratorLoop( new SequenceParallel(new Sequence(this.wander(ChrA, wander1, wander2),new LeafWait(6000)),new Sequence(this.move(ChrB, meetC),this.sayHi(ChrB,ChrC), this.move(ChrB, fetchBall))));
        return roaming;
    }
    protected Node wander(GameObject ppl0, Transform wander1, Transform wander2)
    {
        Animator animator0 = ppl0.GetComponent<Animator>();
        return new Sequence(ST_ApproachAndWait(ppl0, wander1),new LeafAssert(()=> this.feelSad(animator0)), ST_ApproachAndWait(ppl0, wander2),new LeafAssert(()=> this.crying(animator0)));
    }
    protected Node ST_ApproachAndWait(GameObject ppl,Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(ppl.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(100));
    }
    public bool feelSad(Animator animator)
    {
        AnimatorStateInfo state= animator.GetCurrentAnimatorStateInfo(0);
        bool isSad = state.IsName("IdleSad");
        animator.SetTrigger("FeelSad");
        /*if (isSad)
        {
            animator.SetTrigger("Idle");
        }*/
        animator.SetTrigger("Idle");
        return true;
    }
    public bool crying(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isCry = state.IsName("Cry");
        animator.SetTrigger("H_Cry");
        animator.SetTrigger("Idle");
        return true;
    }
    protected Node move(GameObject people, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }
    protected Node sayHi(GameObject ppl1, GameObject ppl2)
    {
        Animator animator1 = ppl1.GetComponent<Animator>();
        Animator animator2 = ppl2.GetComponent<Animator>();
        return new Sequence(this.Greeting(animator1, animator2),new LeafWait(1000), this.Talking(animator1),this.response(animator2));
    }
    protected Node Greeting(Animator animator1, Animator animator2)
    {
        return new Sequence(new LeafAssert(() => this.Greet(animator1,1)),new LeafWait(1000), new LeafAssert(() => this.Greet(animator2,2)),new LeafWait(2000));
    }
    protected Node Talking(Animator animator1)
    {
        return new SelectorParallel(new LeafAssert(() => this.pointing(animator1)), new LeafWait(800));
        //new LeafInvoke(() => this.pointing(animator1))
    }
    protected Node response(Animator animator2)
    {
        return new Sequence(new LeafAssert(()=> this.responsing(animator2)),new LeafWait(1000));
    }
    public bool Greet(Animator m_Animator, int index)
    {
        AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0);;
        if (index == 1)
        {
           // if (stWave1)
           // {
                m_Animator.SetTrigger("H_Wave");
                stWave1 = false;
           // }
        }
        else
        {
           // if (stWave2)
           // {
                m_Animator.SetTrigger("H_Wave");
                stWave2 = false;
           // }
        }
        return true;
    }
    public bool pointing(Animator m_Animator1)
    {
        AnimatorStateInfo state1 = m_Animator1.GetCurrentAnimatorStateInfo(0);
        bool isPoint = state1.IsName("LookUp");

        m_Animator1.SetTrigger("H_LookUp");
        //if (isPoint)
        //{
            m_Animator1.SetTrigger("Idle");
        //}
        return true;
    
    }
    public bool responsing(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isThink = state.IsName("Think");
        animator.SetTrigger("H_Think");
       if (isThink)
        {
            animator.SetTrigger("Idle");
        }
        return true;
    }
}

