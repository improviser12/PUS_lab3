using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUS_lab3.Models;

namespace PUS_lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserInfoesController : ControllerBase
    {
        private readonly DataMemory _context;

        public UserInfoesController(DataMemory context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        // GET: api/UserInfoes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/UserInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(long id)
        {
            var userInfo = await _context.Users.FindAsync(id);

            if (userInfo == null)
            {
                return NotFound();
            }

            return userInfo;
        }

        // PUT: api/UserInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUserInfo(long id, UserInfo userInfo)
        {
            if (id != userInfo.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        {
            _context.Users.Add(userInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInfo", new { id = userInfo.UserId }, userInfo);
        }

        // DELETE: api/UserInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInfo(long id)
        {
            var userInfo = await _context.Users.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserInfoExists(long id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
