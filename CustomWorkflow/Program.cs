namespace CustomWorkflow
{
    class Program
    {
        void Main(string[] args)
        {
            var inMemoryDatabase = new Dictionary<Guid, OrderState>();
            WorkflowWithFullPathExample(inMemoryDatabase);
            WorkflowWithSkippedStepsExample(inMemoryDatabase);
            WorkflowWithRestoreExample(inMemoryDatabase);
        }

        void WorkflowWithFullPathExample(Dictionary<Guid, OrderState> inMemoryDatabase)
        {
            var workflow = new Workflow(inMemoryDatabase);
            workflow.Initialize();

            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
                
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
                
            workflow.FireTrigger(OrderTrigger.SkipProcessing);
            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
        }

        /// <summary>
        /// This example shows we can continue processing workflow at any moment afeter some pause, we need only workflowId to proceed
        /// </summary>
        /// <param name="inMemoryDatabase"></param>
        void WorkflowWithRestoreExample(Dictionary<Guid, OrderState> inMemoryDatabase)
        {
            var workflow = new Workflow(inMemoryDatabase);
            workflow.Initialize();

            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");
                
            workflow.FireTrigger(OrderTrigger.StartProcessing);
            Console.WriteLine($"Current workflow ID is {workflow.WorkflowId}, current state is {workflow.GetCurrentState()}");

            var workflowId = workflow.WorkflowId;

            var restoredWorkflow = new Workflow(inMemoryDatabase, workflowId, inMemoryDatabase[workflowId]);
            workflow.FireTrigger(OrderTrigger.FinishProcessing);
            Console.WriteLine($"Current workflow ID is {restoredWorkflow.WorkflowId}, current state is {restoredWorkflow.GetCurrentState()}");
        }
    }
}
