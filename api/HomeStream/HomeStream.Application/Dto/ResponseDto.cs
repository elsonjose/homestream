namespace HomeStream.Application.Dto;

public class ResponseDto<T>(T data)
{
    public T Data { get; set; } = data;
}
