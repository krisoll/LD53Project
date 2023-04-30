using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Entity
{
    public int resourceLimit;
    public bool isFull;
    public bool isGoingToExtractResources;
    public float extractionRate;
    public Resource currentResource;
    private Coroutine extractResourcesCoroutine;
    private RatState currentState;
    public GameObject target;
    [SerializeField] private string stateName;
    private void Start()
    {
        ChangeState(new Idling(this));
    }
    public void Update()
    {
        currentState.Running();
    }
    public override void MoveTo(Vector3 movepos)
    {
        base.MoveTo(movepos);
        ChangeState(new MoveToPos(this));
    }
    public override void InteractWith(GameObject g)
    {
        base.InteractWith(g);
        target = g;
        ChangeState(new GoToExtract(this));
        //Debug.Log("Interacting entity of " + gameObject.name + " is " + g.name);
    }
    void ChangeState(RatState state)
    {
        currentState?.End();
        currentState = null;
        currentState = state;
        currentState?.Start();
        stateName = currentState.GetType().ToString();
    }
    #region [GoToDeposit]
    private class GoToDeposit : RatState
    {
        GameObject target;
        CityCenter center;
        public GoToDeposit(Rat rat) : base(rat)
        {
            this.rat = rat;
        }

        public override void End()
        {
            rat.currentResource.quantity = 0;
        }

        public override void Running()
        {
            if ((center.transform.position - rat.transform.position).magnitude < 3f)
            {
                center?.AddResource(rat.currentResource);
                rat.ChangeState(new GoToExtract(rat));
            }
        }

        public override void Start()
        {
            target = rat.target;
            center = GameManager.Instance.cityCenter;
            rat.agent.SetDestination(center.transform.position);
        }
    }
    #endregion
    #region [GoToExtract]
    private class GoToExtract : RatState
    {
        GameObject target;

        public GoToExtract(Rat rat) : base(rat)
        {
            this.rat = rat;
        }

        public override void End()
        {
        }

        public override void Running()
        {
            if (target != null)
            {
                if ((target.transform.position - rat.transform.position).magnitude < 1f)
                {
                    rat.ChangeState(new Extracting(rat));
                }
            }
        }

        public override void Start()
        {
            target = rat.target;
            rat.agent.SetDestination(target.transform.position);
        }
    }
    #endregion
    #region [Extracting]
    private class Extracting : RatState
    {
        GameObject target;
        Coroutine extracting = null;
        public Extracting(Rat rat) : base(rat)
        {
            this.rat = rat;
        }
        IEnumerator ExtractResourceCoroutine()
        {
            rat.currentResource.quantity = 0;
            Debug.Log(rat.gameObject.name + " started extracting resources from " + target.name);
            ResourceEntity resource = target.GetComponent<ResourceEntity>();
            if (resource != null) rat.currentResource.type = resource.type;
            while (rat.currentResource.quantity < rat.resourceLimit)
            {
                yield return new WaitForSeconds(rat.extractionRate);
                rat.currentResource.quantity += 1;
            }
            rat.isFull = true;
            rat.ChangeState(new GoToDeposit(rat));
        }
        public override void End()
        {
            if (extracting != null) rat.StopCoroutine(extracting);
        }

        public override void Running()
        {
        }

        public override void Start()
        {
            target = rat.target;
            extracting = rat.StartCoroutine(ExtractResourceCoroutine());
        }
    }
    #endregion
    #region [MoveToPos]
    private class MoveToPos : RatState
    {
        public MoveToPos(Rat rat) : base(rat)
        {
        }

        public override void End()
        {
        }

        public override void Running()
        {
            if ((rat.agent.destination - rat.transform.position).magnitude < 0.5f)
            {
                rat.ChangeState(new Idling(rat));
            }
        }

        public override void Start()
        {
        }
    }
    #endregion
    #region [Idling]
    private class Idling : RatState
    {
        public Idling(Rat rat) : base(rat)
        {
        }

        public override void End()
        {
        }

        public override void Running()
        {
        }

        public override void Start()
        {
        }
    }
    #endregion
    private abstract class RatState
    {
        public Rat rat;
        public RatState(Rat rat)
        {
            this.rat = rat;
        }
        public abstract void Start();
        public abstract void Running();
        public abstract void End();
    }
}
