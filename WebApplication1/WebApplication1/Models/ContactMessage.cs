using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WebApplication1.Models
{
    public class ContactMessage
    {
        [Required]
        public string contactName { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        [EmailAddress]
        public string FromEmail { get; set; }
    }
}
