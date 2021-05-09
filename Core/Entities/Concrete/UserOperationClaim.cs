using System;
using Core.Entities;

namespace Core.Entities.Concrete
{
    public class UserOperationClaim : IEntity
    {
        public UserOperationClaim()
        {
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
