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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IRepositoryBill _IRepositoryBill;
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryAccounts _IRepositoryAccounts;
        private readonly IRepositoryVourcher _IRepositoryVourcher;
        private readonly IRepositoryBillDetail _IRepositoryBillDetail;
        private readonly IRepositoryAccountDetail _IRepositoryAccountDetail;
        private readonly IRepositoryBrand _IRepositoryBrand;
        private readonly IRepositoryStorage _IRepositoryStorage;
        private readonly IRepositoryFileService _IRepositoryFileService;
        private readonly IMailHandle _IMailHandle;


        public BillsController(
            IRepositoryBill iRepositoryBill,
            IRepositoryProduct iRepositoryProduct,
            IRepositoryAccounts iRepositoryAccounts,
            IRepositoryVourcher iRepositoryVourcher,
            IRepositoryBillDetail iRepositoryBillDetail,
            IRepositoryAccountDetail iRepositoryAccountDetail,
            IRepositoryBrand repositoryBrand,
            IRepositoryFileService iRepositoryFileService,
            IMailHandle iMailHandle,
            IRepositoryStorage iRepositoryStorage
            )
        {
            _IRepositoryBill = iRepositoryBill;
            _IRepositoryProduct = iRepositoryProduct;
            _IRepositoryAccounts = iRepositoryAccounts;
            _IRepositoryVourcher = iRepositoryVourcher;
            _IRepositoryBillDetail = iRepositoryBillDetail;
            _IRepositoryAccountDetail = iRepositoryAccountDetail;
            _IRepositoryBrand = repositoryBrand;
            _IRepositoryFileService = iRepositoryFileService;
            _IMailHandle = iMailHandle;
            _IRepositoryStorage = iRepositoryStorage;
        }

        // GET: api/Bills/
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BillInDTO>>> GetBill([FromQuery] PaginationFilter<BillInDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        discount = e.bill.Discount,
                        status = e.bill.Status,
                        total = e.bill.Total,
                        address = e.accountDetail.Address,
                        voucher = e.bill.VoucherID.ToString(),
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).ToList();


            foreach (var e in result)
            {
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();

                // get bill detail
                var billDetailItem = billdetail
                    .Join(product,
                        e => e.ProductID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        q => q.Id,
                        (e, q) => new { e.product, e.billdetail, brand = q })
                    .Select(e => new BillDetailOutDTO
                    {
                        Id = e.billdetail.Id,
                        billID = e.billdetail.BillID,
                        product_name = e.product.Name,
                        productID = e.product.Id,
                        brand_name = e.brand.Name,
                        img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                        quantity = e.billdetail.Quantity,
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));

                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }


        //GET: api/Bills/name, email, phone number
        [HttpGet("/api/getbill/{user_id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetBill(int user_id, [FromQuery] PaginationFilter<BillOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync(e => e.AccountID == user_id);
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        address = e.accountDetail.Address,
                        discount = e.bill.Discount,
                        total = e.bill.Total,
                        voucher = e.bill.VoucherID.ToString(),
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).OrderByDescending(e => e.status)
                    .ToList();

            if (result.Count == 0)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }

            foreach (var e in result)
            {
                // get bill detail
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();
                var brand = await _IRepositoryBrand.GetAllAsync();

                var billDetailItem = billdetail
                    .Join(product,
                        e => e.ProductID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        q => q.Id,
                        (e, q) => new { e.product, e.billdetail, brand = q })
                    .Select(e => new BillDetailOutDTO
                    {
                        Id = e.billdetail.Id,
                        billID = e.billdetail.BillID,
                        product_name = e.product.Name,
                        productID = e.product.Id,
                        brand_name = e.brand.Name,
                        img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                        quantity = e.billdetail.Quantity,
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));

                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }


        //GET: api/Bills/id
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetBills(int id)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        total = e.bill.Total,
                        discount = e.bill.Discount,
                        address = e.accountDetail.Address,
                        voucher = e.bill.VoucherID.ToString(),
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).FirstOrDefault(e => e.Id == id);

            if (result == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }

            // get bill detail
            var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == result.Id);
            var product = await _IRepositoryProduct.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();

            var billDetailItem = billdetail
                .Join(product,
                    e => e.ProductID,
                    q => q.Id,
                    (e, q) => new { billdetail = e, product = q })
                .Join(brand,
                    e => e.product.BrandID,
                    e => e.Id,
                    (e, q) => new { e.product, e.billdetail, brand = q })
                .Select(e => new BillDetailOutDTO
                {
                    Id = e.billdetail.Id,
                    billID = e.billdetail.BillID,
                    product_name = e.product.Name,
                    productID = e.product.Id,
                    brand_name = e.brand.Name,
                    img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                    quantity = e.billdetail.Quantity,
                    total = e.billdetail.Total,
                    price = e.billdetail.Price,
                })
                .ToList();

            //get voucher from voucherID
            var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(result.voucher.Equals("") ? "0" : result.voucher)));

            result.voucher = voucherItem == null ? null : voucherItem.Voucher;
            result.products = billDetailItem;

            return Ok(new Response<BillOutDTO> { Data = result, Succeeded = true });
        }


        //GET: api/Bills/order/id
        [HttpGet("/api/getorders")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetBill([FromQuery] PaginationFilter<BillOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Where(e => e.bill.Status == "PENDING" || e.bill.Status == "SUCCESSED")
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        discount = e.bill.Discount,
                        address = e.accountDetail.Address,
                        total = e.bill.Total,
                        voucher = e.bill.VoucherID.ToString(),
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).OrderByDescending(e => e.order_Time).OrderBy(e => e.status).ToList();

            foreach (var e in result)
            {
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();
                var brand = await _IRepositoryBrand.GetAllAsync();

                var billDetailItem = billdetail
                    .Join(product,
                        e => e.ProductID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        e => e.Id,
                        (e, q) => new { e.product, e.billdetail, brand = q })
                    .Select(e => new BillDetailOutDTO
                    {
                        Id = e.billdetail.Id,
                        billID = e.billdetail.BillID,
                        product_name = e.product.Name,
                        productID = e.product.Id,
                        brand_name = e.brand.Name,
                        img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                        quantity = e.billdetail.Quantity,
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));
                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        //GET: api/Bills/order/id
        [HttpGet("/api/search/order")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> SeachBill([FromQuery] PaginationFilter<BillOutDTO>? filter, string? query)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Where(e => (e.bill.Status == "PENDING" || e.bill.Status == "SUCCESSED") && e.accountDetail.FullName.ToLower().Contains(query != null ? query.ToLower() : ""))
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        address = e.accountDetail.Address,
                        total = e.bill.Total,
                        voucher = e.bill.VoucherID.ToString(),
                        discount = e.bill.Discount,
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).OrderByDescending(e => e.order_Time).ToList();

            foreach (var e in result)
            {
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();
                var brand = await _IRepositoryBrand.GetAllAsync();

                var billDetailItem = billdetail
                    .Join(product,
                        e => e.ProductID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        e => e.Id,
                        (e, q) => new { e.product, e.billdetail, brand = q })
                    .Select(e => new BillDetailOutDTO
                    {
                        Id = e.billdetail.Id,
                        billID = e.billdetail.BillID,
                        product_name = e.product.Name,
                        productID = e.product.Id,
                        brand_name = e.brand.Name,
                        img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                        quantity = e.billdetail.Quantity,
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));
                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }

        //GET: api/Bills/order/id
        [HttpGet("/api/invoices")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> GetInvoices([FromQuery] PaginationFilter<BillOutDTO>? filter)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Where(e => e.bill.Status == "DONE")
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        address = e.accountDetail.Address,
                        discount = e.bill.Discount,
                        total = e.bill.Total,
                        voucher = e.bill.VoucherID.ToString(),
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).ToList();

            foreach (var e in result)
            {
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();
                var brand = await _IRepositoryBrand.GetAllAsync();

                var billDetailItem = billdetail
                    .Join(product,
                        e => e.ProductID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        e => e.Id,
                        (e, q) => new { e.product, e.billdetail, brand = q })
                    .Select(e => new BillDetailOutDTO
                    {
                        Id = e.billdetail.Id,
                        billID = e.billdetail.BillID,
                        product_name = e.product.Name,
                        productID = e.product.Id,
                        brand_name = e.brand.Name,
                        img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                        quantity = e.billdetail.Quantity,
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));
                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }


        //GET: api/Bills/order/id
        [HttpGet("/api/search/invoice")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BillOutDTO>>> SearchInvoices([FromQuery] PaginationFilter<BillOutDTO>? filter, string? query)
        {
            var bill = await _IRepositoryBill.GetAllAsync();
            var accountDetail = await _IRepositoryAccountDetail.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            // Bill
            var result = bill
                    .Join(accountDetail,
                        e => e.AccountID,
                        q => q.Id,
                        (e, q) => new { bill = e, accountDetail = q })
                    .Where(e => e.bill.Status == "DONE" && e.accountDetail.FullName.ToLower().Contains(query != null ? query.ToLower() : ""))
                    .Select(e => new BillOutDTO
                    {
                        Id = e.bill.Id,
                        full_name = e.accountDetail.FullName,
                        order_Time = e.bill.OrderTime,
                        status = e.bill.Status,
                        address = e.accountDetail.Address,
                        discount = e.bill.Discount,
                        total = e.bill.Total,
                        voucher = e.bill.VoucherID.ToString(),
                        sub_total = e.bill.SubTotal,
                        email = e.accountDetail.Email,
                        phone_number = e.accountDetail.PhoneNumber,
                    }).ToList();

            foreach (var e in result)
            {
                var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == e.Id);
                var product = await _IRepositoryProduct.GetAllAsync();
                var brand = await _IRepositoryBrand.GetAllAsync();

                var billDetailItem = billdetail
                    .Join(product,
                        e => e.ProductID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        e => e.Id,
                        (e, q) => new { e.product, e.billdetail, brand = q })
                    .Select(e => new BillDetailOutDTO
                    {
                        Id = e.billdetail.Id,
                        billID = e.billdetail.BillID,
                        product_name = e.product.Name,
                        productID = e.product.Id,
                        brand_name = e.brand.Name,
                        img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                        quantity = e.billdetail.Quantity,
                        total = e.billdetail.Total,
                        price = e.billdetail.Price,
                    })
                    .ToList();

                //get voucher from voucherID
                var voucherItem = (await _IRepositoryVourcher.GetAsync(v => v.Id == int.Parse(e.voucher.Equals("") ? "0" : e.voucher)));
                e.voucher = voucherItem == null ? null : voucherItem.Voucher;
                e.products = billDetailItem;
            }

            var pageFilter = new PaginationFilter<BillOutDTO>(filter.PageNumber, filter.PageSize);
            var pagedData = pageFilter.GetPageList(result);

            return Ok(new PagedResponse<List<BillOutDTO>>
            {
                Data = pagedData,
                Succeeded = pagedData == null ? false : true,
                Pagination = new PagedResponseDetail<List<BillOutDTO>>
                {
                    current_page = pagedData == null ? 0 : pageFilter.PageNumber,
                    Page_pize = pagedData == null ? 0 : pageFilter.PageSize,
                    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
                    total_records = result.Count
                }
            });
        }


        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{userID}")]
        [Authorize]
        public async Task<IActionResult> PutBill(int userID, string voucher)
        {
            var getVoucher = await _IRepositoryVourcher.GetAsync(e => e.Voucher == voucher && e.Status == true);

            if (getVoucher == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "voucher invalid", Succeeded = false });
            }
            else if (getVoucher.ExpirationDate < DateTime.Now)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "the voucher has expired", Succeeded = false });
            }


            var result = await _IRepositoryBill.GetAsync(e => e.AccountID == userID && e.Status == "ORDERING");

            if (result == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "account invalid or the bill not available", Succeeded = false });
            }

            await _IRepositoryBill.UpdateVoucherAsync(new Bill
            {
                AccountID = userID,
                VoucherID = getVoucher.Id,
            });

            // update subtotal total bill
            var bill = await _IRepositoryBill.GetAsync(e => e.Status == "ORDERING" && e.AccountID == userID);
            var BillDetails = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == bill.Id);
            var voucher2 = await _IRepositoryVourcher.GetAsync(e => e.Id == bill.VoucherID);
            double subTotal = 0;
            BillDetails.ForEach(e => subTotal += e.Total);
            var total = voucher != null ? (100 - voucher2.Discount) * 0.01 * subTotal : subTotal;
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

        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/api/success/{billID:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutSuccess(int billID)
        {
            // ORDERING
            // PENDING
            // SUCCESSED
            // DONE

            var bill = await _IRepositoryBill.GetAsync(e => e.Id == billID && e.Status == "PENDING");
            if (bill == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "bill invalid", Succeeded = false });
            }
            var user = await _IRepositoryAccountDetail.GetAsync(e => e.Id == bill.AccountID);
            if (user == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "user invalid", Succeeded = false });
            }

            await _IRepositoryBill.UpdateStatusAsync(new Bill { AccountID = user.Id, Status = "SUCCESSED" }, "PENDING");


            _IMailHandle.send("Confirm order", "<h2>Ice Cream Shop</h2>" +
                "<div style='display: inline-block; border: 1px solid #00000029; border-radius: 7px; padding: 15px'>" +
                "<h3 style='font-weight: 100; color: black'> Woo hoo! Your order is on its way. Your order details can be found below.</h3>" +
                "<h3>ORDER SUMMARY:</h3>" +
                $"<p>Order : IC{bill.Id}</p>" +
                $"<p>Order Date : {bill.OrderTime}</p>" +
                $"<p>Order Total : {bill.Total}</p>" +
                $"<p>SHIPPING ADDRESS: {user.Address}</p>" +
                $"</div>",
                user.Email);


            return Ok(new Response<BillDetailInDTO> { Succeeded = true });
        }

        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/api/confirm/{billID:int}")]
        public async Task<IActionResult> PutConfirm(int billID)
        {
            // ORDERING
            // PENDING
            // SUCCESSED
            // DONE

            var bill = await _IRepositoryBill.GetAsync(e => e.Id == billID && e.Status == "SUCCESSED");
            if (bill == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "bill invalid", Succeeded = false });
            }
            var user = await _IRepositoryAccounts.GetAsync(e => e.Id == bill.AccountID);
            if (user == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "user invalid", Succeeded = false });
            }

            await _IRepositoryBill.UpdateStatusAsync(new Bill { AccountID = user.Id, Status = "DONE" }, "SUCCESSED");
            return Ok(new Response<BillDetailInDTO> { Succeeded = true });
        }

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/api/order")]
        [Authorize]
        public async Task<ActionResult<BillOutDTO>> PutBilll(int userID, string? voucher)
        {
            if (userID <= 0)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "value incorrect", Succeeded = false });
            }
            var user = await _IRepositoryAccountDetail.GetAsync(e => e.Id == userID);
            if (user == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "user incorrect", Succeeded = false });
            }


            var bill = await _IRepositoryBill.GetAsync(e => e.AccountID == userID && e.Status == "ORDERING");
            if (bill == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "order something please", Succeeded = false });
            }

            if (voucher != null)
            {
                var Voucher = await _IRepositoryVourcher.GetAsync(e => e.Voucher == voucher);
                if (Voucher == null)
                {
                    return BadRequest(new Response<BillOutDTO> { Message = "voucher not correct", Succeeded = false });
                }
                await _IRepositoryBill.UpdateVoucherAsync(new Bill
                {
                    VoucherID = Voucher.Id,
                    AccountID = user.Id,
                });
            }


            if (user.ExpirationDate > DateTime.Now)
            {
                await _IRepositoryBill.UpdateDiscountAsync(bill, 5);
            }

            await _IRepositoryBill.UpdateStatusAsync(new Bill { AccountID = userID, Status = "PENDING", OrderTime = DateTime.Now }, "ORDERING");

            var NewVoucher = await _IRepositoryVourcher.GetAsync(e => e.Voucher == voucher);

            var result = new BillOutDTO
            {
                Id = bill.Id,
                full_name = user.FullName,
                order_Time = bill.OrderTime,
                status = bill.Status,
                total = (user.ExpirationDate > DateTime.Now ? bill.Total * 0.95 : bill.Total) * (voucher != null ? (100 - NewVoucher.Discount) / 100 : 1),
                voucher = bill.VoucherID.ToString(),
                sub_total = bill.SubTotal,
                discount = user.ExpirationDate > DateTime.Now ? 5 : 0,
                email = user.Email,
                phone_number = user.PhoneNumber,
            };

            var billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == result.Id);

            // get bill detail
            billdetail = await _IRepositoryBillDetail.GetAllAsync(q => q.BillID == result.Id);
            var product = await _IRepositoryProduct.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();
            string url = $"{Request.Scheme}://{Request.Host}/api/image/";

            var billDetailItem = billdetail
                .Join(product,
                    e => e.ProductID,
                    q => q.Id,
                    (e, q) => new { billdetail = e, product = q })
                .Join(brand,
                    e => e.product.BrandID,
                    e => e.Id,
                    (e, q) => new { e.product, e.billdetail, brand = q })
                .Select(e => new BillDetailOutDTO
                {
                    Id = e.billdetail.Id,
                    billID = e.billdetail.BillID,
                    product_name = e.product.Name,
                    productID = e.product.Id,
                    brand_name = e.brand.Name,
                    img = _IRepositoryFileService.CheckImage(e.product.Img, "Images") ? url + e.product.Img : null,
                    quantity = e.billdetail.Quantity,
                    total = e.billdetail.Total,
                    price = e.billdetail.Price,
                })
                .ToList();



            //update storage
            foreach (var item in billDetailItem)
            {
                await _IRepositoryStorage.UpdateAsync(item.productID, 0 - item.quantity, false);
            }

            string urlConfirm = $"{Request.Scheme}://{Request.Host}/api/success/";

            _IMailHandle.send("Confirm order", "<h2>Ice Cream Shop</h2>" +
                "<div style='display: inline-block; border: 1px solid #00000029; border-radius: 7px; padding: 15px'>" +
                $"<h3 style='font-weight: 100; color: black'> You have received a delivery request from {user.FullName}.</h3>" +
                "<h3>ORDER SUMMARY:</h3>" +
                $"<p>Order : IC{bill.Id}</p>" +
                $"<p>Order Date : {bill.OrderTime}</p>" +
                $"<p>Order Total : {bill.Total}</p>" +
                $"<p>SHIPPING ADDRESS: {user.Address}</p>" +
                $"</div>",
                "icecreammra@gmail.com");

            return Ok(new Response<BillOutDTO> { Succeeded = true });

        }

        //// DELETE: api/Bills/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var result = await _IRepositoryBill.GetAsync(e => e.Id == id);

            if (result == null)
            {
                return BadRequest(new Response<BillOutDTO> { Message = "not found", Succeeded = false });
            }

            await _IRepositoryBill.DeleteAsync(result);
            var billList = await _IRepositoryBillDetail.GetAllAsync(e => e.BillID == result.Id);
            billList.ForEach(async e => await _IRepositoryBillDetail.DeleteAsync(e));


            return NoContent();
        }
    }
}
