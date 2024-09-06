namespace CustomWorkflow
{
    class Program
    {
        void Main(string[] args)
        {
            var inMemoryDatabase = new Dictionary<Guid, OrderState>();
            WorkflowWithFullPathExample(inMemoryDatabase);
            WorkflowWithSkippedStepsExample(inMemoryDatabase);
        }

        void WorkflowWithFullPathExample(Dictionary<Guid, OrderState> inMemoryDatabase)
        {
            var workflow = new Workflow(inMemoryDatabase);
            workflow.Initialize();

            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
            Console.WriteLine($"Current state is {workflow.GetCurrentState()}");
                
            workflow.FireTrigger(OrderTrigger.StartProcessing);
            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
            
            workflow.FireTrigger(OrderTrigger.FinishProcessing);
            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
        }

        void WorkflowWithSkippedStepsExample(Dictionary<Guid, OrderState> inMemoryDatabase)
        {
            var workflow = new Workflow(inMemoryDatabase);
            workflow.Initialize();

            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
            Console.WriteLine($"Current state is {workflow.GetCurrentState()}");
                
            workflow.FireTrigger(OrderTrigger.SkipProcessing);
            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
        }
    }
}
