using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test1.Model;

namespace test1.Services
{
    public interface GetTeamMember
    {
        public TeamMember getTeamMember(int id);
    }
}