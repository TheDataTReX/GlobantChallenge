using CsvHelper.Configuration;

namespace GlobantChallenge.Models
{
    public class JobCsv
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
    }

    public class JobCsvMap : ClassMap<JobCsv>
    {
        public JobCsvMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.JobTitle).Name("job");
        }
    }
}
