using System;
namespace Core.CrossCuttingConcerns.Logging
{
    public class LogParameter
    {
        public LogParameter()
        {
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }

    }
}
