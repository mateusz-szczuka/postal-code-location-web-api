namespace WebApi.DTO.Messeges.Abstractions;

public interface IApiResponse
{
    IFault Fault { get; }
}