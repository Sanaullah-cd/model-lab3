using System;
using System.Collections.Generic;

namespace SolidPrinciplesDemo
{
    // =========================
    // 1. SRP – Single Responsibility Principle
    // =========================
    public class Invoice
    {
        public int Id { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public double TaxRate { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class InvoiceCalculator
    {
        public double CalculateTotal(Invoice invoice)
        {
            double subTotal = 0;
            foreach (var item in invoice.Items)
                subTotal += item.Price;

            return subTotal + (subTotal * invoice.TaxRate);
        }
    }

    public class InvoiceRepository
    {
        public void SaveToDatabase(Invoice invoice)
        {
            Console.WriteLine($"Invoice {invoice.Id} saved to database.");
        }
    }

    // =========================
    // 2. OCP – Open Closed Principle
    // =========================
    public interface IDiscountStrategy
    {
        double ApplyDiscount(double amount);
    }

    public class RegularDiscount : IDiscountStrategy
    {
        public double ApplyDiscount(double amount) => amount;
    }

    public class SilverDiscount : IDiscountStrategy
    {
        public double ApplyDiscount(double amount) => amount * 0.9;
    }

    public class GoldDiscount : IDiscountStrategy
    {
        public double ApplyDiscount(double amount) => amount * 0.8;
    }

    public class PlatinumDiscount : IDiscountStrategy
    {
        public double ApplyDiscount(double amount) => amount * 0.7;
    }

    public class DiscountCalculator
    {
        private readonly IDiscountStrategy _discountStrategy;
        public DiscountCalculator(IDiscountStrategy discountStrategy)
        {
            _discountStrategy = discountStrategy;
        }
        public double Calculate(double amount) => _discountStrategy.ApplyDiscount(amount);
    }

    // =========================
    // 3. ISP – Interface Segregation Principle
    // =========================
    public interface IWork { void Work(); }
    public interface IEat { void Eat(); }
    public interface ISleep { void Sleep(); }

    public class HumanWorker : IWork, IEat, ISleep
    {
        public void Work() => Console.WriteLine("Human is working...");
        public void Eat() => Console.WriteLine("Human is eating...");
        public void Sleep() => Console.WriteLine("Human is sleeping...");
    }

    public class RobotWorker : IWork
    {
        public void Work() => Console.WriteLine("Robot is working...");
    }

    // =========================
    // 4. DIP – Dependency Inversion Principle
    // =========================
    public interface INotificationService
    {
        void Send(string message);
    }

    public class EmailService : INotificationService
    {
        public void Send(string message) => Console.WriteLine($"Email sent: {message}");
    }

    public class SmsService : INotificationService
    {
        public void Send(string message) => Console.WriteLine($"SMS sent: {message}");
    }

    public class Notification
    {
        private readonly INotificationService _notificationService;
        public Notification(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public void Send(string message) => _notificationService.Send(message);
    }

    // =========================
    // Program Demo
    // =========================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SRP Demo ===");
            var invoice = new Invoice { Id = 1, TaxRate = 0.2 };
            invoice.Items.Add(new Item { Name = "Laptop", Price = 1000 });
            invoice.Items.Add(new Item { Name = "Mouse", Price = 50 });
            var calculator = new InvoiceCalculator();
            Console.WriteLine($"Invoice Total: {calculator.CalculateTotal(invoice)}");
            new InvoiceRepository().SaveToDatabase(invoice);

            Console.WriteLine("\n=== OCP Demo ===");
            var goldCustomer = new DiscountCalculator(new GoldDiscount());
            Console.WriteLine($"Gold discount: {goldCustomer.Calculate(1000)}");

            var platinumCustomer = new DiscountCalculator(new PlatinumDiscount());
            Console.WriteLine($"Platinum discount: {platinumCustomer.Calculate(1000)}");

            Console.WriteLine("\n=== ISP Demo ===");
            IWork human = new HumanWorker();
            human.Work();
            ((HumanWorker)human).Eat();
            ((HumanWorker)human).Sleep();

            IWork robot = new RobotWorker();
            robot.Work();

            Console.WriteLine("\n=== DIP Demo ===");
            Notification emailNotification = new Notification(new EmailService());
            emailNotification.Send("Hello via Email!");

            Notification smsNotification = new Notification(new SmsService());
            smsNotification.Send("Hello via SMS!");
        }
    }
}
