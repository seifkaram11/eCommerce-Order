namespace Infrastructure.Contracts;

//publishing
public record OrderCretated(Guid OrderId, Guid ProductId, Guid ProductVarID, Guid userId, int Quantity, int ExpectedItemsCount, decimal totalPrice);
public record AddNewItemToOrder(Guid OrderId, Guid ProductId, Guid ProductVarID, int Quantity, decimal totalPrice);
public record OrderCanceled(Guid orderId);
public record ItemCanceled(Guid orderId, Guid ProductId, Guid ProductVarID);


//conumer
public record PaymentFailed(Guid OrderId, Guid ProductId, Guid ProductVarID, int Quantity);
public record PaymentCompleted(Guid OrderId);

public record StockReservationFailed(Guid OrderId, string Reason);
public record StockReservationFulfilled(Guid ReservationId, Guid OrderId, string ProductId, int Quantity, DateTimeOffset FulfilledAt);
public record OrderInventoryCommitted(Guid OrderId, Guid UserId, DateTimeOffset CommittedAt);


public record NewProduct(Guid ProductID, Guid ProductVariantId, string ProductName, string SKU, decimal UnitPrice);
public record UpdateProduct(Guid ProductID, Guid ProductVariantId, string ProductName, string SKU, decimal UnitPrice);
public record DeleteProduct(Guid ProductID, Guid ProductVariantId);


