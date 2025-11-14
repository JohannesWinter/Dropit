using UnityEngine;

public class Gnome_Script : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;
    public int attackMode;
    
    public bool runningAnim;
    public bool attackingAnim;
    public bool fleeing;
    public string currentState;
    public float gravity;
    float velocity;
    float movementSpeed;
    public float baseMovementSpeed;
    float movementSpeedModifier;
    float rotationSpeedModifier;
    public float baseRotationSpeed;
    float rotationSpeed;
    public float aimX, aimZ;
    GameObject nearestToAttack;
    public float attackDamage;
    public int attackedHall;
    public float idleWaitTimer;
    float discoverdDuration;
    public float discoveryDuration;
    public Renderer gnomeRenderer;
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.characterController = GetComponent<CharacterController>();
        animator.applyRootMotion = false;
        aimX = transform.position.x;
        aimZ = transform.position.z;
        movementSpeedModifier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.paused == false)
        {
            string newState = "";

            if (runningAnim)
            {
                newState = "Gnome_Walking";
            }
            else if (attackingAnim)
            {
                newState = "Gnome_Attack";
            }
            else
            {
                newState = "Gnome_Idle";
            }

            if (newState != currentState)
            {
                animator.CrossFade(newState, 0.5f);
                currentState = newState;
            }

            //detecting
            Vector3 mouseScreenPos = Input.mousePosition;
            Vector3 objectScreenPos = Manager.m.getCurrentCamera().WorldToScreenPoint(this.transform.position);
            float screenDistance = Vector2.Distance((Vector2)Input.mousePosition, (Vector2)Manager.m.getCurrentCamera().WorldToScreenPoint(this.transform.position));
            if (screenDistance < 25)
            {
                discoverdDuration += Time.deltaTime;
            }
            else
            {
                discoverdDuration -= Time.deltaTime;
                if (discoverdDuration < 0)
                {
                    discoverdDuration = 0;
                }
            }
            if (discoverdDuration >= discoveryDuration && attackMode > 0)
            {
                attackMode -= 1;
                fleeing = true;
                Manager.m.factorySpeaker.laught(Manager.m.getNearestDropperCamera(transform.position));
                if (attackMode > 0)
                {
                    discoverdDuration = 0;
                    Reposition(70, 90, 60, 85);
                }
            }
            gnomeRenderer.material.color = new Color(1, 1 - (discoverdDuration / discoveryDuration), 1 - (discoverdDuration / discoveryDuration));


            //movement
            if (velocity < -gravity * 5)
            {
                Destroy(this.gameObject);
            }
            if (fleeing)
            {
                movementSpeedModifier = 3;
                rotationSpeedModifier = 3;
            }
            else
            {
                movementSpeedModifier = 1;
                rotationSpeedModifier = 1;
            }
            movementSpeed = baseMovementSpeed * movementSpeedModifier;
            rotationSpeed = baseRotationSpeed * rotationSpeedModifier;

            animator.SetFloat("WalkSpeed", movementSpeed * 0.4f);
            if (characterController.isGrounded)
            {
                velocity = 0;
            }
            else
            {
                velocity -= gravity * Time.deltaTime;
            }
            characterController.Move(new Vector3(0, velocity, 0) * Time.deltaTime);
            Vector3 movementDirection = new Vector3(aimX - transform.position.x, 0, aimZ - transform.position.z);
            if (movementDirection.magnitude > 0.5f * gameObject.transform.localScale.y && nearestToAttack == null)
            {
                runningAnim = true;
            }
            else
            {
                runningAnim = false;
            }
            if (movementDirection.magnitude > movementDirection.normalized.magnitude * Time.deltaTime * 2 * movementSpeed && nearestToAttack == null)
            {
                float movementAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + 180;
                float delta = Mathf.DeltaAngle(gameObject.transform.eulerAngles.y, movementAngle);
                if (movementDirection.magnitude > movementDirection.normalized.magnitude * Time.deltaTime * 10) //Prevent Gnome from rotating fast at end of the path
                {
                    this.gameObject.transform.Rotate(0, delta * Time.deltaTime * rotationSpeed, 0);
                }

                float angleDifference = Mathf.Abs(Mathf.DeltaAngle(movementAngle, gameObject.transform.eulerAngles.y));

                movementDirection = movementDirection.normalized;
                characterController.Move(movementDirection * Time.deltaTime * movementSpeed * gameObject.transform.localScale.y * (1 - angleDifference * (1 / 180f)));
            }
            if (attackMode > 0 && attackingAnim == false && runningAnim == false)
            {
                idleWaitTimer += Time.deltaTime;
                if (idleWaitTimer > 1)
                {
                    idleWaitTimer = 0;
                    Reposition(20, 50, 20, 50);
                }
            }
            if (runningAnim == false)
            {
                fleeing = false;
            }
            //attacking
            if (attackMode > 0)
            {
                RepairDropper[] factroyObjects = GameObject.FindObjectsByType<RepairDropper>(FindObjectsSortMode.None);
                RepairDropper nearest = null;
                float minDistance = Mathf.Infinity;

                foreach (RepairDropper r in factroyObjects)
                {
                    Vector3 rposition2D = new Vector3(r.gameObject.transform.position.x, this.gameObject.transform.position.y, r.gameObject.transform.position.z);
                    float distSqr = Mathf.Abs((rposition2D - gameObject.transform.position).magnitude);
                    if (distSqr < minDistance && r.isScrap == false && r.durability > 0 && r.gameObject.tag == "FactoryObject" && r.factoryHall == attackedHall)
                    {
                        minDistance = distSqr;
                        nearest = r;
                    }
                }
                if (nearest == null || fleeing)
                {
                    nearestToAttack = null;
                    attackingAnim = false;
                    return;
                }
                else
                {
                    aimX = nearest.gameObject.transform.position.x;
                    aimZ = nearest.gameObject.transform.position.z;
                }
                Vector3 nearestPosition = new Vector3(nearest.gameObject.transform.position.x, this.gameObject.transform.position.y, nearest.gameObject.transform.position.z);
                Vector3 nearestDirection = gameObject.transform.position - nearestPosition;

                float movementAngle = Mathf.Atan2(nearestDirection.x, nearestDirection.z) * Mathf.Rad2Deg;
                float delta = Mathf.DeltaAngle(gameObject.transform.eulerAngles.y, movementAngle);
                float angleDifference = Mathf.Abs(Mathf.DeltaAngle(movementAngle, gameObject.transform.eulerAngles.y));

                if (nearestDirection.magnitude < 10)
                {
                    this.gameObject.transform.Rotate(0, delta * Time.deltaTime * rotationSpeed, 0);
                    nearestToAttack = nearest.gameObject;
                    runningAnim = false;
                    if (angleDifference < 35 && nearestDirection.magnitude < 10)
                    {
                        attackingAnim = true;
                    }
                    else
                    {
                        attackingAnim = false;
                    }
                }
                else
                {
                    attackingAnim = false;
                    nearestToAttack = null;
                }
            }
            else
            {
                Vector2 closestExit = new Vector2(Mathf.Infinity, Mathf.Infinity);
                float closestDistance = Mathf.Infinity;
                nearestToAttack = null;
                attackingAnim = false;
                fleeing = true;
                for (int i = 0; i < Manager.m.factoryExits.Count; i++)
                {
                    float distance = (new Vector2(gameObject.transform.position.x, gameObject.transform.position.z) - Manager.m.factoryExits[i]).magnitude;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestExit = Manager.m.factoryExits[i];
                    }
                }
                aimX = closestExit.x;
                aimZ = closestExit.y;

                if (closestDistance < 1)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public void PlayStepSound()
    {
        Manager.m.factorySpeaker.step(Manager.m.getNearestDropperCamera(transform.position));
    }
    public void Attack()
    {
        if (nearestToAttack != null)
        {
            nearestToAttack.GetComponent<RepairDropper>().durability -= attackDamage;
            if (nearestToAttack.GetComponent<RepairDropper>().durability <= 0)
            {
                nearestToAttack = null;
                aimX = gameObject.transform.position.x;
                aimZ = gameObject.transform.position.z;
            }
        }
        Manager.m.factorySpeaker.attack(Manager.m.getNearestDropperCamera(transform.position));
    }
    void Reposition(float minX, float maxX, float minZ, float maxZ)
    {
        if (attackedHall == 0) attackedHall = 1;
        int modifier = Random.Range(0, 2);
        if (modifier == 0)
            modifier--;
        aimX = Manager.m.factoryCenters[attackedHall - 1].x + modifier * Random.Range(minX, maxX);
        modifier = Random.Range(0, 2);
        if (modifier == 0)
            modifier--;
        aimZ = Manager.m.factoryCenters[attackedHall - 1].y + modifier * Random.Range(minZ, maxZ);
    }

    int ChangeFactoryHall(int currentHall)
    {
        int random3 = Random.Range(0, 3);
        int random5 = Random.Range(0, 5);
        if (currentHall == 0)
            return 1;
        if (currentHall == 1)
        {
            switch (random3)
            {
                case 0:
                    {
                        return 2;
                    }
                case 1:
                    {
                        return 6;
                    }
                case 2:
                    {
                        return 7;
                    }
            }
        }
        else if (currentHall == 5)
        {
            switch (random3) 
            {
                case 0:
                    {
                        return 4;
                    }
                case 1:
                    {
                        return 10;
                    }
                case 2:
                    {
                        return 9;
                    }
            }
        }
        else if (currentHall == 6)
        {
            switch (random3)
            {
                case 0:
                    {
                        return 1;
                    }
                case 1:
                    {
                        return 2;
                    }
                case 2:
                    {
                        return 7;
                    }
            }
        }
        else if (currentHall == 10)
        {
            switch (random3)
            {
                case 0:
                    {
                        return 5;
                    }
                case 1:
                    {
                        return 4;
                    }
                case 2:
                    {
                        return 9;
                    }
            }
        }
        else
        {
            switch (random5)
            {
                case 0:
                    {
                        return currentHall + 1;
                    }
                case 1:
                    {
                        return currentHall - 1;
                    }
                case 2:
                    {
                        return (currentHall + 5) % 10;
                    }
                case 3:
                    {
                        return ((currentHall + 5) % 10) + 1;
                    }
                case 4:
                    {
                        return ((currentHall + 5) % 10) - 1;
                    }
            }
        }
        return currentHall;
    }
}
