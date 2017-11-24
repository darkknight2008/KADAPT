using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;

public class MyBehaviorTree_auto : MonoBehaviour
{
    public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public Transform bitepoint;
    public Text successtext;
    public Text failtext;
    public GameObject door;
    public GameObject sword;
    public GameObject player;
    public GameObject king;
    public GameObject dead;
    public GameObject zombie1;
    public GameObject zombie2;
    public GameObject zombie3;
    public bool keyGot = false;

    private BehaviorAgent behaviorAgent;
	// Use this for initialization
	//void Start ()
	//{
	//	behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
	//	BehaviorManager.Instance.Register (behaviorAgent);
	//	behaviorAgent.StartBehavior ();
	//}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	//protected Node moveTo(GameObject people,Transform target)
	//{
	//	Val<Vector3> position = Val.V (() => target.position);
	//	return new Sequence( people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	//}

 //   public class canGotBite : Node
 //   {
 //       protected GameObject zombie;
 //       protected GameObject player;
 //       public canGotBite(GameObject zombie, GameObject player)
 //       {
 //           this.player = player;
 //           this.zombie = zombie;
 //       }
 //       public override IEnumerable<RunStatus> Execute()
 //       {
 //           while (true)
 //           {
 //               if (Vector3.Distance(player.transform.position, zombie.transform.position) < 6)
 //               {
 //                   yield return RunStatus.Success;
 //                   yield break;
 //               }
 //               else
 //                   yield return RunStatus.Running;
 //           }
 //       }
 //   }
 //   protected Node canBite(GameObject zombie, GameObject player)
 //   {
 //       return new canGotBite(zombie, player);
 //   }

 //   public class canTheyTalk : Node
 //   {
 //       protected GameObject player;
 //       protected GameObject dead;
 //       public canTheyTalk(GameObject player, GameObject dead)
 //       {
 //           this.player = player;
 //           this.dead = dead;
 //       }
 //       public override IEnumerable<RunStatus> Execute()
 //       {
 //           while (true)
 //           {
 //               if (Vector3.Distance(player.transform.position, dead.transform.position) < 4)
 //               {
 //                   yield return RunStatus.Success;
 //                   yield break;
 //               }
 //               else
 //                   yield return RunStatus.Running;
 //           }
 //       }
 //   }
 //   protected Node canTalk(GameObject player, GameObject dead)
 //   {
 //       return new canTheyTalk(player, dead);
 //   }

 //   public class canGetSword : Node
 //   {
 //       protected GameObject player;
 //       protected GameObject sword;
 //       public canGetSword(GameObject player, GameObject sword)
 //       {
 //           this.player = player;
 //           this.sword = sword;
 //       }
 //       public override IEnumerable<RunStatus> Execute()
 //       {
 //           while (true)
 //           {
 //               if (Vector3.Distance(player.transform.position, sword.transform.position) < 4)
 //               {
 //                   yield return RunStatus.Success;
 //                   yield break;
 //               }
 //               else
 //                   yield return RunStatus.Running;
 //           }
 //       }
 //   }
 //   protected Node getSword(GameObject player, GameObject sword)
 //   {
 //       return new canGetSword(player, sword);
 //   }
 //   public class canDoorOpen : Node
 //   {
 //       protected GameObject player;
 //       protected GameObject door;
 //       protected bool keyGot;
 //       public canDoorOpen(GameObject player, GameObject door, bool keyGot)
 //       {
 //           this.player = player;
 //           this.door = door;
 //           this.keyGot = keyGot;
 //       }
 //       public override IEnumerable<RunStatus> Execute()
 //       {
 //           while (true)
 //           {
 //               if (Vector3.Distance(player.transform.position, door.transform.position) < 4 && keyGot == true)
 //               {
 //                   yield return RunStatus.Success;
 //                   yield break;
 //               }
 //               else
 //                   yield return RunStatus.Running;
 //           }
 //       }
 //   }
 //   protected Node canOpenDoor(GameObject player, GameObject door, bool keyGot)
 //   {
 //       return new canDoorOpen(player, door, keyGot);
 //   }

 //   public class nowGetKey : Node
 //   {
 //       protected bool keyGot;
 //       public nowGetKey(bool keyGot)
 //       {
 //           this.keyGot = keyGot;
 //       }
 //       public override IEnumerable<RunStatus> Execute()
 //       {
 //           keyGot = true;
 //           yield return RunStatus.Success;
 //       }
 //   }
 //   protected Node switchKey(bool keyGot)
 //   {
 //       return new nowGetKey(keyGot);
 //   }

 //   public class nowSwitchDoor : Node
 //   {
 //       protected GameObject door;
 //       public nowSwitchDoor(GameObject door)
 //       {
 //           this.door = door;
 //       }
 //       public override IEnumerable<RunStatus> Execute()
 //       {
 //           door.key_got = true;
 //           yield return RunStatus.Success;
 //       }
 //   }
 //   protected Node switchDoor(GameObject door)
 //   {
 //       return new nowSwitchDoor(door);
 //   }

 //   public class isHE : Node
 //   {
 //       protected Text successtext;
 //       public isHE(Text successtext)
 //       {
 //           this.successtext = successtext;
 //       }
 //       public override IEnumerable<RunStatus> Execute()
 //       {
 //           successtext.end = true;
 //           yield return RunStatus.Success;
 //       }
 //   }
 //   protected Node HE(Text successtext)
 //   {
 //       return new isHE(successtext);
 //   }

 //   protected Node BuildTreeRoot()
 //   {
 //       Node zombie1wander = new Sequence
 //           (
 //               new SuccessLoop
 //               (
 //                   new SelectorParallel
 //                   (
 //                       new DecoratorLoop
 //                       (
 //                           this.wander(this.wander1)
 //                       ),
 //                       this.canBite(zombie1, player)
 //                   )
 //               ),
 //               this.bite(zombie1, player)
 //           );
 //       Node zombie2wander = new Sequence
 //           (
 //               new SuccessLoop
 //               (
 //                   new SelectorParallel
 //                   (
 //                       new DecoratorLoop
 //                       (
 //                           this.wander(this.wander2)
 //                       ),
 //                       this.canBite(zombie2, player)
 //                   )
 //               ),
 //               this.bite(zombie2, player)
 //           );
 //       Node zombie3wander = new Sequence
 //           (
 //               new SuccessLoop
 //               (
 //                   new SelectorParallel
 //                   (
 //                       new DecoratorLoop
 //                       (
 //                           this.wander(this.wander3)
 //                       ),
 //                       this.canBite(zombie3, player)
 //                   )
 //               ),
 //               this.bite(zombie3, player)
 //           );
 //       Node openDoor = new Sequence
 //           (
 //               new SuccessLoop
 //               (
 //                   this.canOpenDoor(player, door, keyGot)
 //               ),
 //               this.switchDoor(door),
 //               new LeafWait(10000000000000)
 //           );
 //       Node getKey = new Sequence
 //           (
 //               new SuccessLoop
 //               (
 //                   this.canTalk(player, dead)
 //               ),
 //               this.talk(player, dead),
 //               this.switchKey(keyGot),
 //               new LeafWait(1000000000000)
 //           );
 //       Node getSword = new Sequence
 //           (
 //               new SuccessLoop
 //               (
 //                   this.getSword(player, sword)
 //               ),
 //               this.HE(successtext)
 //           );

 //       Node peoplemove = new Sequence
 //           (
 //               this.moveTo(player, bitepoint),
 //               new LeafWait(1000000)
 //           );
 //       Node root = new Sequence
 //           (
 //               this.task(king, player),
 //               new SuccessLoop
 //               (
 //                    new SelectorParallel
 //                    (     
 //                        peoplemove,
 //                         openDoor,
 //                         getKey,
 //                         zombie1wander,
 //                         zombie2wander,
 //                         zombie3wander,
 //                         getSword
 //                   )
 //               ),
 //               new LeafWait(100000000000)
 //           );
 //       return root;
 //   }
}