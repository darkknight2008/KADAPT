using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree_full : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
    public Transform wander4;
    public Transform wander5;
    public Transform wander6;
    public float radius;

    public GameObject door;
    public GameObject sword;
	public GameObject Hero;
	public GameObject King;
    public GameObject Dying;
    public GameObject Zombie1;
    public GameObject Zombie2;
    public GameObject Zombie3;
    public bool keyGot = false;


    //Text boolean variables
    public bool Task = false;
    public bool Passwords = false;
    public bool Failure = false;
    public bool Success = true;

    //Text
    public Text winText;
    public Text failtext;
    public Text assignText;
    public Text dyingText;
    public Camera cam;

    private Vector3 reach_posi;
    private Vector3 reach_posi1;
    private Vector3 reach_posi2;
    private Vector3 reach_posi3;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        //KeepData.keepLevelName = SceneManager.GetActiveScene().name;
        //DontDestroyOnLoad(King);
        //DontDestroyOnLoad(Hero);

        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

        //text
        failtext.text = "";
        winText.text = "";
        assignText.text = "";
        dyingText.text = "";
    }

    // Update is called once per frame
    void Update ()
	{
        Vector3 zombie1_posi = Zombie1.GetComponent<Transform>().position;
        Vector3 zombie2_posi = Zombie2.GetComponent<Transform>().position;
        Vector3 zombie3_posi = Zombie3.GetComponent<Transform>().position;
        Vector3 hero_posi = Hero.GetComponent<Transform>().position;
        reach_posi1 = 0.20f * hero_posi + 0.80f * zombie1_posi;
        reach_posi2 = 0.20f * hero_posi + 0.80f * zombie2_posi;
        reach_posi3 = 0.20f * hero_posi + 0.80f * zombie3_posi;
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
        else
        {
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
                if (Vector3.Distance(Hero.transform.position, zombie.transform.position) < 6)
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
                if (Vector3.Distance(Hero.transform.position, Dying.transform.position) < 4)
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
                if (Vector3.Distance(Hero.transform.position, sword.transform.position) < 4)
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
                if (Vector3.Distance(Hero.transform.position, door.transform.position) < 4 && keyGot == true)
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

    //public class isHE : Node
    //{
    //    protected Text winText;
    //    public isHE(Text winText)
    //    {
    //        this.winText = winText;
    //    }
    //    public override IEnumerable<RunStatus> Execute()
    //    {
    //        winText.end = true;
    //        yield return RunStatus.Success;
    //    }
    //}
    //protected Node HE(Text winText)
    //{
    //    return new isHE(winText);
    //}

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
                            this.Randomwalk(Zombie1, this.wander1, 3.0f, 1000)
                            //this.wander(Zombie1,wander1,wander2)
                        ),
                        this.canBite(Zombie1, Hero)
                    )
                ),
                this.ZombieBite(Zombie1, Hero)
            );
        Node zombie2wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie2, this.wander3, 3.0f, 1000)
                            //this.wander(Zombie2, wander3, wander4)
                        ),
                        this.canBite(Zombie2, Hero)
                    )
                ),
                this.ZombieBite(Zombie2, Hero)
            );
        Node zombie3wander = new Sequence
            (
                new SuccessLoop
                (
                    new SelectorParallel
                    (
                        new DecoratorLoop
                        (
                            this.Randomwalk(Zombie3, this.wander5, 3.0f, 1000)
                            //this.wander(Zombie3, wander5, wander6)

                        ),
                        this.canBite(Zombie3, Hero)
                    )
                ),
                this.ZombieBite(Zombie3, Hero)
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
                new LeafAssert(()=> this.HE())
            );

        //Node root = new DecoratorLoop( new Sequence
        Node root = new Sequence
            (
                this.task(King, Hero),
                new SuccessLoop
                (
                     new SelectorParallel
                     (
                          openDoor,
                          getKey,
                          zombie1wander,
                          zombie2wander,
                          zombie3wander,
                          //new LeafWait(10000000000000)
                          getSword
                    )
                ),
                new LeafWait(100000000000)
                //new LeafWait(2000))
            );
        return root;
    }
    //Text
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
    public bool HE()
    {
        Success = true;
        return true;
    }
    // task
    protected Node task(GameObject King,GameObject Hero)
    {
        return new Sequence(this.MeetKing(Hero,King),this.Assign_task(King, Hero), new LeafAssert(() => this.StopWorking(Hero.GetComponent<Animator>())));
    }
    public class canMeetKing : Node
    {
        protected GameObject Hero;
        protected GameObject King;
        public canMeetKing(GameObject Hero, GameObject King)
        {
            this.Hero = Hero;
            this.King = King;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, King.transform.position) < 4)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node MeetKing(GameObject Hero, GameObject King)
    {
        return new canMeetKing(Hero, King);
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
        return new Sequence(new LeafAssert(() => this.Talk(chr)), new LeafWait(4000), new LeafAssert(() => this.Say(chr)), new LeafWait(2000));

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
        //Hero.GetComponent<Animator>().SetTrigger("Idle");
        return true;
    }
    protected Node Kneeldown(Animator chr)
    {
        return new Sequence(new LeafAssert(() => this.Kneel(chr)), new LeafWait(4000));

    }
    public bool Kneel(Animator m_Animator)

    {
        m_Animator.SetTrigger("Kneel");
        Task = false;
        return true;
    }

    //Zombie bites
    protected Node ZombieBite(GameObject Zombie, GameObject Hero)
    {
        return new Sequence(this.Bite(Zombie, Hero), new LeafWait(1000), new LeafAssert(() => this.GameOver()));
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
        Val<Vector3> reach;
        if (Zombie == Zombie1)
        {
            reach = Val.V(() => reach_posi1);
            return new Sequence(turn_move(Zombie, reach), new LeafAssert(() => this.Bite_hero(zombie_ani)));
        }else if (Zombie == Zombie2)
        {
            reach = Val.V(() => reach_posi2);
            return new Sequence(turn_move(Zombie, reach), new LeafAssert(() => this.Bite_hero(zombie_ani)));
        }
        else
        {
            reach = Val.V(() => reach_posi3);
            return new Sequence(turn_move(Zombie, reach), new LeafAssert(() => this.Bite_hero(zombie_ani)));
        }
        //reach = Val.V(() => reach_posi1);
        //return new Sequence(turn_move(Zombie, reach), new LeafAssert(() => this.Bite_hero(zombie_ani)));
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
        return new Sequence(this.Salute(Hero, Dying), this.Tell(Hero, Dying));
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
    protected Node Tell(GameObject Hero, GameObject Dying)
    {
        Animator hero_ani = Hero.GetComponent<Animator>();
        Animator dying_ani = Dying.GetComponent<Animator>();
        return new Sequence(new LeafWait(2000), new LeafAssert(() => this.Telling_secret(dying_ani)), new LeafWait(8000), new LeafAssert(() => this.StopWorking(hero_ani)), new LeafWait(1500));

    }
    public bool Telling_secret(Animator dying_ani)
    {
        dying_ani.SetTrigger("Tell_secret");
        Passwords = true;
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
