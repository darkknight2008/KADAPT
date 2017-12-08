using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree_part4 : MonoBehaviour {
    public GameObject villager1, villager2, villager3, villager4, villager5, villager6, info1, mayor, companian, Hero;
    public Transform v1, v2, v3, v4, v5, v6, v7, v8, v10;

    public Text thanks_hero;
    public Text win_hero;

    public Camera cam;

    public bool thanks = false;
    public bool thanks_disappear = false;
    public bool end = false;
    public bool end_disappear = false;
    private BehaviorAgent behaviorAgent;

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

        thanks_hero.text = "";
        win_hero.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (thanks == true)
        {
            PositionTrans(villager1, thanks_hero);
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

    public bool back2steer()
    {
        villager1.GetComponent<UnitySteeringController>().enabled = true;
        villager2.GetComponent<UnitySteeringController>().enabled = true;
        villager3.GetComponent<UnitySteeringController>().enabled = true;
        villager4.GetComponent<UnitySteeringController>().enabled = true;
        villager5.GetComponent<UnitySteeringController>().enabled = true;
        villager6.GetComponent<UnitySteeringController>().enabled = true;
        info1.GetComponent<UnitySteeringController>().enabled = true;
        mayor.GetComponent<UnitySteeringController>().enabled = true;
        return true;
    }
    protected Node BuildTreeRoot()
    {
        Node root = new Sequence
            (
                this.moveTo(companian, v1, 100),
                new Sequence
                (
                    new LeafAssert(() => this.back2steer()),
                    new LeafWait(1000),
                    new SelectorParallel
                    (
                        this.moveTo(villager1, v2, 22000),
                        this.moveTo(villager2, v3, 22000),
                        this.moveTo(villager3, v4, 22000),
                        this.moveTo(villager4, v5, 22000),
                        this.moveTo(villager5, v6, 22000),
                        this.moveTo(villager6, v7, 22000),
                        this.moveTo(info1, v8, 22000),
                        this.moveTo(mayor, v10, 22000)
                    ),

                    new SelectorParallel
                    (
                        this.Clap(villager1),
                        this.Clap(villager2),
                        this.Clap(villager3),
                        this.Clap(villager4),
                        this.Clap(villager5),
                        this.Clap(villager6),
                        this.Clap(info1),
                        this.Clap(companian),
                        this.Clap(mayor)
                    ),
                    new LeafWait(2000),
                    this.Hero_soso(Hero),
                    new LeafWait(100000)
                )
            );
        return root;
    }

    protected Node Clap(GameObject villager1)
    {
        Animator villager_ani = villager1.GetComponent<Animator>();
        return new Sequence(new LeafAssert(() => this.clap_villager(villager_ani)), new LeafWait(7000), new LeafAssert(() => this.StopClap(villager_ani)));
    }

    public bool clap_villager(Animator chr)
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

    public void PositionTrans(GameObject gb, Text assignText)
    {
        Vector3 worldPosition = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z) + new Vector3(0, 1.5f, 0);
        Vector2 position = cam.WorldToScreenPoint(worldPosition);
        position = new Vector2(position.x, position.y);
        assignText.transform.position = position;
    }
}
