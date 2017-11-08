using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree_test : MonoBehaviour
{

    public GameObject ChrA, ChrB, ChrC;
    public bool stWave1;
    public bool stWave2;

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
        Node roaming = new DecoratorLoop(new Sequence(this.sayHi(ChrB,ChrC),new LeafWait(100)));
        //return this.sayHi(ChrB, ChrC);
        return roaming;
    }
    protected Node sayHi(GameObject ppl1, GameObject ppl2)
    {
        Animator animator1 = ppl1.GetComponent<Animator>();
        Animator animator2 = ppl2.GetComponent<Animator>();
        return new Sequence(this.Greeting(animator1, animator2),new LeafWait(1000), this.Talking(animator1),this.response(animator2));
    }
    protected Node Greeting(Animator animator1, Animator animator2)
    {
        //return new SelectorParallel(new LeafInvoke(() => this.Greet(animator1, animator2)),new LeafWait(100));
        return new Sequence(new LeafAssert(() => this.Greet(animator1,1)),new LeafWait(1000), new LeafAssert(() => this.Greet(animator2,2)));
        //return new LeafAssert(() => this.Greet(animator1,animator2));
    }
    protected Node Talking(Animator animator1)
    {
        return new SelectorParallel(new LeafAssert(() => this.pointing(animator1)), new LeafWait(1000));
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
            if (stWave1)
            {
                m_Animator.SetTrigger("H_Wave");
                stWave1 = false;
            }
        }
        else
        {
            if (stWave2)
            {
                m_Animator.SetTrigger("H_Wave");
                stWave2 = false;
            }
        }
        return true;
    }
    public bool pointing(Animator m_Animator1)
    {
        AnimatorStateInfo state1 = m_Animator1.GetCurrentAnimatorStateInfo(0);
        bool isPoint = state1.IsName("LookUp");

        m_Animator1.SetTrigger("H_LookUp");
        if (isPoint)
        {
            m_Animator1.SetTrigger("Idle");
        }
        return true;
    
    }
    public bool responsing(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isSurprise = state.IsName("Surprised");
        animator.SetTrigger("H_Think");
        return true;
    }
}

