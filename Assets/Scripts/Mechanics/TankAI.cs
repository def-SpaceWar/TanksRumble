using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class TankAI : MonoBehaviour
{
        private Tank m_Tank;
        private TankWeapon m_TankWeapon;
        private TankMovement m_TankMovement;

        private TankMovementControls m_TankMovementControls;
        private TankWeaponControls m_TankWeaponControls;

        private bool m_IsLeft;

        private Rigidbody2D m_Rb;
        private Seeker m_Seeker;
        private Path m_Path;
        private const float m_NextWayPointDistance = 75f;
        private int m_CurrentWayPoint = 1;

        [SerializeField] private LayerMask shootingLayerMask;

        private enum Direction
        {
                Left,
                Right,
                Forward
        }

        private void Start()
        {
                m_Tank = GetComponent<Tank>();
                m_TankWeapon = GetComponent<TankWeapon>();
                m_TankMovement = GetComponent<TankMovement>();

                m_Rb = GetComponent<Rigidbody2D>();
                m_Seeker = GetComponent<Seeker>();
        }

        private void Update()
        {
                if (m_TankWeaponControls == null)
                {
                        var tankWeaponControls = FindObjectsOfType<TankWeaponControls>();

                        foreach (var tankWeaponControl in tankWeaponControls)
                        {
                                if (tankWeaponControl.tankNumber != m_Tank.tankNumber) continue;

                                m_TankWeaponControls = tankWeaponControl;
                                break;
                        }
                }

                if (m_TankMovementControls == null)
                {
                        var tankMovementControls = FindObjectsOfType<TankMovementControls>();

                        foreach (var tankMovementControl in tankMovementControls)
                        {
                                if (tankMovementControl.tankNumber != m_Tank.tankNumber) continue;

                                m_TankMovementControls = tankMovementControl;
                                break;
                        }
                }

                if (!m_TankMovementControls.isInitialized)
                        return;

                if (!m_TankWeaponControls.isInitialized)
                        return;

                if (PauseMenu.IsPaused)
                        return;

                if (FindObjectOfType<GameSettings>().isWon)
                        return;

                if (GetComponent<TankHealth>().respawning)
                        return;

                // Shoot when something is detected via Raycast!
                ShootIfTarget(Physics2D.Raycast(m_TankWeapon.shootPoint.position, m_TankWeapon.shootPoint.right, shootingLayerMask));

                if (m_TankMovementControls != null) m_TankMovementControls.isLeft = m_IsLeft;

                // Turn towards nearest player / power up box!
                Transform[] targets = { };
                Tank[] tanks = FindObjectsOfType<Tank>();

                foreach (var tank in tanks)
                {
                        if (tank.tankNumber == m_Tank.tankNumber) continue;
                        if (tank.GetComponent<TankHealth>().Lives <= 0) continue;
                        if (tank.GetComponent<TankHealth>().respawning) continue;

                        if (GameModeManager.GameModes[FindObjectOfType<GameSettings>().gameMode].HasTeams)
                        {
                                if (tank.GetComponent<TankTeam>().team == GetComponent<TankTeam>().team) continue;
                                targets = AddElementToArray(targets, tank.transform);
                        }
                        else
                        {
                                targets = AddElementToArray(targets, tank.transform);
                        }
                }

                if (m_Tank.ActivePowerup == null)
                {
                        var powerupBoxes = FindObjectsOfType<PowerupBox>();

                        foreach (var powerupBox in powerupBoxes)
                        {
                                targets = AddElementToArray(targets, powerupBox.transform);
                        }
                }

                Direction direction = DirectionToTarget(targets);

                switch (direction)
                {
                        case Direction.Left:
                                if (m_IsLeft == false)
                                {
                                        m_TankMovement.TurnRight();
                                        m_IsLeft = !m_IsLeft;
                                }
                                else
                                {
                                        m_TankMovement.TurnLeft();
                                }

                                break;
                        case Direction.Right:
                                if (m_IsLeft)
                                {
                                        m_TankMovement.TurnLeft();
                                        m_IsLeft = !m_IsLeft;
                                }
                                else
                                {
                                        m_TankMovement.TurnRight();
                                }

                                break;
                        case Direction.Forward:
                                m_IsLeft = !m_IsLeft;

                                break;
                }
        }

        private void ShootIfTarget(RaycastHit2D hitInfo)
        {
                try {
                        if (hitInfo.collider.gameObject.GetComponent<TankHealth>() != null)
                        {
                                ShootIfTank(hitInfo);
                        }
                        else
                        {
                                ShootIfProjectile(hitInfo);
                        }
                } catch {
                }
        }

        private void ShootIfTank(RaycastHit2D hitInfo)
        {
                if (hitInfo.collider.gameObject.GetComponent<TankHealth>().health <= 0) return;

                if (GameModeManager.GameModes[GameSettings.Instance.gameMode].HasTeams)
                {
                        if (hitInfo.collider.gameObject.GetComponent<TankTeam>().team == GetComponent<TankTeam>().team) return;
                        m_TankWeapon.Shoot();
                }
                else
                {
                        m_TankWeapon.Shoot();
                }
        }

        private void ShootIfProjectile(RaycastHit2D hitInfo)
        {
                // Not detect instantly
                float random = Random.Range(0f, 100f);
                if (random <= 90f) return;

                if (hitInfo.collider.gameObject.GetComponent<TankBullet>() != null)
                {
                        m_TankWeapon.Shoot();
                }

                if (hitInfo.collider.gameObject.GetComponent<MiniRocket>() != null)
                {
                        m_TankWeapon.Shoot();
                }

                if (hitInfo.collider.gameObject.GetComponent<BigRocket>() != null)
                {
                        m_TankWeapon.Shoot();
                }
        }

        private T[] AddElementToArray<T>(T[] array, T element)
        {
                var newArray = new T[array.Length + 1];
                int i;
                for (i = 0; i < array.Length; i++)
                {
                        newArray[i] = array[i];
                }
                newArray[i] = element;
                return newArray;
        }

        private Direction DirectionToTarget(Transform[] targets)
        {
                if (targets == null) return Direction.Forward;

                Direction direction;
                Transform target = GetClosestTarget(targets);

                try
                {
                        m_Seeker.StartPath(m_Rb.position, target.position, OnPathComplete);
                        Vector3 nextPoint = m_Path.vectorPath[m_CurrentWayPoint];

                        if (Vector3.Distance(transform.position, nextPoint) < m_NextWayPointDistance)
                        {

                                m_CurrentWayPoint++;
                                nextPoint = m_Path.vectorPath[m_CurrentWayPoint];
                        }

                        direction = DirectionTowards(nextPoint);
                }
                catch
                {
                        try
                        {
                                direction = DirectionTowards(target);
                        }
                        catch
                        {
                                direction = Direction.Forward;
                        }
                }

                return direction;
        }

        private Transform GetClosestTarget(Transform[] targets)
        {
                Transform bestTarget = null;
                float closestDistanceSqr = Mathf.Infinity;
                Vector3 currentPosition = transform.position;

                foreach (var potentialTarget in targets)
                {
                        var directionToTarget = potentialTarget.position - currentPosition;
                        var dSqrToTarget = directionToTarget.sqrMagnitude;
                        if (dSqrToTarget < closestDistanceSqr)
                        {
                                closestDistanceSqr = dSqrToTarget;
                                bestTarget = potentialTarget;
                        }
                }

                return bestTarget;
        }

        private void OnPathComplete(Path p)
        {
                if (!p.error)
                {
                        m_CurrentWayPoint = 1;
                        m_Path = p;
                }
        }

        private Direction DirectionTowards(Vector3 target)
        {
                float angle = Vector3.Angle(target - transform.position, transform.right);
                Vector3 cross = Vector3.Cross(target - transform.position, transform.right);

                if (cross.y < 0) angle = -angle;

                const float checkAngle = 7.5f;

                if (angle < checkAngle)
                        return Direction.Left;

                if (angle > checkAngle)
                        return Direction.Right;

                return Direction.Forward;
        }

        private Direction DirectionTowards(Transform target)
        {
                float angle = Vector3.Angle(target.position - transform.position, transform.right);
                Vector3 cross = Vector3.Cross(target.position - transform.position, transform.right);

                if (cross.y < 0) angle = -angle;

                const float checkAngle = 7.5f;

                if (angle < checkAngle)
                        return Direction.Left;

                if (angle > checkAngle)
                        return Direction.Right;

                return Direction.Forward;
        }
}
