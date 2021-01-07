using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rhino : Character
{
    private static readonly int XP_MULT = 15;
    public Player owner;
    private PlayerMovement ownerMovement;

    public List<GameObject> abilities;

    //public GameObject shield;
    //public AbilityScript ability;

    private void Update()
    {
        if(ownerMovement.inputEnabled) {
            //Maximo 3 habilidades por rino
            switch (Input.inputString)
            {
                case "1":
                    if (abilities.Count > 0) {
                        abilities[0].GetComponent<Ability>().Activate();
                    }
                    break;
                case "2":
                    if (abilities.Count > 1)
                    {
                        abilities[1].GetComponent<Ability>().Activate();
                    }
                    break;
                case "3":
                    if (abilities.Count > 2)
                    {
                        abilities[2].GetComponent<Ability>().Activate();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    override protected void OnAwake() 
    {
        base.OnAwake();
        requiredXp = level * XP_MULT;
        ownerMovement = owner.GetComponent<PlayerMovement>();
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i] = Instantiate(abilities[i], this.transform.position, Quaternion.identity);
            abilities[i].transform.parent= gameObject.transform;
            abilities[i].transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        //abilities = new List<Ability>();
    }

    protected override void TakeDamage(float damage)
    {
        var shield = abilities.OfType<Shield>();
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

    protected override void UpdateRequiredXp()
    {
        //for now a simple xp curve, can make more complex later
        requiredXp = level * 15;
    }

    override protected void IncreaseAttributes()
    {
        baseAttack += 1.5f;
    }

    override protected void OnDeath()
    {
        movement.Flee();
    }
}
