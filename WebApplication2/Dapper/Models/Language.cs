﻿namespace WebApplication2.Dapper.Models
{
    public class Language
    {
        public int language_id { get; set; }

        public string? Name { get; set; }
        public DateTime last_update { get; set; } = DateTime.Now;

    }
}
