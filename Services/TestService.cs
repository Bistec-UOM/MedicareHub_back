using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TestService
    {
        private readonly IRepository<Test> _test;
        public TestService(IRepository<Test> test)
        {
            _test = test;
        }

        public async Task<IEnumerable<Test>> GetAllTests()
        {
            return await _test.GetAll();
        }

        public async Task<Test> GetTest(int id)
        {
                return await _test.Get(id);
        }

        public async Task Addtest(Test item)
        {
            await _test.Add(item);
        }

        public async Task EditTest(Test item)
        {
            await _test.Update(item);
        }

        public async Task DeleteTest(int id)
        {
            var x = await _test.Get(id);
            if (x != null)
            {
                await _test.Delete(id);
            }
        }
    }
}
