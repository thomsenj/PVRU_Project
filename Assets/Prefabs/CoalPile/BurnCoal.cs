using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

namespace Fusion.XRShared.Demo
{
    public class BurnCoal : NetworkBehaviour
    {
        public float fuelstand = 100f;
        public HealthbarController controller;

        private TrainManager trainManager;
        private float fuelModifier = 1.0f;
        private GameOverManager gameOverManager;
        [SerializeField] private float spawnInterval = 5.0f;
        private float spawnTimer;
        public AddCoal addCoal;
        public AudioSource burningSound;

        private void Start()
        {
            controller.SetMaxValue(150);
            try
            {
                trainManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<TrainManager>();
                gameOverManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<GameOverManager>();
                fuelModifier = trainManager.getFuelModifier();
            }
            catch
            {
                Debug.LogError("This scene lacks a train manager.");
            }
        }


        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (Object.HasStateAuthority)
            {
                spawnTimer += Time.fixedDeltaTime;

                if (spawnTimer >= spawnInterval)
                {
                    fuelstand = fuelstand - (1 * fuelModifier);

                    if (fuelstand < 1)
                    {
                        gameOverManager.TriggerGameOver();
                    }
                    spawnTimer = 0f;
                }
            }
            controller.SetValue((int)fuelstand);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagConstants.COAL))
            {
                addFuel();
                //if (trainManager != null)
                //{
                //    trainManager.setSpeed(getSpeed());
                //} else
                //{
                //    Debug.LogError("This scene lacks a train manager.");
                //}
                NetworkObject networkObject = other.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    addCoal.DespawnCoal(networkObject);
                }
                else
                {
                    Debug.LogError("No NetworkObject component found on the colliding GameObject.");
                }
            }
        }

        private void addFuel()
        {
            fuelstand = Mathf.Clamp(fuelstand + 10, 0, 100);

            if (burningSound != null)
            {
                burningSound.Play();
            }
        }

        private float getSpeed()
        {


            if (fuelstand >= 1 && fuelstand <= 10)
                return 1;
            else if (fuelstand > 10 && fuelstand <= 20)
                return 2;
            else if (fuelstand > 20 && fuelstand <= 30)
                return 3;
            else if (fuelstand > 30 && fuelstand <= 40)
                return 4;
            else if (fuelstand > 40 && fuelstand <= 50)
                return 5;
            else if (fuelstand > 50 && fuelstand <= 60)
                return 6;
            else if (fuelstand > 60 && fuelstand <= 70)
                return 7;
            else if (fuelstand > 70 && fuelstand <= 80)
                return 8;
            else if (fuelstand > 80 && fuelstand <= 90)
                return 9;
            else if (fuelstand > 90 && fuelstand <= 100)
                return 10;
            else
                return 0;
        }
    }
}
