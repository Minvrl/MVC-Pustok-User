﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Pustok.Data;
using MVC_Pustok.Models;


namespace MVC_Pustok.Controllers
{
    public class BookController:Controller
    {
        private AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult GetBookById(int id)
        {


            Book book = _context.Books.Include(x => x.Genre).Include(x => x.BookImages.Where(x => x.PosterStatus == true)).FirstOrDefault(x => x.Id == id);
            return PartialView("_BookModalPartial", book);
        }

    }
}
