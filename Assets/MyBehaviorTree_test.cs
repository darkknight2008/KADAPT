using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree_test : MonoBehaviour
{

    public GameObject King, Hero, Dying, Zombie;
    public Transform wander1, wander2, wander3, wander4;
    public bool Task = false;
    public bool Passwords = false;
    public bool Failure = false;
    public bool Success = true;
    private Vector3 reach_posi;

    public Text failtext;
    public Text winText;
    public Text assignText;
    public Text dyingText;
    public Camera cam;

    

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
        failtext.text = "";
        winText.text = "";
        assignText.text = "";
        dyingText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 zombie_posi = Zombie.GetComponent<Transform>().position;
        Vector3 hero_posi = Hero.GetComponent<Transform>().position;
        reach_posi = 0.20f * hero_posi + 0.80f * zombie_posi;
        if (Success == true)
        {
            winText.text = "Congratulations!";
        }
        if (Failure == true)
        {
            failtext.text = "OOPS, Zombie killed you!";
        }
        if (Task == true)
        {
            PositionTransKing(assignText);
            assignText.text = "HERO, Eric knew where to find sword, help him!";
        }
        else {
            PositionTransKing(assignText);
            assignText.text = "";
        }
        if (Passwords == true)
        {
            PositionTransDead(dyingText);
            dyingText.text = "I can't go with you, go and get the sword!!!";
        }
        else
        {
            PositionTransDead(dyingText);
            dyingText.text = "";
        }
    }

    public void PositionTransKing(Text assignText)
    {
        Vector3 worldPosition = new Vector3(King.transform.position.x, King.transform.position.y, King.transform.position.z);
        Vector2 position = cam.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, position.y);
        assignText.transform.position = position;
    }

    public void PositionTransDead(Text dyingText)
    {
        Vector3 worldPosition = new Vector3(Dying.transform.position.x, Dying.transform.position.y, Dying.transform.position.z);
        Vector2 position = cam.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, position.y);
        dyingText.transform.position = position;
    }

    protected Node BuildTreeRoot()
    {
        //Node roaming =new DecoratorLoop( new SequenceParallel(new Sequence(this.wander(ChrA, wander1, wander2),new LeafWait(6000)),new Sequence(this.move(ChrB, meetC),this.sayHi(ChrB,ChrC), this.move(ChrB, fetchBall))));
        //Node roaming = new DecoratorLoop(new Sequence(this.Assign_task(King, Hero), new LeafAssert(()=> this.StopWorking(Hero.GetComponent<Animator>()))));
        //new SequenceParallel(new LeafAssert(()=> this.StopWorking(Hero.GetComponent<Animator>())),new LeafWait(70000))this.wander(Hero, wander1, wander2)
        //Node roaming = new DecoratorLoop(new Sequence(this.wander(Hero, wander1, wander2), this.Salute(Hero, Dying), this.Tell(Hero,Dying)));

        //Three animations
        //Node roaming = new DecoratorLoop(new Sequence(this.Assign_task(King, Hero), new LeafAssert(()=> this.StopWorking(Hero.GetComponent<Animator>()))));
        Node roaming = new DecoratorLoop(new Sequence(new SequenceParallel(new LeafWait(2000), this.wander(Zombie, wander3, wander4)), this.Bite(Zombie, Hero), new LeafWait(1000),new LeafAssert(()=> this.GameOver())));
        //Node roaming = new DecoratorLoop(new Sequence(this.Salute(Hero, Dying), this.Tell(Hero, Dying)));
        return roaming;
    }
    protected Node Salute(GameObject Hero, GameObject Dying)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator dying_ani = Dying.GetComponent<Animator>();
        return new SequenceParallel(new LeafAssert(() => this.saluting(hero_ani)), new LeafAssert(() => this.saluting(dying_ani)));
    }
    public bool saluting(Animator chr)
    {
        chr.SetTrigger("Salute");
        return true;
    }
    protected Node Tell(GameObject Hero,GameObject Dying)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator dying_ani = Dying.GetComponent<Animator>();
        return new Sequence(new LeafWait(2000),new LeafAssert(()=> this.Telling_secret(dying_ani)),new LeafWait(8000),new LeafAssert(()=> this.StopWorking(hero_ani)),new LeafWait(1500));
        
    }
    public bool Telling_secret(Animator dying_ani)
    {
        dying_ani.SetTrigger("Tell_secret");
        Passwords = true;
        return true;
    }
    protected Node Bite(GameObject Zombie, GameObject Hero)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.Hero_stop(hero_ani)), this.Biting(Zombie, Hero), new LeafWait(200), this.HeroDies(hero_ani));
    }
    public bool Hero_stop(Animator hero)
    {
        hero.SetTrigger("Idle");
        return true;
    }
    protected Node Biting(GameObject Zombie, GameObject Hero)
    {
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        Val<Vector3> reach = Val.V(() => reach_posi);
        return new Sequence(turn_move(Zombie, reach), new LeafAssert(() => this.Bite_hero(zombie_ani)));
    }
    protected Node turn_move(GameObject Zombie, Val<Vector3> reach)
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
        return new Sequence(new LeafWait(500), new LeafAssert(() => this.HeroDying(hero)));
    }
    public bool HeroDying(Animator hero)
    {
        hero.SetTrigger("B_Dying");
        return true;
    }
    public bool GameOver()
    {
        Failure = true;
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
    protected Node Assign_task(GameObject King, GameObject Hero)
    {
        Animator king_ani = King.GetComponent<Animator>();
        Animator hero_ani = Hero.GetComponent<Animator>();
        return new Sequence(this.Greeting(king_ani), this.Bowing(hero_ani), this.Talking(king_ani), this.Kneeldown(hero_ani));
    }
    protected Node Greeting(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Greet(chr)), new LeafWait(1000), new LeafAssert(() => this.StopWorking(chr)));

    }
    public bool Greet(Animator m_Animator)

    {
        // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("H_Wave");
        return true;
    }
    public bool StopWorking(Animator m_Animator)

    {
        // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("Idle");
        if (m_Animator == Hero.GetComponent<Animator>())
        {
            Passwords = false;
        }
        return true;
    }
    protected Node Bowing(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Bow(chr)), new LeafWait(100));

    }
    public bool Bow(Animator m_Animator)

    {
        m_Animator.SetTrigger("bow");
        return true;
    }

    protected Node Talking(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Talk(chr)), new LeafWait(4000),new LeafAssert(()=> this.Say(chr)),new LeafWait(2000));

    }
    public bool Talk(Animator m_Animator)

    {
        // AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        m_Animator.SetTrigger("talk_tasks");
        return true;
    }
    public bool Say(Animator m_Animator)

    {
        Task = true;
        return true;
    }
    protected Node Kneeldown(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Kneel(chr)), new LeafWait(7000));

    }
    public bool Kneel(Animator m_Animator)

    {
        m_Animator.SetTrigger("Kneel");
        Task = false;
        return true;
    }

}

