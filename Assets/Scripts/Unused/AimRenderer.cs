using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AimRenderer : MonoBehaviour
{
    [Range(2, 30)]
    private float g;

    int simSteps = 40;
        
    Vector3 position;
    Vector3 direction;
    float velocity;

    Vector3[] arcPointArray;

    private Scene mainScene;
    private Scene physicsScene;

    public CannonballPhysics ball;
    public CannonballPhysics testBall;

    void Start()
    {
        arcPointArray = new Vector3[simSteps];
        g = Mathf.Abs(Physics.gravity.y);

        mainScene = SceneManager.GetActiveScene();
        physicsScene = SceneManager.CreateScene("physics scene", new CreateSceneParameters(LocalPhysicsMode.Physics3D)) ;

        PrepatePhysicsScene();       
    }

    void Update()
    {
        position = gameObject.GetComponent<TurretController>().muzzlePoint;
        direction = gameObject.GetComponent<TurretController>().muzzleVector;
        velocity = gameObject.GetComponent<TurretController>().muzzleVel;

        CalculateTrajectory();
    }

    void PrepatePhysicsScene()
    {
        SceneManager.SetActiveScene(physicsScene);

        testBall = Instantiate(ball, position, Quaternion.Euler(direction));
        Destroy(testBall.GetComponent<MeshRenderer>());
        testBall.name = "testBall";

        SceneManager.SetActiveScene(mainScene);
    }

    void CalculateTrajectory()
    {
        testBall.GetComponent<Transform>().position = position;
        testBall.GetComponent<Transform>().rotation = Quaternion.Euler(direction);

        //testBall.GetComponent<Rigidbody>().angularVelocity = (Quaternion.Euler(direction) * Vector3.right) * 720 * Mathf.Deg2Rad;
        testBall.GetComponent<Rigidbody>().velocity = direction * velocity;

        for (int i = 0; i < arcPointArray.Length; i++)
        {
            physicsScene.GetPhysicsScene().Simulate(Time.fixedDeltaTime * 4);
            arcPointArray[i] = testBall.GetComponent<Transform>().position;
        }

        print(direction);
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 point in arcPointArray)
        {
            Gizmos.DrawWireSphere(point, .1f);
        }
    }
}
