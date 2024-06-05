using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dozer : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public GameObject pointer;
    public float rotationSpeed = 1000f;
    public String imagePath;


    public List<AudioClip> selectionAudioClips;
    public List<AudioClip> movementAudioClips;

    public List<string> buildingImages;
    public GridLayoutGroup controlPanelGrid;

    public Button canvasButtonPrefab;

    private BuildingController buildingController;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isSelected = false;
    private int maxHealth = 100;
    private HealthBar healthBar;
    private CanvasGroup healthBarCanvasGroup;


    private AudioSource audioSource;


    private int currentPathIdx;

    private Rigidbody rb;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        buildingController = GetComponent<BuildingController>();

        //navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform);
        healthBar = healthBarInstance.GetComponent<HealthBar>();
        healthBarCanvasGroup = healthBarInstance.GetComponent<CanvasGroup>();
        healthBarInstance.transform.localPosition = new Vector3(0, 3, 0);

        rb = GetComponent<Rigidbody>();

    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetHealth(maxHealth, maxHealth);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isSelected)
        {
            healthBarCanvasGroup.alpha = 1;
        }
        else
        {
            healthBarCanvasGroup.alpha = 0;
        }
    }

    private void showControlPanelActions()
    {
        ButtonDefinition buttonDefinition = new ButtonDefinition("Barracks", "Pictures/usaBarracks", selectBuilding);
        var button = Instantiate(canvasButtonPrefab);
        button.GetComponent<ButtonHandler>().buttonDefinition = buttonDefinition;

        button.transform.SetParent(controlPanelGrid.transform, false);

        
        //controlPanelGrid.
    }

    private void selectBuilding()
    {
        buildingController.selectBuilding(BuildingType.USA_BARRACKS);
    }

    private void Update()
    {
        if (isSelected)
        {
            handleMovement();
            deselectUnit();
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance  &&  navMeshAgent.velocity.sqrMagnitude == 0f)
        {
            animator.SetBool("moving", false);
        }

    }

    private void handleMovement()
    {
        if (Input.GetButtonDown(KeyMappings.LeftClick))
        {

            bool isPointerOnPlayingSurface = pointer.GetComponent<PointerController>().isPointerOnPlayingSurface();


            // dont move if pointer not on playing surface
            if  (!isPointerOnPlayingSurface)
            {
                return;
            }

            Vector3 pointerLocation = pointer.gameObject.transform.position;
            Vector3 destination = new Vector3(pointerLocation.x, transform.position.y, pointerLocation.z);


            // do not move if destination and current location is too close (same spot)

            float distance = Vector3.Distance(transform.position, destination);

            if (distance > 1.3f)
            {
                playMovementAudio();
                StartCoroutine(RotateAndMove(destination));
            }
        }
    }

    private void deselectUnit()
    {
        if (Input.GetButtonDown(KeyMappings.RightClick))
        {
            DeSelect(gameObject.GetInstanceID());
            clearGrid();

            EventManager.TriggerEvent<DeSelectEvent, int>(gameObject.GetInstanceID());
        }
    }

    private void clearGrid()
    {
        foreach (Transform button in controlPanelGrid.transform)
        {
            Destroy(button.gameObject);
        }
    }

    private IEnumerator RotateAndMove(Vector3 destination)
    {
        // Stop any current movement
        navMeshAgent.isStopped = true;
        animator.SetBool("moving", false);
        animator.StopPlayback();

        // Rotate towards the destination
        yield return StartCoroutine(RotateTowards(destination));

        yield return new WaitForSeconds(0.1f);

        // Move to the destination
        navMeshAgent.isStopped = false;


        navMeshAgent.SetDestination(destination);
        animator.SetBool("moving", true);
    }

    private IEnumerator RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        
        if (Quaternion.Angle(transform.rotation, lookRotation) > 5f)
        {
            bool isTurningLeft = ShouldMirrorAnimation(target);

            if (isTurningLeft)
            {
                animator.SetTrigger("turnLeft");
            }
            else
            {
                animator.SetTrigger("turnRight");
            }
        }


        while (Quaternion.Angle(transform.rotation, lookRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = lookRotation; // Ensure final rotation
    }

    private bool ShouldMirrorAnimation(Vector3 targetPosition)
    {
        // Get the forward directions

        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Calculate the angle between the forward directions
        float angle = Vector3.Angle(transform.forward, targetPosition);

        // If the angle is greater than 90 degrees, mirror the animation
        return angle < 90f;
    }

    //private void OnAnimatorMove()
    //{
    //    Vector3 rootPosition = animator.rootPosition;
    //    rootPosition.y = navMeshAgent.nextPosition.y;
    //    transform.position = rootPosition;
    //    navMeshAgent.nextPosition = rootPosition;
    //}

    private void OnMouseDown()
    {
        isSelected = true;
        EventManager.TriggerEvent<SelectEvent, string, int>(imagePath, gameObject.GetInstanceID());
        playSelectionAudio();
        clearGrid();
        showControlPanelActions();
    }

    private void playSelectionAudio()
    {
        int randomIdx = Random.Range(0, selectionAudioClips.Count);
        AudioClip clip = selectionAudioClips[randomIdx];
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void playMovementAudio()
    {
        int randomIdx = Random.Range(0, movementAudioClips.Count);
        AudioClip clip = movementAudioClips[randomIdx];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void DeSelect(int id)
    {
        if (id == gameObject.GetInstanceID())
        {
            isSelected = false;
        }
    }

    private void OnDestroy()
    {
        isSelected = false;
    }

}
