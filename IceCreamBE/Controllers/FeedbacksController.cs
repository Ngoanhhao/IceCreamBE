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
using Microsoft.AspNetCore.Authorization;
using System.Data;

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
            var result = feedback.Select(e => new FeedbackDetailDTO
            {
                Id = e.Id,
                message = e.Message,
                email = e.Email,
                full_name = e.FullName,
                release_date = e.ReleaseDate,
            }).OrderByDescending(e => e.release_date).ToList();

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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FeedbackDTO>> GetFeedback(int id)
        {
            var feedback = (await _IRepositoryFeedback.GetAsync(e => e.Id == id));
            var result = new FeedbackDetailDTO
            {
                Id = feedback.Id,
                message = feedback.Message,
                email = feedback.Email,
                full_name = feedback.FullName,
                release_date = feedback.ReleaseDate,
            };

            if (result == null)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "not found", Succeeded = false });
            }
            return Ok(new Response<FeedbackDetailDTO> { Data = result, Succeeded = true });
        }

        //GET: api/Feedbacks/fullname
        [HttpGet("/api/search/feedback")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<FeedbackDetailDTO>>> GetFeedbacks(string? query)
        {
            var feedback = (await _IRepositoryFeedback.GetAllAsync(e => e.FullName.ToLower().Contains(query != null ? query.ToLower() : "")));
            var result = feedback.Select(e => new FeedbackDetailDTO
            {
                Id = e.Id,
                message = e.Message,
                email = e.Email,
                full_name = e.FullName,
                release_date = e.ReleaseDate,
            }).OrderByDescending(e => e.release_date).ToList();


            if (result.Count == 0)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "not found", Succeeded = false });
            }
            return Ok(new Response<List<FeedbackDetailDTO>> { Data = result, Succeeded = true });
        }

        // POST: api/Feedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Feedback>> PostFeedback(FeedbackDTO feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "value incorrect", Succeeded = false });
            }

            await _IRepositoryFeedback.CreateAsync(new Feedback
            {
                FullName = feedback.full_name,
                Email = feedback.email,
                Message = feedback.message,
                ReleaseDate = DateTime.Now,
            });

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var result = await _IRepositoryFeedback.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return BadRequest(new Response<FeedbackDetailDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryFeedback.DeleteAsync(result);

            return NoContent();
        }
    }
}
