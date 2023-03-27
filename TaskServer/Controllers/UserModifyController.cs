using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TaskServer.Controllers.DTO;
using TaskServer.Controllers.Schemes;
using TaskServer.Interfaces;
using TaskServer.Entities;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Intrinsics.X86;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TaskServer.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "BasicAuthentication")]
[Route("Auth")]
public class UserModifyController : ControllerBase
{
    private readonly IUsers _users;

    public UserModifyController(IUsers users)
    {
        _users = users;
    }

    [HttpPost("CreateUser")]
    [Consumes("application/xml")]
    public IActionResult Create([FromBody] UserCreateRequest request)
    {
        _users.Create(request.User);

        var serializer = new XmlSerializer(typeof(UserCreateResponse));

        using (var stringWriter = new StringWriter())
        {
            var response = new UserCreateResponse { User = request.User };
            serializer.Serialize(stringWriter, response);

            var xmlString = stringWriter.ToString();

            return Content(xmlString, "application/xml");
        }
    }

    [HttpPost("RemoveUser")]
    [Consumes("application/json")]
    public Object Remove([FromBody] UserRemoveRequest request)
    {
        try
        {
            var user = _users.Remove(request.RemoveUser.Id);
            return new
            {
                Msg = "User was removed",
                Success = true,
                user
            };
        } catch(Exception e) {
            return new
            {
                Msg = e.Message,
                ErrorId = 2,
                Success = false
            };
        }
    }

    [HttpPost("SetStatus")]
    [Consumes("application/x-www-form-urlencoded")]
    public Object SetStatus([FromForm] UserChangeStatusRequest request)
    {
        try
        {
            return _users.SetStatus(
                request.Id,
                (UserStatus) Enum.Parse(typeof(UserStatus), request.NewStatus)
            );
        }
        catch (Exception e)
        {
            return new
            {
                Msg = e.Message,
                ErrorId = 2,
                Success = false
            };
        }
    }
}

