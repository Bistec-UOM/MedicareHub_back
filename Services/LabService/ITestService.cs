using Models.DTO.Lab;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LabService
{
    public interface ITestService
    {
        public Task<IEnumerable<Test>> GetAllTests();

        public Task<Test> GetTest(int id);

        public Task Addtest(Test item);

        public Task EditTest(Test item);

        public Task DeleteTest(int id);

        public Task<IEnumerable<ReportFields>> GetAllFields();

        public Task<ReportFields> GetField(int id);

        public Task<IEnumerable<ReportFields>> GetFieldByTest(int id);

        public Task AddTemplate(TemplateObj data);

        public Task EditField(ReportFields item);

        public Task EditTemplate(EdittemplateObj data);

        public Task UpdateFields(List<ReportFields> data);

        public Task DeleteField(int id);
    }
}