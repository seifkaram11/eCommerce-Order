namespace Infrastructure.Contracts;

//publishing
public record OrderCretated(Guid OrderId, Guid ProductId, Guid ProductVarID, int Quantity);
//if user is Canceled the order
public record OrderCanceled(Guid ProductId, Guid ProductVarID, int Quantity);


//conumer
public record PaymentFailed(Guid OrderId, Guid ProductId, Guid ProductVarID, int Quantity);
public record PaymentCompleted(Guid OrderId);

public record StockReservationFailed(Guid OrderId, string Reason);

public record NewProduct(Guid ProductID, Guid ProductVariantId, string ProductName, string SKU, decimal UnitPrice);
public record UpdateProduct(Guid ProductID, Guid ProductVariantId, string ProductName, string SKU, decimal UnitPrice);
public record DeleteProduct(Guid ProductID, Guid ProductVariantId);
