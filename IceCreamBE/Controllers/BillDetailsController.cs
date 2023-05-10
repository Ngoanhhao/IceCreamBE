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
using IceCreamBE.DTO.PageList;
using IceCreamBE.DTO;
using IceCreamBE.Repository;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using IceCreamBE.Migrations;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillDetailsController : ControllerBase
    {
        private readonly IRepositoryBillDetail _IRepositoryBillDetail;
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryBill _IRepositoryBill;

        public BillDetailsController(IRepositoryBillDetail IRepositoryBillDetail, IRepositoryProduct repositoryProduct, IRepositoryBill repositoryBill)
        {
            _IRepositoryBillDetail = IRepositoryBillDetail;
            _IRepositoryProduct = repositoryProduct;
            _IRepositoryBill = repositoryBill;
        }

        // GET: api/BillDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillDetail>>> GetBillDetail([FromQuery] PaginationFilter<BillDetailOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();

            var result = bill
                .Join(billDetail,
                    b => b.Id,
                    dt => dt.BillID,
                    (b, dt) => new { bill = b, billDetail = dt })
                .Join(product,
                    e => e.billDetail.ProductID,
                    p => p.Id,
                    (e, p) => new { product = p, billDetail = e.billDetail, bill = e.bill })
                .Select(e => (new BillDetailOutDTO
                {
                    Id = e.billDetail.Id,
                    billID = e.bill.Id,
                    product_name = e.product.Name,
                    quantity = e.billDetail.Quantity,
                    total = e.billDetail.Total
                }))
                .ToList();

            var pageFilter = new PaginationFilter<BillDetailOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillDetailOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillDetailOutDTO>>
                {
                    current_page = pageFilter.PageNumber,
                    Page_pize = pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // GET: api/BillDetails/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BillDetail>> GetBillDetail(int id)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();

            var result = billDetail.
                Join(bill,
                    dt => dt.Id,
                    b => b.BillDetailID,
                    (dt, b) => new { billDetail = dt, bill = b })
                .Join(product,
                    dt => dt.billDetail.ProductID,
                    p => p.Id,
                    (dt, p) => new { bill = dt.bill, billDetail = dt.billDetail, product = p })
                .Select(e => new BillDetailOutDTO
                {
                    Id = e.bill.Id,
                    billID = e.bill.Id,
                    product_name = e.product.Name,
                    quantity = e.billDetail.Quantity,
                    total = e.billDetail.Total
                })
                .FirstOrDefault(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new Response<BillDetailOutDTO> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<BillDetailOutDTO> { Data = result, Succeeded = true });
        }

        // GET: api/BillDetails/productname
        [HttpGet("{product_name}")]
        public async Task<ActionResult<IEnumerable<BillDetail>>> GetBillDetail(string product_name, [FromQuery] PaginationFilter<BillDetailOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();

            var result = billDetail.
                Join(bill,
                    dt => dt.Id,
                    b => b.BillDetailID,
                    (dt, b) => new { billDetail = dt, bill = b })
                .Join(product,
                    dt => dt.billDetail.ProductID,
                    p => p.Id,
                    (dt, p) => new { bill = dt.bill, billDetail = dt.billDetail, product = p })
                .Select(e => (new BillDetailOutDTO
                {
                    Id = e.bill.Id,
                    billID = e.bill.Id,
                    product_name = e.product.Name,
                    quantity = e.billDetail.Quantity,
                    total = e.billDetail.Total
                }))
                .Where(e => e.product_name == product_name)
                .ToList();

            var pageFilter = new PaginationFilter<BillDetailOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillDetailOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillDetailOutDTO>>
                {
                    current_page = pageFilter.PageNumber,
                    Page_pize = pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        // PUT: api/BillDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillDetail(int id, BillDetailInDTO billDetail)
        {
            if (id != billDetail.Id)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "value incorrect", Succeeded = false });
            }

            var BillDetail = await _IRepositoryBillDetail.GetAsync(e => e.Id == id);

            if (BillDetail == null)
            {
                return NotFound(new Response<BillDetailInDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryBillDetail.UpdateAsync(new BillDetail
            {
                Id = billDetail.Id,
                BillID = billDetail.billID,
                ProductID = billDetail.productID,
                Quantity = billDetail.quantity,
                Total = billDetail.total
            });

            return NoContent();
        }

        // POST: api/BillDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BillDetail>> PostBillDetail(BillDetailInDTO billDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "value incorrect", Succeeded = false });
            }

            await _IRepositoryBillDetail.CreateAsync(new BillDetail
            {
                Id = billDetail.Id,
                ProductID = billDetail.productID,
                Quantity = billDetail.quantity,
                BillID = billDetail.billID,
                Total = billDetail.total,
            });

            return CreatedAtAction("GetBillDetail", new { id = billDetail.Id }, billDetail);
        }

        // DELETE: api/BillDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillDetail(int id)
        {
            var result = await _IRepositoryBillDetail.GetAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound(new Response<BillDetailInDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryBillDetail.DeleteAsync(result);

            return NoContent();
        }
    }
}
