﻿using System;

namespace GigHub.Core.Dtos
{
    public class GigDto
    {
        public int Id { get; set; }

        public bool isCancelled { get; private set; }


        public UserDto Artist { get; set; }


        public DateTime DateTime { get; set; }

        public string Venue { get; set; }
        public GenreDto Genre { get; set; }
    }
}