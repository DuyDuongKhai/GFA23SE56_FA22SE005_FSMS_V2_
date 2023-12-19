using FSMS.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Entity.Repositories.SeasonRepositories
{
    public class SeasonRepository : RepositoryBase<Season>, ISeasonRepository
    {
        public SeasonRepository() { }
    }
}
