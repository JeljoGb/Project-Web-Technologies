using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConesOfAmazonshire.Data;
using ConesOfAmazonshire.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Net.Http.Headers;
using System.Xml;

namespace ConesOfAmazonshire.Controllers
{
    public class BoardGamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BoardGamesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: BoardGames
        public async Task<IActionResult> Index(string sortOrder, string searchTitles, string searchLocations, string searchUsers, string boardGameGenre, Condition boardGameCondition, List<int?> boardGamePriceRange)
        {

            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.ConditionSortParm = sortOrder == "condition_asc" ? "condition_desc" : "condition_asc";
            ViewBag.GenreSortParm = sortOrder == "genre_asc" ? "genre_desc" : "genre_asc";
            ViewBag.PublisherSortParm = sortOrder == "publisher_asc" ? "publisher_desc" : "publisher_asc";
            ViewBag.UserSortParm = sortOrder == "user_asc" ? "user_desc" : "user_asc";
            ViewBag.LocationSortParm = sortOrder == "location_asc" ? "location_desc" : "location_asc";
            ViewBag.PurchaseSortParm = sortOrder == "purchase_asc" ? "purchase_desc" : "purchase_asc";

            IQueryable<string> genreQuery = from b in _context.BoardGames
                                            orderby b.Genre
                                            select b.Genre;


            var boardGames = from b in _context.BoardGames
                             .Include(a => a.User)
                             .Include(b => b.User.Location)
                             select b;

            if (!String.IsNullOrEmpty(searchTitles))
            {
                boardGames = boardGames.Where(b => b.Title.Contains(searchTitles));
            }

            if (!String.IsNullOrEmpty(searchLocations))
            {
                boardGames = boardGames.Where(b => b.User.Location.ToString().Contains(searchLocations));
            }

            if (!String.IsNullOrEmpty(searchUsers))
            {
                boardGames = boardGames.Where(b => b.User.UserName.Contains(searchUsers));
            }

            if (!String.IsNullOrEmpty(boardGameGenre))
            {
                boardGames = boardGames.Where(x => x.Genre == boardGameGenre);
            }

            if (Enum.IsDefined(typeof(Condition), boardGameCondition))
            {
                boardGames = boardGames.Where(c => c.Condition == boardGameCondition);
            }

            if (boardGamePriceRange.Any())
            {
                boardGames = boardGames.Where(x => x.Price > (boardGamePriceRange[0] ?? 0) && x.Price < (boardGamePriceRange[1] ?? 1000));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Title);
                    break;
                case "price_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Price);
                    break;
                case "price_asc":
                    boardGames = boardGames.OrderBy(b => b.Price);
                    break;
                case "condition_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Condition);
                    break;
                case "condition_asc":
                    boardGames = boardGames.OrderBy(b => b.Condition);
                    break;
                case "genre_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Genre);
                    break;
                case "genre_asc":
                    boardGames = boardGames.OrderBy(b => b.Genre);
                    break;
                case "publisher_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Publisher);
                    break;
                case "publisher_asc":
                    boardGames = boardGames.OrderBy(b => b.Publisher);
                    break;
                case "user_desc":
                    boardGames = boardGames.OrderByDescending(b => b.User);
                    break;
                case "user_asc":
                    boardGames = boardGames.OrderBy(b => b.User);
                    break;
                case "location_desc":
                    boardGames = boardGames.OrderByDescending(b => b.User.Location);
                    break;
                case "location_asc":
                    boardGames = boardGames.OrderBy(b => b.User.Location);
                    break;
                case "purchase_desc":
                    boardGames = boardGames.OrderByDescending(b => b.PurchaseDate);
                    break;
                case "purchase_asc":
                    boardGames = boardGames.OrderBy(b => b.PurchaseDate);
                    break;
                default:
                    boardGames = boardGames.OrderBy(b => b.Title);
                    break;
            }

            var boardGameQueryVM = new BoardGameQueryViewModel
            {

                genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                boardGames = await boardGames.ToListAsync(),
                SearchTitles = searchTitles
            };

            return View(boardGameQueryVM);
            //return View(await _context.BoardGames
            //    .Include(a => a.User)
            //    .ToListAsync());
        }

        // GET: BoardGames/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardGame = await _context.BoardGames
                .Include(a => a.User)
                .Include(b => b.User.Location)
                .SingleOrDefaultAsync(m => m.Id == id);

            
            string url = "https://www.boardgamegeek.com/xmlapi2/";
            string searchBoardGameId = "search?query=Monopoly&exact=1";
            //string url = "http://www.google.com/ig/api?weather=vilnius&hl=lt";

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            IEnumerable<BoardGame> boardGames = new List<BoardGame>();

            //GET Method  
            HttpResponseMessage idResponse = client.GetAsync(searchBoardGameId).Result;
            if (idResponse.IsSuccessStatusCode)
            {

                XDocument nameQuery = XDocument.Parse(Encoding.UTF8.GetString(idResponse.Content.ReadAsByteArrayAsync().Result));
                string boardGameId = nameQuery.Descendants("item").First().Attribute("id").Value;

                string searchDescription = "thing?id=" + boardGameId;
                HttpResponseMessage descriptionResponse = client.GetAsync(searchDescription).Result;
                if (descriptionResponse.IsSuccessStatusCode)
                {
                    XDocument descriptionQuery = XDocument.Parse(Encoding.UTF8.GetString(descriptionResponse.Content.ReadAsByteArrayAsync().Result));
                    var a = descriptionQuery.Descendants("item");
                    var b = a.First();
                    var c = b.Element("description");
                    string d = c.Value;
                    boardGame.Description = descriptionQuery.Descendants("item").First().Element("description").Value;
                }

            }
            else
            {
                Console.WriteLine("Internal server Error");
            }



            if (boardGame == null)
            {
                return NotFound();
            }
            return View(boardGame);
        }

        // GET: BoardGames/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var boardGame = new BoardGame
            {
                User = user
            };
            var vm = new CreateViewModel
            {
                BoardGame = boardGame,
                UserId = user.Id
            };
            return View(vm);
        }

        // POST: BoardGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Id,Title,Price,Condition,Genre,Publisher,Location,Image,PurchaseDate,User")] */CreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var boardGame = vm.BoardGame;
                boardGame.Id = Guid.NewGuid();
                var user = await _context.Users.SingleOrDefaultAsync(a => a.Id == vm.UserId);
                boardGame.User = user;
                _context.Add(boardGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: BoardGames/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardGame = await _context.BoardGames.SingleOrDefaultAsync(m => m.Id == id);
            if (boardGame == null)
            {
                return NotFound();
            }
            return View(boardGame);
        }

        // POST: BoardGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Price,Condition,Genre,Publisher,Location,Image,PurchaseDate,User")] BoardGame boardGame)
        {
            if (id != boardGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boardGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardGameExists(boardGame.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(boardGame);
        }

        // GET: BoardGames/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardGame = await _context.BoardGames
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (boardGame == null)
            {
                return NotFound();
            }

            return View(boardGame);
        }

        // POST: BoardGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boardGame = await _context.BoardGames.SingleOrDefaultAsync(m => m.Id == id);
            _context.BoardGames.Remove(boardGame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardGameExists(Guid id)
        {
            return _context.BoardGames.Any(e => e.Id == id);
        }
    }
}
