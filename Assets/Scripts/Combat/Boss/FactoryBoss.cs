﻿using System;
using System.Collections.Generic;
using Interactables.Misc;
using Interactables.Switches;
using UnityEngine;

namespace Combat.Boss
{
    public class FactoryBoss: MonoBehaviour, IDamageable
    {
        [SerializeField] private BossSpawner bossSpawner;
        [SerializeField] private int health = 3;
        [SerializeField] private MovableDoor entranceDoor;
        [SerializeField] private MovableDoor batteryDoor;
        [SerializeField] private PlayerDetector playerDetector;
        [SerializeField] private List<SwitchEventForwarder> doorSwitches;

        private void Start()
        {
            playerDetector.OnPlayerDetected += StartFight;
            foreach (var doorSwitch in doorSwitches)
            {
                doorSwitch.SwitchOff();
                doorSwitch.NewSwitchState += SwitchChanged;
            }
        }

        private void SwitchChanged(bool newSwitchState)
        {
            //if the new state of the switch is on check if all switches are on
            //and if they are open the door
            //else close the door
            if (newSwitchState)
            {
                for(int i = 0; i < doorSwitches.Count; i++)
                {
                    if (!doorSwitches[i].GetSwitchState())
                    {
                        batteryDoor.SwitchOff();
                        return;
                    }
                }
                batteryDoor.SwitchOn();
            }
            else
            {
                batteryDoor.SwitchOff();
            }
        }

        private void StartFight()
        {
            bossSpawner.SpawnEnemies();
            entranceDoor.SwitchOff();
        }

        public void TakeDamage(int damage)
        {
            //cant take damage if door isn't open
            if (!batteryDoor.IsOpen()) return;
            
            health--;
            if (health == 0)
            {
                EndFight();
            }
            else
            {
                batteryDoor.SwitchOff();
                foreach (var doorSwitch in doorSwitches)
                {
                    doorSwitch.SwitchOff();
                }
                bossSpawner.SpawnEnemies();
            }
        }

        void EndFight()
        {
            
        }
    }
}