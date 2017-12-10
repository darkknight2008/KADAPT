using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree_full : MonoBehaviour
{
    public GameObject Hero;
    public GameObject Mayor;
    public GameObject Oldman;
    public GameObject Villager1;
    public GameObject Villager2;
    public GameObject Villager3;
    public GameObject Villager4;
    public GameObject Villager5;
    public GameObject Villager6;


    private BehaviorAgent behaviorAgent;


    //text control
    private bool direction = false;
    private bool direction_disappear = false;
    private bool task = false;
    private bool task_disappear = false;

    //text
    public Text direction_text;
    public Text task_text;

    //camera
    public Camera cam;

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
    public bool sword_bool = false;
    public bool sword_disappear = false;
    public bool save_prin = false;
    public bool save_prin_dis = false;

    //Text
    //public Text winText;
    public Text failtext;
    public Text dyingText;
    public Text sword_text;
    public Text prin_thank;
    private DoorHoldOn D;

    private Vector3 reach_posi;
    private Vector3 reach_posi1;
    private Vector3 reach_posi2;
    private Vector3 reach_posi3;
    private Vector3 reach_posi4;
    private Vector3 reach_posi5;
    private Vector3 reach_posi6;
    private Vector3 reach_posi7;

    //part 3
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;
    public Transform p5;
    public Transform p6;
    public Transform p7;
    public Transform p8;
    public Transform p9;

    public GameObject Princess;
    public GameObject guard;
    public GameObject sc3Zombie1;
    public GameObject sc3Zombie2;
    public GameObject sc3Zombie3;
    public GameObject sc3Zombie4;
    public GameObject sc3Zombie5;

    public float prinMaxSpeed;
    public float prinMinSpeed;
    public float bitedis;
    public float slashdis;

    //part 4
    public Transform v1, v2, v3, v4, v5, v6, v7, v8, v10;

    public Text thanks_hero;
    public Text win_hero;

    public bool thanks = false;
    public bool thanks_disappear = false;
    public bool end = false;
    public bool end_disappear = false;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
        direction_text.text = "";
        task_text.text = "";
        prin_thank.text = "";

        Hero.GetComponent<PlayerController2>().enabled = true;

        //text
        failtext.text = "";
        dyingText.text = "";
        sword_text.text = "";
        D = door.GetComponent<DoorHoldOn>();

        //part 3
        //text
        prinMinSpeed = 0.01f;
        prinMaxSpeed = 3f;
        bitedis = 1f;
        slashdis = 2.5f;

        //part 4
        thanks_hero.text = "";
        win_hero.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //part 1
        //text control
        if (direction == true)
        {
            PositionTrans(Oldman, direction_text);
            direction_text.text = "Finally, here you come. The evil took our mayor's young daughter, ALICE and we really need your help. Please go this way, and ask mayor for more information.";
        }
        if (direction_disappear == true)
        {
            direction_text.text = "";
        }
        if (task == true)
        {
            PositionTrans(Mayor, task_text);
            task_text.text = "Hi HERO, thanks for your coming. Our brave villager ERIC tried to rescue ALICE but we have lost him contact for several days. ALICE is all I have and please get her back.";
        }
        if (task_disappear == true)
        {
            task_text.text = "";
        }
        //part 2
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
        if (sword_bool == true)
        {
            PositionTrans(Hero, sword_text);
            sword_text.text = "Congratulations, you get a SWORD! Make good use of it!";
        }
        if (sword_disappear == true)
        {
            sword_text.text = "";
        }

        if (keyGot == true)
        {
            D.key_get = true;
        }
        if (save_prin == true)
        {
            PositionTrans(Princess, prin_thank);
            prin_thank.text = "HERO,thanks.You are the sunshine in my life.";
        }
        if (save_prin_dis == true)
        {
            prin_thank.text = "";
        }

        //part 4
        if (thanks == true)
        {
            PositionTrans(Villager1, thanks_hero);
            thanks_hero.text = "HERO, thanks for helping Eric! We will remember your kindness and bravery forever.";
        }
        if (thanks_disappear == true)
        {
            thanks_hero.text = "";
        }
        if (end == true)
        {
            PositionTrans(Hero, win_hero);
            win_hero.text = "Don't mind. It's my honor.";
        }
        if (end_disappear == true)
        {
            win_hero.text = "";
        }

    }
    //root
    protected Node BuildTreeRoot()
    {
        return new Sequence(
            new SequenceParallel(
            Part1Node(),
            Part2Node(),
            Part3Node()),
            Part4Node(),
            new LeafWait(100)
            );
    }
    //part 1
    public void PositionTrans(GameObject gb, Text assignText)
    {
        Vector3 worldPosition = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z) + new Vector3(0, 2f, 0);
        Vector2 position = cam.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, position.y);
        assignText.transform.position = position;
    }
    public class canHeroask : Node
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
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canask(GameObject Hero, GameObject Oldman)
    {
        return new canHeroask(Hero, Oldman);
    }
    public class waving : Node
    {
        protected GameObject Hero;
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
    public class stayHere : Node
    {
        protected GameObject Chrc;
        public stayHere(GameObject Chrc)
        {
            this.Chrc = Chrc;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Chrc.GetComponent<PlayerController2>().enabled = false;
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
            yield return RunStatus.Success;

        }
    }
    public class back2live : Node
    {
        protected GameObject Chrc;
        public back2live(GameObject Chrc)
        {
            this.Chrc = Chrc;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Chrc.GetComponent<PlayerController2>().enabled = true;
            yield return RunStatus.Success;

        }
    }
    public Node lookAtEachOther(GameObject G1, GameObject G2)
    {
        Val<Vector3> p1 = Val.V(() => G1.transform.position);
        Val<Vector3> p2 = Val.V(() => G2.transform.position);
        return new SequenceParallel(
                    G1.GetComponent<BehaviorMecanim>().Node_OrientTowards(p2),
                    G2.GetComponent<BehaviorMecanim>().Node_OrientTowards(p1)
                    );
    }
    protected Node askdire(GameObject Hero, GameObject Oldman)
    {
        //Val<Vector3> reach = Val.V(() => (Oldman.transform.position-Hero.transform.position)/ Vector3.Distance(Hero.transform.position, Oldman.transform.position));
        Val<Vector3> reach = Val.V(() => Oldman.transform.position);
        Val<float> dist = Val.V(() => 2.5f);
        return new Sequence(
            new stayHere(Hero),
            new LeafWait(1000),
            Hero.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(reach, dist),
            lookAtEachOther(Hero, Oldman),
            new LeafWait(100),
            new waving(Hero),
            new LeafWait(1000),
            new back2idle(Hero),
            new LeafWait(3000),
            new back2live(Hero)
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
    public bool showdire()
    {
        direction = true;
        return true;
    }
    public bool closedire()
    {
        direction_disappear = true;
        return true;
    }
    protected Node pointdire(GameObject Oldman)
    {

        return new Sequence(
            new LeafAssert(() => this.showdire()),
            new turnreverse(Oldman),
            new LeafWait(700),
            new pointingup(Oldman),
            new LeafWait(3000),
            new back2idle(Oldman),
            new LeafAssert(() => this.closedire())
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
    protected Node canmeet(GameObject Hero, GameObject Mayor)
    {
        return new canmeetChief(Hero, Mayor);
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
            hero.SetTrigger("Kneel");//Kneel down
            //text?
            yield return RunStatus.Success;
        }
    }
    protected Node intro2self(GameObject Hero, GameObject Mayor)
    {
        Val<Vector3> reach = Val.V(() => Mayor.transform.position);
        Val<float> dist = Val.V(() => 2.5f);
        return new Sequence(
            new stayHere(Hero),
            new LeafWait(1000),
            Hero.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(reach, dist),
            lookAtEachOther(Hero, Mayor),
            new LeafWait(100),
            new bowing(Hero),
            new LeafWait(500),
            new back2idle(Hero),
            new LeafWait(3000),
            new back2live(Hero)
            );
    }
    public class requesting_down : Node
    {
        protected GameObject Mayor;
        public requesting_down(GameObject Mayor, bool text)
        {
            this.Mayor = Mayor;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator chief = Mayor.GetComponent<Animator>();
            chief.SetTrigger("Request_down");//empty now
            yield return RunStatus.Success;
        }
    }
    public class requesting_up : Node
    {
        protected GameObject Mayor;
        public requesting_up(GameObject Mayor)
        {
            this.Mayor = Mayor;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            Animator chief = Mayor.GetComponent<Animator>();
            chief.SetTrigger("Request_up");
            yield return RunStatus.Success;
        }
    }
    public bool showtask()
    {
        task = true;
        return true;
    }
    public bool closetask()
    {
        task_disappear = true;
        return true;
    }
    protected Node request(GameObject Mayor)
    {
        return new Sequence(
            new requesting_down(Mayor, task),
            new LeafAssert(() => this.showtask()),
            new LeafWait(4000),
            new requesting_up(Mayor),
            new LeafWait(6000),
            new LeafAssert(() => this.closetask()),
            new back2idle(Mayor)
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
    protected Node bargain(GameObject Villager1, GameObject Villager2)
    {
        return new SequenceParallel(
            new bargaining(Villager1),
            new Sequence(
                new LeafWait(3000),
                new thinking(Villager2)),
            new LeafWait(8000)
            );
    }
    public class quarrel_ind : Node
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
    protected Node Part1Node()
    {
        Node AskDire = new Sequence(
            new SuccessLoop(
                this.canask(Hero, Oldman)
                ),
            this.askdire(Hero, Oldman),
            this.pointdire(Oldman)
            );
        Node Chiefstask = new Sequence(
            new SuccessLoop(
                this.canmeet(Hero, Mayor)
                ),
            this.intro2self(Hero, Mayor),
            this.request(Mayor)
            );
        Node Shop = new DecoratorLoop(
            this.bargain(Villager1, Villager2)
            );
        Node Quarrel = new DecoratorLoop(
                this.quarrelling(Villager3, Villager4)
            );
        Node Chat = new DecoratorLoop(
            this.chatting(Villager5, Villager6)
            );
        //Node root = new DecoratorLoop(new Sequence
        Node root = new Sequence
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
            // )
            );
        return root;
    }



    //part 2
    public void PositionTransDead(Text dyingText)
    {
        Vector3 worldPosition = new Vector3(Dying.transform.position.x, Dying.transform.position.y, Dying.transform.position.z) + new Vector3(0, 1.5f, 0);
        Vector2 position = cam.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, position.y);
        dyingText.transform.position = position;
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
        return new Sequence(
            new canGetSword(Hero, sword),
            new LeafAssert(() => pickSword(Hero, sword)),
            new LeafWait(1000),
            new LeafAssert(() => sword_text_disappear(sword_text)));
    }
    public bool pickSword(GameObject Hero, GameObject sword)
    {
        SwordController script = sword.GetComponent<SwordController>();

        script.isHold = true;
        script.holdBy = Hero;
        sword_bool = true;

        sword.transform.position = script.holdBy.transform.Find("Hero/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/Sword").transform.position;
        sword.transform.rotation = script.holdBy.transform.Find("Hero/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/Sword").transform.rotation;

        sword.transform.parent = script.holdBy.transform.Find("Hero/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand").transform;

        return true;
    }
    public bool sword_text_disappear(Text sword_text)
    {
        sword_disappear = true;
        return true;
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
        }
        else if (Zombie == Zombie2)
        {
            reach = Val.V(() => reach_posi2);
            return new Sequence(
                turn_move(Zombie, reach),
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
        }
        else if (Zombie == Zombie3)
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
            new LeafAssert(() => this.HeroBack()));

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

    protected Node Part2Node()
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



    //Part 3
    protected Node ZombieBiteP(GameObject Zombie, GameObject Princess)
    {
        return new Sequence(this.BiteP(Zombie, Princess), new LeafWait(1000), new LeafAssert(() => this.GameOver()));
    }

    protected Node BiteP(GameObject Zombie, GameObject Princess)
    {
        Animator Prin_ani = Princess.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.Princess_stop(Princess)), this.Bitting(Zombie, Princess), new LeafWait(200), this.HeroDies(Prin_ani));
    }
    protected Node ZombieBiteH(GameObject Zombie, GameObject Princess)
    {
        return new Sequence(this.BiteH(Zombie, Princess), new LeafWait(1000), new LeafAssert(() => this.GameOver()));
    }

    protected Node BiteH(GameObject Zombie, GameObject Princess)
    {
        Animator Prin_ani = Princess.GetComponent<Animator>();
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.Bite_hero(zombie_ani)), new LeafWait(200), this.HeroDies(Prin_ani));
    }
    protected Node Bitting(GameObject Zombie, GameObject Princess)
    {
        Animator zombie_ani = Zombie.GetComponent<Animator>();
        Val<Vector3> reach;
        reach = Val.V(() => Princess.transform.position);
        return new Sequence(
                turn_move3(Zombie, reach),
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
    }
    protected Node turn_move3(GameObject Zombie, Val<Vector3> reach)
    {
        return new Sequence(Zombie.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(reach, Val.V(() => bitedis)), new LeafWait(100));
    }

    public bool Princess_stop(GameObject Princess)
    {
        Princess.GetComponent<UnitySteeringController>().maxSpeed = prinMinSpeed;
        Princess.GetComponent<UnitySteeringController>().minSpeed = 0.005f;
        return true;
    }
    protected Node ZombieDie(GameObject Zombie)
    {
        Animator Zombie_ani = Zombie.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.HeroDying(Zombie_ani)), new LeafWait(1500));
    }
  
    protected Node boom(GameObject Zombie, GameObject Princess)
    {
        return new Sequence(new LeafAssert(() => this.zombieFire(Zombie)), new LeafAssert(() => this.prin_start(Princess)));
    }
    public bool zombieFire(GameObject Zombie)
    {
        Zombie.SetActive(false);
        Zombie.transform.parent.transform.GetChild(1).gameObject.SetActive(true);
        Zombie.transform.parent.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        return true;
    }
    public bool prin_start(GameObject Princess)
    {
        Princess.GetComponent<UnitySteeringController>().maxSpeed = prinMaxSpeed;
        Princess.GetComponent<UnitySteeringController>().minSpeed = 0.005f;
        return true;
    }
    public class canAttack : Node
    {
        protected GameObject Hero;
        protected GameObject Zombie;
        public canAttack(GameObject Hero, GameObject Zombie)
        {
            this.Hero = Hero;
            this.Zombie = Zombie;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, Zombie.transform.position) < 2.5f && GameObject.FindGameObjectWithTag("Sword").GetComponent<SwordController>().kill)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node Attack(GameObject Hero, GameObject Zombie)
    {
        return new canAttack(Hero, Zombie);
    }
   
  
    public class canGotBiteH : Node
    {
        protected GameObject zombie;
        protected GameObject Hero;
        public canGotBiteH(GameObject zombie, GameObject Hero)
        {
            this.Hero = Hero;
            this.zombie = zombie;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                if (Vector3.Distance(Hero.transform.position, zombie.transform.position) <1f &&  Vector3.Angle(Hero.transform.position- zombie.transform.position, zombie.transform.forward)<90.0f)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
        }
    }
    protected Node canBiteH(GameObject zombie, GameObject Hero)
    {
        return new canGotBiteH(zombie, Hero);
    }
    protected Node zombieAttack(GameObject Zombie, GameObject Hero, GameObject Princess)
    {
        return new Sequence
            (
                new SelectorParallel
                (
                    new Sequence
                    (
                        new SuccessLoop
                        (
                           this.canBite(Zombie, Princess)
                         ),
                        this.ZombieBiteP(Zombie, Princess),
                        new LeafWait(1000000)
                    ),
                    new Sequence
                    (
                        new SuccessLoop
                        (
                           this.canBiteH(Zombie, Hero)
                         ),
                        this.ZombieBiteH(Zombie, Hero),
                        new LeafWait(1000000)
                    ),
                    new SuccessLoop
                     (
                        this.Attack(Hero, Zombie)
                      )
                ),
                this.ZombieDie(Zombie),
                this.boom(Zombie, Princess),
                new LeafWait(1000000)
            );
    }
    protected Node moveTo(GameObject people, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(100));
    }

    
    
    protected Node talking(GameObject Villager5, GameObject Villager6)
    {
        return new SequenceParallel(
            new Sequence(
                new LeafAssert(() => this.save_prin_app()),
                new bargaining(Villager5)),
            new Sequence(
                new LeafWait(3000),
                new bargaining(Villager6),
                new back2idle(Villager5),
                new back2idle(Villager6),
                new LeafAssert(() => this.save_prin_disapp())),
            new LeafWait(7200)
            );
    }
    public bool save_prin_app()
    {
        save_prin = true;
        return true;
    }
    public bool save_prin_disapp()
    {
        save_prin_dis = true;
        return true;
    }
    protected Node Part3Node()
    {
        Node scene3 = new Sequence
    (
        new SuccessLoop
        (
            this.Attack(Hero, guard)
        ),
        this.ZombieDie(guard),
        this.boom(guard, Princess),
        new SuccessLoop
        (
            this.canTalk(Hero, Princess)
        ),
        this.lookAtEachOther(Princess,Hero),
        this.talking(Princess, Hero),
        new SelectorParallel
        (
              new Sequence
              (
                   this.moveTo(Princess, p1),
                   this.moveTo(Princess, p2),
                   this.moveTo(Princess, p3),
                   this.moveTo(Princess, p4),
                   this.moveTo(Princess, p5),
                   this.moveTo(Princess, p6),
                   this.moveTo(Princess, p7),
                   this.moveTo(Princess, p8),
                   this.moveTo(Princess, p9)
               ),
                this.zombieAttack(sc3Zombie1, Hero, Princess),
                this.zombieAttack(sc3Zombie2, Hero, Princess),
                this.zombieAttack(sc3Zombie3, Hero, Princess),
                this.zombieAttack(sc3Zombie4, Hero, Princess),
                this.zombieAttack(sc3Zombie5, Hero, Princess)
        )
    );
        return scene3;
    }

    //part 4
    public bool back2steer()
    {
        Villager1.GetComponent<UnitySteeringController>().enabled = true;
        Villager2.GetComponent<UnitySteeringController>().enabled = true;
        Villager3.GetComponent<UnitySteeringController>().enabled = true;
        Villager4.GetComponent<UnitySteeringController>().enabled = true;
        Villager5.GetComponent<UnitySteeringController>().enabled = true;
        Villager6.GetComponent<UnitySteeringController>().enabled = true;
        Oldman.GetComponent<UnitySteeringController>().enabled = true;
        Mayor.GetComponent<UnitySteeringController>().enabled = true;

        return true;
    }
    protected Node Clap(GameObject Villager1)
    {
        Animator Villager_ani = Villager1.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.clap_Villager(Villager_ani)), new LeafWait(7000), new LeafAssert(() => this.StopClap(Villager_ani)));
    }

    public bool clap_Villager(Animator chr)
    {
        chr.SetTrigger("clap");
        thanks = true;
        return true;
    }

    public bool StopClap(Animator chr)
    {
        chr.SetTrigger("Idle");
        thanks_disappear = true;
        return true;
    }

    protected Node Hero_soso(GameObject Hero)
    {
        Animator Hero_ani = Hero.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.hero_soso(Hero_ani)), new LeafWait(6000), new LeafAssert(() => this.stop_herososo(Hero_ani)));

    }

    public bool hero_soso(Animator chr)
    {
        chr.SetTrigger("thankful");
        end = true;
        return true;
    }

    public bool stop_herososo(Animator chr)
    {
        chr.SetTrigger("Idle");
        end_disappear = true;
        return true;
    }

    protected Node moveTo(GameObject people, Transform target, int waittime)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(waittime));
    }
    protected Node Part4Node()
    {
        Node root = new Sequence
            (
                this.moveTo(Princess, v1, 100),
                new Sequence
                (
                    new LeafAssert(() => this.back2steer()),
                    new LeafWait(1000),
                    new SequenceParallel(
                        new back2idle(Villager1),
                        new back2idle(Villager2),
                        new back2idle(Villager3),
                        new back2idle(Villager4),
                        new back2idle(Villager5),
                        new back2idle(Villager6),
                        new back2idle(Oldman),
                        new back2idle(Mayor)
                        ),
                    new SequenceParallel
                    (
                        this.moveTo(Villager1, v2, 100),
                        this.moveTo(Villager2, v3, 100),
                        this.moveTo(Villager3, v4, 100),
                        this.moveTo(Villager4, v5, 100),
                        this.moveTo(Villager5, v6, 100),
                        this.moveTo(Villager6, v7, 100),
                        this.moveTo(Oldman, v8, 100),
                        this.moveTo(Mayor, v10, 100)
                    ),

                    new SelectorParallel
                    (
                        this.Clap(Villager1),
                        this.Clap(Villager2),
                        this.Clap(Villager3),
                        this.Clap(Villager4),
                        this.Clap(Villager5),
                        this.Clap(Villager6),
                        this.Clap(Oldman),
                        this.Clap(Princess),
                        this.Clap(Mayor)
                    ),
                    new LeafWait(2000),
                    this.Hero_soso(Hero),
                    new LeafWait(100000)
                )
            );
        return root;
    }
}
