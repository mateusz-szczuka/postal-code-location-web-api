namespace WebApi.DTO.Messeges.Abstractions;

public interface IFault
{
    string Title { get; }

    string Detail { get; }

    int Status { get; }
}