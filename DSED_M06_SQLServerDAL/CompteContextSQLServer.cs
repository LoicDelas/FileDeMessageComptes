using DSED_M06_SQLServerDAL.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_SQLServerDAL
{
    public class CompteContextSQLServer : DbContext
    {
        public DbSet<CompteSQLDTO> Compte => Set<CompteSQLDTO>();
        public DbSet<TransactionSQLDTO> Transaction => Set<TransactionSQLDTO>();

        public CompteContextSQLServer()
        {
            ;
        }

        public CompteContextSQLServer(DbContextOptions options) : base(options)
        {
            ;
        }
    }
}
