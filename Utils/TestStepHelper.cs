using System;

namespace FionaAutomation.Utils
{
    public static class TestStepHelper
    {
        public static async Task RunValidation(string testCaseId, string description, Func<Task> validationFunc)
        {
            try
            {
                await validationFunc();
                TestStepLogger.LogResult(testCaseId, description, true);
            }
            catch (Exception ex)
            {
                TestStepLogger.LogResult(testCaseId, description, false, ex.Message);
            }
        }
    }
}