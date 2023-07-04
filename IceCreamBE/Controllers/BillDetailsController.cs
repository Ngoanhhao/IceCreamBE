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
using IceCreamBE.Modules;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;
        private readonly IRepositoryBrand _IRepositoryBrand;
        private readonly IRepositoryFileService _IRepositoryFileService;
        private readonly IRepositoryStorage _IRepositoryStorage;

        public BillDetailsController(IRepositoryBillDetail IRepositoryBillDetail, IRepositoryProduct repositoryProduct, IRepositoryBill repositoryBill, IRepositoryVourcher repositoryVourcher, IRepositoryAccounts repositoryAccounts, IRepositoryBrand iRepositoryBrand, IRepositoryFileService iRepositoryFileService, IRepositoryAccountDetail iRepositoryAccountDetail, IRepositoryStorage iRepositoryStorage)
        {
            _IRepositoryBillDetail = IRepositoryBillDetail;
            _IRepositoryProduct = repositoryProduct;
            _IRepositoryBill = repositoryBill;
            _IRepositoryVourcher = repositoryVourcher;
            _IRepositoryAccounts = repositoryAccounts;
            _IRepositoryAccountDetail = iRepositoryAccountDetail;
            _IRepositoryBrand = iRepositoryBrand;
            _IRepositoryFileService = iRepositoryFileService;
            _IRepositoryStorage = iRepositoryStorage;
        }

        // GET: api/BillDetails
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BillDetailOutDTO>>> GetBillDetail([FromQuery] PaginationFilter<BillDetailOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = billDetail
                .Join(product,
                    e => e.ProductID,
                    q => q.Id,
                    (e, q) => new { billDetail = e, product = q })
                .Join(brand,
                    e => e.product.BrandID,
                    q => q.Id,
                    (e, q) => new { e.product, e.billDetail, brand = q })
                .Select(e => (new BillDetailOutDTO
                {
                    Id = e.billDetail.Id,
                    billID = e.billDetail.BillID,
                    productID = e.product.Id,
                    product_name = e.product.Name,
                    brand_name = e.brand.Name,
                    quantity = e.billDetail.Quantity,
                    img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
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
        [HttpGet("{billID:int}")]
        [Authorize]
        public async Task<ActionResult<BillDetail>> GetBillDetail(int billID)
        {
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = billDetail
                 .Join(product,
                     e => e.ProductID,
                     q => q.Id,
                     (e, q) => new { billDetail = e, product = q })
                 .Join(brand,
                    e => e.product.BrandID,
                    q => q.Id,
                    (e, q) => new { e.product, e.billDetail, brand = q })
                 .Select(e => (new BillDetailOutDTO
                 {
                     Id = e.billDetail.Id,
                     billID = e.billDetail.BillID,
                     productID = e.product.Id,
                     product_name = e.product.Name,
                     quantity = e.billDetail.Quantity,
                     img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                     brand_name = e.brand.Name,
                     total = e.billDetail.Total,
                     price = e.billDetail.Price,
                 }))
                .FirstOrDefault(e => e.billID == billID);

            if (result == null)
            {
                return NotFound(new Response<BillDetailOutDTO> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<BillDetailOutDTO> { Data = result, Succeeded = true });
        }


        // GET: api/BillDetails/5
        [HttpGet("/api/cart/{userID:int}")]
        [Authorize]
        public async Task<ActionResult<BillDetail>> GetCart(int userID)
        {
            var bill = await _IRepositoryBill.GetAsync(e => e.AccountID == userID && e.Status == "ORDERING");
            if (bill == null)
            {
                return BadRequest(new Response<List<BillDetailOutDTO>> { Message = "Your cart is empty, let's order something", Succeeded = false });
            }
            var billDetail = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == bill.Id);
            var product = await _IRepositoryProduct.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";
            var brand = await _IRepositoryBrand.GetAllAsync();

            var result = billDetail
                .Join(product,
                    e => e.ProductID,
                    q => q.Id,
                    (e, q) => new { billDetail = e, product = q })
                .Join(brand,
                    e => e.product.BrandID,
                    q => q.Id,
                    (e, q) => new { e.product, e.billDetail, brand = q })
                .Select(e => (new BillDetailOutDTO
                {
                    Id = e.billDetail.Id,
                    billID = e.billDetail.BillID,
                    productID = e.product.Id,
                    product_name = e.product.Name,
                    brand_name = e.brand.Name,
                    quantity = e.billDetail.Quantity,
                    total = e.billDetail.Total,
                    img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                    price = e.billDetail.Price,
                }))
                .ToList();

            if (result == null)
            {
                return BadRequest(new Response<List<BillDetailOutDTO>> { Message = "not found", Succeeded = false });
            }

            return Ok(new Response<List<BillDetailOutDTO>> { Data = result, Succeeded = true });
        }

        // GET: api/BillDetails/productname
        [HttpGet("{product_name}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BillDetail>>> GetBillDetail(string product_name, [FromQuery] PaginationFilter<BillDetailOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var billDetail = await _IRepositoryBillDetail.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync(e => e.Name.Contains(product_name));
            var brand = await _IRepositoryBrand.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var result = billDetail
                .Join(product,
                    e => e.ProductID,
                    q => q.Id,
                    (e, q) => new { billDetail = e, product = q })
                .Join(brand,
                    e => e.product.BrandID,
                    q => q.Id,
                    (e, q) => new { e.product, e.billDetail, brand = q })
                .Select(e => (new BillDetailOutDTO
                {
                    Id = e.billDetail.Id,
                    billID = e.billDetail.BillID,
                    product_name = e.product.Name,
                    productID = e.product.Id,
                    brand_name = e.brand.Name,
                    img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
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

        // POST: api/BillDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/addtocart")]
        [Authorize]
        public async Task<ActionResult<BillDetailOutDTO>> PostBillDetail(BillDetailInDTO billDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "something wrong please try again", Succeeded = false });
            }

            var product = await _IRepositoryProduct.GetAsync(e => e.Id == billDetail.product_id);
            if (product == null)
            {
                return NotFound(new Response<BillDetailOutDTO> { Message = "product not found", Succeeded = false });
            }


            if (billDetail.user_id <= 0)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "user incorrect", Succeeded = false });
            }
            var user = await _IRepositoryAccounts.GetAsync(e => e.Id == billDetail.user_id);
            if (user == null)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "user not available", Succeeded = false });
            }

            // ORDERING
            // PENDING
            // SUCCESSED
            // DONE
            var billCheck = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == billDetail.user_id);
            // check bill cũ chưa thanh toán
            if (billCheck != null)
            {
                // bill có sẵn chưa thanh toán
                var detail = await _IRepositoryBillDetail.GetAsync(e => e.ProductID == billDetail.product_id && e.BillID == billCheck.Id); //check sản phẩm trong bill
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
                        ProductID = billDetail.product_id,
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
                    AccountID = billDetail.user_id,
                    Status = "ORDERING",
                });
                var getBill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == billDetail.user_id);
                // add sp
                await _IRepositoryBillDetail.CreateAsync(new BillDetail
                {
                    ProductID = billDetail.product_id,
                    BillID = getBill.Id,
                    Quantity = billDetail.quantity,
                    Price = product.Price,
                    Total = product.Price * billDetail.quantity,
                });
            }



            // update subtotal total bill
            var bill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == billDetail.user_id);
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

            return CreatedAtAction("GetBillDetail", new { id = billDetail.id }, billDetail);
        }

        // POST: api/BillDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/guest/order")]
        public async Task<ActionResult<BillDetailOutDTO>> PostBillDetailGuest(CartGuestDTO item, string? voucher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "something wrong please try again", Succeeded = false });
            }

            foreach (var i in item.cart)
            {
                var product = await _IRepositoryProduct.GetAsync(e => e.Id == i.product_id);
                if (product == null)
                {
                    return BadRequest(new Response<BillDetailOutDTO> { Message = "product not found", Succeeded = false });
                }
                var quantity = await _IRepositoryStorage.GetAsync(e => e.ProductID == i.product_id && e.Quantity >= i.quantity);
                if (quantity == null)
                {
                    return BadRequest(new Response<BillDetailOutDTO> { Message = product.Name + " is out of stock", Succeeded = false });
                }
            }

            var user = await _IRepositoryAccountDetail.GetAsync(e => e.PhoneNumber == item.phone_number);
            if (user == null)
            {
                var random = Coupon.CouponGenarate(10);
                await _IRepositoryAccounts.CreateAsync(new Accounts
                {
                    Password = random,
                    Username = random,
                });
                var account = await _IRepositoryAccounts.GetAsync(e => e.Username == random && e.Password == random);
                await _IRepositoryAccountDetail.CreateAsync(new AccountDetail
                {
                    Id = account.Id,
                    FullName = item.full_name,
                    PhoneNumber = item.phone_number,
                    Email = "guest@gmail.com",
                    Address = item.address,
                    Avatar = null,
                    CreateDate = DateTime.Now,
                    ExpirationDate = DateTime.Now,
                    ExtensionDate = DateTime.Now,
                    RoleID = 3,
                });
                user = await _IRepositoryAccountDetail.GetAsync(e => e.Id == account.Id);
            }



            // ORDERING
            // PENDING
            // SUCCESSED
            // DONE
            // tạo bill
            await _IRepositoryBill.CreateAsync(new Bill
            {
                AccountID = user.Id,
                Status = "ORDERING",
                OrderTime = DateTime.Now,
            });
            var getBill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == user.Id);

            // add sp
            foreach (var i in item.cart)
            {
                int quantity = i.quantity;
                var product = await _IRepositoryProduct.GetAsync(e => e.Id == i.product_id);
                await _IRepositoryBillDetail.CreateAsync(new BillDetail
                {
                    ProductID = product.Id,
                    BillID = getBill.Id,
                    Quantity = quantity,
                    Price = product.Price,
                    Total = product.Price * quantity,
                });

                await _IRepositoryStorage.UpdateAsync(product.Id, 0 - quantity, false);
            }

            //update voucher
            if (voucher != null)
            {
                var voucher2 = await _IRepositoryVourcher.GetAsync(e => e.Voucher == voucher);
                if (voucher2 == null)
                {
                    return BadRequest(new Response<BillOutDTO> { Message = "voucher not correct", Succeeded = false });
                }
                await _IRepositoryBill.UpdateVoucherAsync(new Bill
                {
                    VoucherID = voucher2.Id,
                    AccountID = user.Id,
                });
            }

            // update subtotal total bill
            var bill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == user.Id);
            var BillDetails = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == bill.Id);
            var Voucher = await _IRepositoryVourcher.GetAsync(e => e.Id == bill.VoucherID);
            double subTotal = 0;
            BillDetails.ForEach(e => subTotal += e.Total);
            var total = Voucher != null ? (100 - Voucher.Discount) * 0.01 * subTotal : subTotal;
            await _IRepositoryBill.UpdateAsync(new Bill
            {
                Id = bill.Id,
                VoucherID = bill.VoucherID,
                Status = "PENDING",
                SubTotal = subTotal,
                Total = total,
            });


            return CreatedAtAction("GetBillDetail", new { id = bill.Id }, BillDetails);
        }


        // DELETE: api/remove bill item
        [HttpDelete("/api/cartremove")]
        [Authorize]
        public async Task<IActionResult> DeleteBillDetail(int userId, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "something wrong please try again", Succeeded = false });
            }

            var product = await _IRepositoryProduct.GetAsync(e => e.Id == productId);
            if (product == null)
            {
                return NotFound(new Response<BillDetailOutDTO> { Message = "product not found", Succeeded = false });
            }


            if (userId <= 0)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "user incorrect", Succeeded = false });
            }
            var user = await _IRepositoryAccounts.GetAsync(e => e.Id == userId);
            if (user == null)
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "user not available", Succeeded = false });
            }

            // ORDERING
            // PENDING
            // SUCCESSED
            // DONE
            var billCheck = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == userId);
            // check bill cũ chưa thanh toán
            if (billCheck != null)
            {
                // bill có sẵn chưa thanh toán
                var detail = await _IRepositoryBillDetail.GetAsync(e => e.ProductID == productId && e.BillID == billCheck.Id); //check sản phẩm trong bill
                if (detail != null)
                {
                    // sp đã tồn tại 
                    await _IRepositoryBillDetail.DeleteAsync(detail);
                    // update subtotal total bill
                    var bill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == userId);
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
                }
                else
                {
                    //sp chưa tồn tại
                    return BadRequest(new Response<BillDetailInDTO> { Message = "product not available in your cart", Succeeded = false });
                }
            }
            else
            {
                return BadRequest(new Response<BillDetailInDTO> { Message = "your cart is empty please order something", Succeeded = false });
            }
            return NoContent();
        }

        // DELETE: api/BillDetails/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
