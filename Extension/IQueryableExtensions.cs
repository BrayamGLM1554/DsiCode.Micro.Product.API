using DsiCode.Micro.Product.API.Model.Dto;

namespace DsiCode.Micro.Product.API.Extension
{
    public static class IQueryableExtensions
    {
        public static IQueryable <T> Paginar <T> (this IQueryable <T> queryable, PagerDto pagerDto)
        {
            return queryable
                .Skip((pagerDto.Page - 1) * pagerDto.RecordsPerPage)
                // establece el numero de paginas que se va a saltar
                .Take(pagerDto.RecordsPerPage);
            //
        }
    }
}
