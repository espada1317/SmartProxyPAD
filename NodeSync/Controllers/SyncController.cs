using Common.Models;
using Microsoft.AspNetCore.Mvc;
using NodeSync.Services;

namespace NodeSync.Controllers
{
    [Route("[controller]")]
     [ApiController]
     public class SyncController : ControllerBase
     {
          private readonly SyncService _service;

          public SyncController(SyncService service)
          {
               _service = service;
          }

          [HttpPost]
          public IActionResult Sync(SyncEntity entity)
          {
               _service.AddItem(entity);
               return Ok();
          }
     }
}
