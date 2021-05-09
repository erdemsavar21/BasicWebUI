using System;
namespace Core.CrossCuttingConcerns.Logging
{
    public class LogDetailWithException:LogDetail
    {
        public LogDetailWithException()
        {
        }

        public string ExceptionMessage { get; set; }
    }
}
