using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;


public class MyBehaviorTree_part3 : MonoBehaviour
{
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;
    public Transform p5;
    public Transform p6;
    public Transform p7;
    public Transform p8;
    public Transform p9;

    public GameObject Hero;
    public GameObject Princess;
    public GameObject guard;
    public GameObject sc3Zombie1;
    public GameObject sc3Zombie2;
    public GameObject sc3Zombie3;
    public GameObject sc3Zombie4;
    public GameObject sc3Zombie5;
    public Text failtext;

    public float prinMaxSpeed;
    public float prinMinSpeed;
    public float bitedis;
    public float slashdis;
    public bool Failure = false;
    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

        //text
        failtext.text = "";
        prinMinSpeed = 0.01f;
        prinMaxSpeed = 3f;
        bitedis = 1f;
        slashdis = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

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
                turn_move(Zombie, reach),
                new LeafAssert(() => this.Bite_hero(zombie_ani))
                );
    }
    protected Node turn_move(GameObject Zombie, Val<Vector3> reach)
    {
        return new Sequence(Zombie.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(reach, Val.V(() => bitedis)), new LeafWait(100));
    }
    public bool Bite_hero(Animator zombie)
    {
        zombie.SetTrigger("Bite");
        return true;
    }
    public bool Princess_stop(GameObject Princess)
    {
        Princess.GetComponent<UnitySteeringController>().maxSpeed = prinMinSpeed;
        Princess.GetComponent<UnitySteeringController>().minSpeed = 0.005f;
        return true;
    }
    public bool GameOver()
    {
        Failure = true;
        return true;
    }
    protected Node ZombieDie(GameObject Zombie)
    {
        Animator Zombie_ani = Zombie.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.HeroDying(Zombie_ani)), new LeafWait(1500));
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
                if (Vector3.Distance(Hero.transform.position, zombie.transform.position) < 1f)
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
    protected Node talking(GameObject Villager5, GameObject Villager6)
    {
        return new SequenceParallel(
           new bargaining(Villager5),
            new Sequence(
                new LeafWait(3000),
                new bargaining(Villager6),
                new back2idle(Villager5),
                new back2idle(Villager6)),
            new LeafWait(7000)
            );
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

    protected Node BuildTreeRoot()
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
}
