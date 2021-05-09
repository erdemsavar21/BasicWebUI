using System;
using Core.Entities;

namespace Core.Entities.Concrete
{
    public class OperationClaim : IEntity
    {
        public OperationClaim()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
