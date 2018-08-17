using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConesOfAmazonshire.Data;
using ConesOfAmazonshire.Models;
using Microsoft.EntityFrameworkCore;

namespace ConesOfAmazonshire.Services

{
    public class BoardGameService : IBoardGameService
    {
        private readonly ApplicationDbContext _context;

        public BoardGameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BoardGame>> GetBoardGamesAsync()
        {
            return await _context.BoardGames.ToArrayAsync();
        }

        public async Task<bool> AddBoardGameAsync(NewBoardGame newBoardGame)
        {
            var entity = new BoardGame
            {
                Genre = "",
                Id = Guid.NewGuid(),
                Title = newBoardGame.Title,
                User = null

            };

            _context.BoardGames.Add(entity);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }

}