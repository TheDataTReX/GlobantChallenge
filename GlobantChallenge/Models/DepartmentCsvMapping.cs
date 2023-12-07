using CsvHelper.Configuration;

namespace GlobantChallenge.Models
{
    public class DepartmentCsv
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
    }

    public class DepartmentCsvMap : ClassMap<DepartmentCsv>
    {
        public DepartmentCsvMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.DepartmentName).Name("department");
        }
    }
}
