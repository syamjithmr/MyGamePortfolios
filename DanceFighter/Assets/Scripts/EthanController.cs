using UnityEngine;

//[Flags]
public enum DanceMoves
{
    None,
    Dance1,
    Dance2,
    Dance3
}
public enum FightMoves
{
    None,
    Fight1,
    Fight2,
    Fight3
}

public class EthanController : MonoBehaviour
{
    Animator ethenAnimator;

    float forwardRotationAngle;

    public DanceMoves SelectedDanceMove;
    public FightMoves SelectedFightMove;
    public bool IsDancing { get; set; }
    public bool IsFighting { get; set; }
    public bool GameStarted { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        forwardRotationAngle = 1.0f;
        ethenAnimator = GetComponent<Animator>();
        SelectedDanceMove = DanceMoves.None;
        IsFighting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStarted)
        {
            ethenAnimator.SetBool("Walking", Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
            ethenAnimator.SetBool("Running", Input.GetKey(KeyCode.Space));
            SetDanceAnimation();
            SetFightAnimation();

            if (Input.GetKey(KeyCode.W))
                forwardRotationAngle = 0.0f;
            if (Input.GetKey(KeyCode.A))
                forwardRotationAngle = -90.0f;
            if (Input.GetKey(KeyCode.S))
                forwardRotationAngle = 180.0f;
            if (Input.GetKey(KeyCode.D))
                forwardRotationAngle = 90.0f;
            if (forwardRotationAngle != 1.0f)
            {
                Vector3 cameraDirectionXZ = Vector3.ProjectOnPlane(transform.position - Camera.main.transform.position, transform.up);
                Vector3 forwardRotatedXZ = Quaternion.AngleAxis(forwardRotationAngle, transform.up) * cameraDirectionXZ;
                //print("Curr XZ positions: " + cameraDirectionXZ + ", Rotated XZ positions: " + forwardRotatedXZ);
                transform.forward = forwardRotatedXZ.normalized;
            }
            forwardRotationAngle = 1.0f;
        }
    }

    void SetDanceAnimation()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (!IsDancing && SelectedDanceMove == DanceMoves.Dance1)
            {
                ethenAnimator.SetBool("Dance1", true);
            }
            else if (!IsDancing && SelectedDanceMove == DanceMoves.Dance2)
            {
                ethenAnimator.SetBool("Dance2", true);
            }
            else if (!IsDancing && SelectedDanceMove == DanceMoves.Dance3)
            {
                ethenAnimator.SetBool("Dance3", true);
            }
        }
    }

    void SetFightAnimation()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (!IsFighting && SelectedFightMove == FightMoves.Fight1)
            {
                ethenAnimator.SetBool("Fight1", true);
            }
            else if (!IsFighting && SelectedFightMove == FightMoves.Fight2)
            {
                ethenAnimator.SetBool("Fight2", true);
            }
            else if (!IsFighting && SelectedFightMove == FightMoves.Fight3)
            {
                ethenAnimator.SetBool("Fight3", true);
            }
        }
    }

    public void SetDanceMove(int danceMove)
    {
        SelectedDanceMove = (DanceMoves)danceMove;
    }

    public void SetFightMove(int fightMove)
    {
        SelectedFightMove = (FightMoves)fightMove;
    }

    public void StartGame()
    {
        GameStarted = true;
    }
}
