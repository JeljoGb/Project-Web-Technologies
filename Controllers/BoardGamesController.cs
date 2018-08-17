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
        public async Task<IActionResult> Index(string sortOrder)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var boardGames = from b in _context.BoardGames
                           select b;
            switch (sortOrder)
            {
                case "title_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Title);
                    break;
                case "price_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Price);
                    break;
                case "publisher_desc":
                    boardGames = boardGames.OrderByDescending(b => b.Publisher);
                    break;
                case "user_desc":
                    boardGames = boardGames.OrderByDescending(b => b.User);
                    break;
                case "Date":
                    boardGames = boardGames.OrderBy(b => b.PurchaseDate);
                    break;
                case "date_desc":
                    boardGames = boardGames.OrderByDescending(b => b.PurchaseDate);
                    break;
                default:
                    boardGames = boardGames.OrderBy(b => b.Title);
                    break;
            }

            return View(boardGames.ToList());
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
                .SingleOrDefaultAsync(m => m.Id == id);
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
            var boardGame = new BoardGame();
            boardGame.User = user;
            var vm = new CreateViewModel();
            vm.BoardGame = boardGame;
            vm.UserId = user.Id;
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
