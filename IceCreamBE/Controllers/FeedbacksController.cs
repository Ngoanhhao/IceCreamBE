using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IceCreamBE.Data;
using IceCreamBE.Models;
using IceCreamBE.Repository.Irepository;
using IceCreamBE.DTO;
using IceCreamBE.Migrations;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IRepositoryFeedback _IRepositoryFeedback;
        private readonly IRepositoryAccounts _IRepositoryAccounts;
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;

        public FeedbacksController
        (
            IRepositoryFeedback IRepositoryFeedback,
            IRepositoryAccountDetail iRepositoryAccountDetail,
            IRepositoryAccounts iRepositoryAccounts
        )
        {
            _IRepositoryFeedback = IRepositoryFeedback;
            _IRepositoryAccountDetail = iRepositoryAccountDetail;
            _IRepositoryAccounts = iRepositoryAccounts;
        }

        // GET: api/Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDetailDTO>>> GetFeedback()
        {
            var feedback = (await _IRepositoryFeedback.GetAllAsync()).AsQueryable<Feedback>();
            var account = (await _IRepositoryAccounts.GetAllAsync()).AsQueryable<Accounts>();
            var accountDetail = (await _IRepositoryAccountDetail.GetAllAsync()).AsQueryable<AccountDetail>();
            var result = feedback
                .Join(accountDetail,
                    e => e.AccountID,
                    q => q.Id,
                    (e, q) => new { feedback = e, accountDetail = q })
                .Join(account,
                    k => k.feedback.AccountID,
                    t => t.Id,
                    (k, t) => new { feedback = k.feedback, accountDetail = k.accountDetail, t = account })
                .Select(e => new FeedbackDetailDTO
                {
                    Id = e.feedback.Id,
                    Fullname = e.accountDetail.FullName,
                    Username = e.feedback.Account.Username,
                    Email = e.accountDetail.Email,
                    PhoneNumber = e.accountDetail.PhoneNumber,
                    FeedBackProduct = e.feedback.FeedBackProduct,
                    ReleaseDate = e.feedback.ReleaseDate,
                })
                .ToList();
            return Ok(result);
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FeedbackDTO>> GetFeedback(int id)
        {
            var feedback = (await _IRepositoryFeedback.GetAllAsync()).AsQueryable<Feedback>();
            var account = (await _IRepositoryAccounts.GetAllAsync()).AsQueryable<Accounts>();
            var accountDetail = (await _IRepositoryAccountDetail.GetAllAsync()).AsQueryable<AccountDetail>();
            var result = feedback
                .Join(accountDetail,
                    e => e.AccountID,
                    q => q.Id,
                    (e, q) => new { feedback = e, accountDetail = q })
                .Join(account,
                    k => k.feedback.AccountID,
                    t => t.Id,
                    (k, t) => new { feedback = k.feedback, accountDetail = k.accountDetail, t = account })
                .Select(e => new FeedbackDetailDTO
                {
                    Id = e.feedback.Id,
                    Fullname = e.accountDetail.FullName,
                    Username = e.feedback.Account.Username,
                    Email = e.accountDetail.Email,
                    PhoneNumber = e.accountDetail.PhoneNumber,
                    FeedBackProduct = e.feedback.FeedBackProduct,
                    ReleaseDate = e.feedback.ReleaseDate,
                })
                .Where(e => e.Id == id)
                .ToList();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        //GET: api/Feedbacks/fullname
        [HttpGet("{query}")]
        public async Task<ActionResult<IEnumerable<FeedbackDetailDTO>>> GetFeedbacks(string query)
        {
            var feedback = (await _IRepositoryFeedback.GetAllAsync()).AsQueryable<Feedback>();
            var account = (await _IRepositoryAccounts.GetAllAsync()).AsQueryable<Accounts>();
            var accountDetail = (await _IRepositoryAccountDetail.GetAllAsync()).AsQueryable<AccountDetail>();
            var result = feedback
                .Join(accountDetail,
                    e => e.AccountID,
                    q => q.Id,
                    (e, q) => new { feedback = e, accountDetail = q })
                .Join(account,
                    k => k.feedback.AccountID,
                    t => t.Id,
                    (k, t) => new { feedback = k.feedback, accountDetail = k.accountDetail, t = account })
                .Select(e => new FeedbackDetailDTO
                {
                    Id = e.feedback.Id,
                    Fullname = e.accountDetail.FullName,
                    Username = e.feedback.Account.Username,
                    Email = e.accountDetail.Email,
                    PhoneNumber = e.accountDetail.PhoneNumber,
                    FeedBackProduct = e.feedback.FeedBackProduct,
                    ReleaseDate = e.feedback.ReleaseDate,
                })
                .Where(e => e.Fullname.Contains(query))
                .ToList();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Feedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, FeedbackDTO feedback)
        {
            if (id != feedback.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _IRepositoryFeedback.GetAsync(e => e.Id == feedback.Id);

            await _IRepositoryFeedback.UpdateAsync(new Feedback
            {
                Id = feedback.Id,
                FeedBackProduct = feedback.FeedBackProduct,
                AccountID = feedback.AccountID,
                ReleaseDate = feedback.ReleaseDate
            });

            return NoContent();
        }

        // POST: api/Feedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(FeedbackDTO feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _IRepositoryFeedback.CreateAsync(new Feedback
            {
                AccountID = feedback.AccountID,
                FeedBackProduct = feedback.FeedBackProduct,
                ReleaseDate = DateTime.UtcNow,
            });

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var result = await _IRepositoryFeedback.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            await _IRepositoryFeedback.DeleteAsync(result);

            return NoContent();
        }
    }
}
