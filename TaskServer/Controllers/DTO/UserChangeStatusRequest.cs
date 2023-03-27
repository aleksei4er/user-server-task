using System;
namespace TaskServer.Controllers.DTO
{
    public class UserChangeStatusRequest
    {
        public int Id { get; set; }
        public string NewStatus { get; set; }
    }
}

