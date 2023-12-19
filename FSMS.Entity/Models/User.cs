using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class User
    {
        public User()
        {
            ChatHistoryReceiverNavigations = new HashSet<ChatHistory>();
            ChatHistorySenderNavigations = new HashSet<ChatHistory>();
            Comments = new HashSet<Comment>();
            FruitHistories = new HashSet<FruitHistory>();
            Fruits = new HashSet<Fruit>();
            Gardens = new HashSet<Garden>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
            Posts = new HashSet<Post>();
            ReviewFruits = new HashSet<ReviewFruit>();
            Weathers = new HashSet<Weather>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? ImageMomoUrl { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<ChatHistory> ChatHistoryReceiverNavigations { get; set; }
        public virtual ICollection<ChatHistory> ChatHistorySenderNavigations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FruitHistory> FruitHistories { get; set; }
        public virtual ICollection<Fruit> Fruits { get; set; }
        public virtual ICollection<Garden> Gardens { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ReviewFruit> ReviewFruits { get; set; }
        public virtual ICollection<Weather> Weathers { get; set; }
    }
}
