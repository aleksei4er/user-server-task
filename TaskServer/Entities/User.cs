using System;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TaskServer.Entities;

namespace TaskServer.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserStatus
    {
        New, Active, Blocked, Deleted
    }

    public class User
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string? Name { get; set; }

        public UserStatus Status { get; set; }

        public User()
        {
        }
    }
}

