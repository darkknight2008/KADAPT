using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree_part1 : MonoBehaviour
{
	public GameObject Hero;
    public GameObject Chief;
    public GameObject Oldman;
	public GameObject Villager1;
    public GameObject Villager2;
    public GameObject Villager3;
    public GameObject Villager4;
    public GameObject Villager5;
    public GameObject Villager6;

    private BehaviorAgent behaviorAgent;

    private Vector3 reach_oldman;
    private Vector3 reach_chief;
    private float dist_oldman;
    private float dist_chief;

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

    }

    // Update is called once per frame
    void Update ()
	{
        Vector3 hero_posi = Hero.GetComponent<Transform>().position;
        Vector3 oldman_posi = Oldman.GetComponent<Transform>().position;
        Vector3 chief_posi = Chief.GetComponent<Transform>().position;
        reach_oldman = 0.50f * oldman_posi + 0.50f * hero_posi;
        reach_chief = 0.20f * chief_posi + 0.80f * hero_posi;

        float dist_oldman = Vector3.Distance(Hero.transform.position, Oldman.transform.position);
        float dist_chief = Vector3.Distance(Hero.transform.position, Chief.transform.position);


    }
    public class canHeroask: Node
    {
        protected GameObject Hero;
        protected GameObject Oldman;
        public canHeroask(GameObject Hero, GameObject Oldman)
        {
            this.Hero = Hero;
            this.Oldman = Oldman;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, Oldman.transform.position) < 6)
                {
                    yield return RunStatus.Success;
                    //Hero.GetComponent<PlayerController2>().enabled = false;
                    UnityEngine.Debug.Log("turn");
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canask(GameObject Hero,GameObject Oldman)
    {
        return new canHeroask(Hero,Oldman);
    }
    public class waving : Node
    {
        protected GameObject Hero;
        //protected GameObject Oldman;
        public waving(GameObject Hero)
        {
            this.Hero = Hero;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator hero = Hero.GetComponent<Animator>();
            hero.SetTrigger("H_Wave");//waving
            //text?
            yield return RunStatus.Success;

        }
    }
    public class back2idle : Node
    {
        protected GameObject Chrc;
        public back2idle(GameObject Chrc)
        {
            this.Chrc = Chrc;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator chrc = Chrc.GetComponent<Animator>();
            chrc.SetTrigger("Idle");//Idle
            //Chrc.GetComponent<PlayerController2>().enabled = true;
            yield return RunStatus.Success;

        }
    }
    protected Node askdire(GameObject Hero, GameObject Oldman, Vector3 reach_oldman)
    {
        Val<Vector3> reach = Val.V(() => reach_oldman);
        return new Sequence(
            //Hero.GetComponent<BehaviorMecanim>().Node_GoTo(reach), 
            //new LeafWait(100),
            new waving(Hero),
            new LeafWait(500),
            new back2idle(Hero),
            new LeafWait(3000)
            );
    }
    public class turnreverse : Node
    {
        protected GameObject Oldman;
        public turnreverse(GameObject Oldman)
        {
            this.Oldman = Oldman;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator oldman = Oldman.GetComponent<Animator>();
            oldman.SetTrigger("turn_village");//turn 180 degrees
            yield return RunStatus.Success;

        }
    }
    public class pointingup : Node
    {
        protected GameObject Oldman;
        public pointingup(GameObject Oldman)
        {
            this.Oldman = Oldman;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator oldman = Oldman.GetComponent<Animator>();
            oldman.SetTrigger("H_LookUp");//LookUp
            yield return RunStatus.Success;
    }
    }
    protected Node pointdire(GameObject Oldman)
    {

        return new Sequence(
            new turnreverse(Oldman),
            new LeafWait(700),
            new pointingup(Oldman),
            new LeafWait(1000),
            new back2idle(Oldman)
            );
    }

    public class canmeetChief : Node
    {
        protected GameObject Hero;
        protected GameObject Chief;
        public canmeetChief(GameObject Hero, GameObject Chief)
        {
            this.Hero = Hero;
            this.Chief = Chief;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, Chief.transform.position) < 6)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canmeet(GameObject Hero,GameObject Chief)
    {
        return new canmeetChief(Hero, Chief);
    }
    public class bowing : Node
    {
        protected GameObject Hero;
        public bowing(GameObject Hero)
        {
            this.Hero = Hero;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator hero = Hero.GetComponent<Animator>();
            hero.SetTrigger("bow");//bowing
            //text?
            yield return RunStatus.Success;
        }
    }
    protected Node intro2self(GameObject Hero,Val<Vector3> reach_chief)
    {
        return new Sequence(
           // Hero.GetComponent<BehaviorMecanim>().Node_GoTo(reach_chief),
           // new LeafWait(100),
            new bowing(Hero),
            new LeafWait(700),
            new back2idle(Hero),
            new LeafWait(3000)
            );
    }
    public class requesting_down : Node
    {
        protected GameObject Chief;
        public requesting_down(GameObject Chief)
        {
            this.Chief = Chief;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator chief = Chief.GetComponent<Animator>();
            chief.SetTrigger("Request_down");//empty now
            //text?
            yield return RunStatus.Success;
        }
    }
    public class requesting_up : Node
    {
        protected GameObject Chief;
        public requesting_up(GameObject Chief)
        {
            this.Chief = Chief;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator chief = Chief.GetComponent<Animator>();
            chief.SetTrigger("Request_up");
            //text?
            yield return RunStatus.Success;
        }
    }
    protected Node request(GameObject Chief)
    {
        return new Sequence(
            new requesting_down(Chief),
            new LeafWait(4000),
            new requesting_up(Chief),
            new LeafWait(4000)
            );
    }
    public class bargaining : Node
    {
        protected GameObject Villager1;
        public bargaining(GameObject Villager1)
        {
            this.Villager1 = Villager1;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator v1 = Villager1.GetComponent<Animator>();
            v1.SetTrigger("talk_tasks");//talking
            yield return RunStatus.Success;
        }
    }
    public class thinking : Node
    {
        protected GameObject Villager2;
        public thinking(GameObject Villager2)
        {
            this.Villager2 = Villager2;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator v2 = Villager2.GetComponent<Animator>();
            v2.SetTrigger("H_Think");//think
            yield return RunStatus.Success;
        }
    }
    protected Node bargain(GameObject Villager1,GameObject Villager2)
    {
        return new SequenceParallel(
            new bargaining(Villager1),
            new Sequence(
                new LeafWait(3000),
                new thinking(Villager2)),
            new LeafWait(8000)
            );
    }
    public class quarrel_ind: Node
    {
        protected GameObject Villager;
        public quarrel_ind(GameObject Villager)
        {
            this.Villager = Villager;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator v = Villager.GetComponent<Animator>();
            v.SetTrigger("angry");
            yield return RunStatus.Success;
        }
    }
    public class cry_ind : Node
    {
        protected GameObject Villager;
        public cry_ind(GameObject Villager)
        {
            this.Villager = Villager;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator v = Villager.GetComponent<Animator>();
            v.SetTrigger("H_Cry");
            yield return RunStatus.Success;
        }
    }
    protected Node quarrelling(GameObject Villager3, GameObject Villager4)
    {
        return new SequenceParallel(
            new quarrel_ind(Villager3),
            new cry_ind(Villager4),
            new LeafWait(7000)
            );
    }

    public class chat_ld : Node
    {
        protected GameObject Villager;
        public chat_ld(GameObject Villager)
        {
            this.Villager = Villager;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator v = Villager.GetComponent<Animator>();
            v.SetTrigger("chat_ld");
            yield return RunStatus.Success;
        }
    }
    public class chat_qt : Node
    {
        protected GameObject Villager;
        public chat_qt(GameObject Villager)
        {
            this.Villager = Villager;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator v = Villager.GetComponent<Animator>();
            v.SetTrigger("chat_qt");
            yield return RunStatus.Success;
        }
    }
    protected Node chatting(GameObject Villager5, GameObject Villager6)
    {
        return new SequenceParallel(
           new chat_ld(Villager5),
           new Sequence(
               new LeafWait(3000),
               new chat_qt(Villager6)),
           new LeafWait(8000)
            );
    }
    protected Node BuildTreeRoot()
    {
        Node AskDire = new Sequence(
            new SuccessLoop(
                this.canask(Hero, Oldman)
                ),
            this.askdire(Hero,Oldman, reach_oldman),
            this.pointdire(Oldman)
            );
        Node Chiefstask = new Sequence(
            new SuccessLoop(
                this.canmeet(Hero, Chief)
                ),
            this.intro2self(Hero, reach_chief),
            this.request(Chief)
            );
        Node Shop = new DecoratorLoop(
            this.bargain(Villager1,Villager2)
            );
        Node Quarrel = new DecoratorLoop(
                this.quarrelling(Villager3,Villager4)
            );
        Node Chat = new DecoratorLoop(
            this.chatting(Villager5,Villager6)
            );
        //Node root = new DecoratorLoop(new Sequence
        Node root = new Sequence
            (
                //AskDire,
                new SelectorParallel
                (
                    new Sequence(
                        Chiefstask,
                        new LeafWait(100000)
                        ),
                    Shop,//loop
                    Quarrel,//loop
                    Chat //loop
                ),
                new LeafWait(100000000000)
           // )
            );
        return root;
    }
   
}
