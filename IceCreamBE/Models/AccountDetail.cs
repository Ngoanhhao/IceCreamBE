﻿using System.ComponentModel.DataAnnotations;

namespace IceCreamBE.Models
{
    public class AccountDetail
    {
        public int Id { get; set; }
        public virtual Accounts Accounts { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExtensionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}