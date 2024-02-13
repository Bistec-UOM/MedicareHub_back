using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
   public class Prescript_drugService
    { 
        private readonly IRepository<Prescript_drug> _pre;
        public Prescript_drugService(IRepository<Prescript_drug> pre)
        {
            _pre = pre;
        }

        public async Task AddPrecript_drug(Prescript_drug item)
        {
            await _pre.Add(item);
        }
    }
}
