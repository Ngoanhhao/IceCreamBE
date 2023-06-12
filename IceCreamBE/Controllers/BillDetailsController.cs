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
        private readonly IRepositoryVourcher _IRepositoryVourcher;
        private readonly IRepositoryAccounts _IRepositoryAccounts;

        public BillDetailsController(IRepositoryBillDetail IRepositoryBillDetail, IRepositoryProduct repositoryProduct, IRepositoryBill repositoryBill, IRepositoryVourcher repositoryVourcher, IRepositoryAccounts repositoryAccounts)
        {
            _IRepositoryBillDetail = IRepositoryBillDetail;
            _IRepositoryProduct = repositoryProduct;
            _IRepositoryBill = repositoryBill;
            _IRepositoryVourcher = repositoryVourcher;
            _IRepositoryAccounts = repositoryAccounts;
        }

        // GET: api/BillDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillDetailOutDTO>>> GetBillDetail([FromQuery] PaginationFilter<BillDetailOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();

            var result = billDetail
                .Join(product,
                    e => e.ProductID,
                    q => q.Id,
                    (e, q) => new { billDetail = e, product = q })
                .Select(e => (new BillDetailOutDTO
                {
                    Id = e.billDetail.Id,
                    billID = e.billDetail.BillID,
                    product_name = e.product.Name,
                    quantity = e.billDetail.Quantity,
                    total = e.billDetail.Total,
                    price = e.billDetail.Price,
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
        [HttpGet("{bill_id:int}")]
        public async Task<ActionResult<BillDetail>> GetBillDetail(int bill_id)
        {
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();

            var result = billDetail
                 .Join(product,
                     e => e.ProductID,
                     q => q.Id,
                     (e, q) => new { billDetail = e, product = q })
                 .Select(e => (new BillDetailOutDTO
                 {
                     Id = e.billDetail.Id,
                     billID = e.billDetail.BillID,
                     product_name = e.product.Name,
                     quantity = e.billDetail.Quantity,
                     total = e.billDetail.Total,
                     price = e.billDetail.Price,
                 }))
                .FirstOrDefault(e => e.billID == bill_id);

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
            var product = await _IRepositoryProduct.GetAllAsync(e => e.Name.Contains(product_name));

            var result = billDetail
                 .Join(product,
                     e => e.ProductID,
                     q => q.Id,
                     (e, q) => new { billDetail = e, product = q })
                 .Select(e => (new BillDetailOutDTO
                 {
                     Id = e.billDetail.Id,
                     billID = e.billDetail.BillID,
                     product_name = e.product.Name,
                     quantity = e.billDetail.Quantity,
                     total = e.billDetail.Total,
                     price = e.billDetail.Price,
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

        // PUT: api/BillDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Bill_detail_id}")]
        public async Task<IActionResult> PutBillDetail(int Bill_detail_id, BillDetailInDTO billDetail)
        {
            if (Bill_detail_id != billDetail.Id)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "value incorrect", Succeeded = false });
            }

            var BillDetail = await _IRepositoryBillDetail.GetAsync(e => e.Id == Bill_detail_id);
            var product = await _IRepositoryProduct.GetAsync(e => e.Id == BillDetail.ProductID);

            if (BillDetail == null)
            {
                return NotFound(new Response<BillDetailInDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryBillDetail.UpdateAsync(new BillDetail
            {
                Id = billDetail.Id,
                Quantity = billDetail.quantity,
                Price = product.Price,
                Total = product.Price * billDetail.quantity,
            });

            //update infomation bill
            var bill = await _IRepositoryBill.GetAsync(e => e.Id == BillDetail.BillID);
            var BillDetails = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == BillDetail.BillID);
            var voucher = await _IRepositoryVourcher.GetAsync(e => e.Id == bill.VoucherID);
            double subTotal = 0;
            BillDetails.ForEach(e => subTotal += e.Total);
            var total = voucher != null ? (100 - voucher.Discount) * 0.01 * subTotal : subTotal;
            await _IRepositoryBill.UpdateAsync(new Bill
            {
                Id = bill.Id,
                VoucherID = bill.VoucherID,
                Status = bill.Status,
                SubTotal = subTotal,
                Total = total,
            });

            return NoContent();
        }

        // POST: api/BillDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BillDetailOutDTO>> PostBillDetail(int userID, BillDetailInDTO billDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "user not available", Succeeded = false });
            }

            if (userID <= 0)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "user incorrect", Succeeded = false });
            }

            var product = await _IRepositoryProduct.GetAsync(e => e.Id == billDetail.productID);
            if (product == null)
            {
                return NotFound(new Response<BillDetailOutDTO> { Message = "product not found", Succeeded = false });
            }

            var user = await _IRepositoryAccounts.GetAsync(e => e.Id == userID);
            if (user == null)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "user not available", Succeeded = false });
            }
            // ORDERING
            // PENDING
            // SUCCESSED
            // DONE
            var billCheck = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == userID);
            // check bill cũ chưa thanh toán
            if (billCheck != null)
            {
                // bill có sẵn chưa thanh toán
                var detail = await _IRepositoryBillDetail.GetAsync(e => e.ProductID == billDetail.productID && e.BillID == billCheck.Id); //check sản phẩm trong bill
                if (detail != null)
                {
                    // sp đã tồn tại thì +1
                    await _IRepositoryBillDetail.UpdateAsync(new BillDetail
                    {
                        Id = detail.Id,
                        ProductID = detail.ProductID,
                        Quantity = detail.Quantity + billDetail.quantity,
                        Price = product.Price,
                        Total = detail.Total + (product.Price * billDetail.quantity),
                    });
                }
                else
                {
                    // sp chưa tồn tại thì add vào
                    await _IRepositoryBillDetail.CreateAsync(new BillDetail
                    {
                        Id = billDetail.Id,
                        ProductID = billDetail.productID,
                        BillID = billCheck.Id,
                        Quantity = billDetail.quantity,
                        Price = product.Price,
                        Total = product.Price * billDetail.quantity,
                    });
                }


                //return NotFound(new Response<BillDetailOutDTO> { Message = "bill not found", Succeeded = false });
            }
            else
            {
                // trường hợp tất cả bill đã thanh toán
                // tạo bill
                await _IRepositoryBill.CreateAsync(new Bill
                {
                    AccountID = userID,
                    Status = "ORDERING",
                });
                var getBill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == userID);
                // add sp
                await _IRepositoryBillDetail.CreateAsync(new BillDetail
                {
                    Id = billDetail.Id,
                    ProductID = billDetail.productID,
                    BillID = getBill.Id,
                    Quantity = billDetail.quantity,
                    Price = product.Price,
                    Total = product.Price * billDetail.quantity,
                });
            }



            // update subtotal total bill
            var bill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == userID);
            var BillDetails = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == bill.Id);
            var voucher = await _IRepositoryVourcher.GetAsync(e => e.Id == bill.VoucherID);
            double subTotal = 0;
            BillDetails.ForEach(e => subTotal += e.Total);
            var total = voucher != null ? (100 - voucher.Discount) * 0.01 * subTotal : subTotal;
            await _IRepositoryBill.UpdateAsync(new Bill
            {
                Id = bill.Id,
                VoucherID = bill.VoucherID,
                Status = bill.Status,
                SubTotal = subTotal,
                Total = total,
            });

            return CreatedAtAction("GetBillDetail", new { id = billDetail.Id }, billDetail);
        }

        // DELETE: api/BillDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillDetail(int id)
        {
            var result = await _IRepositoryBillDetail.GetAsync(e => e.Id == id);
            var billID = result.BillID;
            if (result == null)
            {
                return NotFound(new Response<BillDetailInDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryBillDetail.DeleteAsync(result);

            //update bill infomation
            var bill = await _IRepositoryBill.GetAsync(e => e.Id == billID);
            var BillDetails = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == billID);
            var voucher = await _IRepositoryVourcher.GetAsync(e => e.Id == bill.VoucherID);
            double subTotal = 0;
            BillDetails.ForEach(e => subTotal += e.Total);
            var total = voucher != null ? (100 - voucher.Discount) * 0.01 * subTotal : subTotal;
            await _IRepositoryBill.UpdateAsync(new Bill
            {
                Id = bill.Id,
                VoucherID = bill.VoucherID,
                Status = bill.Status,
                SubTotal = subTotal,
                Total = total,
            });


            return NoContent();
        }
    }
}
