using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Animator animator;
    protected EntityMovement movement;

    public FloatValue maxHealth;
    public string entityName;
    public int baseAttack;
    [ReadOnly] public float health;

    public GameObject healthBarPrefab;
    private GameObject healthBar;

    private static readonly Color GREEN = new Color(0, 1, 0);
    private static readonly Color GREENYELLOW = new Color(0.5f, 1, 0);
    private static readonly Color YELLOW = new Color(1, 1, 0);
    private static readonly Color ORANGE = new Color(1, 0.7f, 0);
    private static readonly Color RED = new Color(1, 0, 0);


    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<EntityMovement>();
        CreateHealthBar();
    }

    private void CreateHealthBar()
    {
        healthBar = Instantiate(healthBarPrefab);
        healthBar.transform.position = new Vector3(0, 2, 0);
        healthBar.transform.SetParent(transform, false);
        
    }

    protected void Awake()
    {
        health = maxHealth.value;
        OnAwake();
    }

    private void FixedUpdate()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        Transform midRect = healthBar.transform.Find("Middle Rect");
        Transform frontRect = healthBar.transform.Find("Front Rect");

        float percentage = Mathf.Max(health / maxHealth.value, 0);
        float newPosition = midRect.localScale.x * (percentage - 1) / 2;
        float newWidth = midRect.localScale.x * percentage;
        frontRect.localScale = new Vector3(newWidth, midRect.localScale.y);
        frontRect.localPosition = new Vector3(newPosition, midRect.localPosition.y);

        frontRect.GetComponent<SpriteRenderer>().color = ChooseBarColor(percentage);
    }

    public Color ChooseBarColor(float percentage)
    {
        if (percentage == 1)
            return GREEN;
        else if (percentage >= 0.75)
            return GREENYELLOW;
        else if (percentage >= 0.5)
            return YELLOW;
        else if (percentage >= 0.25)
            return ORANGE;
        else
            return RED;

    }
    abstract protected void OnAwake();

    protected virtual void TakeDamage(float damage)
    {
        // caso um ativista morto seja atacado evita que onDeath seja chamado novamente
        if (health < 0) {
            return;
        }
        health -= damage;
        if (health <= 0)
            OnDeath();
    }

    protected abstract void OnDeath();

    protected void GiveXp(int xpReceived)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        foreach (var player in players)
        {
            player.transform.GetComponent<Player>().ReceiveXp(xpReceived);
        }

        GameObject[] rhinos = GameObject.FindGameObjectsWithTag("rhino");
        foreach (var rhino in rhinos)
        {
            rhino.transform.GetComponent<Rhino>().ReceiveXp(xpReceived);
        }
    }

    public void Knock(Rigidbody2D myRigidBody, float knockTime, float damage)
    {
        StartCoroutine(movement.KnockCo(myRigidBody, knockTime));
        TakeDamage(damage);
    }

    public virtual void FullRestore()
    {
        health = maxHealth.value;
    }

    public bool IsOtherEntity(GameObject gameObject)
    {
        return (gameObject.CompareTag("player") ||
                gameObject.CompareTag("rhino") ||
                gameObject.CompareTag("enemy")) &&
                !(gameObject == this.gameObject);
    }
}
