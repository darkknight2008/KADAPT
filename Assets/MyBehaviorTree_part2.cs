using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree_part2 : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
    public Transform wander4;
    public Transform wander5;
    public Transform wander6;
    public Transform wander7;
    public float radius;

    public GameObject door;
    public GameObject sword;
	public GameObject Hero;
    public GameObject Dying;
    public GameObject Zombie1;
    public GameObject Zombie2;
    public GameObject Zombie3;
    public GameObject Zombie4;
    public GameObject Zombie5;
    public GameObject Zombie6;
    public GameObject Zombie7;
    public bool keyGot = false;

    //Text boolean variables
    public bool Passwords = false;
    public bool Failure = false;
    public bool Success = false;

    //Text
    //public Text winText;
    public Text failtext;
    public Text dyingText;
    public Camera cam;
    private DoorHoldOn D;

    private Vector3 reach_posi;
    private Vector3 reach_posi1;
    private Vector3 reach_posi2;
    private Vector3 reach_posi3;
    private Vector3 reach_posi4;
    private Vector3 reach_posi5;
    private Vector3 reach_posi6;
    private Vector3 reach_posi7;


    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

        //text
        failtext.text = "";
        dyingText.text = "";
        D = door.GetComponent<DoorHoldOn>();
        Hero.GetComponent<PlayerController2>().enabled = true;
    }

    // Update is called once per frame
    void Update ()
	{
        Vector3 zombie1_posi = Zombie1.GetComponent<Transform>().position;
        Vector3 zombie2_posi = Zombie2.GetComponent<Transform>().position;
        Vector3 zombie3_posi = Zombie3.GetComponent<Transform>().position;
        Vector3 zombie4_posi = Zombie4.GetComponent<Transform>().position;
        Vector3 zombie5_posi = Zombie5.GetComponent<Transform>().position;
        Vector3 zombie6_posi = Zombie6.GetComponent<Transform>().position;
        Vector3 zombie7_posi = Zombie7.GetComponent<Transform>().position;
        Vector3 hero_posi = Hero.GetComponent<Transform>().position;
        reach_posi1 = 0.20f * hero_posi + 0.80f * zombie1_posi;
        reach_posi2 = 0.20f * hero_posi + 0.80f * zombie2_posi;
        reach_posi3 = 0.20f * hero_posi + 0.80f * zombie3_posi;
        reach_posi4 = 0.20f * hero_posi + 0.80f * zombie4_posi;
        reach_posi5 = 0.20f * hero_posi + 0.80f * zombie5_posi;
        reach_posi6 = 0.20f * hero_posi + 0.80f * zombie6_posi;
        reach_posi7 = 0.20f * hero_posi + 0.80f * zombie7_posi;

        if (Failure == true)
        {
            failtext.text = "OOPS, Zombie killed you!";
        }
        if (Passwords == true)
        {
            PositionTransDead(dyingText);
            dyingText.text = "I can't go with you, go and get the sword!!! ALICE's life is counting on you.";
        }
        else
        {
            PositionTransDead(dyingText);
            dyingText.text = "";
        }
	if (keyGot==true)
        {
            D.key_get = true;
        }
    }

    public class canGotBite : Node
    {
        protected GameObject zombie;
        protected GameObject Hero;
        public canGotBite(GameObject zombie, GameObject Hero)
        {
            this.Hero = Hero;
            this.zombie = zombie;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, zombie.transform.position) < 5)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canBite(GameObject zombie, GameObject Hero)
    {
        return new canGotBite(zombie, Hero);
    }

    public class canTheyTalk : Node
    {
        protected GameObject Hero;
        protected GameObject Dying;
        public canTheyTalk(GameObject Hero, GameObject Dying)
        {
            this.Hero = Hero;
            this.Dying = Dying;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, Dying.transform.position) < 3)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canTalk(GameObject Hero, GameObject Dying)
    {
        return new canTheyTalk(Hero, Dying);
    }

    public class canGetSword : Node
    {
        protected GameObject Hero;
        protected GameObject sword;
        public canGetSword(GameObject Hero, GameObject sword)
        {
            this.Hero = Hero;
            this.sword = sword;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, sword.transform.position) < 3)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node getSword(GameObject Hero, GameObject sword)
    {
        return new canGetSword(Hero, sword);
    }
    public class canDoorOpen : Node
    {
        protected GameObject Hero;
        protected GameObject door;
        protected bool keyGot;
        public canDoorOpen(GameObject Hero, GameObject door, bool keyGot)
        {
            this.Hero = Hero;
            this.door = door;
            this.keyGot = keyGot;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, door.transform.position) < 2 && keyGot == true)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canOpenDoor(GameObject Hero, GameObject door, bool keyGot)
    {
        return new canDoorOpen(Hero, door, keyGot);
    }

    public class nowGetKey : Node
    {
        protected bool keyGot;
        public nowGetKey(bool keyGot)
        {
            this.keyGot = keyGot;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            keyGot = true;
            yield return RunStatus.Success;
        }
    }
    protected Node switchKey(bool keyGot)
    {
        return new nowGetKey(keyGot);
    }

    public class nowSwitchDoor : Node
    {
        protected GameObject door;
        public nowSwitchDoor(GameObject door)
        {
            this.door = door;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            //door.key_got = true;
            yield return RunStatus.Success;
        }
    }
    protected Node switchDoor(GameObject door)
    {
        return new nowSwitchDoor(door);
    }

    protected Node BuildTreeRoot()
    {
        Node zombie1wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie1, this.wander1, 1.0f, 1000)
                        ),
                        this.canBite(Zombie1, Hero)
                    )
                ),
                this.ZombieBite(Zombie1, Hero),
                new LeafWait(100000)
            );
        Node zombie2wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie2, this.wander2, 1.0f, 1000)
                        ),
                        this.canBite(Zombie2, Hero)
                    )
                ),
                this.ZombieBite(Zombie2, Hero),
                new LeafWait(100000)
            );
        Node zombie3wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie3, this.wander3, 1.0f, 1000)

                        ),
                        this.canBite(Zombie3, Hero)
                    )
                ),
                this.ZombieBite(Zombie3, Hero),
                new LeafWait(100000)
            );
        Node zombie4wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie4, this.wander4, 1.0f, 1000)

                        ),
                        this.canBite(Zombie4, Hero)
                    )
                ),
                this.ZombieBite(Zombie4, Hero),
                new LeafWait(100000)
            );
        Node zombie5wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie5, this.wander5, 1.0f, 1000)

                        ),
                        this.canBite(Zombie5, Hero)
                    )
                ),
                this.ZombieBite(Zombie5, Hero),
                new LeafWait(100000)
            );
        Node zombie6wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie6, this.wander6, 1.0f, 1000)

                        ),
                        this.canBite(Zombie6, Hero)
                    )
                ),
                this.ZombieBite(Zombie6, Hero),
                new LeafWait(100000)
            );
        Node zombie7wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie7, this.wander7, 1.0f, 1000)

                        ),
                        this.canBite(Zombie7, Hero)
                    )
                ),
                this.ZombieBite(Zombie7, Hero),
                new LeafWait(100000)
            );
        Node openDoor = new Sequence
            (
                new SuccessLoop
                (
                    this.canOpenDoor(Hero, door, keyGot)
                ),
                this.switchDoor(door),
                new LeafWait(10000000000000)
            );
        Node getKey = new Sequence
            (
                new SuccessLoop
                (
                    this.canTalk(Hero, Dying)
                ),
                this.TellKey(Hero, Dying),
                this.switchKey(keyGot),
                new LeafWait(1000000000000)
            );
        Node getSword = new Sequence
            (
                new SuccessLoop
                (
                    this.getSword(Hero, sword)
                ),
                //new LeafAssert(()=> this.HE())
                new LeafWait(100)
            );

        //Node root = new DecoratorLoop( new Sequence
        Node root = new Sequence
            (
                new SuccessLoop
                (
                     new SelectorParallel
                     (
                          openDoor,
                          getKey,
                          zombie1wander,
                          zombie2wander,
                          zombie3wander,
                          zombie4wander,
                          zombie5wander,
                          zombie6wander,
                          zombie7wander,
                          //new LeafWait(10000000000000)
                          getSword
                    )
                ),
                //new LeafWait(100000000000)
                new LeafWait(2000)
            );
        return root;
    }
    //Text
    public void PositionTransDead(Text dyingText)
    {
        Vector3 worldPosition = new Vector3(Dying.transform.position.x, Dying.transform.position.y, Dying.transform.position.z)+new Vector3(0,1.5f,0);
        Vector2 position = cam.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, position.y);
        dyingText.transform.position = position;
    }
    public bool HE()
    {
        Success = true;
        return true;
    }
    public bool StopWorking(Animator m_Animator)

    {
        m_Animator.SetTrigger("Idle");
        if (m_Animator == Hero.GetComponent<Animator>())
        {
            Passwords = false;
        }
        return true;
    }

    //Zombie bites
    protected Node ZombieBite(GameObject Zombie, GameObject Hero)
    {
        return new Sequence(
            this.Bite(Zombie, Hero), 
            new LeafWait(1000), 
            new LeafAssert(() => this.GameOver()));
    }
    protected Node Bite(GameObject Zombie, GameObject Hero)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        return new Sequence(
            new LeafAssert(() => this.Hero_stop(hero_ani)), 
            this.Biting(Zombie, Hero),
            new LeafWait(200), this.HeroDies(hero_ani));
    }
    public bool Hero_stop(Animator hero)
    {
        hero.SetTrigger("Idle");
        Hero.GetComponent<PlayerController2>().enabled = false;
        return true;
    }
    protected Node Biting(GameObject Zombie, GameObject Hero)
    {
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        Val<Vector3> reach;
        if (Zombie == Zombie1)
        {
            reach = Val.V(() => reach_posi1);
            return new Sequence(
                turn_move(Zombie, reach), 
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }else if (Zombie == Zombie2)
        {
            reach = Val.V(() => reach_posi2);
            return new Sequence(
                turn_move(Zombie, reach), 
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }
        else if(Zombie == Zombie3)
        {
            reach = Val.V(() => reach_posi3);
            return new Sequence(
                turn_move(Zombie, reach), 
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }
        else if (Zombie == Zombie4)
        {
            reach = Val.V(() => reach_posi4);
            return new Sequence(
                turn_move(Zombie, reach), 
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }
        else if (Zombie == Zombie5)
        {
            reach = Val.V(() => reach_posi5);
            return new Sequence(
                turn_move(Zombie, reach), 
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }
        else if (Zombie == Zombie6)
        {
            reach = Val.V(() => reach_posi6);
            return new Sequence(
                turn_move(Zombie, reach), 
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }
        else
        {
            reach = Val.V(() => reach_posi7);
            return new Sequence(
                turn_move(Zombie, reach), 
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }
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

    //Tell key
    protected Node TellKey(GameObject Hero, GameObject Dying)
    {
        return new Sequence(
            this.Salute(Hero, Dying), 
            this.Tell(Hero, Dying));
    }
    protected Node Salute(GameObject Hero, GameObject Dying)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator dying_ani = Dying.GetComponent<Animator>();
        return new SequenceParallel(
            new LeafAssert(() => this.saluting(hero_ani)), 
            new LeafAssert(() => this.saluting(dying_ani))
            );
    }
    public bool saluting(Animator chr)
    {
        chr.SetTrigger("Salute");
        Hero.GetComponent<PlayerController2>().enabled = false;
        return true;
    }
    protected Node Tell(GameObject Hero, GameObject Dying)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator dying_ani = Dying.GetComponent<Animator>();
        return new Sequence(
            new LeafWait(2000), 
            new LeafAssert(() => this.Telling_secret(dying_ani)), 
            new LeafWait(8000), 
            new LeafAssert(() => this.StopWorking(hero_ani)), 
            new LeafWait(1500),
            new LeafAssert(()=> this.HeroBack()));

    }
    public bool Telling_secret(Animator dying_ani)
    {
        dying_ani.SetTrigger("chat_qt");
        Passwords = true;
        keyGot = true;
        return true;
    }
    public bool HeroBack()
    {
        Hero.GetComponent<PlayerController2>().enabled = true;
        return true;
    }
    //Random walk
    protected Node Randomwalk(GameObject people, Transform center, float radius, int waitingTime)
    {
        Val<Vector3> position = Val.V(() => center.position);
        return new Sequence(
            people.GetComponent<BehaviorMecanim>().Node_GoToRandom(position, radius),
            new DecoratorInvert(
                new DecoratorLoop(
                    new LeafAssert(
                        () => Vector3.Distance(people.transform.position, people.GetComponent<UnityEngine.AI.NavMeshAgent>().destination) > 0.1))),
            new LeafWait(waitingTime));
    }
}
