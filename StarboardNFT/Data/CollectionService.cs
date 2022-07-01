using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarboardNFT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarboardNFT.Interface;
using StarboardNFT.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StarboardNFT.Utilities;
using System.IO;

namespace StarboardNFT.Data
{
    public class CollectionService
    {
        #region Private Variables
        private readonly IHttpContextAccessor _accessor;
        private readonly LinkGenerator _generator;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileService> _logger;

        public CollectionService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor,
            LinkGenerator generator,
            ILogger<ProfileService> logger)
        {
            _context = context;
            _userManager = userManager;
            _accessor = accessor;
            _generator = generator;
            _logger = logger;
        }

        #endregion

        public async Task<Guid> CreateCollection(Collection collection)
        {
            await _context.Collection.AddAsync(collection);
            await _context.SaveChangesAsync();
            return collection.Id;
        }

        public async Task<List<Collection>> GetCollectionsByProfileHeaderId(Guid UserProfileId)
        {
            List<Collection> collections = await _context.Collection.Where(x => x.UserProfileId == UserProfileId && x.Active == true)
                .AsNoTracking()
                .ToListAsync(); 
            return collections;
        }
    }
}
