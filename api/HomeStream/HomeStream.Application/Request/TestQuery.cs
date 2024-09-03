using HomeStream.Application.Dto;
using MediatR;

namespace HomeStream.Application.Request;

public class TestQuery : IRequest<ResponseDto<int>>
{
    public int LeftOperand {  get; set; }
    public int RightOperand {  get; set; }
}

public class TestQueryHandler : IRequestHandler<TestQuery, ResponseDto<int>>
{
    public async Task<ResponseDto<int>> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ResponseDto<int>(request.LeftOperand + request.RightOperand));
    }
}
