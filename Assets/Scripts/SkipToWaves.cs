using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipToWaves : MonoBehaviour
{
    private bool skipped = false;

    public Player manuel;
    public Rhino lonely;

    private bool manuelWasActive;

    private ActivistsManager activistsManager;
    private Building[] buildings;
    private MissionsManager missionsManager;

    private void Awake()
    {
        activistsManager = FindObjectOfType<ActivistsManager>();
        buildings = FindObjectsOfType<Building>();
        missionsManager = FindObjectOfType<MissionsManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !skipped)
        {
            skipped = true;
            Skip();
        }
    }

    private void CaptureLonely()
    {
        manuel.SetRhino(lonely);
        lonely.SetOwner(manuel);
        manuel.GetComponent<PlayerMovement>().TeleportRhino();       
        if (manuel.gameObject.activeSelf)
        {
            manuel.GetComponent<PlayerMovement>().EnableRhino();
        }
        manuelWasActive = manuel.gameObject.activeSelf;
        FindObjectOfType<RhinosManager>().rhinos.Add(lonely.GetComponent<RhinoMovement>());
    }

    private void Skip()
    {
        CaptureLonely();
        if (!manuelWasActive)
            manuel.GetComponent<PlayerMovement>().EnableRhino();
        // Subir ativistas para lvl 5 e rinos para lvl 4, aprender todas as habilidades dos rhinos e dar upgrade aos equipamentos
        foreach (Player player in activistsManager.players)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (i > player.level)
                    player.ReceiveXp(player.requiredXp - player.xp);
                if (i - 1 > player.rhino.level)
                    player.rhino.ReceiveXp(player.rhino.requiredXp - player.rhino.xp);
                if (i > player.rhino.trainingXp)
                    player.rhino.ReceiveTrainingXp(1);
            }

            foreach (Equipment equipment in player.equipments)
                if (equipment.level == equipment.initialLevel)
                    equipment.Upgrade();
        }
        if (!manuelWasActive)
            manuel.GetComponent<PlayerMovement>().DisableRhino();

        // Construir tudo
        foreach (Building building in buildings)
        {
            if (!building.IsBuilt())
            {
                if (building.level == 0)
                    building.Upgrade();
                else
                    building.Repair();
            }
        }

        // Curar
        activistsManager.HealAll();

        // Avançar nas missões
        missionsManager.SkipToRottenFlesh();
    }
}
