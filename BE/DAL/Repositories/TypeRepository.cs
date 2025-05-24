using DAL.Data;
using DAL.Entities;
using DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TypeRepository : GenericRepository<Types>, ITypeRepository
    {
        public TypeRepository(SpeakAIContext context) : base(context)
        {
        }
        public async Task<Types> GetByIdAsyncc(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
