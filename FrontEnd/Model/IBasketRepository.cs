namespace FrontEnd.Model;

public interface IBasketRepository
{
    Task<CustomerBasket> GetBasketAsync(string customerId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task DeleteBasketAsync(string id);
}
