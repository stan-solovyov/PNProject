using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Services.ArticleService;
using Domain.Entities;
using PriceNotifier.DTO;

namespace PriceNotifier.Controllers
{
    public class ArticlesController : ApiController
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: api/Articles
        public PageResult<ArticleDto> GetArticles(ODataQueryOptions<Article> options)
        {
            var allArticles = _articleService.Query();
            IQueryable articles = options.ApplyTo(allArticles);
            var results = articles.ProjectTo<ArticleDto>();

            return new PageResult<ArticleDto>(
                results,
                Request.ODataProperties().NextLink,
                Request.ODataProperties().TotalCount);
        }

        // GET: api/Articles/5
        [ResponseType(typeof(ArticleDto))]
        public async Task<ArticleDto> GetArticle(int id)
        {
            Article article = await _articleService.GetById(id);
            if (article == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<Article, ArticleDto>(article);
        }

        // PUT: api/Articles/5
        [ResponseType(typeof(ArticleDto))]
        public async Task<ArticleDto> PutArticle(ArticleDto articleDto)
        {
            if (!ModelState.IsValid)
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                throw new HttpResponseException(response);
            }

            Article article = await _articleService.GetById(articleDto.ArticleId);
            if (article != null)
            {
                var articleUpdated = Mapper.Map(articleDto, article);
                await _articleService.Update(articleUpdated);
                articleDto = Mapper.Map(articleUpdated, articleDto);
                return articleDto;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // POST: api/Articles
        [ResponseType(typeof(ArticleDto))]
        public async Task<ArticleDto> PostArticle(ArticleDto articleDto)
        {
            if (!ModelState.IsValid)
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                throw new HttpResponseException(response);
            }
            var article = Mapper.Map<ArticleDto, Article>(articleDto);
            await _articleService.Create(article);

            return articleDto;
        }

        // DELETE: api/Articles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteArticle(int id)
        {
            Article article = await _articleService.GetById(id);
            if (article == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            await _articleService.Delete(article);

            return Ok();
        }
    }
}