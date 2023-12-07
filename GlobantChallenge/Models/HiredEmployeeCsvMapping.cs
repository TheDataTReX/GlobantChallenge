using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace GlobantChallenge.Models
{
    public class HiredEmployeeCsv
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
        // Estos campos pueden ser nulos en el archivo CSV
        public int? DepartmentId { get; set; }
        public int? JobId { get; set; }
    }

    public class HiredEmployeeCsvMap : ClassMap<HiredEmployeeCsv>
    {
        public HiredEmployeeCsvMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("name");
            Map(m => m.DateTime).Name("datetime");
            // Añadir TypeConverter para manejar la conversión de enteros nulos
            Map(m => m.DepartmentId).Name("department_id").TypeConverter<NullableIntConverter>();
            Map(m => m.JobId).Name("job_id").TypeConverter<NullableIntConverter>();
        }
    }

    // El convertidor personalizado maneja la conversión de la cadena vacía a nulo
    public class NullableIntConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
