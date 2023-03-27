using System;
using System.Xml.Serialization;
using TaskServer.Entities;

namespace TaskServer.Controllers.Schemes
{
    [XmlRoot("Response")]
    public class UserCreateResponse
    {
        private string Success = "true";
        private int ErrorId = 0;

        [XmlElement("user")]
        public User User { get; set; }
    }
}

