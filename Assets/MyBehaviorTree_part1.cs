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
    public GameObject Oldlady;
	public GameObject Villager1;
    public GameObject Villager2;
    public GameObject Villager3;
    public GameObject Villager4;
    public GameObject Villager5;
    public GameObject Villager6;

    private BehaviorAgent behaviorAgent;

    private Val<Vector3> reach_oldlady;
    private Val<Vector3> reach_chief;

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
        Vector3 oldlady_posi = Oldlady.GetComponent<Transform>().position;
        Vector3 chief_posi = Chief.GetComponent<Transform>().position;
        reach_oldlady = Val.V(() => (0.20f * oldlady_posi + 0.80f * hero_posi)) ;
        reach_chief = Val.V(() => (0.20f * chief_posi + 0.80f * hero_posi));

    }
    public class canHeroask: Node
    {
        protected GameObject Hero;
        protected GameObject Oldlady;
        public canHeroask(GameObject Hero, GameObject Oldlady)
        {
            this.Hero = Hero;
            this.Oldlady = Oldlady;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, Oldlady.transform.position) < 6)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canask(GameObject Hero,GameObject Oldlady)
    {
        return new canHeroask(Hero,Oldlady);
    }
    public class waving : Node
    {
        protected GameObject Hero;
        protected GameObject Oldlady;
        public waving(GameObject Hero)
        {
            this.Hero = Hero;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator hero = Hero.GetComponent<Animator>();
            hero.SetTrigger("H_wave");//waving
            //text?
            yield return RunStatus.Success;

        }
    }
    protected Node askdire(GameObject Hero, GameObject Oldlady, Val<Vector3> reach_oldlady)
    {

        return new Sequence(
            Hero.GetComponent<BehaviorMecanim>().Node_GoTo(reach_oldlady), 
            new LeafWait(100),
            new waving(Hero),
            new LeafWait(1000)
            );
    }
    public class turnreverse : Node
    {
        protected GameObject Oldlady;
        public turnreverse(GameObject Oldlady)
        {
            this.Oldlady = Oldlady;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator oldlady = Oldlady.GetComponent<Animator>();
            oldlady.SetTrigger("turn_village");//turn 180 degrees
            yield return RunStatus.Success;

        }
    }
    public class pointingup : Node
    {
        protected GameObject Oldlady;
        public pointingup(GameObject Oldlady)
        {
            this.Oldlady = Oldlady;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator oldlady = Oldlady.GetComponent<Animator>();
            oldlady.SetTrigger("H_LookUp");//LookUp
            yield return RunStatus.Success;
    }
    }
    protected Node pointdire(GameObject Oldlady)
    {

        return new Sequence(
            new turnreverse(Oldlady),
            new LeafWait(1000),
            new pointingup(Oldlady),
            new LeafWait(1000)
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
            Hero.GetComponent<BehaviorMecanim>().Node_GoTo(reach_chief),
            new LeafWait(100),
            new bowing(Hero),
            new LeafWait(1000)
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
                new LeafWait(1000),
                new thinking(Villager2))
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
            new cry_ind(Villager4)
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
               new LeafWait(1500),
               new chat_qt(Villager6))
            );
    }
    protected Node BuildTreeRoot()
    {
        Node AskDire = new Sequence(
            new SuccessLoop(
                this.canask(Hero, Oldlady)
                ),
            this.askdire(Hero,Oldlady, reach_oldlady),
            this.pointdire(Oldlady)
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
        Node root = new DecoratorLoop(new Sequence
        //Node root = new Sequence
            (
                AskDire,
                new SelectorParallel
                (
                    new Sequence(
                        Chiefstask,
                        new LeafWait(100000)
                        ),
                    Shop,//loop
                    Quarrel,//loop
                    Chat //loop
                )
                //new LeafWait(100000000000)
            )
            );
        return root;
    }
   
}
