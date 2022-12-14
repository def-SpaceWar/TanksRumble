using UnityEngine;

public class Global : MonoBehaviour {

    #region BulletConstants

    public static readonly float BulletKnockback = 5f;
    public static readonly float BulletExplodeRadius = 0.25f;
    public static readonly float WaitToCheckBulletSpeed = 0.5f;
    public static readonly float MINBulletSpeed = 0.1f;

    #endregion

    #region StartUpScreen

    public static readonly float StartUpScreenFallPower = 1000;

    #endregion

    #region TankConstants

    public static readonly float WaitToSetTankNumber = 0.2f;
    public static readonly float WaitToInitControls = 1f;

    #endregion

    #region ControlConstants

    public static readonly float ControlsTransparency = 1f;

    #endregion

    public static Global Instance;

    #region InspectorConstants

    public GameObject bigExplosion;
    public GameObject regExplosion;
    public GameObject smallExplosion;

    public GameObject fireball;
    public GameObject indicator;

    #endregion

    private void Start()
    {
        if (Instance != null)
        {
            DestroySelf();
            return;
        }

        Instance = this; // Some objects need to be set in inspector.
        DontDestroyOnLoad(gameObject);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
        Destroy(this);
    }

}
