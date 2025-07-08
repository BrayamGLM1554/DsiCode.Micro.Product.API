namespace DsiCode.Micro.Product.API.Model.Dto
{
    public record PagerDto(int Page = 1, int RecordsPerPage = 10)
    {
        private const int MaxRecordsPerPage = 50;
        public int Page { get; set; } = Math.Max(1, Page);

        ///<sumary>
        /// clamp me permite identificar el valor valido entre 1 y minimo de un valor
        ///</sumary>
        public int RecordsPerPage { get; set; } = Math.Clamp(RecordsPerPage, 1, MaxRecordsPerPage);

    }

}
