using Stateless;

namespace CustomWorkflow;

public class Workflow
{
    public Guid WorkflowId { get; set; }
    private StateMachine<OrderState, OrderTrigger> _stateMachine;
    private readonly Dictionary<Guid, OrderState> _inMemoryDatabase;

    public Workflow(Dictionary<Guid, OrderState> inMemoryDatabase, Guid? workflowId = null, OrderState? currentState = null)
    {
        _inMemoryDatabase = inMemoryDatabase;
        WorkflowId = workflowId ?? Guid.NewGuid();
        InitializeStateMachine(currentState ?? OrderState.NewOrder);
        SaveState();
    }

    private void InitializeStateMachine(OrderState initialState)
    {
        _stateMachine = new StateMachine<OrderState, OrderTrigger>(initialState);

        _stateMachine.Configure(OrderState.NewOrder)
            .Permit(OrderTrigger.StartProcessing, OrderState.Processing)
            .Permit(OrderTrigger.SkipProcessing, OrderState.Completed)
            .OnEntry(_ =>
            {
                Console.WriteLine("NewOrder step processing start..");
            })
            .OnExit(_ =>
            {
                Console.WriteLine("NewOrder step processing finished");
            });
            
        _stateMachine.Configure(OrderState.Processing)
            .Permit(OrderTrigger.FinishProcessing, OrderState.Completed)
            .OnEntry(_ =>
            {
                Console.WriteLine("Processing step processing start..");
            })
            .OnExit(_ =>
            {
                Console.WriteLine("Processing step processing finished");
            });

        _stateMachine.Configure(OrderState.Completed)
            .OnEntry(_ =>
            {
                Console.WriteLine("Workflow completed");
            });

        _stateMachine.OnTransitionCompleted(_ => SaveState());
    }

    public void Initialize() => SaveState();

    public void FireTrigger(OrderTrigger trigger)
    {
        if (_stateMachine.CanFire(trigger))
        {
            _stateMachine.Fire(trigger);
        }
    }

    private void SaveState()
    {
        _inMemoryDatabase[WorkflowId] = _stateMachine.State;
    }

    public OrderState GetCurrentState()
    {
        return _stateMachine.State;
    }
}