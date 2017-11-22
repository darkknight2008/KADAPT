using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree_test : MonoBehaviour
{

    public GameObject King, Hero, Dying,Zombie;
    //public bool stWave1;
    //public bool stWave2;
    public Transform wander1, wander2, wander3, wander4;
    //public Transform meetC;
    //public Transform fetchBall;
    private Vector3 reach_posi;


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
        Vector3 zombie_posi = Zombie.GetComponent<Transform>().position;
        Vector3 hero_posi = Hero.GetComponent<Transform>().position;
        reach_posi = 0.20f * hero_posi + 0.80f * zombie_posi;
    }

 
    protected Node BuildTreeRoot()
    {
        //Node roaming =new DecoratorLoop( new SequenceParallel(new Sequence(this.wander(ChrA, wander1, wander2),new LeafWait(6000)),new Sequence(this.move(ChrB, meetC),this.sayHi(ChrB,ChrC), this.move(ChrB, fetchBall))));
        //Node roaming =new DecoratorLoop( new Sequence(this.Assign_task(King,Hero),new LeafWait(6000)));
        Node roaming = new DecoratorLoop(new Sequence(new SequenceParallel(this.wander(Hero,wander1,wander2), this.wander(Zombie, wander3, wander4)), this.Bite(Zombie,Hero),new LeafWait(1000)));
        //Node roaming = new DecoratorLoop(new Sequence(new SequenceParallel(this.wander(Zombie, wander1, wander2), this.wander(Hero, wander3, wander4)), new LeafWait(1000)));
        return roaming;
    }
    protected Node Bite(GameObject Zombie, GameObject Hero)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        return new Sequence(new LeafAssert(()=> this.Hero_stop(hero_ani)),this.Biting(Zombie,Hero),new LeafWait(200),this.HeroDies(hero_ani));
    }
    public bool Hero_stop(Animator hero)
    {
        hero.SetTrigger("Idle");
        return true;
    }
    protected Node Biting(GameObject Zombie,GameObject Hero)
    {
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        //Vector3 zombie_posi = Zombie.GetComponent<Transform>().position;
        //Vector3 hero_posi = Hero.GetComponent<Transform>().position;
        //Vector3 reach_posi = 0.50f * hero_posi+0.50f*zombie_posi;
        Val<Vector3> reach =Val.V(()=>reach_posi);
        return new Sequence(turn_move(Zombie, reach), new LeafAssert(()=> this.Bite_hero(zombie_ani)));
    }
    protected Node turn_move(GameObject Zombie,Val<Vector3> reach) 
    {
        return new Sequence(Zombie.GetComponent<BehaviorMecanim>().Node_GoTo(reach), new LeafWait(100));
    }
    public bool Bite_hero(Animator zombie)
    {
        zombie.SetTrigger("Bite");
        return true;
    }
    protected Node HeroDies(Animator hero)
    {
        return new Sequence(new LeafWait(500),new LeafAssert(()=> this.HeroDying(hero)));
    }
    public bool HeroDying(Animator hero)
    {
        hero.SetTrigger("B_Dying");
        return true;
    }

    protected Node wander(GameObject ppl0, Transform wander1, Transform wander2)
    {
        Animator animator0 = ppl0.GetComponent<Animator>();
        return new Sequence(ST_ApproachAndWait(ppl0, wander1), ST_ApproachAndWait(ppl0, wander2));
    }
    protected Node ST_ApproachAndWait(GameObject ppl, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(ppl.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(100));
    }
    protected Node Assign_task(GameObject King,GameObject Hero)
    {
        Animator king_ani = King.GetComponent<Animator>();
        Animator hero_ani = Hero.GetComponent<Animator>();
        return new Sequence(this.Greeting(king_ani),this.Bowing(hero_ani),this.Talking(king_ani),this.Kneeldown(hero_ani));
    }
    protected Node Greeting(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Greet(chr)), new LeafWait(1000),new LeafAssert(()=> this.StopGreeting(chr)));

    }
    public bool Greet(Animator m_Animator)

    {
        // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("H_Wave");
        return true;
    }
    public bool StopGreeting(Animator m_Animator)

    {
        // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("Idle");
        return true;
    }
    protected Node Bowing(Animator chr)
    {
        return new Sequence(new LeafAssert(()=> this.Bow(chr)),new LeafWait(1000));

    }
   public bool Bow(Animator m_Animator)

    {
       // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("bow");
        return true;
    }
    
    protected Node Talking(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Talk(chr)), new LeafWait(2500));

    }
    public bool Talk(Animator m_Animator)

    {
        // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("talk_tasks");
        return true;
    }
    protected Node Kneeldown(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Kneel(chr)), new LeafWait(1000));

    }
    public bool Kneel(Animator m_Animator)

    {
        // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("Kneel");
        return true;
    }

}

