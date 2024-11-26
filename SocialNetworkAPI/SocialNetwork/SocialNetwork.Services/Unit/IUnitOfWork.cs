using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Unit
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
    }
}
