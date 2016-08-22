using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using PriceNotifier.AuthFilter;

namespace PriceNotifier.Controllers
{
    [MyAuthorize]
    public class ProductsController : ApiController
    {
        //private UserContext db = new UserContext();

        private readonly IProductRepository _productRepository;

        public ProductsController()
        {
            _productRepository = new ProductRepository(new UserContext());
        }

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Products

        public IQueryable<Product> GetProducts()
        {
            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");
            //return db.Products.Where(c=>c.UserId==userId);

            return _productRepository.GetProducts().Where(c => c.UserId == userId);

        }

        //// GET: api/Products/5
        //[ResponseType(typeof(Product))]
        //public async Task<IHttpActionResult> GetProduct(int id)
        //{
        //    Product product = await db.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);
        //}

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productRepository.PutProduct(product);

            try
            {
                await _productRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");
            product.UserId = userId;

            var productFound = _productRepository.GetProducts().Where(c=>c.UserId==product.UserId).FirstOrDefault(c => c.ProductId == product.ProductId);

            if (productFound == null) 
            {
                await _productRepository.PostProduct(product);
                return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
            }
            return Conflict();
        }

        [HttpDelete]
        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await _productRepository.FindAsync(id); ;
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProduct(product);

            return Ok() ;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return _productRepository.GetProducts().Count(e => e.Id == id) > 0;
        }
    }
}