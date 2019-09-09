using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        byte[] bytes = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(bytes, 0, (int)stream.Length);
                        string data = Encoding.ASCII.GetString(bytes);
                    }
                }
            }

            return Ok(new { count = files.Count, size, filePath });
        }
    }
}