using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rhino : Character
{
    private static readonly int XP_MULT = 15;
    private static readonly int RADIATION_REQUIREMENT = 10;
    public Player owner;
    public GameObject captureRadius;
    private PlayerMovement ownerMovement;

    [ReadOnly] public int radiation;
    [ReadOnly] public int trainingXp;

    public List<GameObject> abilitiesToLearn;
    List<GameObject> abilitiesLearnt;

    //public GameObject shield;
    //public AbilityScript ability;

    private void Update()
    {
        if (ownerMovement != null) { 
            if(ownerMovement.inputEnabled) {
                //Maximo 3 habilidades por rino
                switch (Input.inputString)
                {
                    case "1":
                        if (abilitiesLearnt.Count > 0) {
                            abilitiesLearnt[0].GetComponent<Ability>().Activate();
                        }
                        break;
                    case "2":
                        if (abilitiesLearnt.Count > 1)
                        {
                            abilitiesLearnt[1].GetComponent<Ability>().Activate();
                        }
                        break;
                    case "3":
                        if (abilitiesLearnt.Count > 2)
                        {
                            abilitiesLearnt[2].GetComponent<Ability>().Activate();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    override protected void OnAwake() 
    {
        base.OnAwake();
        radiation = 0;
        requiredXp = level * XP_MULT;
        trainingXp = 0;
        if (owner != null) { 
            ownerMovement = owner.GetComponent<PlayerMovement>();
            Destroy(captureRadius);
        }
        for (int i = 0; i < abilitiesToLearn.Count; i++)
        {
            abilitiesToLearn[i] = Instantiate(abilitiesToLearn[i], this.transform.position, Quaternion.identity);
            abilitiesToLearn[i].transform.parent= gameObject.transform;
            abilitiesToLearn[i].transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        //abilities = new List<Ability>();
    }

    public bool HasOwner() {
        return owner != null;
    }

    public void SetOwner(Player player) {
        owner = player;
        ownerMovement = player.GetComponent<PlayerMovement>();
        var mov = gameObject.GetComponent<RhinoMovement>();
        mov.SetPlayer(owner, ownerMovement);
        Destroy(captureRadius);
    }
    public override void TakeDamage(float damage)
    {
        var shield = abilitiesLearnt.OfType<Shield>();
        if (shield.Count() > 0 && shield.First().active)
        {
            return;
        }
        base.TakeDamage(damage);
    }

    public override void FullRestore()
    {
        base.FullRestore();
        GetComponent<RhinoMovement>().currentState = RhinoState.walk;
    }

    override public void UpdateBarHealth()
    {
        ActivistsManager manager = FindObjectOfType<ActivistsManager>();

        if (manager.IsCurrentActivist(owner))
        {
            barHealth.value = Mathf.Max(health, 0);
            healthSignal.Raise();
        }
    }
    public void ReceiveRadiation(int radiationReceived)
    {
        radiation += radiationReceived;
        if (radiation >= RADIATION_REQUIREMENT)
        {
            radiation = 0;
            GetMutation();
        }

    }

    override public void ReceiveXp(int xpReward)
    {
        base.ReceiveXp(xpReward);

        ActivistsManager manager = FindObjectOfType<ActivistsManager>();
        if (manager.IsCurrentActivist(owner))
        {
            barXp.value = xp;
            XpSignal.Raise();
        }
    }

    public void ReceiveTrainingXp(int xpReward)
    {
        if (abilitiesToLearn.Count > abilitiesLearnt.Count)
        {
            trainingXp += xpReward;
            if (trainingXp > 5 * (abilitiesLearnt.Count + 1))
            {
                abilitiesToLearn.Add(abilitiesToLearn[abilitiesLearnt.Count]);
            }
        }
    }

    private void GetMutation()
    {

    }

    protected override void UpdateRequiredXp()
    {
        //for now a simple xp curve, can make more complex later
        requiredXp = level * 15;
    }

    override protected void IncreaseAttributes()
    {
        maxHealth += 4;
        if (health > 0)
        {
            health += 4;
        }
    }

    override protected void OnDeath()
    {
        movement.Flee();
    }

    public List<Ability> GetAbilities() 
    {
        List<Ability> abs = new List<Ability>();
        foreach (var item in abilitiesLearnt)
        {
            abs.Add(item.GetComponent<Ability>());
        }
        return abs;
    }
}
