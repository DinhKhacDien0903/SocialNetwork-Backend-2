using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories
{
    public class EmotionTypeRepository : IEmotionTypeRepository
    {
        private readonly  SocialNetworkdDataContext _context;
        public EmotionTypeRepository(SocialNetworkdDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EmotionTypeEntity>> GetAllAsync()
        {
            var emotions=await _context.EmotionTypes.ToListAsync();
            return emotions;
        }
    }
}
