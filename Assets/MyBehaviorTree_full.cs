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

    public GameObject door;
    public GameObject sword;
	public GameObject Hero;
	public GameObject King;
    public GameObject Dying;
    public GameObject zombie1;
    public GameObject zombie2;
    public GameObject zombie3;
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

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
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
        //Node zombie1wander = new Sequence
        //    (
        //        new SuccessLoop
        //        (
        //            new SelectorParallel
        //            (
        //                new DecoratorLoop
        //                (
        //                    this.wander(this.wander1)
        //                ),
        //                this.canBite(zombie1, Hero)
        //            )
        //        ),
        //        this.bite(zombie1, Hero)
        //    );
        //Node zombie2wander = new Sequence
        //    (
        //        new SuccessLoop
        //        (
        //            new SelectorParallel
        //            (
        //                new DecoratorLoop
        //                (
        //                    this.wander(this.wander2)
        //                ),
        //                this.canBite(zombie2, Hero)
        //            )
        //        ),
        //        this.bite(zombie2, Hero)
        //    );
        //Node zombie3wander = new Sequence
        //    (
        //        new SuccessLoop
        //        (
        //            new SelectorParallel
        //            (
        //                new DecoratorLoop
        //                (
        //                    this.wander(this.wander3)
        //                ),
        //                this.canBite(zombie3, Hero)
        //            )
        //        ),
        //        this.bite(zombie3, Hero)
        //    );
        //Node openDoor = new Sequence
        //    (
        //        new SuccessLoop
        //        (
        //            this.canOpenDoor(Hero, door, keyGot)
        //        ),
        //        this.switchDoor(door),
        //        new LeafWait(10000000000000)
        //    );
        //Node getKey = new Sequence
        //    (
        //        new SuccessLoop
        //        (
        //            this.canTalk(Hero, Dying)
        //        ),
        //        this.TellKey(Hero, Dying),
        //        this.switchKey(keyGot),
        //        new LeafWait(1000000000000)
        //    );
        //Node getSword = new Sequence
        //    (
        //        new SuccessLoop
        //        (
        //            this.getSword(Hero, sword)
        //        ),
        //        this.HE(winText)
        //    );

        //Node root = new DecoratorLoop( new Sequence
        Node root = new Sequence
            (
                this.task(King, Hero),
                //new SuccessLoop
                //(
                //     new SelectorParallel
                //     (
                //          openDoor,
                //          getKey,
                //          zombie1wander,
                //          zombie2wander,
                //          zombie3wander,
                //          getSword
                //    )
                //),
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
}
