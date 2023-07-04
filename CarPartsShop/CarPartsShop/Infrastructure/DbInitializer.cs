using CarPartsShop.Data;
using CarPartsShop.Data.Models;

namespace CarPartsShop.Infrastructure
{
    public static class DbInitializer
    {

        public static void Initialize(CarPartsShopDbContext context)
        {
            SeedPartCondition(context);
            SeedPartType(context);
            SeedCartStatus(context);
            SeedOrderStatus(context);
        }

        private static void SeedPartCondition(CarPartsShopDbContext context)
        {
            if(!context.PartCondtions.Any())
            {
                var conditions = new List<PartCondtion>()
                {
                    new PartCondtion {Condition = "Ново"},
                    new PartCondtion {Condition = "Втора ръка"},
                };

                var log = new log_19118067
                {
                    TableName = "PartCondition",
                    OperationType = "Insert",
                    OperationCreateTime = DateTime.Now
                };
                context.log_19118067.Add(log);

                context.PartCondtions.AddRange(conditions);
                context.SaveChanges();
            }
        }

        private static void SeedPartType(CarPartsShopDbContext context)
        {
            if(!context.PartTypes.Any())
            {
                var types = new List<PartType>()
                {
                    new PartType {Type = "Спирачна система"},
                    new PartType {Type = "Окачване"},
                    new PartType {Type = "Кормилна система"},
                    new PartType {Type = "Части за двигател"},
                    new PartType {Type = "Части за цилиндров блок"},
                    new PartType {Type = "Гарнитури"},
                    new PartType {Type = "Датчици и сенозри"},
                    new PartType {Type = "Горивна система"},
                    new PartType {Type = "Ауспуси и гърнета"},
                    new PartType {Type = "Тунинг"},
                    new PartType {Type = "Стартова система"},
                    new PartType {Type = "Светлини"},
                    new PartType {Type = "Автоаксесоари"}
                };

                var log = new log_19118067
                {
                    TableName = "PartType",
                    OperationType = "Insert",
                    OperationCreateTime = DateTime.Now
                };
                context.log_19118067.Add(log);
                context.PartTypes.AddRange(types);
                context.SaveChanges();
            }
        }

        private static void SeedCartStatus(CarPartsShopDbContext context)
        {
            if (!context.CartStatuses.Any())
            {
                var statuses = new List<CartStatus>
                {
                    new CartStatus{Status= "Active", LastModified_19118067 = DateTime.Now},
                    new CartStatus{Status = "Closed", LastModified_19118067= DateTime.Now},
                };
                var log = new log_19118067
                {
                    TableName = "CartStatuses",
                    OperationType = "Insert",
                    OperationCreateTime = DateTime.Now
                };
                context.log_19118067.Add(log);
                context.CartStatuses.AddRange(statuses);
                context.SaveChanges();
            }
        }

        private static void SeedOrderStatus(CarPartsShopDbContext context)
        {
            if (!context.OrderStatuses.Any())
            {
                var orderStatuses = new List<OrderStatus>
                {
                    new OrderStatus{Status = "Обработва се", LastModified_19118067 = DateTime.Now},
                    new OrderStatus{Status = "Предадена на куриер", LastModified_19118067 = DateTime.Now},
                    new OrderStatus{Status = "Доставена", LastModified_19118067 = DateTime.Now},
                    new OrderStatus{Status = "Отказана", LastModified_19118067=DateTime.Now}
                };

                var log = new log_19118067
                {
                    TableName = "OrderStatuses",
                    OperationType = "Insert",
                    OperationCreateTime = DateTime.Now
                };
                context.log_19118067.Add(log);

                context.OrderStatuses.AddRange(orderStatuses);
                context.SaveChanges();
            }
        }
    }
}
