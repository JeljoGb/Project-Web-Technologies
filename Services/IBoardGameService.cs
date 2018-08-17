using ConesOfAmazonshire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConesOfAmazonshire.Services
{
    public interface IBoardGameService
    {
        Task<IEnumerable<BoardGame>> GetBoardGamesAsync();

        Task<bool> AddBoardGameAsync(NewBoardGame newBoardGame);
    }
}
