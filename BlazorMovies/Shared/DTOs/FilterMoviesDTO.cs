using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMovies.Shared.DTOs
{
    public class FilterMoviesDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;

        // This propoerty
        //public PaginationDTO Pagination
        //{
        //    get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; }
        //}
        // can be expressed in the following way as well, so called
        // expression-bodied member
        // Ref. https://stackoverflow.com/questions/31764532/what-is-the-assignment-in-c-sharp-in-a-property-signature
        public PaginationDTO Pagination => new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage };

        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool InTheaters { get; set; }
        public bool UpcomingReleases { get; set; }
    }
}