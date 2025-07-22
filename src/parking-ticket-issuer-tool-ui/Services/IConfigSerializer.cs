namespace ParkingTicketIssuerToolUI.Services;

public interface IConfigSerializer<T>
{
    T? Deserialize();
}