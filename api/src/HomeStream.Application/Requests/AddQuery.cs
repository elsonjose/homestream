using HomeStream.Application.Abstractions.Services;
using HomeStream.Application.Dtos;
using HomeStream.Application.Implementations.Services;
using MediatR;

namespace HomeStream.Application.Requests;

public class AddQuery : IRequest<ResponseDto<int>>
{
    public int LeftOperand { get; set; }
    public int RightOperand { get; set; }
}

public class AddQueryHandler : IRequestHandler<AddQuery, ResponseDto<int>>
{
    private readonly IJobExecutionService jobExecutionService;

    public AddQueryHandler(IJobExecutionService j)
    {
        jobExecutionService = j;
    }

    public async Task<ResponseDto<int>> Handle(AddQuery request, CancellationToken cancellationToken)
    {
        var id = await jobExecutionService.EnqueueJobAsync<FileScannerService, FileScannerServicePayload>(
            new FileScannerServicePayload()
            {
                Path = "",
                CancellationTokenSource = new CancellationTokenSource()
            }
        );

        await Task.Delay(5000);

        await jobExecutionService.CancelJob(id, cancellationToken);

        return await Task.FromResult(new ResponseDto<int>(request.LeftOperand + request.RightOperand));
    }
}
