using IceCreamBE.DTO;
using IceCreamBE.Repository.Irepository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IRepositoryBill _IRepositoryBill;
        private readonly IRepositoryProduct _IRepositoryProduct;
        private readonly IRepositoryBillDetail _IRepositoryBillDetail;
        private readonly IRepositoryBrand _IRepositoryBrand;


        public ReportController(
            IRepositoryBill iRepositoryBill,
            IRepositoryProduct iRepositoryProduct,
            IRepositoryBillDetail iRepositoryBillDetail,
            IRepositoryBrand repositoryBrand
            )
        {
            _IRepositoryBill = iRepositoryBill;
            _IRepositoryProduct = iRepositoryProduct;
            _IRepositoryBillDetail = iRepositoryBillDetail;
            _IRepositoryBrand = repositoryBrand;
        }

        // GET
        [HttpGet("/api/BrandReport")]
        public async Task<IActionResult> GetBrandReport(int? month, int? year)
        {
            var billdetail = await _IRepositoryBillDetail.GetAllAsync();
            var bill = await _IRepositoryBill.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();

            var result = billdetail
                    .Join(bill,
                        e => e.BillID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, bill = q })
                    .Join(product,
                        e => e.billdetail.ProductID,
                        q => q.Id,
                        (e, q) => new { e.billdetail, e.bill, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        q => q.Id,
                        (e, q) => new { e.billdetail, e.bill, e.product, brand = q })
                    .Where(e => e.bill.Status == "DONE"
                                    && e.bill.OrderTime.Month == (month != null ? month : DateTime.Now.Month)
                                    && e.bill.OrderTime.Year == (year != null ? year : DateTime.Now.Year))
                    .GroupBy(e => e.brand.BrandName)
                    .Select(e => new BrandReportDTO
                    {
                        brand_name = e.Key,
                        sell_count = e.Sum(g => g.billdetail.Quantity),
                        sell_total = e.Sum(g => g.billdetail.Total)
                    });

            return Ok(new Response<IEnumerable<BrandReportDTO>> { Data = result, Succeeded = true });
        }


        // GET
        [HttpGet("/api/MonthReport")]
        public async Task<IActionResult> GetMonthReport(int? month, int? year)
        {
            var billdetail = await _IRepositoryBillDetail.GetAllAsync();
            var bill = await _IRepositoryBill.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();

            var sellTotal = billdetail
                    .Join(bill,
                        e => e.BillID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, bill = q })
                    .Where(e => e.bill.Status == "DONE"
                        && e.bill.OrderTime.Month == (month != null ? month : DateTime.Now.Month)
                        && e.bill.OrderTime.Year == (year != null ? year : DateTime.Now.Year))
                    .Sum(e => e.billdetail.Total);

            var sellCount = billdetail
                    .Join(bill,
                            e => e.BillID,
                            q => q.Id,
                            (e, q) => new { billdetail = e, bill = q })
                    .Where(e => e.bill.Status == "DONE"
                        && e.bill.OrderTime.Month == (month != null ? month : DateTime.Now.Month)
                        && e.bill.OrderTime.Year == (year != null ? year : DateTime.Now.Year))
                    .Sum(e => e.billdetail.Quantity);

            var brandResult = billdetail
                    .Join(bill,
                        e => e.BillID,
                        q => q.Id,
                        (e, q) => new { billdetail = e, bill = q })
                    .Join(product,
                        e => e.billdetail.ProductID,
                        q => q.Id,
                        (e, q) => new { e.billdetail, e.bill, product = q })
                    .Join(brand,
                        e => e.product.BrandID,
                        q => q.Id,
                        (e, q) => new { e.billdetail, e.bill, e.product, brand = q })
                    .Where(e => e.bill.Status == "DONE"
                                    && e.bill.OrderTime.Month == (month != null ? month : DateTime.Now.Month)
                                    && e.bill.OrderTime.Year == (year != null ? year : DateTime.Now.Year))
                    .GroupBy(e => e.brand.BrandName)
                    .Select(e => new BrandReportDTO
                    {
                        brand_name = e.Key,
                        sell_count = e.Sum(g => g.billdetail.Quantity),
                        sell_total = e.Sum(g => g.billdetail.Total)
                    });

            return Ok(new Response<MonthReportDTO>
            {
                Data = new MonthReportDTO
                {
                    month = month ?? DateTime.Now.Month,
                    sell_total = sellTotal,
                    sell_count = sellCount,
                },
                Succeeded = true
            });
        }


        // GET
        [HttpGet("/api/YearReport")]
        public async Task<IActionResult> GetYearReport(int year)
        {
            var billdetail = await _IRepositoryBillDetail.GetAllAsync();
            var bill = await _IRepositoryBill.GetAllAsync();
            var product = await _IRepositoryProduct.GetAllAsync();
            var brand = await _IRepositoryBrand.GetAllAsync();

            var MonthList = new List<MonthReportDTO>();

            for (int month = 1; month <= 12; month++)
            {
                var sellTotal = billdetail
                        .Join(bill,
                            e => e.BillID,
                            q => q.Id,
                            (e, q) => new { billdetail = e, bill = q })
                        .Where(e => e.bill.Status == "DONE"
                            && e.bill.OrderTime.Month == month
                            && e.bill.OrderTime.Year == (year != 0 ? year : DateTime.Now.Year))
                        .Sum(e => e.billdetail.Total);

                var sellCount = billdetail
                        .Join(bill,
                                e => e.BillID,
                                q => q.Id,
                                (e, q) => new { billdetail = e, bill = q })
                        .Where(e => e.bill.Status == "DONE"
                            && e.bill.OrderTime.Month == month
                            && e.bill.OrderTime.Year == (year != 0 ? year : DateTime.Now.Year))
                        .Sum(e => e.billdetail.Quantity);

                MonthList.Add(new MonthReportDTO
                {
                    month = month,
                    sell_total = sellTotal,
                    sell_count = sellCount,
                });
            }

            return Ok(new Response<List<MonthReportDTO>>
            {
                Data = MonthList,
                Succeeded = true
            });

        }
    }
}
