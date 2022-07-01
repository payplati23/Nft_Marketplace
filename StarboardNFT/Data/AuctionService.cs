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
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using StarboardNFT.Utilities;

namespace StarboardNFT.Data
{
    public class AuctionService
    {
        #region Private Variables
        private readonly IHttpContextAccessor _accessor;
        private readonly LinkGenerator _generator;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<NFTService> _logger;
        private IHubContext<NotificationHub> _hubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly NFTService _nftService;

        public AuctionService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor,
            LinkGenerator generator,
            ILogger<NFTService> logger,
            IUserConnectionManager userConnectionManager,
            NFTService nftService,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _accessor = accessor;
            _generator = generator;
            _logger = logger;
            _hubContext = hubContext;
            _userConnectionManager = userConnectionManager;
            _nftService = nftService;
        }

        #endregion

        public async Task<List<Auction>> GetAuctionList()
        {
            var auctionList = await _context.Auction.Where(x => x.Active == true && x.NFTData.Active)
                .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .ToListAsync();

            return auctionList;
        }

        public async Task<List<Auction>> GetLatestAuctionList()
        {
            List<Auction> auctionList = new List<Auction>();
            try
            {
                auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true)
                .OrderBy(x => x.CreateDateTimeUtc)
                .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .ToListAsync();

            } catch(Exception error)
            {
                Console.WriteLine(error);
            }
            
            return auctionList;
        }

        public async Task<List<Auction>> GetRecentlyAddedAuctionSale(string strSearch, bool isVerified, decimal priceRange, NFT.Categories category)
        {
            strSearch = "";
            List<Auction> auctionList = new List<Auction>();

            if (priceRange == 0)
            {
                if (category == NFT.Categories.AllItems)
                {
                    if (isVerified == true)
                    {
                        try
                        {

                            auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.CreateDateTimeUtc)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();

                        }
                        catch (Exception error)
                        {
                            Console.WriteLine(error);
                        }

                    }
                    else
                    {
                        try
                        {

                            auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                                (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                                .OrderBy(x => x.CreateDateTimeUtc)
                                .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                                .AsNoTracking()
                                .ToListAsync();

                        }
                        catch (Exception error)
                        {
                            Console.WriteLine(error);
                        }

                    }
                    
                }
                else 
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.CreateDateTimeUtc)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.CreateDateTimeUtc)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
            }
            else
            {
                if (category == NFT.Categories.AllItems)
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.CreateDateTimeUtc)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.CreateDateTimeUtc)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.CreateDateTimeUtc)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.CreateDateTimeUtc)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
            }

            return auctionList;
        }

        public async Task<List<Auction>> GetEndingSoonAuctionSale(string strSearch, bool isVerified, decimal priceRange, NFT.Categories category)
        {
            List<Auction> auctionList = new List<Auction>();

            if (priceRange == 0)
            {
                if (category == NFT.Categories.AllItems)
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
            }
            else 
            {
                if (category == NFT.Categories.AllItems)
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true &&
                            x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .OrderBy(x => x.NFTData.SaleEndtDate)
                            .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
            }

            return auctionList;
        }

        public async Task<List<IGrouping<Guid, Auction>>> GetMostViewedAuctionSale(string strSearch, bool isVerified, decimal priceRange, NFT.Categories category)
        {
            //List<Auction> auctionList = new List<Auction>();
            List<IGrouping<Guid, RecentViewNFT>> auctionList = new List<IGrouping<Guid, RecentViewNFT>>();
            List<IGrouping<Guid, Auction>> aucList = new List<IGrouping<Guid, Auction>>();

            try
            {
                var query = new List<RecentViewNFT>();

                if (category == NFT.Categories.AllItems)
                {
                    query = await _context.RecentViewNFT
                        .Where(x => x.NFTData.IsSaleEnded == false && x.NFTData.IsSaleStarted == true &&
                        (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                   .Include(x => x.NFTData)
                   .Include(x => x.NFTData.NFT)
                   .Include(x => x.NFTData.NFT.UserProfile)
                   .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                   .AsNoTracking()
                   .ToListAsync();
                }
                else
                {
                    query = await _context.RecentViewNFT
                        .Where(x => x.NFTData.IsSaleEnded == false && x.NFTData.IsSaleStarted == true && x.NFTData.NFT.Category == category &&
                        (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                   .Include(x => x.NFTData)
                   .Include(x => x.NFTData.NFT)
                   .Include(x => x.NFTData.NFT.UserProfile)
                   .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                   .AsNoTracking()
                   .ToListAsync();
                }

                if (priceRange == 0)
                {
                    if (isVerified == true)
                    {
                        try
                        {
                            aucList = query.Join(_context.Auction, RecentViewNFT => RecentViewNFT.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.RecentViewNFT.NFTDataId,
                                NFTData = t.RecentViewNFT.NFTData
                            })
                            .Where(x => x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();

                        } catch(Exception error)
                        {
                            Console.WriteLine(error);
                        }
                        
                    }
                    else
                    {
                        try
                        {
                            aucList = query.Join(_context.Auction, RecentViewNFT => RecentViewNFT.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                             .Select(t => new Auction
                             {
                                 NFTDataId = t.RecentViewNFT.NFTDataId,
                                 NFTData = t.RecentViewNFT.NFTData
                             })
                             .GroupBy(x => x.NFTDataId)
                             .OrderByDescending(x => x.Count())
                             .Take(10)
                             .ToList();

                        }
                        catch (Exception error)
                        {
                            Console.WriteLine(error);
                        }
                        
                    }
                    
                }
                else 
                {
                    if (isVerified == true)
                    {
                        aucList = query.Join(_context.Auction, RecentViewNFT => RecentViewNFT.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                    else
                    {
                        aucList = query.Join(_context.Auction, RecentViewNFT => RecentViewNFT.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex;
            }

            return aucList;
        }

        public async Task<List<IGrouping<Guid, Auction>>> GetMostFavoritesAuctionSale(string strSearch, bool isVerified, decimal priceRange, NFT.Categories category)
        {
            //List<Auction> auctionList = new List<Auction>();
            List<IGrouping<Guid, Auction>> auctionList = new List<IGrouping<Guid, Auction>>();

            try
            {
                var query = new List<NFTFavorites>();

                if (category == NFT.Categories.AllItems)
                {
                    query = await _context.NFTFavorites.Where(x => x.NFTData.IsSaleEnded == false && x.NFTData.IsSaleStarted == true &&
                        (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                   .Include(x => x.NFTData)
                   .Include(x => x.NFTData.NFT)
                   .Include(x => x.NFTData.NFT.UserProfile)
                   .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                   .AsNoTracking()
                   .ToListAsync();
                }
                else
                {
                    query = await _context.NFTFavorites.Where(x => x.NFTData.IsSaleEnded == false && x.NFTData.IsSaleStarted == true && x.NFTData.NFT.Category == category &&
                        (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                       .Include(x => x.NFTData)
                       .Include(x => x.NFTData.NFT)
                       .Include(x => x.NFTData.NFT.UserProfile)
                       .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                       .AsNoTracking()
                       .ToListAsync();
                }

                if (priceRange == 0)
                {
                    if (isVerified == true)
                    {
                        auctionList = query.Join(_context.Auction, NFTFavorites => NFTFavorites.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                    else
                    {
                        auctionList = query.Join(_context.Auction, NFTFavorites => NFTFavorites.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = query.Join(_context.Auction, NFTFavorites => NFTFavorites.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                    else
                    {
                        auctionList = query.Join(_context.Auction, NFTFavorites => NFTFavorites.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                }

                //auctionList = queryGroup;
            }
            catch (Exception ex)
            {
                var error = ex;
            }

            return auctionList;
        }

        public async Task<List<IGrouping<Guid, Auction>>> GetMostLikesAuctionSale(string strSearch, bool isVerified, decimal priceRange, NFT.Categories category)
        {
            //List<Auction> auctionList = new List<Auction>();
            List<IGrouping<Guid, Auction>> auctionList = new List<IGrouping<Guid, Auction>>();

            try
            {
                var query = new List<NFTLikes>();

                if (category == NFT.Categories.AllItems)
                {
                    query = await _context.NFTLikes.Where(x => x.NFTData.IsSaleEnded == false && x.NFTData.IsSaleStarted == true &&
                        (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                   .Include(x => x.NFTData)
                   .Include(x => x.NFTData.NFT)
                   .Include(x => x.NFTData.NFT.UserProfile)
                   .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                   .AsNoTracking()
                   .ToListAsync();
                }
                else
                {
                    query = await _context.NFTLikes.Where(x => x.NFTData.IsSaleEnded == false && x.NFTData.IsSaleStarted == true && x.NFTData.NFT.Category == category &&
                        (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                       .Include(x => x.NFTData)
                       .Include(x => x.NFTData.NFT)
                       .Include(x => x.NFTData.NFT.UserProfile)
                       .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                       .AsNoTracking()
                       .ToListAsync();
                }

                if (priceRange == 0)
                {
                    if (isVerified == true)
                    {
                        auctionList = query.Join(_context.Auction, NFTLikes => NFTLikes.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                    else
                    {
                        auctionList = query.Join(_context.Auction, NFTLikes => NFTLikes.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = query.Join(_context.Auction, NFTLikes => NFTLikes.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                    else
                    {
                        auctionList = query.Join(_context.Auction, NFTLikes => NFTLikes.NFTDataId, Auction => Auction.NFTDataId, (recent, auc) => new { RecentViewNFT = recent, Auction = auc })
                            .Select(t => new Auction
                            {
                                NFTDataId = t.Auction.NFTDataId,
                                NFTData = t.Auction.NFTData,
                            })
                            .Where(x => x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange)
                            .GroupBy(x => x.NFTDataId)
                            .OrderByDescending(x => x.Count())
                            .Take(10)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex;
            }

            return auctionList;
        }

        public async Task<List<Auction>> GetLowToHighPriceAuctionSale(string strSearch, bool isVerified, decimal priceRange, NFT.Categories category)
        {
            var auctionList = new List<Auction>();

            if (category == NFT.Categories.AllItems)
            {
                if (priceRange == 0)
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderBy(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderBy(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderBy(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderBy(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
            }
            else
            {
                if (priceRange == 0)
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                        .Include(x => x.NFTData)
                        .Include(x => x.NFTData.NFT)
                        .Include(x => x.NFTData.NFT.UserProfile)
                        .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                        .OrderBy(x => x.NFTData.FiatStartPrice)
                        .AsNoTracking()
                        .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderBy(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                        .Include(x => x.NFTData)
                        .Include(x => x.NFTData.NFT)
                        .Include(x => x.NFTData.NFT.UserProfile)
                        .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                        .OrderBy(x => x.NFTData.FiatStartPrice)
                        .AsNoTracking()
                        .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                        .Include(x => x.NFTData)
                        .Include(x => x.NFTData.NFT)
                        .Include(x => x.NFTData.NFT.UserProfile)
                        .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                        .OrderBy(x => x.NFTData.FiatStartPrice)
                        .AsNoTracking()
                        .ToListAsync();
                    }
                }
            }

            return auctionList;
        }

        public async Task<List<Auction>> GetHighToLowPriceAuctionSale(string strSearch, bool isVerified, decimal priceRange, NFT.Categories category)
        {
            var auctionList = new List<Auction>();

            if (category == NFT.Categories.AllItems)
            {
                if (priceRange == 0)
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            //.Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified)
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            //.Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified)
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
            }
            else
            {
                if (priceRange == 0)
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
                else
                {
                    if (isVerified == true)
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.UserProfile.UserProfileHeader.IsVerified == isVerified && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        auctionList = await _context.Auction
                            .Where(x => x.IsAuctionOver == false && x.Active == true && x.NFTData.NFT.Category == category && x.CurrentBidPrice / Startup.CoinPriceDict["ETH"] < priceRange &&
                            (x.NFTData.Tags.Contains(strSearch) || x.NFTData.NFT.Title.Contains(strSearch) || x.NFTData.NFT.Description.Contains(strSearch)))
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .OrderByDescending(x => x.NFTData.FiatStartPrice)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                }
            }

            return auctionList;
        }

        public async Task<List<Auction>> GetOldestAuctionSale()
        {
            var auctionList = await _context.Auction.Where(x => x.Active == true)
                .OrderByDescending(x => x.NFTData.CreateDateTimeUtc)
                .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .ToListAsync();

            return auctionList;
        }

        public System.Collections.Generic.IEnumerable<AuctionBid> GetBidListByAuctionId(Guid auctionId)
        {
            var auctionBidList = _context.AuctionBid.Where(x => x.AuctionId == auctionId && x.Active == true)
                .ToList();

            return auctionBidList;
        }

        public async Task<List<Auction>> GetSoldAuctionList()
        {
            var auctionList = await _context.Auction.Where(x => x.IsAuctionOver == true && x.Active == true)
                .Include(x => x.NFTData)
                .Include(x => x.NFTData.NFT).Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .ToListAsync();

            return auctionList;
        }

        public async Task<Auction> GetAuctionById(Guid auctionId) {
            var auction = await _context.Auction.Where(x => x.Id == auctionId && x.Active == true)
                .Include(x => x.NFTData)
                .Include(x => x.NFTData.NFT)
                .Include(x => x.NFTData.NFT.UserProfile)
                .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return auction;
        }

        public async Task<Auction> GetAuctionByNFTDataId(Guid NFTDataId)
        {
            var auction = await _context.Auction.Where(x => x.NFTDataId == NFTDataId && x.Active == true)
                .Include(x => x.NFTData)
                .Include(x => x.NFTData.NFT)
                .Include(x => x.NFTData.NFT.UserProfile)
                .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return auction;
        }

        public async Task<Auction> GetAuctionByFeaturedNFT()
        {
            Auction auction = new Auction();
            try
            {
                auction = await _context.Auction.Where(x => x.NFTData.IsFeatured == true && x.Active == true)
                  .Include(x => x.NFTData)
                  .Include(x => x.NFTData.NFT)
                  .Include(x => x.NFTData.NFT.UserProfile)
                  .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                  .AsNoTracking()
                  .FirstOrDefaultAsync();

            } catch(Exception error)
            {
                Console.WriteLine(error);
                auction = null;
            }
            

            return auction;
        }

        public async Task<Auction> GetHottestAuction()
        {
            Auction auction = new Auction();
            var auctionBids = await _context.AuctionBid.Where(x => x.Active == true).ToListAsync();

            var topAuctionBid = auctionBids.GroupBy(x => x.AuctionId).OrderBy(x => x.Count()).FirstOrDefault();

            if (topAuctionBid != null)
            {
                auction = await _context.Auction.Where(x => x.IsAuctionOver == false && topAuctionBid.FirstOrDefault().AuctionId == x.Id && x.Active == true)
                    .Include(x => x.NFTData)
                    .Include(x => x.NFTData.NFT)
                    .Include(x => x.NFTData.NFT.UserProfile)
                    .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                    .FirstOrDefaultAsync();
            }

            return auction;
        }

        public async Task<List<IGrouping<Guid, Auction>>> GetPopularSellers()
        {
            List<IGrouping<Guid, Auction>> userList = new List<IGrouping<Guid, Auction>>();
            try
            {
                var query = await _context.Auction.Where(x => x.Active == true && x.IsAuctionOver == true)
               .Include(x => x.NFTData)
               .Include(x => x.NFTData.NFT)
               .Include(x => x.NFTData.NFT.UserProfile)
               .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
               .AsNoTracking()
               .ToListAsync();

                //var queryGroup = query.GroupBy(x => x.NFTData.NFT.UserProfile.UserProfileHeader.Id)
                //    .OrderByDescending(x => x.Count())
                //    .Take(10)
                //    .ToList();

                var queryGroup = query.GroupBy(x => x.NFTData.NFT.UserProfile.UserProfileHeader.Id)
                    .OrderByDescending(x => x.Sum(x => x.CurrentBidPrice))
                    .Take(5)
                    .ToList();

                userList = queryGroup;

                userList.ForEach(x => {

                    var userName = x.FirstOrDefault().NFTData.NFT.UserProfile.UserProfileHeader.UserName; // Or do a query here with the x.Key which is the UserProfileHeaderId
                    var totalEth = x.Sum(y => y.NFTData.EthPurchaseAmount);
                });
            }
            catch(Exception ex)
            {
                var error = ex; 
            }

            return userList;
        }

        public async Task<List<IGrouping<Guid, Auction>>> GetNewSellers()
        {
            List<IGrouping<Guid, Auction>> userList = new List<IGrouping<Guid, Auction>>();
            try
            {
                var query = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true)
                    .Include(x => x.NFTData)
                    .Include(x => x.NFTData.NFT)
                    .Include(x => x.NFTData.NFT.UserProfile)
                    .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                    .AsNoTracking()
                    .ToListAsync();

                var results = query.GroupBy(n => new { n.NFTDataId, n.NFTData })
                .Select(g => new {
                    g.Key.NFTDataId,
                    g.Key.NFTData})
                .ToList();

                var realQuery = new List<Auction>();

                foreach (var q in query)
                {
                    if (_context.Auction.Where(x => x.IsAuctionOver == true && x.CurrentWinningUserId == q.CurrentWinningUserId && x.Active == true).Count() == 0)
                        realQuery.Add(q);
                }
                //var queryCount = query.Where(x => x.IsAuctionOver == true).Count();

                var queryGroup = realQuery.Where(x => x.NFTData.NFT.UserProfile.UserProfileHeader.CreateDateTimeUtc.AddDays(30) > DateTime.UtcNow)
                    .OrderByDescending(x => x.NFTData.NFT.UserProfile.UserProfileHeader.CreateDateTimeUtc)
                    .GroupBy(x => x.NFTData.NFT.UserProfile.UserProfileHeader.Id)
                    //.OrderByDescending(x => x.Count())
                    .Take(10)
                    .ToList();

                userList = queryGroup;

            }
            catch (Exception ex)
            {
                var error = ex;
            }

            return userList;
        }

        public async Task<List<IGrouping<Guid, Auction>>> GetRecentSellers()
        {
            List<IGrouping<Guid, Auction>> userList = new List<IGrouping<Guid, Auction>>();
            try
            {
                var query0 = await _context.Auction.Where(x => x.Active == true)
                    .OrderByDescending(x => x.CreateDateTimeUtc)
                    .Include(x => x.NFTData)
                    .Include(x => x.NFTData.NFT)
                    .Include(x => x.NFTData.NFT.UserProfile)
                    .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                    .AsNoTracking()
                    .ToListAsync();

                var groupedQuery0 = query0.GroupBy(t => t.NFTData.NFT.UserProfile.UserProfileHeaderId).Select(b => b).ToList();

                var query = await _context.Auction.Where(x => x.IsAuctionOver == false && x.Active == true)
                    .Include(x => x.NFTData)
                    .Include(x => x.NFTData.NFT)
                    .Include(x => x.NFTData.NFT.UserProfile)
                    .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                    .AsNoTracking()
                    .ToListAsync();

                var groupedQuery = query.GroupBy(t => t.CurrentWinningUserId).Select(b => b).ToList();

                groupedQuery0.ForEach(x =>
                {
                    groupedQuery.ForEach(y => {
                        if(x.FirstOrDefault().NFTData.NFT.UserProfile.UserProfileHeaderId == y.FirstOrDefault().NFTData.NFT.UserProfile.UserProfileHeaderId)
                            userList.Add(x);
                    });
                });

                //userList = queryGroup;

            }
            catch (Exception ex)
            {
                var error = ex;
            }

            return userList;
        }

        public async Task<List<Auction>> GetAuctionByFeaturedUserHeaderID(Guid userProfileHeaderID)
        {
            List<Auction> auctionList = new List<Auction>();
            try
            {
                auctionList = await _context.Auction.Where(x => x.NFTData.NFT.UserProfile.UserProfileHeaderId == userProfileHeaderID && x.IsAuctionOver == false && x.Active == true)
                .Include(x => x.NFTData)
                .Include(x => x.NFTData.NFT)
                .Include(x => x.NFTData.NFT.UserProfile)
                .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .ToListAsync();
            } catch(Exception error)
            {
                Console.WriteLine(error);
            }
          

            return auctionList;
        }

        public async Task<bool> OnPlaceBid(AuctionBidQueue auctionBid, int amount)
        {
            
            bool IsSuccess = true;

            if (auctionBid.Auction.NFTData.NFT.IsMultiple && auctionBid.Auction.NFTData.NFT.TotalNumberOfMintedNFT > amount)
            {

            } else
            {
                AuctionBidQueue newAuctionBidQueue = new AuctionBidQueue();
                newAuctionBidQueue.BidAmount = auctionBid.BidAmount;
                newAuctionBidQueue.FiatBidAmount = auctionBid.FiatBidAmount;
                newAuctionBidQueue.EthBidAmount = auctionBid.EthBidAmount;
                newAuctionBidQueue.MaxBidAmount = auctionBid.MaxBidAmount;
                newAuctionBidQueue.FiatMaxBidAmount = auctionBid.FiatMaxBidAmount;
                newAuctionBidQueue.EthMaxBidAmount = auctionBid.EthMaxBidAmount;
                newAuctionBidQueue.IsBuyItNow = auctionBid.IsBuyItNow;
                newAuctionBidQueue.IsAutoBid = auctionBid.IsAutoBid;
                newAuctionBidQueue.AuctionId = auctionBid.AuctionId;
                newAuctionBidQueue.BidUserId = auctionBid.BidUserId;
                newAuctionBidQueue.CreateTimeInTicks = DateTime.UtcNow.Ticks;

                await _context.AuctionBidQueue.AddAsync(newAuctionBidQueue);

                await _context.SaveChangesAsync();
            }

           


            if (auctionBid.IsBuyItNow == true || auctionBid.Auction.NFTData.FiatBuyOutPrice <= auctionBid.FiatBidAmount)
            {
                Auction auction = new Auction();
                auction = await _context.Auction.Where(x => x.Id == new Guid(auctionBid.AuctionId.ToString())).FirstOrDefaultAsync();

                NFTData nftData = new NFTData();
                nftData = _context.NFTData.Where(x => x.Id == new Guid(auction.NFTDataId.ToString())).FirstOrDefault();

                NFT nft = new NFT();
                nft = _context.NFT.Where(x => x.Id == new Guid(nftData.NFTId.ToString())).FirstOrDefault();

                UserProfile profile = new UserProfile();
                profile = await _context.UserProfile.Where(x => x.UserProfileHeaderId == new Guid(auctionBid.BidUserId.ToString())).FirstOrDefaultAsync();


                if (auctionBid.Auction.NFTData.NFT.IsMultiple && auctionBid.Auction.NFTData.NFT.TotalNumberOfMintedNFT > amount)
                {
                    nft.TotalNumberOfMintedNFT = nft.TotalNumberOfMintedNFT - amount;
                    await _context.SaveChangesAsync();

                    NFT new_nft = new NFT();
                    NFTData new_nftData = new NFTData();

                    new_nft.Title = nft.Title;
                    new_nft.Description = nft.Description;
                    new_nft.Category = nft.Category;
                    new_nft.FileName = nft.FileName;
                    new_nft.FileContent = nft.FileContent;
                    new_nft.FileType = nft.FileType;
                    new_nft.UserProfileId = profile.Id;
                    new_nft.TotalNumberOfMintedNFT = amount;
                    new_nft.IsMultiple = true;

                    await _context.NFT.AddAsync(new_nft);

                    new_nftData.NFTId = new_nft.Id;
                    new_nftData.UniqueNumberOfMintedNFT = nftData.UniqueNumberOfMintedNFT;
                    new_nftData.HasBuyoutPrice = true;
                    new_nftData.HasReservePrice = true;

                    await _context.NFTData.AddAsync(new_nftData);
                    await _context.SaveChangesAsync();

                }
                else
                {

                    NFT new_nft = new NFT();
                    NFTData new_nftData = new NFTData();

                    new_nft.Title = nft.Title;
                    new_nft.Description = nft.Description;
                    new_nft.Category = nft.Category;
                    new_nft.FileName = nft.FileName;
                    new_nft.FileContent = nft.FileContent;
                    new_nft.FileType = nft.FileType;
                    new_nft.UserProfileId = profile.Id;
                    new_nft.TotalNumberOfMintedNFT = nft.TotalNumberOfMintedNFT;
                    new_nft.IsMultiple = nft.IsMultiple;
                    await _context.NFT.AddAsync(new_nft);


                    new_nftData.NFTId = new_nft.Id;
                    new_nftData.UniqueNumberOfMintedNFT = nftData.UniqueNumberOfMintedNFT;
                    new_nftData.HasBuyoutPrice = true;
                    new_nftData.HasReservePrice = true;
                    await _context.NFTData.AddAsync(new_nftData);

                    string nftId = auction.NFTData.NFTId.ToString();
                    string nftDataId = auction.NFTData.Id.ToString();

                    await _context.SaveChangesAsync();


                    // assigning empty profile id
                    Guid marketplace_profile_id = Guid.Empty;

                    var edited_nft = await _context.NFT.Where(x => x.Id == new Guid(nftId)).FirstOrDefaultAsync();

                    edited_nft.Active = false;
                    nftData.Active = false;
                    await _context.SaveChangesAsync();
                }
            }
                

            return IsSuccess;
        }

        public async Task<bool> OnPurchase(Auction auctionData, int amount)
        {
            
                Auction auction = new Auction();
                auction = await _context.Auction.Where(x => x.Id == new Guid(auctionData.Id.ToString())).FirstOrDefaultAsync();

                NFTData nftData = new NFTData();
                nftData = _context.NFTData.Where(x => x.Id == new Guid(auction.NFTDataId.ToString())).FirstOrDefault();

                NFT nft = new NFT();
                nft = _context.NFT.Where(x => x.Id == new Guid(nftData.NFTId.ToString())).FirstOrDefault();

                UserProfile profile = new UserProfile();
                profile = await _context.UserProfile.Where(x => x.UserProfileHeaderId == new Guid(auctionData.CurrentWinningUserId.ToString())).FirstOrDefaultAsync();


            if (auctionData.NFTData.NFT.IsMultiple && auctionData.NFTData.NFT.TotalNumberOfMintedNFT > amount)
                {
                    nft.TotalNumberOfMintedNFT = nft.TotalNumberOfMintedNFT - amount;
                    await _context.SaveChangesAsync();

                    NFT new_nft = new NFT();
                    NFTData new_nftData = new NFTData();

                    new_nft.Title = nft.Title;
                    new_nft.Description = nft.Description;
                    new_nft.Category = nft.Category;
                    new_nft.FileName = nft.FileName;
                    new_nft.FileContent = nft.FileContent;
                    new_nft.FileType = nft.FileType;
                    new_nft.UserProfileId = profile.Id;
                    new_nft.TotalNumberOfMintedNFT = amount;
                    new_nft.IsMultiple = true;

                    await _context.NFT.AddAsync(new_nft);

                    new_nftData.NFTId = new_nft.Id;
                    new_nftData.UniqueNumberOfMintedNFT = nftData.UniqueNumberOfMintedNFT;
                    new_nftData.HasBuyoutPrice = true;
                    new_nftData.HasReservePrice = true;

                    await _context.NFTData.AddAsync(new_nftData);
                    await _context.SaveChangesAsync();

                } else {

                    NFT new_nft = new NFT();
                    NFTData new_nftData = new NFTData();

                    new_nft.Title = nft.Title;
                    new_nft.Description = nft.Description;
                    new_nft.Category = nft.Category;
                    new_nft.FileName = nft.FileName;
                    new_nft.FileContent = nft.FileContent;
                    new_nft.FileType = nft.FileType;
                    new_nft.UserProfileId = profile.Id;
                    new_nft.TotalNumberOfMintedNFT = nft.TotalNumberOfMintedNFT;
                    new_nft.IsMultiple = nft.IsMultiple;
                    await _context.NFT.AddAsync(new_nft);


                    new_nftData.NFTId = new_nft.Id;
                    new_nftData.UniqueNumberOfMintedNFT = nftData.UniqueNumberOfMintedNFT;
                    new_nftData.HasBuyoutPrice = true;
                    new_nftData.HasReservePrice = true;
                    await _context.NFTData.AddAsync(new_nftData);

                    string nftId = auction.NFTData.NFTId.ToString();
                    string nftDataId = auction.NFTData.Id.ToString();

                    await _context.SaveChangesAsync();


                    // assigning empty profile id
                    Guid marketplace_profile_id = Guid.Empty;

                    var edited_nft = await _context.NFT.Where(x => x.Id == new Guid(nftId)).FirstOrDefaultAsync();

                    edited_nft.Active = false;
                    nftData.Active = false;
                    await _context.SaveChangesAsync();
                }

            bool IsSuccess = true;
            return IsSuccess;
        }

        public async Task<bool> IsFirstBid(Guid auctionId) {
            var auctionBidCount = await _context.AuctionBid.Where(x => x.AuctionId == auctionId && x.Active == true).CountAsync();

            if (auctionBidCount == 1) return true;
            else return false;
        }

        public async Task UpdateWinningBidUser(Guid auctionId, Guid userId)
        {
            var auction = await _context.Auction.Where(x => x.Id == auctionId && x.Active == true).FirstOrDefaultAsync();
            auction.CurrentWinningUserId = userId;

            await _context.SaveChangesAsync();
        }

        public async Task AddNewReport(Report submittedReport)
        {
            var newReport = new Report();
            newReport.ReportReason = submittedReport.ReportReason;
            newReport.ReporterEmail = submittedReport.ReporterEmail;
            newReport.ReportDescription = submittedReport.ReportDescription;
            newReport.ReportedUserId = submittedReport.ReportedUserId;

            await _context.Report.AddAsync(newReport);
            await _context.SaveChangesAsync();

            EmailUtility.SendEmail(newReport.ReporterEmail, "Starboard - Report", "Thank you for your report. We've received it successfully!", "alert", true, "Report", "https://nft.starboard.org");
        }

        public async Task GetPopularSllers() {
            var popularUserList = await _context.Auction.Where(x => x.IsAuctionOver == false)
                    .GroupBy(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                    .OrderByDescending(x => x.Count())
                    .Take(10)
                    .ToListAsync();
        }

        public async Task SaveRecentViewNFT(Guid userProfileId, Guid NftDataId, Guid auctionID)
        {
            var recentNftList = _context.RecentViewNFT.Where(x => x.NFTDataId == NftDataId && x.Active == true).FirstOrDefault();

            if (recentNftList == null)
            {
                var newRecentViewNFT = new RecentViewNFT();
                newRecentViewNFT.UserProfileId = userProfileId;
                newRecentViewNFT.NFTDataId = NftDataId;
                newRecentViewNFT.AuctionId = auctionID;

                _context.RecentViewNFT.Add(newRecentViewNFT);

                var recentNftCount = _context.RecentViewNFT.Where(x => x.UserProfileId == userProfileId && x.Active == true).Count();
                if (recentNftCount > 10)
                {
                    var lastRecentNft = _context.RecentViewNFT.Where(x => x.UserProfileId == userProfileId && x.Active == true)
                        .OrderBy(x => x.CreateDateTimeUtc)
                        .Take(1)
                        .ToList()
                        .FirstOrDefault();

                    _context.RecentViewNFT.Remove(lastRecentNft);
                }
            }

            _context.SaveChanges();
        }

        public async Task<List<RecentViewNFT>> GetRecentViewNFT(string walletAddress)
        {
            var specificAddress = await _context.UserProfile.Where(x => x.EthAddress == walletAddress && x.Active == true).FirstOrDefaultAsync();
            if (specificAddress == null)
            {
                return new List<RecentViewNFT>();
            }
            var recentViewNFTList = await _context.RecentViewNFT.Where(x => x.UserProfileId == specificAddress.Id && x.Active == true)
                                .Include(x => x.Auction)
                                .Include(x => x.NFTData)
                                .Include(x => x.NFTData.NFT)
                                .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                                .ToListAsync();
           
            return recentViewNFTList;
        }

        public async Task SaveFollowNFT(Guid userProfileId, Guid NftDataId)
        {
            var recentNftList = _context.NFTFavorites.Where(x => x.NFTDataId == NftDataId && x.Active == true).FirstOrDefault();

            if (recentNftList == null)
            {
                var newFollowNFT = new RecentViewNFT();
                newFollowNFT.UserProfileId = userProfileId;
                newFollowNFT.NFTDataId = NftDataId;

                _context.RecentViewNFT.Add(newFollowNFT);

                var followNftCount = _context.NFTFavorites.Where(x => x.UserProfileId == userProfileId && x.Active == true).Count();
                if (followNftCount > 10)
                {
                    var lastfollowNft = _context.NFTFavorites.Where(x => x.UserProfileId == userProfileId && x.Active == true)
                        .OrderBy(x => x.CreateDateTimeUtc)
                        .Take(1)
                        .ToList()
                        .FirstOrDefault();

                    _context.NFTFavorites.Remove(lastfollowNft);
                }
            }

            _context.SaveChanges();
        }

        public async Task<List<NFTFavorites>> GetFollowNFT(string walletAddress)
        {
            var specificAddress = await _context.UserProfile.Where(x => x.EthAddress == walletAddress && x.Active == true).FirstOrDefaultAsync();
            if (specificAddress == null)
            {
                return new List<NFTFavorites>();
            }
            var recentViewNFTList = await _context.NFTFavorites.Where(x => x.UserProfileId == specificAddress.Id && x.Active == true)
                            .Include(x => x.NFTData)
                            .Include(x => x.NFTData.NFT)
                            .Include(x => x.NFTData.NFT.UserProfile.UserProfileHeader)
                            .ToListAsync();

            return recentViewNFTList;
        }

        public async Task<NFTData> GetNFTDataById(Guid nftId)
        {
            var nftData = await _context.NFTData.Where(x => x.Id == nftId && x.Active == true)
                .Include(x => x.NFT)
                .Include(x => x.NFT.UserProfile)
                .Include(x => x.NFT.UserProfile.UserProfileHeader)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return nftData;
        }

        //public async Task OnBuyOut(Guid auctionId)
        //{
        //    var auction = await _context.Auction.Where(x => x.Id == auctionId && x.Active == true).FirstOrDefaultAsync();
        //    var NFTData = await _context.NFTData.Where(x => x.Id == auction.NFTDataId && x.Active == true).FirstOrDefaultAsync();

        //    auction.
        //}
    }
}
