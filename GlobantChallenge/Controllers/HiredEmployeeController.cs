using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlobantChallenge.Models;
using System.Globalization;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GlobantChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HiredEmployeeController : ControllerBase
    {
        private readonly GlobantChallengeContext _context;

        public HiredEmployeeController(GlobantChallengeContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadHiredEmployees(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se proporcionó un archivo o el archivo está vacío.");
            }

            var list = new List<HiredEmployeeCsv>();

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using (var stream = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(stream, config))
            {
                csv.Context.RegisterClassMap<HiredEmployeeCsvMap>();
                list = csv.GetRecords<HiredEmployeeCsv>().ToList();
            }

            const int batchSize = 1000; // Tamaño del lote
            var currentBatch = new List<HiredEmployee>();

            foreach (var csvRecord in list)
            {
                DateTime? hireDate = null;
                if (!string.IsNullOrWhiteSpace(csvRecord.DateTime))
                {
                    if (DateTime.TryParseExact(csvRecord.DateTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime parsedDate))
                    {
                        hireDate = parsedDate;
                    }
                    else
                    {
                        // Si no se puede parsear la fecha, continuar con el siguiente registro
                        continue;
                    }
                }

                var employee = new HiredEmployee
                {
                    Id = csvRecord.Id,
                    Name = csvRecord.Name,
                    HireDate = hireDate,
                    DepartmentId = csvRecord.DepartmentId,
                    JobId = csvRecord.JobId
                };

                currentBatch.Add(employee);

                // Guardar por lotes de 1000 o cuando se llegue al final de la lista
                if (currentBatch.Count >= batchSize || csvRecord == list.Last())
                {
                    _context.HiredEmployees.AddRange(currentBatch);

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

            return Ok("Todos los datos válidos se procesaron con éxito.");
        }
        [HttpGet("metrics")]
        public IActionResult GetMetrics()
        {
            var metrics = _context.HiredEmployees
                .Where(e => e.HireDate.HasValue && e.HireDate.Value.Year == 2021)
                .GroupBy(e => new { e.Department.Department1, e.Job.Job1 })
                .Select(group => new
                {
                    Department = group.Key.Department1,
                    Job = group.Key.Job1,
                    Q1 = group.Count(e => e.HireDate.HasValue && e.HireDate.Value.Month >= 1 && e.HireDate.Value.Month <= 3),
                    Q2 = group.Count(e => e.HireDate.HasValue && e.HireDate.Value.Month >= 4 && e.HireDate.Value.Month <= 6),
                    Q3 = group.Count(e => e.HireDate.HasValue && e.HireDate.Value.Month >= 7 && e.HireDate.Value.Month <= 9),
                    Q4 = group.Count(e => e.HireDate.HasValue && e.HireDate.Value.Month >= 10 && e.HireDate.Value.Month <= 12)
                })
                .OrderBy(result => result.Department)
                .ThenBy(result => result.Job)
                .ToList();

            // Crear una tabla CSV como una cadena de texto
            var csvTable = new StringBuilder();
            csvTable.AppendLine("department,job,Q1,Q2,Q3,Q4"); // Encabezados de la tabla

            foreach (var row in metrics)
            {
                csvTable.AppendLine($"{row.Department},{row.Job},{row.Q1},{row.Q2},{row.Q3},{row.Q4}");
            }

            // Devolver la tabla CSV como parte de la respuesta HTTP
            return File(Encoding.UTF8.GetBytes(csvTable.ToString()), "text/csv", "metrics.csv");
        }


        [HttpGet("department-stats")]
        public IActionResult GetDepartmentStatsCsv()
        {
            // Calcular el promedio de empleados contratados en 2021 para todos los departamentos
            var averageHires = _context.HiredEmployees
                .Where(e => e.HireDate.HasValue && e.HireDate.Value.Year == 2021)
                .GroupBy(e => e.DepartmentId)
                .Select(group => group.Count())
                .Average();

            // Obtener la lista de IDs, nombres y número de empleados contratados para departamentos que superan el promedio
            var departmentStats = _context.HiredEmployees
                .Where(e => e.HireDate.HasValue && e.HireDate.Value.Year == 2021)
                .GroupBy(e => new { e.Department.Id, e.Department.Department1 })
                .Select(group => new
                {
                    Id = group.Key.Id,
                    Department = group.Key.Department1,
                    Hired = group.Count()
                })
                .Where(stats => stats.Hired > averageHires)
                .OrderByDescending(stats => stats.Hired)
                .ToList();

            // Crear una tabla CSV como una cadena de texto
            var csvTable = new StringBuilder();
            csvTable.AppendLine("id,department,hired"); // Encabezados de la tabla

            foreach (var row in departmentStats)
            {
                csvTable.AppendLine($"{row.Id},{row.Department},{row.Hired}");
            }

            // Devolver la tabla CSV como parte de la respuesta HTTP
            return File(Encoding.UTF8.GetBytes(csvTable.ToString()), "text/csv", "department-stats.csv");
        }


    }
}