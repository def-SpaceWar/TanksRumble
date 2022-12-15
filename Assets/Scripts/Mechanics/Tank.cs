using TMPro;
using UnityEngine;

public class Tank : MonoBehaviour
{
        public int tankNumber; // Defined in the inspector so player 1, 2, 3, or 4!

        public float movePower;
        public float turnSpeed;

        public float rechargeTime;
        public float bulletSpeed;
        public float bulletRecoil;
        public float bulletLifeSpan;
        public int damage;

        public int health;

        [HideInInspector] public GameObject bullet;
        public Powerup ActivePowerup;

        [SerializeField] public TMP_Text backPedalTxt;

        private TankHealth tankHealth;

        [SerializeField] public Transform indicatorLocation;

        private void Start()
        {
                if (tankNumber < 1 || tankNumber > 4) throw new UnityException("tankNumber out of range!");

                tankHealth = GetComponent<TankHealth>();

                GameSettings gameSettings = FindObjectOfType<GameSettings>();
                GetComponent<SpriteRenderer>().color = gameSettings.PlayerColors[tankNumber - 1];
                transform.position = gameSettings.PlayerSpawns[tankNumber - 1].position;
                transform.rotation = gameSettings.PlayerSpawns[tankNumber - 1].rotation;
        }
}
