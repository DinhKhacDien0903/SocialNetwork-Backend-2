using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Unit
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialNetworkdDataContext _context;

        public UnitOfWork(SocialNetworkdDataContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
