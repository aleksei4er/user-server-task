using System;
using System.Xml.Serialization;
using TaskServer.Entities;

namespace TaskServer.Controllers.DTO
{
    [XmlRoot("Request")]
    public class UserCreateRequest
    {
        [XmlElement("user")]
        public User User { get; set; }
    }
}

