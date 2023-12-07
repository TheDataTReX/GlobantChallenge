using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GlobantChallenge.Models;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GlobantChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly GlobantChallengeContext _context;

        public JobController(GlobantChallengeContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadJobs(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se proporcionó un archivo o el archivo está vacío.");
            }

            var list = new List<JobCsv>();

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using (var stream = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(stream, config))
            {
                csv.Context.RegisterClassMap<JobCsvMap>();
                list = csv.GetRecords<JobCsv>().ToList();
            }

            // Obtener IDs existentes de la base de datos
            var existingJobIds = new HashSet<int>(await _context.Jobs.Select(j => j.Id).ToListAsync());

            const int batchSize = 1000; // Tamaño del lote
            var currentBatch = new List<Job>();

            foreach (var csvRecord in list)
            {
                if (!existingJobIds.Contains(csvRecord.Id))
                {
                    var job = new Job
                    {
                        Id = csvRecord.Id,
                        Job1 = csvRecord.JobTitle
                    };
                    currentBatch.Add(job);

                    // Guardar por lotes de 1000 o cuando se llegue al final de la lista
                    if (currentBatch.Count >= batchSize || csvRecord == list.Last())
                    {
                        _context.Jobs.AddRange(currentBatch);

                        try
                        {
                            await _context.SaveChangesAsync();
                            currentBatch.Clear(); // Limpiar el lote actual después de guardar
                        }
                        catch (Exception ex)
                        {
                            // Si hay un error al guardar, devolver el error
                            return StatusCode(500, $"Error al guardar los datos: {ex.Message}");
                        }
                    }
                }
            }

            return Ok("Todos los datos válidos se procesaron con éxito.");
        }
    }
}
    