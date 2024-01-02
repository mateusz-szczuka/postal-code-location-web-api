using MediatR;
using WebApi.DTO.Messeges.Responses;

namespace WebApi.DTO.Messeges.Requests;

public class GetAverageLocationRequest : IRequest<GetAverageLocationResponse>
{
}