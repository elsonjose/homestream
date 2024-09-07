using HomeStream.Application.Dtos;
using MediatR;

namespace HomeStream.Application.Requests;

public class AddQuery : IRequest<ResponseDto<int>>
{
    public int LeftOperand {  get; set; }
    public int RightOperand {  get; set; }
}

public class AddQueryHandler : IRequestHandler<AddQuery, ResponseDto<int>>
{
    public async Task<ResponseDto<int>> Handle(AddQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ResponseDto<int>(request.LeftOperand + request.RightOperand));
    }
}
