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
using IceCreamBE.DTO.PageList;

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
        public async Task<ActionResult<IEnumerable<FeedbackDetailDTO>>> GetFeedback([FromQuery] PaginationFilter<FeedbackDetailDTO>? filter)
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
                    (k, t) => new { feedback = k.feedback, accountDetail = k.accountDetail, account = t })
                .Select(e => new FeedbackDetailDTO
                {
                    Id = e.feedback.Id,
                    full_name = e.accountDetail.FullName,
                    username = e.feedback.Account.Username,
                    email = e.accountDetail.Email,
                    phone_number = e.accountDetail.PhoneNumber,
                    feedBack_product = e.feedback.FeedBackProduct,
                    release_date = e.feedback.ReleaseDate,
                })
                .ToList();

            var pageFilter = new PaginationFilter<FeedbackDetailDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<FeedbackDetailDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<FeedbackDetailDTO>>
                {
                    current_page = pageFilter.PageNumber,
                    Page_pize = pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FeedbackDTO>> GetFeedback(int id)
        {
            var feedback = (await _IRepositoryFeedback.GetAllAsync());
            var account = (await _IRepositoryAccounts.GetAllAsync());
            var accountDetail = (await _IRepositoryAccountDetail.GetAllAsync());
            var result = feedback
                .Join(accountDetail,
                    e => e.AccountID,
                    q => q.Id,
                    (e, q) => new { feedback = e, accountDetail = q })
                .Join(account,
                    k => k.feedback.AccountID,
                    t => t.Id,
                    (k, t) => new { feedback = k.feedback, accountDetail = k.accountDetail, account = t })
                .Select(e => new FeedbackDetailDTO
                {
                    Id = e.feedback.Id,
                    full_name = e.accountDetail.FullName,
                    username = e.feedback.Account.Username,
                    email = e.accountDetail.Email,
                    phone_number = e.accountDetail.PhoneNumber,
                    feedBack_product = e.feedback.FeedBackProduct,
                    release_date = e.feedback.ReleaseDate,
                })
                .FirstOrDefault(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<FeedbackDetailDTO> { Message = "not found", Succeeded = false });
            }
            return Ok(new Response<FeedbackDetailDTO> { Data = result, Succeeded = true });
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
                    full_name = e.accountDetail.FullName,
                    username = e.feedback.Account.Username,
                    email = e.accountDetail.Email,
                    phone_number = e.accountDetail.PhoneNumber,
                    feedBack_product = e.feedback.FeedBackProduct,
                    release_date = e.feedback.ReleaseDate,
                })
                .Where(e => e.full_name.Contains(query))
                .ToList();

            if (result.Count == 0)
            {
                return NotFound(new Response<FeedbackDetailDTO> { Message = "not found", Succeeded = false });
            }
            return Ok(new Response<List<FeedbackDetailDTO>> { Data = result, Succeeded = true });
        }

        // PUT: api/Feedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, FeedbackDTO feedback)
        {
            if (id != feedback.Id || !ModelState.IsValid)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "value incorrect", Succeeded = false });
            }

            var result = await _IRepositoryFeedback.GetAsync(e => e.Id == feedback.Id);

            await _IRepositoryFeedback.UpdateAsync(new Feedback
            {
                Id = feedback.Id,
                FeedBackProduct = feedback.feedBack_product,
                AccountID = feedback.accountID,
                ReleaseDate = feedback.release_date
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
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "value incorrect", Succeeded = false });
            }

            await _IRepositoryFeedback.CreateAsync(new Feedback
            {
                AccountID = feedback.accountID,
                FeedBackProduct = feedback.feedBack_product,
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
                return NotFound(new Response<FeedbackDetailDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryFeedback.DeleteAsync(result);

            return NoContent();
        }
    }
}
