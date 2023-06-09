﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingShop_Models
{
    public class InquiryHeader
    {
        [Key]
        public int Id { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public DateTime InquiryDate { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
